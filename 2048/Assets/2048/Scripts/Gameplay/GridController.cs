using UnityEngine;

public class GridController : MonoBehaviour
{
    GameObject[,] NumElements;

    [Header("Grid Settings")]
    public int rows = 8;
    public int cols = 6;
    [SerializeField] float spacing = 100;

    public GameObject NumElement;
    public GameObject background;
    public Vector2 ElementfallOffset;
    public float ElementFallDuration;

    void Start()
    {
        gridSetup();
    }

    void gridSetup()
    {
        NumElements = new GameObject[rows,cols];

        for (int i = 0; i < rows; i++)       // spawning the elements
        {
            for (int j = 0; j < cols; j++)
            {
                //GameObject element = DependencyManager.Instance.pooler.SpawnfromPool();

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
