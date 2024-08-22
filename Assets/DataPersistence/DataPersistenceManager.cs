using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DataPersistenceManager : MonoBehaviour
{
    public string fileName;
    public GameData gameData;
    private FileDataHandler fileDataHandler;
    public StageManager stageManager;

    public static DataPersistenceManager instance { get; private set; }

    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        Debug.Log(Application.persistentDataPath);
        LoadGame();
    }

    private void OnApplicationQuit() {
        SaveGame();    
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    
    private void LoadGame()
    {
        gameData = fileDataHandler.Load();
        if (gameData == null)
        {
            Debug.Log("no data was found. Initializing data to defaults.");
            NewGame();
        }
        Debug.Log("load highscores");
    }

    private void SaveGame()
    {
        fileDataHandler.Save(gameData);
        Debug.Log("save highscore");
    }   

    public void SaveHighscore(int score,int stageIndex)
    {
        if (score > gameData.highscores[stageIndex])
        {
            gameData.highscores[stageIndex] = score;
            SaveGame();
        }
    }
}
