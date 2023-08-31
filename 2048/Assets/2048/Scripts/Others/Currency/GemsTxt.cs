using UnityEngine;
using TMPro;

public class GemsTxt : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        GemsManager.Instance.updateGemsTxt += SetText;
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
