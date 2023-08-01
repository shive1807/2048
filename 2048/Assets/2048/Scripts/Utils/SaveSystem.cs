using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    public static void SaveGame(int gems = -1, bool gridChanged = false, Element[,] gameGrid = default, double highScore = -1, int soundPref = -1, int musicPref = -1, int vibrationPref = -1, DateTime date = default, int rewardStreak = -1)
    {
        Num[,] grid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        if (!gridChanged)
        {
            GameData _gameData = LoadGame();
            if (_gameData != null)
            {
                grid = _gameData.SavedGrid;
            }
        }
        else
        {
            if(gameGrid != null)
            {
                for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
                {
                    for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
                    {
                        grid[i, j] = gameGrid[i, j].num;
                    }
                }
            }
            else
            {
                grid = null;
            }
        }

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log(soundPref);
        GameData gameData = new GameData(gems, grid, highScore, soundPref, musicPref, vibrationPref, date, rewardStreak);

        formatter.Serialize(stream, gameData);
        stream.Close();
        GameManager.Instance.ReloadGameData();
    }
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/GameData.Data";


        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file was not found in " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            GameData data = new GameData(0, null, 0);
            formatter.Serialize(stream, data);
            stream.Close();
            return null;
        }
    }
    public static void DeleteGameData()
    {
        string path = Application.persistentDataPath + "/GameData.data";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    public static void ResetGrid() 
    {
        SaveGame(-1, true, null);
        DependencyManager.Instance.gridController.startingGrid = false;
    }
}
