using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    public void PanelOpenAnimation(GameObject objectToMove)
    {
        RectTransform rect = objectToMove.GetComponent<RectTransform>();
        DOTweenModuleUI.DOAnchorPosX(rect, 0, 1);
    }
}
