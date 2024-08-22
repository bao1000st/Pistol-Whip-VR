using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMenuManager : MonoBehaviour
{
    public DataPersistenceManager dataPersistenceManager;
    public Transform highscores;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        var highscoreTexts = highscores.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        foreach (TMPro.TextMeshProUGUI highscore in highscoreTexts) {
            highscore.text = "highscore: " + dataPersistenceManager.gameData.highscores[index];
            index++;
        }
    }

    public void ResetHighscores()
    {
        dataPersistenceManager.NewGame();
    }
     
    public void QuitGame()
    {
        Application.Quit();
    }
}
