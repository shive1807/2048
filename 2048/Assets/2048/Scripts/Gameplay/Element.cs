using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Element : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    System.Random random = new System.Random();

    [HideInInspector] public int numVal;
    [HideInInspector] public Num num;

    [HideInInspector] public bool selected = false;
    [HideInInspector] public int y;
    [HideInInspector] public int x;
    [HideInInspector] public bool onElement = false;

    [HideInInspector] public Vector3 elementPos;
    [HideInInspector] public RectTransform rectTransform;
    private Element element;

    private void Start()
    {
        element         = this.gameObject.GetComponent<Element>();
        rectTransform   = this.gameObject.GetComponent<RectTransform>();
    }
    private void Update()
    {
        // Example anchored position
        Vector2 anchoredPosition = new Vector2(0, 0);

        // Convert anchored position to transform position
        elementPos = rectTransform.TransformPoint(anchoredPosition);
    }
    public void OnPointerEnter(PointerEventData eventData)  // to get element when mouse button is already pressed and being dragged on the other
    {
        if (DependencyManager.Instance.inputManager.pressed && !DependencyManager.Instance.gameController.smashing && !DependencyManager.Instance.gameController.swaping)  // to avoid the un-wanted calls if the mouse button isn't pressed and the cursor is hovering over the buttons
        {
            onElement = true;
            SelectCheck(eventData);
            //Debug.Log("run drag");
        }
    }

    public void OnPointerDown(PointerEventData eventData) // to get the element when the mouse button gets pressed while on the element
    {
        onElement = true;
        SelectCheck(eventData);
        //Debug.Log("run down");
    }
    private void SelectCheck(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag(this.tag)) // so that it only gets called when the cursor is on the element button nor on any button
        {
            if (DependencyManager.Instance.gameController.smashing)
            {
                DependencyManager.Instance.gameController.SmashBlock(this);
                //Debug.Log("smashing run");
            }
            else if (DependencyManager.Instance.gameController.swaping)
            {
                StartCoroutine(DependencyManager.Instance.gameController.SwapBlock(this));
                //Debug.Log("swaping run");
            }
            else
            {
                GameController.Instance.Chaining(this);
            }
        }
    }
    public void ElementSetup(int i, int j, Vector2 elementMoveOffset = default, float elementMoveDuration = default)
    {
        if (element == null || rectTransform == null)
            Start();

        Vector2 targetPos = new Vector2((i * GameSettings.SPACING) - 370, (j * GameSettings.SPACING) - 520);  // calculating Pos with respect to anchors

        rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(targetPos.x + elementMoveOffset.x, targetPos.y + elementMoveOffset.y);  // spawning the element a bit up to make room for drop animatio
        SetElementCoord(i, j);  // naming according to in-matrix position

        //TO GET IN MATRIX POSTION OF THE ELEMENTS.
        
        element.SetNum();

        StartCoroutine(MoveElement(targetPos, elementMoveDuration));  // moving the element down (drop animation on spawn)
    }
    public void SetElementCoord(int i, int j)
    {
        element.x = i;
        element.y = j;
        transform.name = "(" + i + ", " + j + ")";
    }
    public IEnumerator MoveElement(Vector2 targetPos, float duration)
    {
        yield return null;

        if(rectTransform == null)
        {
            //Debug.LogError("  rectTransform is null");
            yield break;
        }

        DependencyManager.Instance.gameController.raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;

        Vector2 initialPos  = rectTransform.anchoredPosition;
        float elapsedTime   = 0f;
        //Debug.Log("moving");
        while (elapsedTime < duration)               // moving the element down gradually
        {
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;  // for ensuring the final position of element is correctly set

        DependencyManager.Instance.gameController.raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
    }
    public void SetNum(int val = default, Num _num = null)
    {
        if (val == 0 && _num == null)
        {
           Num.NumSetup(ref num);
        }
        else if(val != 0)
        {
            Num.NumSetup(ref num, val);
        }
        else if(_num != null)
        {
            this.num = _num;
        }
        this.numVal = num.numVal;

        this.GetComponentInChildren<TextMeshProUGUI>().text = num.txt;
    }
}
