using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int chainSize = 0;

    private List<ElementController> chain;

    // Start is called before the first frame update
    void Start()
    {
        chain = new List<ElementController>();
        chainSize = 0;
        Debug.Log(chainSize);
    }

    // Update is called once per frame
    void Update()
    {
        ClearChain();
    }

    public void Chaining(ElementController numElement)
    {
        Debug.Log("chaining " + chainSize);

        if (!numElement.selected)
        {
            Debug.Log("Chain Count " + chain.Count);
            if (chainSize == 0)
            {
                chain.Add(numElement);
                Debug.Log("added1");
            }
            else
            {
                int x = chain[chainSize - 1].rowIndex;
                int y = chain[chainSize - 1].colIndex;
                Debug.Log("added");

                if ((numElement.rowIndex == x + 1 || numElement.rowIndex == x - 1) &&
                   (numElement.colIndex == y + 1 || numElement.colIndex == y - 1))
                {
                    chain.Add(numElement);
                }
            }
            chainSize++;
        }
    }
    public void ClearChain()
    {
        if(InputManager.released)
        {
            chain.Clear();
            chainSize = 0;
            InputManager.released = false;
            Debug.Log("clear");
        }
    }
    private void Reset()
    {
        chain.Clear();
        chainSize = 0;
    }
}
