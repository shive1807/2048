using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(Element[,] gameGrid = default, int highScore = default , int gems = default)
    {
        BinaryFormatter formatter= new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData(gameGrid, highScore, gems);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/GameData";

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
}
