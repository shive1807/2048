using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    Element[,] NumElements;

    [Header("Grid Settings")]
    public int rows = 8;
    public int cols = 6;

    [SerializeField] float spacing = 100;

    public GameObject NumElement;
    public GameObject background;
    public Vector2 ElementfallOffset;
    public float ElementFallDuration;

    private void Start() => gridSetup();

    void gridSetup()
    {
        NumElements = new Element[GameSettings.GRID_HEIGHT, GameSettings.GRID_WIDTH];

        for (int i = 0; i < GameSettings.GRID_HEIGHT; i++)       // spawning the elements
        {
            for (int j = 0; j < GameSettings.GRID_WIDTH; j++)
            {
                //GameObject element = DependencyManager.Instance.pooler.SpawnfromPool();

                GameObject element = Instantiate(NumElement) as GameObject;
                Vector2 Pos = new Vector2((j * spacing) - 370, (i * spacing) - 520);  // calculating Pos with respect to anchors
                
                NumElementSetup(i, j, element, Pos);
            }
        }
    }
    public void GridRefill(List<Element> chain)
    {
        for(int i = 0; i < chain.Count - 1; i++)
        {
            Element element = chain[i];
            GameObject _element =  Instantiate(NumElement) as GameObject;
            Vector2 Pos = new Vector2((element.colIndex * spacing) - 370, (GameSettings.GRID_HEIGHT * spacing) - 520);

            NumElementSetup(GameSettings.GRID_HEIGHT - 1, element.colIndex - 1, _element, Pos);

            for (int j = element.rowIndex; j < GameSettings.GRID_HEIGHT; j++)
            {
                NumElements[j - 1 , element.colIndex - 1] = NumElements[j, element.colIndex - 1];
                Vector2 _Pos = new Vector2(element.transform.position.x, element.transform.position.y);
                StartCoroutine(NumElements[j - 1, element.colIndex - 1].MoveElement(NumElements[j, element.colIndex - 1].rectTransform, _Pos, ElementFallDuration));
            }
            NumElements[GameSettings.GRID_HEIGHT - 1, element.colIndex - 1] = _element.GetComponent<Element>();
        }
    }
    private void NumElementSetup(int i, int j, GameObject element, Vector2 pos)
    {
        NumElements[i, j] = element.GetComponent<Element>();
        element.transform.SetParent(this.transform, false);

        NumElements[i,j].ElementSetup(i, j, element, pos, ElementfallOffset, ElementFallDuration);
    }
}
