using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class Element : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    System.Random random = new System.Random();

    [HideInInspector] public int num;
    [HideInInspector] public bool selected = false;
    [HideInInspector] public int y;
    [HideInInspector] public int x;

    [HideInInspector] public Vector3 elementPos;
    public RectTransform rectTransform;
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
            SelectCheck(eventData);
            Debug.Log("run drag");
        }
    }

    public void OnPointerDown(PointerEventData eventData) // to get the element when the mouse button gets pressed while on the element
    {
        SelectCheck(eventData);
        Debug.Log("run down");

    }

    public void SelectCheck(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag(this.tag)) // so that it only gets called when the cursor is on the element button nor on any button
        {
            if (DependencyManager.Instance.gameController.smashing)
            {
                DependencyManager.Instance.gameController.SmashBlock(this);
                Debug.Log("smashing run");
            }
            else if (DependencyManager.Instance.gameController.swaping)
            {
                DependencyManager.Instance.gameController.SwapBlock(this);
                Debug.Log("swaping run");
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
        transform.name = "("+ i + ", " + j + ")";  // naming according to in-matrix position
        
        //TO GET IN MATRIX POSTION OF THE ELEMENTS.
        element.x = i;
        element.y = j;
        element.SetNum();

        StartCoroutine(MoveElement(targetPos, elementMoveDuration));  // moving the element down (drop animation on spawn)
    }
    public IEnumerator MoveElement(Vector2 targetPos, float duration)
    {
        yield return null;

        if(rectTransform == null)
        {
            Debug.LogError("rectTransform is null");
            yield break;
        }

        Vector2 initialPos  = rectTransform.anchoredPosition;
        float elapsedTime   = 0f; 

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
        int x;
        if(num == 0)
        {
            x = (int)Mathf.Pow(2, random.Next(1, 1));
        }
        else
        {
            x = num;
        }
        this.num = x;
        this.GetComponentInChildren<TextMeshProUGUI>().text = this.num.ToString();
    }
}
