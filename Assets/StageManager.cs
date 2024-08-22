using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class StageManager : MonoBehaviour
{
    public Player player;
    public SimpleShoot leftGun;
    public SimpleShoot rightGun;
    public Transform hub;
    private Transform stage;
    private int stageIndex;
    private  PlayableDirector playableStageDirector;

    public GameMenuManager menu;
    public DataPersistenceManager dataPersistenceManager;
    public PlayerPositionManager playerPositionManager;
    public AudioSource mainMenuMusic;

    public ReflectionProbe reflectionProbe;
    public Material enemyOutlineMaterial;
    private Color[] stageSkyboxColors = { 
        Color.cyan, 
        Color.green,
        Color.red, 
    };
    private Color[] enemyOutlineColors = {
        Color.yellow,
        Color.cyan,
    };


    // Start is called before the first frame update
    void Start()
    {
        stage = transform.GetChild(0);
        menu.DisplayCanvas();
        menu.menu.SetActive(false);
        RenderSettings.skybox.SetColor("_Tint",stageSkyboxColors[0]);
        reflectionProbe.RenderProbe();
    }

    // Update is called once per frame
    void Update()
    {
        // remove all enemies when player's Health is below 0
        if (player.currentHealth <= 0 )
        {    
            stage.GetChild(0).gameObject.SetActive(false);
        }
        // if the timeline is completed, execute CompleStage()
        if (playableStageDirector)
        {
            if (playableStageDirector.time == playableStageDirector.duration) CompleteStage();
        }
    }
    

    public void StartStage(int stageNumber)
    {
        hub.gameObject.SetActive(false);
        menu.DisplayCanvas();
        menu.menu.SetActive(false);
        
        stage = transform.GetChild(stageNumber);
        this.stageIndex = stageNumber - 1;
        enemyOutlineMaterial.SetColor("_Outline_Color", enemyOutlineColors[this.stageIndex]);

        playableStageDirector = stage.GetComponent<PlayableDirector>();
        stage.gameObject.SetActive(true);

        RenderSettings.skybox.SetColor("_Tint",stageSkyboxColors[stageNumber]);
        reflectionProbe.RenderProbe();
    }

    public void ResumeStage()
    {
        Time.timeScale = 1.0f;
        menu.DisplayCanvas();
    }

    public void RestartCurrentStage() 
    {
        Time.timeScale = 1.0f;
        menu.DisplayCanvas();
        player.currentHealth = player.maxHealth;
        leftGun.currentAmmo = leftGun.maxAmmo;
        leftGun.enabled = true;
        rightGun.currentAmmo = rightGun.maxAmmo;
        rightGun.enabled = true;
        player.currentScore = 0;
        player.gameOverText.text = "";

        mainMenuMusic.gameObject.SetActive(false);
        foreach ( var interactive in stage.GetChild(0).GetComponentsInChildren<Transform>(true) )
        {
            interactive.gameObject.SetActive(true);
        }
        ResetEnemies();

        playableStageDirector.time = 0;
        playableStageDirector.Stop();
        playableStageDirector.Evaluate();
        playableStageDirector.Play();
    }

    public void ExitStage()
    {
        Time.timeScale = 1.0f;
        menu.DisplayCanvas();
        player.currentHealth = player.maxHealth;
        leftGun.currentAmmo = leftGun.maxAmmo;
        leftGun.enabled = true;
        rightGun.currentAmmo = rightGun.maxAmmo;
        rightGun.enabled = true;
        player.currentScore = 0;
        player.gameOverText.text = "";

        mainMenuMusic.gameObject.SetActive(false);
        foreach ( var interactive in stage.GetChild(0).GetComponentsInChildren<Transform>(true) )
        {
            interactive.gameObject.SetActive(true);
        }
        ResetEnemies();

        playableStageDirector.time = 0;
        playableStageDirector.Stop();
        playableStageDirector.Evaluate();

        menu.DisplayCanvas();
        stage.gameObject.SetActive(false);
        hub.gameObject.SetActive(true);
        playerPositionManager.ResetPlayerPosition();

        RenderSettings.skybox.SetColor("_Tint",stageSkyboxColors[0]);
        reflectionProbe.RenderProbe();
    }

    void CompleteStage()
    {
        player.currentScore += Mathf.RoundToInt(10*player.currentHealth);
        dataPersistenceManager.SaveHighscore( player.currentScore, this.stageIndex);
        playableStageDirector.time = 0;
        playableStageDirector.Stop();
        mainMenuMusic.gameObject.SetActive(true);
        menu.DisplayCanvas();
    }

    void ResetEnemies()
    {
        //reset positions
        var enemies =  stage.GetChild(0).GetComponentsInChildren<Enemy>(true);
        foreach (var enemy in enemies)
        {
            enemy.resetPosition();
        }
        //reset health, ragdoll
        var healths =  stage.GetChild(0).GetComponentsInChildren<Health>(includeInactive: true);
        foreach (var health in healths)
        {
            health.currentHealth = health.maxHealth;
            health.ragdoll.DeactivateRagdoll();
            // turn on outline effects
            health.gameObject.layer = 8;
            var children = health.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var child in children)
            {
                child.gameObject.layer = 8;
            }
            // reset dissolve effects
            var dissolveObject = health.gameObject.GetComponent<DissolveObject>();
            dissolveObject.SetValue(0);
            dissolveObject.value = 1;
        }
    }

}
