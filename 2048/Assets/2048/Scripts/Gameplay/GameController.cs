using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static GameController Instance;

    private LineRenderer lineRenderer;
        
    public List<Element> chain;
    private void Awake()
    {
        chain = new List<Element>();
        lineRenderer= GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ClearChain();  // to clear the chain when mouse button is released
        LineMatching();
    }

    public void Chaining(Element numElement)
    {
        if (!numElement.selected)
        {
            if (chain.Count == 0)
            {
                AddToChain(numElement);
            }
            else
            {
                int x = chain[chain.Count - 1].rowIndex;
                int y = chain[chain.Count - 1].colIndex;

                if (numElement.rowIndex == x - 1 || numElement.rowIndex == x + 1 || numElement.rowIndex == x)
                {
                    if (numElement.colIndex == y - 1 || numElement.colIndex == y + 1 || numElement.colIndex == y)
                    {
                        if(chain.Count == 1)
                        {
                            if (numElement.num == chain[chain.Count - 1].num)
                            {
                                AddToChain(numElement);
                            }
                        }
                        else
                        {
                            if (numElement.num == chain[chain.Count - 1].num || numElement.num/chain[chain.Count - 1].num == 2)
                            {
                                AddToChain(numElement);
                            }
                        }
                        
                    }
                }
            }
        }
        else
        {
            if(chain.Count > 1)
            {
                if (numElement == chain[chain.Count - 2])    // so that if there's only one element and if checking second last element because the current element
                {                                                               // is the element on which cursor is currently on
                    RemoveFromChain(numElement);
                }
            }
        }
    }
    public void LineMatching()
    {
        if (DependencyManager.Instance.inputManager.pressed)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = chain.Count + 2;

            for (int i = 0; i < chain.Count; i++)
            {
                lineRenderer.SetPosition(i, chain[i].elementPos);
            }

            // Set the position of the end of the line to follow the mouse cursor
            Vector3 mousePos = DependencyManager.Instance.inputManager.mousePos;
            lineRenderer.SetPosition(chain.Count + 1, new Vector3(mousePos.x, mousePos.y, 3));
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void RemoveFromChain(Element numElement)  // removing the last element from chain
    {
        chain[chain.Count - 1].selected = false;
        chain.Remove(chain[chain.Count - 1]);
        //chain.Remove(LastElement);
        //LastElement.selected = false;
        //LastElement = chain[chain.Count - 1];
        //if (chain.Count > 1)
        //{
        //    SecondLastElement = chain[chain.Count - 2];
        //}
    }
    private void AddToChain(Element numElement)       // adding element to chain
    {
        chain.Add(numElement);
        numElement.selected = true;
        //LastElement = numElement;
        //if (chain.Count > 1)
        //{
        //    SecondLastElement = chain[chain.Count - 2];
        //}

        Debug.Log("(" + numElement.rowIndex + "," + numElement.colIndex + ") Added");
    }   

    public void ClearChain()
    {
        if (DependencyManager.Instance.inputManager.released)  // resetting the variables on mouse release
        {
            if(chain.Count > 1)
            {
                for (int i = 0; i < chain.Count - 1; i++)
                {
                    //DependencyManager.Instance.pooler.Deactivate(chain[i].gameObject);
                    //chain[i].selected = false;

                    Destroy(chain[i].gameObject);
                }
                chain[chain.Count - 1].SetNum();
                chain[chain.Count - 1].selected = false; 
            }
            chain.Clear();
            DependencyManager.Instance.inputManager.released = false;

            Debug.Log("clear");
        }
    }
}
