using UnityEngine;
[System.Serializable]
public class GameData
{
    public Num[,] SavedGrid = null;
    public double HighScore = -1;
    public int Gems = -1;

    public GameData(int gems = -1, Num[,] Grid = default, double highScore = -1)
    {
        SavedGrid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        if(Grid != null)
        {
            SavedGrid = Grid;
        }
        if ( highScore != -1)
        {
            HighScore = highScore;
        }
        if( gems != -1)
        {
            Gems = gems;
        }
    }
}
