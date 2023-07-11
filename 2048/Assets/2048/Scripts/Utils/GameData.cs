using UnityEngine;
[System.Serializable]
public class GameData
{
    public Num[,] SavedGrid;
    public double HighScore = 0;
    public int Gems = 0;

    public GameData(Num[,] Grid = default, double highScore = default, int gems = default)
    {
        SavedGrid = new Num[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        if(Grid != null)
        {
            SavedGrid = Grid;
        }
        if ( highScore != 0)
        {
            HighScore = highScore;
        }
        if( gems != 0)
        {
            Gems = gems;
        }
    }
}
