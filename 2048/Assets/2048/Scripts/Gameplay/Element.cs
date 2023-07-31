using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Transform))]
public class Element : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [HideInInspector] public int numVal;
    [HideInInspector] public Num num;
    [HideInInspector] public Image image;

    [HideInInspector] public bool selected = false;
    [HideInInspector] public int y;
    [HideInInspector] public int x;

    [HideInInspector] public Vector3 elementPos;
    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public ParticleSystem breakEffect;
    private void Start()
    {
        rectTransform   = this.gameObject.GetComponent<RectTransform>();
        image           = this.gameObject.GetComponent<Image>();
        breakEffect     = this.gameObject.GetComponent<ParticleSystem>();
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
        }
    }
    public void OnPointerDown(PointerEventData eventData) // to get the element when the mouse button gets pressed while on the element
    {
        SelectCheck(eventData);
    }
    private void SelectCheck(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag(this.tag)) // so that it only gets called when the cursor is on the element button nor on any button
        {
            if (DependencyManager.Instance.gameController.smashing)
            {
                StartCoroutine(DependencyManager.Instance.gameController.SmashBlock(this));
            }
            else if (DependencyManager.Instance.gameController.swaping)
            {
                StartCoroutine(DependencyManager.Instance.gameController.SwapBlock(this));
            }
            else
            {
                DependencyManager.Instance.gameController.Chaining(this);
            }
        }
    }
    public void ElementSetup(int i, int j, Vector2 elementMoveOffset = default, float elementMoveDuration = default, Num num = default)
    {
        if ( rectTransform == null)//element == null ||
            Start();

        Vector2 targetPos = new Vector2((i * GameSettings.SPACING) - 370, (j * GameSettings.SPACING) - 520);  // calculating Pos with respect to anchors

        rectTransform.anchoredPosition = new Vector2(targetPos.x + elementMoveOffset.x, targetPos.y + elementMoveOffset.y);  // spawning the element a bit up to make room for drop animatio
        SetElementCoord(i, j);  // naming according to in-matrix position

        //TO GET IN MATRIX POSTION OF THE ELEMENTS.
        
        this.SetNum(0, num);

        StartCoroutine(MoveElement(targetPos, elementMoveDuration));  // moving the element down (drop animation on spawn)
    }
    public void SetElementCoord(int i, int j)
    {
        this.x = i;
        this.y = j;
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

        DependencyManager.Instance.gameController.BlockRaycast(true);

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

        DependencyManager.Instance.gameController.BlockRaycast(false);
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
        image.color = Num.BlockColor(ref this.num);
        this.GetComponentInChildren<TextMeshProUGUI>().text = num.txt;

        SetColor();
    }

    private void SetColor()
    {
        transform.GetComponent<Image>().color = DataManager.GetColor(num.numVal);
    }
}
