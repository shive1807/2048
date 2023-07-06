using UnityEngine;
[System.Serializable]
public class GameData
{
    public Element[,] SavedGrid;
    public int HighScore = 0;
    public int Gems = 0;

    public GameData(Element[,] Grid = default, int highScore = default, int gems = default)
    {
        SavedGrid = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

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
