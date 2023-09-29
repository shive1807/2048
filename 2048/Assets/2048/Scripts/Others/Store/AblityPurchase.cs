using UnityEngine;
using TMPro;

public class AbilityPurchase : MonoBehaviour
{
    // Hidden public variables
    [HideInInspector] public int amount = 0;
    [HideInInspector] public bool swapSelect = false;
    [HideInInspector] public bool smashSelect = false;

    // public variables
    public int price = 1;
    public TextMeshProUGUI infoTxt;
    public TextMeshProUGUI amountTxt;
    public TextMeshProUGUI swapAbilityCount;
    public TextMeshProUGUI smashAbilityCount;
    public string infoTextAmount0 = "Please buy atleast 1";
    public string infoTextNoSelection = "Please Select the Ability to buy";

    public void Buy()
    {
        if (amount > 0)
        {
            if(swapSelect)
            {
                UpdateSwapAbilityCount();
                GemsManager.Instance.RemoveGems(amount * price);
                this.gameObject.SetActive(false);
                reset();
                //UiManager.Instance.SetActive(gameObject.GetComponent<RectTransform>(), false);
            }
            else if(smashSelect)
            {
                UpdateSmashAbilityCount();
                GemsManager.Instance.RemoveGems(amount * price);
                this.gameObject.SetActive(false);
                reset();
                //UiManager.Instance.SetActive(gameObject.GetComponent<RectTransform>(), false);
            }
            else
            {
                infoTxt.text = infoTextNoSelection;
            }
        }
        else
        {
            infoTxt.text = infoTextAmount0;
        }
    }

    public void UpdateQuantTxt()
    {
        amountTxt.text = amount.ToString();
    }

    public void UpdateSwapAbilityCount()
    {
        int swapCount = DependencyManager.Instance.gameController.swapAbilityCount;

        swapCount += amount;
        DependencyManager.Instance.gameController.swapAbilityCount = swapCount;
        swapAbilityCount.text = swapCount.ToString();
        SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, -1, default, -1, -1, null, swapCount, -1);
    }

    public void UpdateSmashAbilityCount()
    {
        int smashCount = DependencyManager.Instance.gameController.smashAbilityCount;

        smashCount += amount;
        DependencyManager.Instance.gameController.smashAbilityCount = smashCount;
        smashAbilityCount.text = smashCount.ToString();
        SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, -1, default, -1, -1, null, -1, smashCount);
    }

    private void reset()
    {
        amount = 0;
        UpdateQuantTxt();
        swapSelect = false;
        smashSelect = false;
        infoTxt.text = "";
    }
}
