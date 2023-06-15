using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private LineRenderer lineRenderer;
    private GameObject line;
    private List<GameObject> lines;

    [HideInInspector] public bool swaping = false;
    private float swapDuration = 1f;
    [HideInInspector] public bool smashing = false;

    public List<Element> chain;
    private void Start()
    {
        chain = new List<Element>();
        lineRenderer= GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        line = Resources.Load<GameObject>(Assets.line);
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
                int x = chain[chain.Count - 1].x;
                int y = chain[chain.Count - 1].y;

                if (numElement.x == x - 1 || numElement.x == x + 1 || numElement.x == x)
                {
                    if (numElement.y == y - 1 || numElement.y == y + 1 || numElement.y == y)
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
            //lineRenderer.positionCount = chain.Count + 2;

            //for (int i = 0; i < chain.Count; i++)
            //{
            //    lineRenderer.SetPosition(i, chain[i].elementPos);
            //}

            //// Set the position of the end of the line to follow the mouse cursor


            Vector3 mousePos = DependencyManager.Instance.inputManager.mousePos;
            lineRenderer.SetPosition(0, chain[chain.Count - 1].elementPos);
            lineRenderer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 90));
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    public void SwapBlock(Element e1, Element e2)
    {
        Vector2 e1Pos = e1.GetComponent<RectTransform>().anchoredPosition;
        Vector2 e2Pos = e2.GetComponent<RectTransform>().anchoredPosition;
        StartCoroutine(e1.MoveElement(e2Pos, swapDuration));
        StartCoroutine(e2.MoveElement(e1Pos, swapDuration));
        swaping = false;
    }
    public void SmashBlock(Element e)
    {
        chain.Add(e);
        DependencyManager.Instance.gridController.GridRefill(chain);
        smashing = false;
    }
    public void CreateLine(Element e1, Element e2)
    {
        GameObject _line = Instantiate(line) as GameObject;
        _line.transform.SetParent(e1.transform.parent);
        _line.transform.SetParent(e1.transform.parent.transform.Find("Lines"), false);  // for keeping the lines below the Numelements

        // Calculate the position
        Vector2 pos = (e1.GetComponent<RectTransform>().anchoredPosition + e2.GetComponent<RectTransform>().anchoredPosition) / 2f;
        _line.GetComponent<RectTransform>().anchoredPosition = pos;

        // Calculate the rotation
        Vector2 dir = e1.GetComponent<RectTransform>().anchoredPosition - e2.GetComponent<RectTransform>().anchoredPosition;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _line.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, angle);
        //lines.Add(_line);
    }
    public int ChangeNum()
    {
        int sum = 0;
        int s = 2;
        foreach(Element element in chain)
        {
            sum += element.num;
        }

        while (s < sum)
        {
            s *= 2;
        }
        return s;
    }
    private void RemoveFromChain(Element numElement)  // removing the last element from chain
    {
        chain[chain.Count - 1].selected = false;
        chain.Remove(chain[chain.Count - 1]);
    }
    private void AddToChain(Element numElement)       // adding element to chain
    {
        chain.Add(numElement);
        numElement.selected = true;
        if(chain.Count > 1)
        {
            CreateLine(chain[chain.Count - 2], numElement);
        }

        Debug.Log("(" + numElement.x + "," + numElement.y + ") Added");
    }   
    private void ClearChain()
    {
        if (DependencyManager.Instance.inputManager.released && chain.Count > 0)  // resetting the variables on mouse release
        {
            if(chain.Count > 1)
            {
                chain[chain.Count - 1].SetNum(ChangeNum());

                //lines.Clear();
            }
            DependencyManager.Instance.gridController.GridRefill(chain);

            chain[chain.Count - 1].selected = false;
            chain.Clear();
            DependencyManager.Instance.inputManager.released = false;
        }
    }
}
