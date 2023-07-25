using UnityEngine;
using TMPro;

public class GemsTxt : MonoBehaviour
{
    public static GemsTxt instance;
    public TextMeshProUGUI text;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetText(GemsManager.Instance.gems);
    }
    public void SetText(int gems)
    {
        text.text = gems.ToString();
    }
}
