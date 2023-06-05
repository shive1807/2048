using UnityEngine;

public class GridController : MonoBehaviour
{
    GameObject[,] NumElements;

    [SerializeField] float spacing = 100;

    public GameObject NumElement;
    public GameObject background;
    public Vector2 ElementfallOffset;
    public float ElementFallDuration;

    private void Start() => GridSetup();

    private void GridSetup()
    {
        NumElements = new GameObject[GameSettings.GRID_HEIGHT,  GameSettings.GRID_WIDTH];

        for (int i = 0; i < GameSettings.GRID_HEIGHT; i++)       // spawning the elements
        {
            for (int j = 0; j < GameSettings.GRID_WIDTH; j++)
            {
                GameObject element = Instantiate(NumElement) as GameObject;
                Vector2 Pos = new Vector2((j * spacing) - 370, (i * spacing) - 520);  // calculating Pos with respect to anchors
                
                NumElementSetup(i, j, element, Pos);
            }
        }
    }

    private void NumElementSetup(int i, int j, GameObject element, Vector2 pos)
    {
        NumElements[i, j] = element;
        element.transform.SetParent(this.transform, false);

        element.GetComponent<Element>().ElementSetup(i, j, element, pos, ElementfallOffset, ElementFallDuration);
    }
}
