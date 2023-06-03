using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int chainSize = 0;
    public int xyz = 0;

    private List<ElementController> chain;

    // Start is called before the first frame update
    void Start()
    {
        chain = new List<ElementController>();
        xyz = 0;
        Debug.Log(xyz);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("update " + xyz);

    }

    public void chaining(ElementController numElement)
    {
        Debug.Log("chaining " + xyz);

        if (!numElement.selected)
        {
            Debug.Log(chain.Count);
            if (xyz == 0)
            {
                chain.Add(numElement);
                Debug.Log("added1");
            }
            else
            {
                int x = chain[xyz - 1].rowIndex;
                int y = chain[xyz - 1].colIndex;
                Debug.Log("added");

                if ((numElement.rowIndex == x + 1 || numElement.rowIndex == x - 1) &&
                   (numElement.colIndex == y + 1 || numElement.colIndex == y - 1))
                {
                    chain.Add(numElement);
                }
            }
            //xyz++;
        }
    }
}
