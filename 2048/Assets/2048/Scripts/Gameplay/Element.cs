using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    [HideInInspector] public bool moving = false;

    public RectTransform SpawnLinePos;
    private void Start()
    {
        rectTransform   = this.gameObject.GetComponent<RectTransform>();
        image           = this.gameObject.GetComponent<Image>();
        breakEffect     = this.gameObject.GetComponent<ParticleSystem>();

        SpawnLinePos = transform.parent.parent.GetChild(0).GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(GameSettings.BLOCK_SIZE, GameSettings.BLOCK_SIZE);
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

    public void ElementSetup(int i, int j, Vector2 elementMoveOffset = default, float elementMoveSpeed = default, Num num = default)
    {
        if ( rectTransform == null)//element == null ||
            Start();


        Vector2 targetPos = new Vector2(-GameSettings.GRID_SIZE.x / 2 + (2 * i + 1) * GameSettings.BLOCK_SIZE/2 + (i + 1) * GameSettings.GRID_SPACING,
                                        -GameSettings.GRID_SIZE.y / 2 + (2 * j + 1) * GameSettings.BLOCK_SIZE /2 + (j + 1) * GameSettings.GRID_SPACING);

        // spawning the element a bit up to make room for drop animation
        rectTransform.anchoredPosition = new Vector2(targetPos.x, 1375) + elementMoveOffset;
        
        // naming according to in-matrix position
        SetElementCoord(i, j);  

        //TO GET IN MATRIX POSTION OF THE ELEMENTS.
        
        this.SetNum(0, num);

        // moving the element down (drop animation on spawn)
        StartCoroutine(MoveElement(targetPos, elementMoveSpeed));  
    }

    public void SetElementCoord(int i, int j)
    {
        this.x = i;
        this.y = j;
        transform.name = "(" + i + ", " + j + ")";
    }

    public IEnumerator MoveElement(Vector2 targetPos, float speed = 1000)
    {
        yield return null;

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is null");
            yield break;
        }
        moving = true;

        Vector2 initialPos = rectTransform.anchoredPosition;

        float distance = Vector2.Distance(initialPos, targetPos);
        float duration = distance / speed;

        float elapsedTime = 0f;
        while (elapsedTime < duration) // moving the element down gradually
        {
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos; // Ensure the final position of the element is correctly set
        moving = false;
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
        transform.GetComponent<Image>().color = DataManager.GetColor(num);
    }
}
