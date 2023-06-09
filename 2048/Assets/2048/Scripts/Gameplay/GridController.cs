using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class GridController : MonoBehaviour
{
    private enum BLOCK
    {
        
    }

    private Element[,]  Grid;
    private GameObject  block;
    private Vector2     ElementfallOffset;

    private float ElementFallDuration;

    private void Awake() => block = Resources.Load<GameObject>(Assets.numElement);

    private void Start() => GridSetup();

    private void GridSetup()
    {
        Grid = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

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
        GameObject element = Instantiate(block) as GameObject;

        element.transform.SetParent(this.transform, false);
        Grid[i, j] = element.GetComponent<Element>();
        Grid[i, j].ElementSetup(i, j, ElementfallOffset, ElementFallDuration);
    }

    //HACK :- REFERENCE GRID
    // 0 0 0 0 0 0
    // 0 0 0 0 0 0
    // 0 0 0 0 0 0
    // 0 0 0 0 0 0     /\
    // 0 0 0 0 0 0  // ||
    // 0 0 0 0 0 0  // ||
    // 0 0 0 0 0 0  // HEIGHT
    // 0 0 0 0 0 0
    // ====> WIDTH

    private int[,] GetDestroyedBlocks(List<Element> chain)
    {
        int[,] deductions = new int[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];

        for(int i = 0; i < chain.Count - 1; i++)
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
        Debug.Log(s);


        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for(int j = 1; j < GameSettings.GRID_HEIGHT; j++)
            {
                deductions[i, j] += deductions[i, j-1];
            }
        }

        for (int i = 0; i < chain.Count - 1; i++)
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
        Debug.Log(s);

  
        return deductions;
    }


    public void GridRefill(List<Element> chain)
    {
        int[,]  deductions = GetDestroyedBlocks(chain);

        for (int i = 0; i < chain.Count - 1; i++)
        {
            Destroy(chain[i].gameObject);
        }

        for(int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for(int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                int depth = deductions[i, j];

                if (depth == -1 || depth == 0)
                    continue;

                Vector2 targetPos = Grid[i, j - depth].GetComponent<RectTransform>().anchoredPosition;
                StartCoroutine(Grid[i, j].MoveElement(targetPos, 1));
            }
        }

        Element[,] temp = new Element[GameSettings.GRID_WIDTH, GameSettings.GRID_HEIGHT];
        temp = Grid;

        for (int i = 0; i < GameSettings.GRID_WIDTH; i++)
        {
            for (int j = 0; j < GameSettings.GRID_HEIGHT; j++)
            {
                int depth = deductions[i, j];

                if (depth == -1 || depth == 0)
                    continue;

                temp[i, j - depth] = Grid[i, j];
            }
        }

        Grid = temp;

        for(int i = 0; i < GameSettings.GRID_WIDTH; i++)
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
}