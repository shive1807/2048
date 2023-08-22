using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public enum Panel
{
    LeaderBoard,
    Store
}

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public GameObject LeaderBoard;
    public GameObject Store;

    protected void Awake()
    {
        Instance = this;
        LeaderBoard = transform.GetChild(1).gameObject;
        Store       = transform.GetChild(2).gameObject;
    }

    bool rolledOut = false;
    public GameObject[] Settings;

    public void PanelOpenAnimation(Panel panel)
    {
        GameObject panelObject  = GetPanel(panel);
        panelObject.SetActive(true);
        RectTransform rect      = panelObject.GetComponent<RectTransform>();
        rect.DOAnchorPos(new Vector2(0, 0), .4f, false).SetEase(Ease.OutBack);
    }
    private GameObject GetPanel(Panel panel)
    {
        switch (panel)
        {
            case Panel.LeaderBoard:
                return LeaderBoard;
            default:
                return Store;
        }
    }
    public void PanelCloseAnimation(Panel panel )
    {
        GameObject objectToMove = GetPanel(panel);
        objectToMove.SetActive(false);
        RectTransform rect = objectToMove.GetComponent<RectTransform>();
        DOTweenModuleUI.DOAnchorPosX(rect, 1000, .1f);
    }
    public void ToggleRoll(RectTransform pos)
    {
        if (rolledOut)
        {
            RollIn(Settings, pos);
            rolledOut = false;
        }
        else
        {
            RollOut(Settings, pos);
            rolledOut = true;
        }
    }
    void RollOut(GameObject[] settings, RectTransform initialPos)
    {
        for (int i = 0; i < settings.Length; i++)
        {
            RectTransform pos = settings[i].GetComponent<RectTransform>();

            Vector2 targetPosition = initialPos.anchoredPosition - Vector2.right * (i + 1) * 250;
            pos.DOAnchorPos(targetPosition, .3f);

            pos.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        }
    }
    public void RollIn(GameObject[] settings, RectTransform initialPos)
    {
        for (int i = 0; i < settings.Length; i++)
        {
            RectTransform pos = settings[i].GetComponent<RectTransform>();

            pos.DOScale(0, 0.5f).SetEase(Ease.OutBounce);

            pos.DOAnchorPos(initialPos.anchoredPosition, .3f);
        }
    }
    public void SetActive(RectTransform obj)
    {
        if (obj.gameObject.activeSelf)
        {
            obj.DOScale(Vector2.zero, .5f).SetEase(Ease.OutBounce);
            obj.gameObject.SetActive(false);
        }
        else
        {
            obj.gameObject.SetActive(true);
            obj.DOScale(new Vector2(1, 1), .5f).SetEase(Ease.OutBounce);
        }
    }
}
