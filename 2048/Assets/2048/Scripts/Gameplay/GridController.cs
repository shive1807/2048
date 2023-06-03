using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{
    GameObject[,] NumElements;

    [Header("Grid Settings")]
    public static int rows = 8;
    public static int cols = 6;
    [SerializeField] float spacing = 100;

    public GameObject NumElement;
    public GameObject background;
    public Vector2 ElementfallOffset;
    public float ElementFallDuration;
    // Start is called before the first frame update
    void Start()
    {
        gridSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void gridSetup()
    {
        NumElements = new GameObject[rows,cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
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

        element.GetComponent<ElementController>().elementSetup(i, j, element, pos, ElementfallOffset, ElementFallDuration);
    }
    
}
