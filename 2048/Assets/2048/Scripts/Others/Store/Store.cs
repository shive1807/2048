using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    private Button StoreItemButton_1;
    private Button StoreItemButton_3;
    private Button StoreItemButton_4;
    private Button StoreItemButton_5;
    private Button StoreItemButton_6;

    private void Start() => StoreButtonsLogic();

    private void StoreButtonsLogic()
    {
        Transform items = transform.GetChild(1);

        StoreItemButton_1 = items.GetChild(0).GetComponent<Button>();
        StoreItemButton_3 = items.GetChild(2).GetComponent<Button>();
        StoreItemButton_4 = items.GetChild(3).GetComponent<Button>();
        StoreItemButton_5 = items.GetChild(4).GetComponent<Button>();
        StoreItemButton_6 = items.GetChild(5).GetComponent<Button>();

        StoreItemButton_1.onClick.AddListener(() =>
        {
            //IAP.Instance.BuyConsumableRemove_Ads();
            ButtonClickSFX();
        });
        StoreItemButton_3.onClick.AddListener(() =>
        {
            //IAP.Instance.Buy50gems();
            ButtonClickSFX();
        });
        StoreItemButton_4.onClick.AddListener(() =>
        {
            //IAP.Instance.Buy100Coins();
            ButtonClickSFX();
        });
        StoreItemButton_5.onClick.AddListener(() =>
        {
            //IAP.Instance.Buy300Coins();
            ButtonClickSFX();
        });
        StoreItemButton_6.onClick.AddListener(() =>
        {
            //IAP.Instance.Buy1000Coins();
            ButtonClickSFX();
        });
    }

    /// Supporting functions
    
    void ButtonClickSFX()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
    }
}
