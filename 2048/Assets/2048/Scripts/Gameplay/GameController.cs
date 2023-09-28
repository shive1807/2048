using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class GameController : MonoBehaviour
{
    // Chain variables
    private List<Element> chain;
    private bool upChain = false;
    private bool downChain = false;

    // Connecting line variables
    private LineRenderer lineRenderer;
    private GameObject line;
    private List<GameObject> lines;
    public Material lineColor;

    // Swap Abillity 
    public List<Element> swapElements;
    [HideInInspector] public bool swaping = false;
    public float swapSpeed = 1f;
    private int swapAbilityCount = 0;

    // Smash Ability
    [HideInInspector] public bool smashing = false;
    private int smashAbilityCount = 0;

    // Max Element and HighScore Check
    [HideInInspector] public Num maxElement = new Num() { numVal = 2, dec = ' ', txt = $"{2}" };
    [HideInInspector] public Num minElement = new Num() { numVal = 2, dec = ' ', txt = $"{2}" };

    [HideInInspector] public int maxPower = 0;
    [HideInInspector] public double HighScore = 0;
    private double tempHighScore = 0;
    public TextMeshProUGUI highScoreTxt;

    // Ability refernces
    [SerializeField] RectTransform hammer;
    [SerializeField] float smashTime;
    [SerializeField] GameObject smashImg;

    [HideInInspector] public GraphicRaycaster raycaster;

    private void Start()
    {
        chain = new List<Element>();
        swapElements = new List<Element>();

        lines = new List<GameObject>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        line = Resources.Load<GameObject>(Assets.line);

        raycaster = transform.parent.root.gameObject.GetComponent<GraphicRaycaster>();
        Debug.Log(minElement.txt);

        HighScore = GameManager.Instance.gameData.HighScore;

        swapAbilityCount = GameManager.Instance.gameData.SwapAbilityCount;
        smashAbilityCount = GameManager.Instance.gameData.SmashAbilityCount;
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
                        if (chain.Count == 1)
                        {
                            if (numElement.numVal == chain[chain.Count - 1].numVal)
                            {
                                AddToChain(numElement);
                            }
                        }
                        else if (chain.Count > 1 && !upChain && !downChain)
                        {
                            ChainCheck(numElement);
                        }
                        else if (downChain)
                        {
                            if (numElement.numVal == chain[chain.Count - 1].numVal || chain[chain.Count - 1].numVal / numElement.numVal == 2
                                 || (numElement.numVal == 512 && chain[chain.Count - 1].numVal == 1))
                            {
                                AddToChain(numElement);
                            }
                        }
                        else if (upChain)
                        {
                            if (numElement.numVal == chain[chain.Count - 1].numVal || numElement.numVal / chain[chain.Count - 1].numVal == 2
                                 || (numElement.numVal == 1 && chain[chain.Count - 1].numVal == 512))
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
            if (chain.Count > 1)
            {
                if (numElement == chain[chain.Count - 2])    // so that if there's only one element and if checking second last element because the current element
                {                                                               // is the element on which cursor is currently on
                    RemoveFromChain(numElement);
                }
            }
        }
    }

    private void ChainCheck(Element numElement)
    {
        if (numElement.numVal / chain[chain.Count - 1].numVal == 2 || (numElement.numVal == 512 && chain[chain.Count - 1].numVal == 1))    // special case: when the decimal changes it wasn't adding to chain
        {
            upChain = true;
            downChain = false;
            AddToChain(numElement);
        }
        else if (chain[chain.Count - 1].numVal / numElement.numVal == 2 || (numElement.numVal == 512 && chain[chain.Count - 1].numVal == 1))
        {
            downChain = true;
            upChain = false;
            AddToChain(numElement);
        }
        else if (numElement.numVal == chain[chain.Count - 1].numVal)
        {
            AddToChain(numElement);
        }
    }

    private void LineMatching()
    {
        if (smashing || swaping || chain.Count == 0)
        {
            lineRenderer.enabled = false;
            return;
        }
        if (DependencyManager.Instance.inputManager.pressed)
        {
            lineRenderer.enabled = true;

            // Setting position
            Vector3 mousePos = DependencyManager.Instance.inputManager.mousePos;

            lineRenderer.SetPosition(0, chain[chain.Count - 1].elementPos);
            lineRenderer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 90));

            // Setting Color
            lineColor.color = chain[chain.Count - 1].image.color; 
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void HighScoreUpdate(Num num)
    {
        int val = (int)(num.numVal * Mathf.Pow(1000, Num.CurrentDec(num)));
        tempHighScore += val;
        highScoreTxt.text = tempHighScore.ToString();

        if(HighScore < tempHighScore)
        {
            HighScore = tempHighScore;
            SaveSystem.SaveGame(-1, false, null, HighScore);
        }
    }

    public IEnumerator MaxElementCheck()
    {
        yield return new WaitForSeconds(2f);

        Num num = DependencyManager.Instance.gridController.GetMaxElement();

        if (Num.Max(maxElement, num) == maxElement)
        {
            yield break;
        }

        maxElement = Num.Max(maxElement, num);

        DependencyManager.Instance.gridController.ElementReShuffle(maxElement, minElement);
    }

    public void BlockInput(bool block)
    {
        if (block)
        {
            raycaster.enabled = false;
        }
        else
        {
            raycaster.enabled = true;
        }
    }

    public IEnumerator SwapBlock(Element e)
    {
        swapElements.Add(e);

        // check for ability count
        if(GameManager.Instance.gameData.SwapAbilityCount != 0)
        {
            if (swapElements.Count == 1)
            {
                Debug.Log("only 1 element");
                yield return null;
            }
            else if (swapElements.Count == 2)
            {

                Element e1 = swapElements[0];
                Element e2 = swapElements[1];

                Debug.Log(e1.moving);


                Vector2 e1Pos = e1.GetComponent<RectTransform>().anchoredPosition;
                Vector2 e2Pos = e2.GetComponent<RectTransform>().anchoredPosition;

                StartCoroutine(e1.MoveElement(e2Pos, DependencyManager.Instance.gridController.ElementFallSpeed));
                StartCoroutine(e2.MoveElement(e1Pos, DependencyManager.Instance.gridController.ElementFallSpeed));

                float duration = Vector2.Distance(e1Pos, e2Pos) / DependencyManager.Instance.gridController.ElementFallSpeed;

                yield return new WaitForSeconds(duration + .1f);

                e1.GetComponent<RectTransform>().anchoredPosition = e1Pos;
                e2.GetComponent<RectTransform>().anchoredPosition = e2Pos;

                // swaping Num
                Num _temp;
                _temp = e1.num;
                e1.num = e2.num;
                e2.num = _temp;
                e1.SetNum(0, e1.num);
                e2.SetNum(0, e2.num);

                swapElements.Clear();
                swaping = false;
                swapAbilityCount--;
                SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, -1, default, -1, -1, null, swapAbilityCount, -1);
            }
        }
        else
        {
            // show some message
        }
    }
    public IEnumerator SmashBlock(Element e)
    {
        // check for ability count
        if (smashAbilityCount != 0)
        {
            //hammer animation
            hammer.gameObject.SetActive(true);

            hammer.DOMove(e.gameObject.transform.position, smashTime);
            yield return new WaitForSeconds(smashTime);

            hammer.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 60), .2f);
            yield return new WaitForSeconds(.2f);

            smashImg.gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);

            smashImg.gameObject.SetActive(false);
            yield return new WaitForSeconds(.1f);

            hammer.gameObject.SetActive(false);

            // vibration
            if (VibrationManager.Instance != null)
                VibrationManager.Instance.Vibrate(5);

            //destroying block
            chain.Add(e);
            StartCoroutine(DependencyManager.Instance.gridController.GridRefill(chain));
            chain.Clear();
            smashAbilityCount--;
            SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, -1, default, -1, -1, null, -1, smashAbilityCount);
            smashing = false;
        }
        else
        {
            // show some message
        }
    }
    public void SetSmash()
    {
        smashing = true;
    }
    public void SetSwap()
    {
        swaping = true;


        // Vibration
        if(VibrationManager.Instance != null)
            VibrationManager.Instance.Vibrate(5);
    }

    private void CreateLine(Element e1, Element e2)
    {
        GameObject _line = Instantiate(line) as GameObject;
        _line.transform.SetParent(e1.transform.parent);
        _line.transform.SetParent(e1.transform.parent.transform.Find("Lines"), false);  // for keeping the lines below the Numelements

        // Calculate the position
        Debug.Log(e1.GetComponent<RectTransform>().anchoredPosition + " e1 and e2 "+ e2.GetComponent<RectTransform>().anchoredPosition);
        Vector2 pos = (e1.GetComponent<RectTransform>().anchoredPosition + e2.GetComponent<RectTransform>().anchoredPosition) / 2f;
        _line.GetComponent<RectTransform>().anchoredPosition = pos;

        // Calculate the rotation
        Vector2 dir = e1.GetComponent<RectTransform>().anchoredPosition - e2.GetComponent<RectTransform>().anchoredPosition;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _line.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, angle);

        // Setting color of line renderer
        _line.GetComponent<Image>().color = e1.image.color;

        lines.Add(_line);

    }
    private void DestroyLine(GameObject _line = default)
    {
        if (_line != null)
        {
            lines.Remove(_line);

            Destroy(_line.gameObject);
        }
        else
        {
            foreach (GameObject line in lines)
            {
                Destroy(line);
            }
            lines.Clear();
        }
        
    }

    private Num ChangeNum()
    {
        Num num = Num.AddElement(chain);
        return num;
    }

    private void RemoveFromChain(Element numElement)  // removing the last element from chain
    {
        // removing from chain
        chain[chain.Count - 1].selected = false;
        chain.Remove(chain[chain.Count - 1]);
        DestroyLine(lines[lines.Count - 1]);

        // animation
        chain[chain.Count - 1].rectTransform.localScale = new Vector2(.8f, .8f);
        chain[chain.Count - 1].rectTransform.DOScale(1.2f, .5f).SetEase(Ease.OutBounce);

        // vibration
        if(VibrationManager.Instance != null)
            VibrationManager.Instance.Vibrate(5);

        //SFX
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("Wrong");
    }
    private void AddToChain(Element numElement)       // adding element to chain
    {
        //adding to chain
        chain.Add(numElement);
        numElement.selected = true;
        if(chain.Count > 1)
        {
            CreateLine(chain[chain.Count - 2], numElement);
        }

        // animation
        numElement.rectTransform.localScale = new Vector2(.8f, .8f);
        numElement.rectTransform.DOScale(1.2f, .5f).SetEase(Ease.OutBounce);

        // vibration
        if(VibrationManager.Instance != null)
            VibrationManager.Instance.Vibrate(5);

        // SFX
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("CorrectMatch");
    }

    private void ClearChain()
    {
        if (DependencyManager.Instance.inputManager.released && chain.Count > 0 && !smashing && !swaping)  // resetting the variables on mouse release
        {
            if(chain.Count > 1)
            {
                Num num = ChangeNum();
                chain[chain.Count - 1].SetNum(0, num);
                HighScoreUpdate(num);
                DestroyLine();
            }
            StartCoroutine(DependencyManager.Instance.gridController.GridRefill(chain));

            upChain = false;
            downChain = false;

            chain[chain.Count - 1].selected = false;
            chain.Clear();
            DependencyManager.Instance.inputManager.released = false;
        }
    }
}
