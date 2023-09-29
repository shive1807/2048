using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Purchasing;

public class GamePlay : MonoBehaviour
{
    private GameObject ButtonsContainer;

    // Main Buttons
    private Button StoreButton;
    private Button PauseButton;
    private Button SmashButton;
    private Button SwapButton;

    // Store Buttons
    private Button PlusButton;
    private Button StoreBackButton;

    // Rank Buttons
    private Button RankButton;
    private Button RankBackButton;

    // Pause Menu Buttons
    private Button ResumeButton;
    private Button RestartButton;
    private Button HomeButton;

    // Ability Buy Buttons
    private Button SmashPlusButton;
    private Button SwapPlusButton;
    private Button SelectSwapButton;
    private Button SelectSmashButton;
    private Button IncQuantButton;
    private Button DecQuantButton;
    private Button AbilityBuyButton;

    private Button newBlockRewardClaimButton;
    private void Start()
    {
        ButtonsContainer = transform.GetChild(1).gameObject;
        
        ButtonsLogic();

        SetBottomButtonPanelSize();
    }

    private void SetBottomButtonPanelSize()
    {
        Debug.Log(ButtonsContainer.transform.GetChild(0).name);
        float height = GameSettings.GRID_SIZE.y;

        Vector2 safeArea = GameSettings.SAFE_AREA_SIZE;

        float panelHeight = (safeArea.y - height) / 2;

        float panelWidth = safeArea.x;

        ButtonsContainer.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta
            = new Vector2(panelWidth, panelHeight);

        ButtonsContainer.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta
            = new Vector2(panelWidth, panelHeight);
    }

    private void ButtonsLogic()
    {
        StoreButtonLogic();

        LeaderBoardButtonLogic();

        PauseButtonLogic();

        SmashButtonLogic();

        SwapButtonLogic();

        newBlockUnlockRewardClaimButtonLogic();

        AbilityBuyButtonLogic();
    }

    private void AbilityBuyButtonLogic()
    {
        Transform AbilityBuyPopup = transform.GetChild(4);

        SwapPlusButton = ButtonsContainer.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        SmashPlusButton = ButtonsContainer.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>();

        SelectSmashButton = AbilityBuyPopup.GetChild(2).GetComponent<Button>();
        SelectSwapButton = AbilityBuyPopup.GetChild(3).GetComponent<Button>();
        IncQuantButton = AbilityBuyPopup.GetChild(4).GetComponent<Button>();
        DecQuantButton = AbilityBuyPopup.GetChild(6).GetComponent<Button>();
        AbilityBuyButton = AbilityBuyPopup.GetChild(7).GetComponent<Button>();

        AbilityPurchase abilityPurchase = DependencyManager.Instance.abilityPurchase;

        UnityAction openBuyPopup = () =>
        {
            //UiManager.Instance.SetActive(abilityPurchase.gameObject.GetComponent<RectTransform>(), true);
            abilityPurchase.gameObject.SetActive(true);
        };

        SwapPlusButton.onClick.AddListener(openBuyPopup);
        SmashPlusButton.onClick.AddListener(openBuyPopup);

        SelectSwapButton.onClick.AddListener(() =>
        {
            abilityPurchase.swapSelect = true;
            abilityPurchase.smashSelect = false;
            abilityPurchase.amount = 0;
            abilityPurchase.UpdateQuantTxt();
        });

        SelectSmashButton.onClick.AddListener(() =>
        {
            abilityPurchase.smashSelect = true;
            abilityPurchase.swapSelect = false;
            abilityPurchase.amount = 0;
            abilityPurchase.UpdateQuantTxt();
        });

        IncQuantButton.onClick.AddListener(() =>
        {
            abilityPurchase.amount++;
            abilityPurchase.UpdateQuantTxt();
        });

        DecQuantButton.onClick.AddListener(() =>
        {
            abilityPurchase.amount--;
            abilityPurchase.UpdateQuantTxt();
        });

        AbilityBuyButton.onClick.AddListener(() =>
        {
            abilityPurchase.Buy();
        });
    }

    private void newBlockUnlockRewardClaimButtonLogic()
    {
        newBlockRewardClaimButton = ButtonsContainer.transform.GetChild(3).GetChild(8).GetComponent<Button>();

        newBlockRewardClaimButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.popup.OnConfirmButtonPressed();
            ButtonClickSound();
        });
    }

    private void StoreButtonLogic()
    {
        var topUiPanel  = ButtonsContainer.transform.GetChild(1);
        PlusButton      = topUiPanel.GetChild(1).GetComponent<Button>();
        StoreButton     = ButtonsContainer.transform.GetChild(0).GetChild(3).GetComponent<Button>();

        StoreBackButton = UiManager.Instance.Store.transform.GetChild(3).GetComponent<Button>();

        UnityAction OpenStore = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.Store);
            ButtonClickSound();
            this.gameObject.SetActive(false);
        };

        PlusButton.onClick.AddListener(OpenStore);
        StoreButton.onClick.AddListener(OpenStore);

        StoreBackButton.onClick.AddListener(() => Back(Panel.Store, gameObject));
    }

    private void LeaderBoardButtonLogic()
    {
        var topUiPanel = ButtonsContainer.transform.GetChild(1);
        RankButton = topUiPanel.GetChild(0).GetComponent<Button>();

        RankBackButton = UiManager.Instance.LeaderBoard.transform.GetChild(1).GetComponent<Button>();

        UnityAction OpenLeaderBoard = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.LeaderBoard);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        RankButton.onClick.AddListener(OpenLeaderBoard);

        RankBackButton.onClick.AddListener(() => Back(Panel.LeaderBoard, gameObject));
    }

    private UnityAction<Panel, GameObject> Back = (panel, gameObject) =>
    {
        gameObject.SetActive(true);
        UiManager.Instance.PanelCloseAnimation(panel);
    };

    private void SmashButtonLogic()
    {
        SmashButton = ButtonsContainer.transform.GetChild(0).GetChild(1).GetComponent<Button>();

        SmashButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.gameController.SetSmash();
        });
    }

    private void SwapButtonLogic()
    {
        SwapButton = ButtonsContainer.transform.GetChild(0).GetChild(0).GetComponent<Button>();

        SwapButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.gameController.SetSwap();
            ButtonClickSound();
        });
    }

    private void PauseButtonLogic()
    {
        PauseButton = ButtonsContainer.transform.GetChild(0).GetChild(2).GetComponent<Button>();

        var PausePanel = ButtonsContainer.transform.GetChild(2);

        ResumeButton    = PausePanel.GetChild(4).GetComponent<Button>();
        RestartButton   = PausePanel.GetChild(3).GetComponent<Button>();
        HomeButton      = PausePanel.GetChild(2).GetComponent<Button>();

        PauseButton.onClick.AddListener(() =>
        {
            ButtonsContainer.transform.GetChild(2).gameObject.SetActive(true);
            ButtonClickSound();
        });
        ResumeButton.onClick.AddListener(() =>
        {
            ButtonsContainer.transform.GetChild(2).gameObject.SetActive(false);
            ButtonClickSound();
        });
        RestartButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.pauseMenu.OnRestartPressed();
            ButtonClickSound();
        });
        HomeButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.pauseMenu.OnHomePressed();
            ButtonClickSound();
        });
    }
    ///Supporting function;
    public void ButtonClickSound()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("ButtonClick");
    }
}
