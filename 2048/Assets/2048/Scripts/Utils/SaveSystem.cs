using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(Element[,] gameGrid = default, double highScore = default , int gems = default)
    {
        BinaryFormatter formatter= new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        Num[,] grid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        for(int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for(int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                grid[i, j] = gameGrid[i, j].num;
            }
        }
        GameData gameData = new GameData(grid, highScore, gems);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/GameData.data";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            GameData gameData =  formatter.Deserialize(stream) as GameData;
            stream.Close();

            return gameData;
        }
        else
        {
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
}
