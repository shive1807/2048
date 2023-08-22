using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class GridController : MonoBehaviour
{
    //Private variables.
    private int             reShuffleThresh = 0;
    private int             reShuffleOffset = 0;
    private int             index           = 0;
    private bool            reShuffling     = false;
    private GameObject      block;
    private RectTransform   linesRectTransform;

    //Public variables.
    public bool         startingGrid = true;
    public float        ElementFallDuration = 3f;
    public float        ElementDestroyDuration = .3f;
    public Element[,]   grid;
    public Vector2      ElementfallOffset;

    public float ElementFallSpeed = 3f;

    public bool gridMoving = false;

    //Public hidden variables.
    [HideInInspector] public int ElementMaxLimit = 8;
    [HideInInspector] public int ElementMinLimit = 1;
    [HideInInspector] public int DecInd = 0;

    //Unity engine funtions.
    private void Awake() => block = Resources.Load<GameObject>(Assets.numElement);

    private void Start() => GridSetup();

    private void Update()
    {
        BlockInputCheck();
    }

    #region public functions
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

    public void ElementReShuffle(Num MaxNum, Num MinNum)
    {
        reShuffleOffset++;

        if (Num.CurrentDec(MaxNum) > 0 && reShuffleOffset > reShuffleThresh)
        {
            //AdManager.Instance.ShowInterstitial();
            Debug.Log("reshuffle");
            StartCoroutine(DependencyManager.Instance.popup.PopupConfirmation(DependencyManager.Instance.gridController.ReShuffleContinued, DependencyManager.Instance.newBlockPopup, MinNum, MaxNum));
            DependencyManager.Instance.vfx.PlayCelebrationVfx();
        }
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

                StartCoroutine(DestroyBlock(chain[i]));

                //Pooler.Instance.DestroyBlock(chain[i].gameObject);
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
                    StartCoroutine(grid[i, j].MoveElement(targetPos, ElementFallSpeed));
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
                    GenerateBlock(i, j, j - (GameSettings.GRID_HEIGHT - blocksToAdd));
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

                StartCoroutine(grid[e.x, i].MoveElement(targetPos, ElementFallSpeed));
                grid[e.x, i - 1] = grid[e.x, i];
                grid[e.x, i].SetElementCoord(e.x, i - 1);
            }
            //Debug.Log("sw");
            Pooler.Instance.DestroyBlock(e.gameObject);
            GenerateBlock(e.x, GameSettings.GRID_HEIGHT - 1, 0);
        }

        StartCoroutine(DependencyManager.Instance.gameController.MaxElementCheck());
        SaveSystem.SaveGame(-1, true, grid, DependencyManager.Instance.gameController.HighScore);
        GameEndCheck();
    }
    #endregion

    #region private functions
    private void BlockInputCheck()
    {
        foreach(Element element in grid)
        {
            if (element.moving)
            {
                DependencyManager.Instance.gameController.BlockInput(true);
                return;
            }
        }
        DependencyManager.Instance.gameController.BlockInput(false);
    }

    private void SetSize()
    {
        transform.GetComponent<RectTransform>().sizeDelta = GameSettings.GRID_SIZE;

        linesRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        linesRectTransform.sizeDelta = GameSettings.GRID_SIZE;
    }

    private void GridSetup()
    {
        SetSize();

        grid = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                GenerateBlock(i, j);
            }
        }
       DependencyManager.Instance.gameController.maxElement = GetMaxElement();
        startingGrid = false;
    }

    private void GenerateBlock(int i, int j, int BlocksBelow = 0)
    {
        GameObject element = Pooler.Instance.GetBlock();//Instantiate(block) as GameObject;
        //GameObject element = DependencyManager.Instance.pooler.SpawnfromPool();

        element.transform.SetParent(this.transform, false);


        grid[i, j] = element.GetComponent<Element>();

        if (!startingGrid || GameManager.Instance.gameData.SavedGrid == null)
        {
            grid[i, j].ElementSetup(i, j, ElementfallOffset, ElementFallSpeed);
        }
        else 
        {
            grid[i, j].ElementSetup(i, j, ElementfallOffset, ElementFallSpeed, GameManager.Instance.gameData.SavedGrid[i, j]);
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
   
    private IEnumerator DestroyBlock(Element e)
    {
        //DependencyManager.Instance.vfx.PlayBreakingFX(e);
        e.rectTransform.DOScale(0, ElementDestroyDuration).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(ElementDestroyDuration);
        Pooler.Instance.DestroyBlock(e.gameObject);
    }
    
    private IEnumerator GameEndCheck()
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
                               yield return null; // Found a match, game has not ended
                            }
                        }
                    }
                }
            }
        }

        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("GameOver");

        yield return new WaitForSeconds(GameSettings.GAME_END_TIME);
        DependencyManager.Instance.gameManager.LoadScene("MainMenu");
        SaveSystem.DeleteGameData();
        //return true; // No match found, game has ended
    }

    private void ReShuffleContinued(Num MinNum)
    {
        //ADTest.Instance.LoadInterstitialAd();
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
    #endregion
}