using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public List<Element> chain;
    public Element LastElement = null;
    public Element SecondLastElement = null; 

    private void Awake()
    {
        Singleton();
    }
    void Start()
    {
        chain = new List<Element>();
    }

    void Update()
    {
        ClearChain();  // to clear the chain when mouse button is released
    }

    public void Chaining(Element numElement)
    {
        if (!numElement.selected && numElement != LastElement)
        {
            if (chain.Count == 0)
            {
                AddToChain(numElement);
            }
            else
            {
                int x = LastElement.rowIndex;
                int y = LastElement.colIndex;

                if (numElement.rowIndex == x - 1 || numElement.rowIndex == x + 1 || numElement.rowIndex == x) 
                {
                    if(numElement.colIndex == y - 1 || numElement.colIndex == y + 1 || numElement.colIndex == y)
                    {
                        if (numElement.num == LastElement.num || numElement.num/LastElement.num == 2)
                        {
                            AddToChain(numElement);
                        }
                    }
                }
            }
        }
        else
        {
            if (numElement == SecondLastElement && chain.Count > 1)    // so that if there's only one element and if checking second last element because the current element
            {                                                          // is the element on which cursor is currently on
                RemoveFromChain(numElement);
            }
        }
    }

    private void RemoveFromChain(Element numElement)  // removing the last element from chain
    {
        chain.Remove(LastElement);
        LastElement.selected = false;
        LastElement = chain[chain.Count - 1];
        if(chain.Count > 1)
        {
            SecondLastElement = chain[chain.Count - 2];
        }

        Debug.Log("(" + numElement.rowIndex + "," + numElement.colIndex + ") Removed");
    }

    private void AddToChain(Element numElement)       // adding element to chain
    {
        chain.Add(numElement);
        numElement.selected = true;
        LastElement = numElement;
        if(chain.Count > 1)
        {
            SecondLastElement = chain[chain.Count - 2];
        }

        Debug.Log("(" + numElement.rowIndex + "," + numElement.colIndex + ") Added");
    }   

    public void ClearChain()
    {
        if(DependencyManager.Instance.inputManager.released)  // resetting the variables on mouse release
        {
            chain.Clear();
            LastElement = null;
            SecondLastElement = null;
            DependencyManager.Instance.inputManager.released = false;

            Debug.Log("clear");
        }
    }
    public void Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
