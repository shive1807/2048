using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using static UnityEditor.PlayerSettings;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ElementController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    System.Random random = new System.Random();

    [HideInInspector] public bool selected = false;
    [HideInInspector] public int rowIndex;
    [HideInInspector] public int colIndex;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InputManager.pressed)
        {
            selectCheck(eventData);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        selectCheck(eventData);
    }
    public void selectCheck(PointerEventData eventData)
    {
        Debug.Log("selectCheck " + gameController.xyz);

        if (eventData.pointerCurrentRaycast.gameObject.CompareTag(this.tag))
        {
            gameController.chaining(this);

        }
    }
    
    public void elementSetup(int i, int j, GameObject element, Vector2 targetPos, Vector2 elementMoveOffset = default, float elementMoveDuration = default)
    {
        RectTransform rectTransform = element.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = new Vector2(targetPos.x + elementMoveOffset.x, targetPos.y + elementMoveOffset.y); ;
        StartCoroutine(MoveElement(rectTransform, targetPos, elementMoveDuration));
        
        element.name = "( " + (i + 1) + ", " + (j + 1) + ")";
        element.GetComponent<ElementController>().rowIndex = i + 1;   // to get in-matrix position of element
        element.GetComponent<ElementController>().colIndex = j + 1;

        element.GetComponentInChildren<TextMeshProUGUI>().text = numGenerator().ToString();
    }
    IEnumerator MoveElement(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        Vector2 initialPos = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }
    public int numGenerator()
    {
        int x = random.Next(1, 11);
        return (int)Mathf.Pow(2, x);
    }
}
