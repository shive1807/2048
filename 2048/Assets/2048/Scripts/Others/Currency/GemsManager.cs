using UnityEngine;
using TMPro;

public class GemsManager : Singleton<GemsManager>
{
    public int gems = 0;

    private void Start()
    {
        if (GameManager.Instance.gameData != null)
        {
            gems = GameManager.Instance.gameData.Gems;
        }
        //GemsTxt.instance.SetText(gems);
    }
    public void AddGems(int amount)
    {
        gems += amount;
        //GemsTxt.instance.SetText(gems);
        SaveSystem.SaveGame(gems);

        DataManager.Gems = gems;
    }
    public void RemoveGems(int amount)
    {
        if (gems > 0)
        {
            gems -= amount;
            //GemsTxt.instance.SetText(gems);
            SaveSystem.SaveGame(gems);

            DataManager.Gems = gems;
        }
        else
        {
            Debug.Log("Not enough gems");
        }
    }
}
