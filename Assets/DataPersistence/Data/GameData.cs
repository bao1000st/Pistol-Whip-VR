using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[] highscores;
    public GameData()
    {
        this.highscores = new int[2];
    }
}
