using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxHealth = 50;
    public float currentHealth;
    public int currentScore;
    public Image healthBar;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro gameOverText;
    public AudioSource source;
    public AudioClip damage;
    public AudioClip die;
    public VolumeProfile volumeProfile;
    
    float chromaticIntensity;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; 
        gameOverText.text = "";
        changeChromaticIntensity(0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        healthBar.fillAmount = currentHealth/maxHealth;
        scoreText.text = currentScore.ToString();
        if ( chromaticIntensity > 0 )
        {
            changeChromaticIntensity(chromaticIntensity-0.05f);
        };
    }

    void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(damage);
        currentHealth -= 10;
        changeChromaticIntensity(2f);
        currentHealth = Mathf.Max(0,currentHealth);
        if (currentHealth == 0) Die();
        
    }

    void Die()
    {
        source.PlayOneShot(die);
        gameOverText.text = "GAME OVER!!!\nPRESS A TO RETRY";
    }

    void changeChromaticIntensity(float value)
    {
        UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
        if(volumeProfile.TryGet(out chromatic))
        {
            chromatic.intensity.Override(value);
            chromaticIntensity = value;
        }
    }

    // public void RestartCurrentScene()
    // {
    //     int currentScene = SceneManager.GetActiveScene().buildIndex;
    //     SceneManager.LoadScene(currentScene);
    // }
}
