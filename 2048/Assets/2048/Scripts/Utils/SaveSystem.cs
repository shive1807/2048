using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(int gems = -1, bool gridChanged = false, Element[,] gameGrid = default, double highScore = -1)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        Num[,] grid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        if(!gridChanged)
        {
            //------------------bug here-------------------------------------
            // gameData loading is  giving some error

            //GameData _gameData = LoadGame();
            //if (_gameData != null)
            //{
            //    grid = _gameData.SavedGrid;
            //}
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
        
        GameData gameData = new GameData(gems, grid, highScore);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/GameData.Data";

        FileStream stream = new FileStream(path, FileMode.Open);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file was not found in " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            GameData data = new GameData();
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

    //--------------bug here-------------------------------------
    // call ResetGrid in pause menu restart instead of DeleteGameData
    public static void ResetGrid() 
    {
        SaveGame(-1, true, null, -1);
    }
}
