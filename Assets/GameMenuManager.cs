using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public GameObject leftHandGun;
    public GameObject rightHandGun;
    public GameObject rightHandRay;
    

    public StageManager stageManager;
    
    public GameObject menu;
    public GameObject logger;
    public float spawnDistance = 1f;
    public InputActionProperty showButton;
    public InputActionProperty retryButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Player player = head.gameObject.GetComponent<Player>();
        // display menu with b button
        if (showButton.action.WasPressedThisFrame() && player.currentHealth > 0 && stageManager.hub.gameObject.activeSelf == false)
        {
            if (Time.timeScale == 1.0f) 
                Time.timeScale = 0.0f;
            else 
                Time.timeScale = 1.0f;
            DisplayCanvas();
        };
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *=-1;

        logger.transform.LookAt(new Vector3(head.position.x, logger.transform.position.y, head.position.z));
        logger.transform.position = head.position + new Vector3(head.forward.x,0,head.forward.z).normalized * (spawnDistance-0.2f);
        logger.transform.forward *=-1;

        // try again current stage with a button
        if (retryButton.action.WasPressedThisFrame() && player.currentHealth <= 0)
        {
            DisplayCanvas();
            stageManager.RestartCurrentStage();
        }
    }

    public void DisplayCanvas()
    {
        rightHandRay.SetActive(!rightHandRay.activeSelf);
        leftHandGun.SetActive(!leftHandGun.activeSelf);
        rightHandGun.SetActive(!rightHandGun.activeSelf);
        menu.SetActive(!menu.activeSelf);
        menu.transform.position = head.position + new Vector3(head.forward.x,0,head.forward.z).normalized * spawnDistance;
    }
    
}
