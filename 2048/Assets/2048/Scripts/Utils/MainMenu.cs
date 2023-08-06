using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;
using Unity.VisualScripting;
using GleyDailyRewards;

public class MainMenu : MonoBehaviour
{
    private GameObject ButtonsContainer;
    private GameObject GemsStatus;

    // Store Buttons
    private Button PlusButton;
    private Button StoreButton;
    private Button StoreBackButton;

    private TextMeshProUGUI GemsText;

    // Leaderboard Buttons
    private Button LeaderBoardButton;
    private Button RankButton;
    private Button LeaderBoardBackButton;

    // Main Buttons
    private Button SettingsButton;

    private Button PlayButton;


    // Daily Reward
    private Button DailyRewardCloseButton;

    private Button Day_1Button;
    private Button Day_2Button;
    private Button Day_3Button;
    private Button Day_4Button;
    private Button Day_5Button;

    // First Login Buttons
    private Button SaveNameButton;
    private Button LoginBackButton;
    private Button ClearNameButton;
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

        DailyRewardButtonsLogic();

        FirstLoginButtonsLogic();
    }

    private void FirstLoginButtonsLogic()
    {
        SaveNameButton = transform.GetChild(3).GetChild(5).GetComponent<Button>();
        LoginBackButton = transform.GetChild(3).GetChild(7).GetComponent<Button>();
        ClearNameButton = transform.GetChild(3).GetChild(6).GetComponent<Button>();

        SaveNameButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.login.OnNewLogin();
        });

        LoginBackButton.onClick.AddListener(() =>
        {
            transform.GetChild(3).gameObject.SetActive(false);
        });

        ClearNameButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.login.OnClearInput();
        });
    }

    private void DailyRewardButtonsLogic()
    {
        UnityAction<DailyReward> claimReward = (DailyReward) =>
        {
            RewardManager.Instance.ClaimDailyReward(DailyReward);
        };

        var dailyReward = transform.GetChild(2).transform;

        Day_1Button = dailyReward.GetChild(2).GetComponent<Button>();
        Day_2Button = dailyReward.GetChild(3).GetComponent<Button>();
        Day_3Button = dailyReward.GetChild(4).GetComponent<Button>();
        Day_4Button = dailyReward.GetChild(5).GetComponent<Button>();
        Day_5Button = dailyReward.GetChild(6).GetComponent<Button>();

        DailyRewardCloseButton = dailyReward.GetChild(7).GetComponent<Button>();


        Day_1Button.onClick.AddListener(() => claimReward(Day_1Button.gameObject.GetComponent<DailyReward>()));
        Day_2Button.onClick.AddListener(() => claimReward(Day_2Button.gameObject.GetComponent<DailyReward>()));
        Day_3Button.onClick.AddListener(() => claimReward(Day_3Button.gameObject.GetComponent<DailyReward>()));
        Day_4Button.onClick.AddListener(() => claimReward(Day_4Button.gameObject.GetComponent<DailyReward>()));
        Day_5Button.onClick.AddListener(() => claimReward(Day_5Button.gameObject.GetComponent<DailyReward>()));

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

        StoreBackButton = UiManager.Instance.Store.transform.GetChild(3).GetComponent<Button>();


        // executing click logic
        UnityAction OpenStore = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.Store);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        PlusButton.onClick.AddListener(OpenStore);
        StoreButton.onClick.AddListener(OpenStore);

        StoreBackButton.onClick.AddListener(() => Back(Panel.Store, gameObject));
    }

    private void LeaderBoardButtonLogic()
    {
        LeaderBoardButton = ButtonsContainer.transform.GetChild(3).GetChild(2).GetComponent<Button>();
        RankButton = ButtonsContainer.transform.GetChild(2).GetChild(0).GetComponent<Button>();

        LeaderBoardBackButton = UiManager.Instance.LeaderBoard.transform.GetChild(1).GetComponent<Button>();

        UnityAction OpenLeaderBoard = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.LeaderBoard);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        LeaderBoardButton.onClick.AddListener(OpenLeaderBoard);
        RankButton.onClick.AddListener(OpenLeaderBoard);

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


