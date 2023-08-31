using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GemsManager : Singleton<GemsManager>
{
    public int gems = 0;

    [HideInInspector] public UnityAction<int> updateGemsTxt;

    private void Start()
    {
        if (GameManager.Instance.gameData != null)
        {
            gems = GameManager.Instance.gameData.Gems;
        }

        updateGemsTxt?.Invoke(gems);
    }
    public void AddGems(int amount)
    {
        gems += amount;

        updateGemsTxt?.Invoke(gems);

        SaveSystem.SaveGame(gems);

        DataManager.Gems = gems;
    }
    public void RemoveGems(int amount)
    {
        if (gems > 0)
        {
            gems -= amount;

            updateGemsTxt?.Invoke(gems);

            SaveSystem.SaveGame(gems);

            DataManager.Gems = gems;
        }
        else
        {
            Debug.Log("Not enough gems");
        }
    }
}
