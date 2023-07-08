using UnityEngine;
using TMPro;

public class GemsManager : MonoBehaviour
{
    public int gems { get; private set; } = 0;
    public TextMeshProUGUI text;

    private void Start()
    {
        GameData gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            gems = gameData.Gems;
        }
        text.text = gems.ToString();
    }
    public void AddGems(int amount)
    {
        gems += amount;
        text.text = gems.ToString();
    }
    public void RemoveGems(int amount)
    {
        if (gems > 0)
        {
            gems -= amount;
        }
        else
        {
            Debug.Log("Not enough gems");
        }
        text.text = gems.ToString();
    }
}
