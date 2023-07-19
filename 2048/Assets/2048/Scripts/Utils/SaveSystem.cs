using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(int gems = -1, bool gridChanged = false, Element[,] gameGrid = default, double highScore = -1, int soundPref = -1, int musicPref = -1, int vibrationPref = -1)
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
            for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
            {
                for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
                {
                    grid[i, j] = gameGrid[i, j].num;
                }
            }
        }

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log(soundPref);
        GameData gameData = new GameData(gems, grid, highScore, soundPref, musicPref, vibrationPref);

        formatter.Serialize(stream, gameData);
        stream.Close();
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
    }
}
