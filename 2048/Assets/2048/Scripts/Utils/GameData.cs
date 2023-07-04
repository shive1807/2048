public class GameData
{
    public Element[,] SavedGrid;
    public int HighScore;
    public GameData(Element[,] Grid, int highScore)
    {
        SavedGrid = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        SavedGrid = Grid;
        HighScore = highScore;
    }
}
