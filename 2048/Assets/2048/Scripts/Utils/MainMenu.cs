using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private GameObject ButtonsContainer;
    private GameObject GemsStatus;

    private Button PlusButton;
    private Button StoreButton;
    private Button StoreBackButton;

    private Button LeaderBoardButton;
    private Button RankButton;
    private Button LeaderBoardBackButton;

    private Button SettingsButton;

    private Button PlayButton;

    private Button DailyRewardCloseButton;

    private TextMeshProUGUI GemsText;

    private Button Day_1Button;
    private Button Day_2Button;
    private Button Day_3Button;
    private Button Day_4Button;
    private Button Day_5Button;
    private void OnEnable()
    {
        //UpdateCurrency();
    }

    private void Start()
    {
        ButtonsContainer = transform.GetChild(1).gameObject;

        PlayButton = ButtonsContainer.transform.GetChild(0).GetComponent<Button>();
        ButtonsClickLogic();
    }
    
    private void ButtonsClickLogic()
    {
        PlayButtonLogic();

        StoreButtonLogic();

        LeaderBoardButtonLogic();

        SettingsButtonLogic();

        DailyRewardCloseButtonLogic();
    }
    private void DailyRewardButtonsLogic()
    {
        UnityAction claimReward = () =>
        {
            //RewardManager.Instance.ClaimDailyReward();
        };
    }
    private void DailyRewardCloseButtonLogic()
    {
        DailyRewardCloseButton = gameObject.transform.GetChild(2).GetChild(0).GetChild(6).GetComponent<Button>();

        DailyRewardCloseButton.onClick.AddListener(() =>
        {
            UiManager.Instance.SetActive(gameObject.transform.GetChild(2).GetComponent<RectTransform>());
        });
    }
    private void StoreButtonLogic()
    {
        // gettting reference
        GemsStatus = ButtonsContainer.transform.GetChild(1).gameObject;

        PlusButton = GemsStatus.transform.GetChild(2).GetComponent<Button>();
        StoreButton = ButtonsContainer.transform.GetChild(3).GetChild(1).GetComponent<Button>();

        GemsText = GemsStatus.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        // executing click logic
        UnityAction OpenStore = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.Store);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        PlusButton.onClick.AddListener(OpenStore);
        StoreButton.onClick.AddListener(OpenStore);

        StoreBackButtonLogic();
    }
    private void StoreBackButtonLogic()
    {
        StoreBackButton = UiManager.Instance.Store.transform.GetChild(3).GetComponent<Button>();

        StoreBackButton.onClick.AddListener(() => Back(Panel.Store, gameObject));
    }
    private void LeaderBoardButtonLogic()
    {
        LeaderBoardButton = ButtonsContainer.transform.GetChild(3).GetChild(2).GetComponent<Button>();
        RankButton = ButtonsContainer.transform.GetChild(2).GetChild(0).GetComponent<Button>();

        UnityAction OpenLeaderBoard = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.LeaderBoard);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        LeaderBoardButton.onClick.AddListener(OpenLeaderBoard);
        RankButton.onClick.AddListener(OpenLeaderBoard);

        LeaderBoardBackButtonLogic();
    }
    private void LeaderBoardBackButtonLogic()
    {
        LeaderBoardBackButton = UiManager.Instance.LeaderBoard.transform.GetChild(1).GetComponent<Button>();

        LeaderBoardBackButton.onClick.AddListener(() => Back(Panel.LeaderBoard, gameObject));
    }
    private void SettingsButtonLogic()
    {
        SettingsButton = ButtonsContainer.transform.GetChild(3).GetChild(3).GetComponent<Button>();

        SettingsButton.onClick.AddListener(() =>
        {
            UiManager.Instance.ToggleRoll(SettingsButton.GetComponent<RectTransform>());
            ButtonClickSound();
        });
    }
    private void PlayButtonLogic()
    {
        PlayButton.onClick.AddListener(()=> {
            GameManager.Instance.LoadScene("GameScene");
            ButtonClickSound();
        });
    }
    UnityAction<Panel, GameObject> Back = (panel, gameObject) =>
    {
        gameObject.SetActive(true);
        UiManager.Instance.PanelCloseAnimation(panel);
    };
    private void UpdateCurrency()
    {
        if(GemsText == null)
            StoreButtonLogic();

        GemsText.text = DataManager.Gems.ToString();
    }

    ///Supporting function;
    private void ButtonClickSound()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("ButtonClick");
    }
}


