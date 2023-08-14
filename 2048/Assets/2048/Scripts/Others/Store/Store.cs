using UnityEngine;
using UnityEngine.Events;
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
            DependencyManager.Instance.iap.BuyConsumableRemove_Ads();
        });
        StoreItemButton_3.onClick.AddListener(() =>
        {
            DependencyManager.Instance.iap.Buy50gems();
        });
        StoreItemButton_4.onClick.AddListener(() =>
        {
            DependencyManager.Instance.iap.Buy100Coins();
        });
        StoreItemButton_5.onClick.AddListener(() =>
        {
            DependencyManager.Instance.iap.Buy300Coins();
        });
        StoreItemButton_6.onClick.AddListener(() =>
        {
            DependencyManager.Instance.iap.Buy1000Coins();
        });
    }
}
