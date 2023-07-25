using System;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public Num[,] SavedGrid = null;
    public double HighScore = -1;
    public int Gems = 0;

    public int SoundPref = 1;
    public int MusicPref = 1;
    public int VibrationPref = 1;

    public DateTime LastClaimRewardDate = DateTime.Now;
    public int RewardClaimStreak = 1;
    public GameData(int gems = -1, Num[,] Grid = default, double highScore = -1, int soundPref = -1, int musicPref = -1, int vibrationPref = -1, DateTime date = default, int rewardStreak = -1)
    {
        SavedGrid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        SavedGrid = Grid;
        
        if ( highScore != -1)
        {
            HighScore = highScore;
        }
        if( gems != -1)
        {
            Gems = gems;
        }
        if( musicPref != -1)
        {
            MusicPref = musicPref;
        }
        if( soundPref != -1)
        {
            SoundPref = soundPref;
        }
        if( vibrationPref != -1)
        {
            VibrationPref = vibrationPref;
        }
        if( date != default)
        {
            LastClaimRewardDate = date; 
        }
        if( rewardStreak != -1)
        {
            RewardClaimStreak = rewardStreak;
        }
    }
}


public static class DataManager
{
    public static int Gems { get; set; }

}