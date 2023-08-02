using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private Rect            safeArea;
    private RectTransform   rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }

    private void Start()
    {
        GameSettings.SAFE_AREA_SIZE = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        GameSettings.GRID_SPACING   = rectTransform.rect.width * 0.05f;
        GameSettings.BLOCK_SIZE     = (rectTransform.rect.width - (GameSettings.GRID.x + 1) * GameSettings.GRID_SPACING) / GameSettings.GRID.x;

        float height = GameSettings.BLOCK_SIZE * GameSettings.GRID.y + (GameSettings.GRID.y + 1) * GameSettings.GRID_SPACING;
        GameSettings.GRID_SIZE = new Vector2(rectTransform.rect.width, height);
    }
}