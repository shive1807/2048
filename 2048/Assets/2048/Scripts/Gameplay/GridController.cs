using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    private Element[,]  grid;
    private GameObject  block;
    public Vector2      ElementfallOffset;

    private int index;

    public float ElementFallDuration;

    [HideInInspector] public int ElementMaxLimit = 8;
    [HideInInspector] public int ElementMinLimit = 1;
    [HideInInspector] public int DecInd = 0;

    private int     reShuffleOffset = 0;
    private int     reShuffleThresh = 0;
    private bool    reShuffling     = false;

    private void Awake() => block = Resources.Load<GameObject>(Assets.numElement);

    private void Start() => GridSetup();

    private void GridSetup()
    {
        grid = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                GenerateBlock(i, j);
            }
        }
    }

    private void GenerateBlock(int i, int j)
    {
        GameObject element = Pooler.Instance.GetBlock();//Instantiate(block) as GameObject;
        //GameObject element = DependencyManager.Instance.pooler.SpawnfromPool();

        element.transform.SetParent(this.transform, false);

        GameData gameData = SaveSystem.LoadGame();

        grid[i, j] = element.GetComponent<Element>();

        if (gameData == null)
        {
            grid[i, j].ElementSetup(i, j, ElementfallOffset, ElementFallDuration);
        }
        else
        {
            grid[i, j].ElementSetup(i, j, ElementfallOffset, ElementFallDuration, gameData.SavedGrid[i, j]);
        }
    }

    private int[,] GetDestroyedBlocks(List<Element> chain)
    {
        int[,] deductions = new int[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        for(int i = 0; i < index; i++)
        {
            deductions[chain[i].x, chain[i].y]++;
        }

        string s = "";
        for (int j = GameSettings.GRID_HEIGHT - 1; j >= 0; j--)
        {
            s += "";
            for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
            {
                s += deductions[i, j].ToString();
            }
        }
        //Debug.Log(s);


        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for(int j = 1; j < GameSettings.GRID_HEIGHT; j++)
            {
                deductions[i, j] += deductions[i, j-1];
            }
        }

        for (int i = 0; i < index; i++)
        {
            deductions[chain[i].x, chain[i].y] = -1;
        }

        s = "";
        for (int j = GameSettings.GRID_HEIGHT - 1; j >= 0; j--)
        {
            s += "/n";
            for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
            {
                s += "  " + deductions[i, j].ToString();
            }
        }
        //Debug.Log(s);

  
        return deductions;
    }
    public void GridRefill(List<Element> chain)
    {
        if (reShuffling)
        {
            index = chain.Count;
        }
        else
        {
            index = chain.Count - 1;
        }
        if(!DependencyManager.Instance.gameController.smashing)
        {
            int[,] deductions = GetDestroyedBlocks(chain);

            for (int i = 0; i < index; i++)
            {
                Pooler.Instance.DestroyBlock(chain[i].gameObject);
                //Destroy(chain[i].gameObject);
                //DependencyManager.Instance.pooler.Deactivate(chain[i].gameObject);

                //Debug.Log("refil");
            }

            for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
            {
                for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
                {
                    int depth = deductions[i, j];

                    if (depth == -1 || depth == 0)
                        continue;

                    Vector2 targetPos = grid[i, j - depth].GetComponent<RectTransform>().anchoredPosition;
                    StartCoroutine(grid[i, j].MoveElement(targetPos, 1));
                }
            }

            Element[,] temp = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];
            temp = grid;

            for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
            {
                for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
                {
                    int depth = deductions[i, j];

                    if (depth == -1 || depth == 0)
                        continue;

                    temp[i, j - depth] = grid[i, j];
                    //temp[i, j - depth].x = i;
                    temp[i, j - depth].SetElementCoord(i, j - depth);
                }
            }

            grid = temp;

            for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
            {
                int blocksToAdd = 0;
                for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
                {
                    if (deductions[i, j] == -1)
                    {
                        blocksToAdd++;
                    }
                }

                for (int j = GameSettings.GRID_HEIGHT - blocksToAdd; j < GameSettings.GRID_HEIGHT; j++)
                {
                    GenerateBlock(i, j);
                }
            }
        }
        else if (DependencyManager.Instance.gameController.smashing)
        {
            Element e = chain[0];
            int j = e.y;
            for (int i = j + 1; i < GameSettings.GRID_HEIGHT; i++)
            {
                Vector2 targetPos = grid[e.x, i - 1].GetComponent<RectTransform>().anchoredPosition;

                StartCoroutine(grid[e.x, i].MoveElement(targetPos, ElementFallDuration));
                grid[e.x, i - 1] = grid[e.x, i];
                grid[e.x, i].SetElementCoord(e.x, i - 1);
            }
            //Debug.Log("sw");
            Destroy(e.gameObject);
            GenerateBlock(e.x, GameSettings.GRID_HEIGHT - 1);
        }

        StartCoroutine(DependencyManager.Instance.gameController.MaxElementCheck());
        SaveSystem.SaveGame(grid, DependencyManager.Instance.gameController.HighScore, DependencyManager.Instance.gemsManager.gems);
        GameEndCheck();
    }
    public Num GetMaxElement()
    {
        Num tempMax = grid[0, 0].num;

        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                tempMax = Num.Max(grid[i, j].num, tempMax);
            }
        }
        return tempMax;
    }
    private void GameEndCheck()
    {
        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                int x = grid[i, j].x;
                int y = grid[i, j].y;

                for (int a = x - 1; a <= x + 1; a++)
                {
                    for (int b = y - 1; b <= y + 1; b++)
                    {
                        if (a >= 0 && a < GameSettings.GRID_WIDTH && b >= 0 && b < GameSettings.GRID_HEIGHT &&
                            (a != x || b != y))
                        {
                            if(grid[a, b].num.txt == grid[i, j].num.txt)
                            {
                                return; // Found a match, game has not ended
                            }
                        }
                    }
                }
            }
        }
        DependencyManager.Instance.gameManager.LoadScene("MainMenu");
        SaveSystem.DeleteGameData();
        //return true; // No match found, game has ended
    }
    public void ElementReShuffle(Num MaxNum, Num MinNum)
    {
        reShuffleOffset++;
        Debug.Log("called");

        if (Num.CurrentDec(MaxNum) > 0 && reShuffleOffset > reShuffleThresh)
        {
            StartCoroutine(DependencyManager.Instance.popup.PopupConfirmation(DependencyManager.Instance.gridController.ReShuffleContinued, DependencyManager.Instance.newBlockPopup, MinNum));
        }
    }
    private void ReShuffleContinued(Num MinNum)
    {
        ADTest.Instance.LoadInterstitialAd();
        reShuffleOffset = 0;
        reShuffling = true;
        List<Element> list = new List<Element>();

        if (ElementMinLimit < 10)
        {
            ElementMinLimit++;
            ElementMaxLimit = ElementMinLimit + 7;
        }
        else if (ElementMinLimit >= 10)
        {
            ElementMinLimit = 1;
            DecInd++;
        }

        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                if (grid[i, j].num.numVal == MinNum.numVal && grid[i, j].num.dec == MinNum.dec)
                {
                    list.Add(grid[i, j]);
                }
            }
        }
        GridRefill(list);
        DependencyManager.Instance.gameController.minElement = Num.Increment(MinNum, 1);
        reShuffling = false;
    }
}