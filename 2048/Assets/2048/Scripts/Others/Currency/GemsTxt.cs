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
    private void OnEnable()
    {
        text.text = GemsManager.Instance.gems.ToString();
    }
    private void Start()
    {
        text.text = GemsManager.Instance.gems.ToString();
    }
    public void SetText(int gems)
    {
        text.text = gems.ToString();
    }
}
