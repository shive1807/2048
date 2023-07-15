using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private Rect _safeArea;
    private RectTransform _rectTransaform;
    private void Awake()
    {
        _rectTransaform = GetComponent<RectTransform>();
        _safeArea = Screen.safeArea;
        Vector2 anchorMin = _safeArea.position;
        Vector2 anchorMax = _safeArea.position + _safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransaform.anchorMin = anchorMin;
        _rectTransaform.anchorMax = anchorMax;
    }
}