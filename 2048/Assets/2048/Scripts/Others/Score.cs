using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI name;
    public Image image;
    public TextMeshProUGUI MaxBlockVal;
    public void SetName(string _name)
    {
        name.text = _name;
    }
    public void SetMaxBlock(Element MaxElement)
    {
        image.color = MaxElement.image.color;
        MaxBlockVal.text = MaxElement.num.txt;
    }
}
