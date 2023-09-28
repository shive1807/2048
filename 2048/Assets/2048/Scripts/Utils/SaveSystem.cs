using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    public static void SaveGame(int gems = -1, bool gridChanged = false, Element[,] gameGrid = default, double highScore = -1, 
        int soundPref = -1, int musicPref = -1, int vibrationPref = -1, int rewardStreak = -1, DateTime date = default, int collected = -1,
        int firstLogin = -1, User user = default, int swapAbilityCount = -1, int smashAbilityCount = -1)
    {
        GameData gameData = GameManager.Instance.gameData;

        if (gridChanged)
        {
            if (gameGrid != null)
            {
                gameData.SavedGrid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

                for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
                {
                    for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
                    {
                        gameData.SavedGrid[i, j] = gameGrid[i, j].num;
                    }
                }
            }
            else
            {
                gameData.SavedGrid = null;
            }
        }

        gameData.HighScore = (highScore == -1) ? gameData.HighScore : highScore;
        gameData.Gems = (gems == -1) ? gameData.Gems : gems;
        gameData.SmashAbilityCount = (smashAbilityCount == -1) ? gameData.SmashAbilityCount : smashAbilityCount;
        gameData.SwapAbilityCount = (swapAbilityCount == -1) ? gameData.SwapAbilityCount : swapAbilityCount;
        gameData.SoundPref = (soundPref == -1) ? gameData.SoundPref : soundPref;
        gameData.MusicPref = (musicPref == -1) ? gameData.MusicPref : musicPref;
        gameData.VibrationPref = (vibrationPref == -1) ? gameData.VibrationPref : vibrationPref;
        gameData.RewardClaimStreak = (rewardStreak == -1) ? gameData.RewardClaimStreak : rewardStreak;
        gameData.Collected = (collected == -1) ? gameData.Collected : collected;
        gameData.LastClaimRewardDate = (date == default) ? gameData.LastClaimRewardDate : date;
        gameData.FirstLogin = (firstLogin == -1) ? gameData.FirstLogin : firstLogin;
        gameData.User = (user == null) ? gameData.User : user;

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/GameData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        //DatabaseRealtimeManager.Instance.UpdateUserData(new User(gameData));
        GameManager.Instance.LoadGameData();
    }
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/GameData.Data";
        GameData data = new GameData();
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as GameData;
            stream.Close();
        }
        else
        {
            //Debug.LogError("Save file was not found in " + path);
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        return data;
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
