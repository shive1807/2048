using System;
using UnityEngine;
[Serializable]
public class GameData
{
    // Game values
    public Num[,] SavedGrid = null;
    public double HighScore = 0;
    public int Gems = 0;
    public int SwapAbilityCount = 3;
    public int SmashAbilityCount = 3;
    public Num MaxBlock = new Num(2);

    // Settings info
    public int SoundPref = 1;
    public int MusicPref = 1;
    public int VibrationPref = 1;

    // Daily Reward info
    public DateTime LastClaimRewardDate = DateTime.Now;
    public int RewardClaimStreak = 0;
    public int Collected = 0;

    // User info
    public int FirstLogin = 1;
    public User User;

    public GameData(int gems = 0, Num[,] Grid = null, double highScore = 0, int soundPref = 1, int musicPref = 1, int vibrationPref = 1, 
        int rewardStreak = 0, DateTime date = default, int collected = 0, int firstLogin = 1, User user = default, int swapAbilityCount = 0, 
        int smashAbilityCount = 0, Num maxBlock = null)
    {
        SavedGrid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];
        
        SavedGrid = Grid;
        
        HighScore = highScore;

        Gems = gems;

        SwapAbilityCount = swapAbilityCount;

        SmashAbilityCount = smashAbilityCount;

        MaxBlock = maxBlock;

        MusicPref = musicPref;

        SoundPref = soundPref;

        VibrationPref = vibrationPref;

        LastClaimRewardDate = date;

        RewardClaimStreak = rewardStreak;

        Collected = collected;

        FirstLogin = firstLogin;

        User = user;
    }
}


public static class DataManager
{
    public static int Gems { get; set; }

    public static Color[] Colors = new Color[]
    {
        new Color(0.8555f, 0.9297f, 0.9492f, 1.0f), // Powder Blue
        new Color(0.7461f, 0.8672f, 0.8555f, 1.0f), // Aquamarine
        new Color(0.9648f, 0.8828f, 0.8164f, 1.0f), // Peachy Pink
        new Color(0.8125f, 0.9297f, 0.8477f, 1.0f), // Mint Cream
        new Color(0.8281f, 0.8633f, 0.9219f, 1.0f), // Periwinkle Blue
        new Color(0.9922f, 0.9063f, 0.8008f, 1.0f), // Apricot
        new Color(0.7734f, 0.8828f, 0.8516f, 1.0f), // Baby Blue
        new Color(0.9609f, 0.7695f, 0.7695f, 1.0f), // Pink Coral
        new Color(0.8672f, 0.9297f, 0.8516f, 1.0f), // Pistachio
        new Color(0.9922f, 0.8594f, 0.7773f, 1.0f), // Light Salmon
        new Color(0.6797f, 0.8477f, 0.8750f, 1.0f), // Soft Sky Blue (#AED9E0)
        new Color(0.7734f, 0.8984f, 0.7852f, 1.0f), // Mellow Mint Green (#C6E6C9)
        new Color(0.8750f, 0.7969f, 0.8750f, 1.0f), // Gentle Lavender (#E0CCE0)
        new Color(0.9766f, 0.8477f, 0.7852f, 1.0f), // Pale Peach (#FAD9C9)
        new Color(0.9102f, 0.7070f, 0.7070f, 1.0f), // Dusty Rose (#E9B5B5)
        new Color(0.9961f, 0.9492f, 0.6875f, 1.0f), // Light Lemon Yellow (#FFF3B0)
        new Color(0.7227f, 0.8984f, 0.8438f, 1.0f), // Serene Seafoam (#B9E6D8)
        new Color(0.8906f, 0.7734f, 0.8906f, 1.0f), // Subtle Lilac (#E4C6E4)
        new Color(0.9883f, 0.7734f, 0.7734f, 1.0f), // Delicate Coral (#FDC6C6)
        new Color(0.9531f, 0.9063f, 0.8203f, 1.0f)  // Tender Tan (#F4E8D2)
    };

    public static Color GetColor(Num num)
    {
        int n = (int)(num.numVal * Math.Pow(1000, Num.CurrentDec(num)));
        if (n <= 0)
            throw new ArgumentException("Input must be a positive integer greater than 0.");

        int index = (int)Math.Log(n, 2);

        return Colors[index %= Colors.Length];
    }
}