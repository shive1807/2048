using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using System.Xml.Linq;

[System.Serializable]

public class Element : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    System.Random random = new System.Random();

    [HideInInspector] public int num;
    [HideInInspector] public bool selected = false;
    [HideInInspector] public int rowIndex;
    [HideInInspector] public int colIndex;

    public RectTransform rectTransform;
    [HideInInspector] public Vector3 elementPos;

    private void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
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
        if (DependencyManager.Instance.inputManager.pressed)  // to avoid the un-wanted calls if the mouse button isn't pressed and the cursor is hovering over the buttons
        {
            SelectCheck(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData) // to get the element when the mouse button gets pressed while on the element
    {
        SelectCheck(eventData);
    }

    public void SelectCheck(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag(this.tag)) // so that it only gets called when the cursor is on the element button nor on any button
        {
            GameController.Instance.Chaining(this);
        }
    }
    
    public void ElementSetup(int i, int j, GameObject element, Vector2 targetPos, Vector2 elementMoveOffset = default, float elementMoveDuration = default)
    {
        RectTransform rectTransform = element.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = new Vector2(targetPos.x + elementMoveOffset.x, targetPos.y + elementMoveOffset.y);  // spawning the element a bit up to make room for drop animatio
        StartCoroutine(MoveElement(rectTransform, targetPos, elementMoveDuration));  // moving the element down (drop animation on spawn)
        
        element.name = "( " + (i + 1) + ", " + (j + 1) + ")";  // naming according to in-matrix position

        Element _element = element.GetComponent<Element>();
        _element.rowIndex = i + 1;   // to get in-matrix position of element
        _element.colIndex = j + 1;

        _element.SetNum();
    }

    public IEnumerator MoveElement(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        Vector2 initialPos = rectTransform.anchoredPosition;
        float elapsedTime = 0f; 

        while (elapsedTime < duration)               // moving the element down gradually
        {
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;  // for ensuring the final position of element is correctly set
    }
    public void SetNum(int num = 0)
    {
        int x = 0;
        if(num == 0)
        {
            x = (int)Mathf.Pow(2, random.Next(1, 11));
        }
        else
        {
            x = num;
        }
        this.num = x;
        this.GetComponentInChildren<TextMeshProUGUI>().text = this.num.ToString();
    }
}
