using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    private Button PlayButton;

    // Settings Buttons
    private Button SettingsButton;
    private Button SettingsCloseButton;
    private Button SoundToggleButton;
    private Button VibrationToggleButton;
    private Button RemoveAds;

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
    private Button RandomNameButton;

    // Profile Buttons
    private Button ProfileButton;
    private TextMeshProUGUI Username;
    private TextMeshProUGUI MaxBlock;
    private TextMeshProUGUI HighScore;
    private TextMeshProUGUI Rank;
    private Button ProfileBackButton;

    // HighScore 
    private TextMeshProUGUI HomeHighScore;

    private void Start()
    {
        Leaderboard.Instance.FetchData();

        ButtonsContainer = transform.GetChild(1).gameObject;

        PlayButton = ButtonsContainer.transform.GetChild(0).GetComponent<Button>();
        ButtonsClickLogic();

        HomeHighScore = ButtonsContainer.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        HomeHighScore.text = GameManager.Instance.gameData.HighScore.ToString();
    }
    
    private void ButtonsClickLogic()
    {
        PlayButtonLogic();

        StoreButtonLogic();

        LeaderBoardButtonLogic();

        SettingsButtonLogic();

        DailyRewardButtonsLogic();

        FirstLoginButtonsLogic();

        ProfileButtonsLogic();
    }

    private void ProfileButtonsLogic()
    {
        Transform Profile = UiManager.Instance.Profile.transform;

        ProfileButton = ButtonsContainer.transform.GetChild(3).GetChild(0).GetComponent<Button>();

        Username = Profile.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        MaxBlock = Profile.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        HighScore = Profile.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        Rank = Profile.GetChild(2).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        ProfileBackButton = Profile.GetChild(5).GetComponent<Button>();

        ProfileButton.onClick.AddListener(() =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.Profile);
            ButtonClickSound();
            gameObject.SetActive(false);

            GameData gamedata = GameManager.Instance.gameData;

            Username.text = gamedata.User.Username;
            MaxBlock.text = gamedata.MaxBlock.txt;
            HighScore.text = gamedata.HighScore.ToString();
            Rank.text = Leaderboard.Instance.rank.ToString();
        });

        ProfileBackButton.onClick.AddListener(() => Back(Panel.Profile, gameObject));
    }
    private void FirstLoginButtonsLogic()
    {
        Transform loginPopup = transform.GetChild(4);

        SaveNameButton = loginPopup.GetChild(5).GetComponent<Button>();
        LoginBackButton = loginPopup.GetChild(7).GetComponent<Button>();
        ClearNameButton = loginPopup.GetChild(6).GetComponent<Button>();
        RandomNameButton = loginPopup.GetChild(8).GetComponent<Button>();

        SaveNameButton.onClick.AddListener(() =>
        {
            StartCoroutine(DependencyManager.Instance.login.UsernameInput());
        });

        RandomNameButton.onClick.AddListener(() =>
        {
            StartCoroutine(DependencyManager.Instance.login.RandomUsername());
        });

        LoginBackButton.onClick.AddListener(() =>
        {
            UiManager.Instance.SetActive(transform.GetChild(4).GetComponent<RectTransform>(), false);
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

        var dailyReward = transform.GetChild(3).transform;

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
            UiManager.Instance.SetActive(dailyReward.GetComponent<RectTransform>(), false);
            transform.GetChild(2).gameObject.SetActive(false);
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
        GetScoresUI();

        LeaderBoardButton = ButtonsContainer.transform.GetChild(3).GetChild(2).GetComponent<Button>();
        //RankButton = ButtonsContainer.transform.GetChild(2).GetChild(0).GetComponent<Button>();

        LeaderBoardBackButton = UiManager.Instance.LeaderBoard.transform.GetChild(6).GetComponent<Button>();

        UnityAction OpenLeaderBoard = () =>
        {
            //Leaderboard.Instance.FetchData();
            UiManager.Instance.PanelOpenAnimation(Panel.LeaderBoard);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        LeaderBoardButton.onClick.AddListener(OpenLeaderBoard);
        //RankButton.onClick.AddListener(OpenLeaderBoard);

        LeaderBoardBackButton.onClick.AddListener(() => Back(Panel.LeaderBoard, gameObject));
    }

    public void GetScoresUI()
    {

        Transform leaderboard = UiManager.Instance.LeaderBoard.transform;

        for (int i = 0; i < Leaderboard.Instance.users.Length; i++)
        {
            Leaderboard.Instance.scores[i] = leaderboard.GetChild(i).GetComponent<Score>();
        }
    }

    private void SettingsButtonLogic()
    {
        SettingsButton = ButtonsContainer.transform.GetChild(3).GetChild(3).GetComponent<Button>();
        SettingsCloseButton = SettingsButton.gameObject.transform.GetChild(2).GetChild(3).GetComponent<Button>();
        SoundToggleButton = SettingsButton.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Button>();
        VibrationToggleButton = SettingsButton.gameObject.transform.GetChild(2).GetChild(1).GetComponent<Button>();
        RemoveAds = SettingsButton.gameObject.transform.GetChild(2).GetChild(2).GetComponent<Button>();

        SettingsButton.onClick.AddListener(() =>
        {
            UiManager.Instance.ToggleRoll(SettingsButton.transform.position);

            UiManager.Instance.SetActive(LeaderBoardButton.gameObject.GetComponent<RectTransform>(), false);
            UiManager.Instance.SetActive(StoreButton.gameObject.GetComponent<RectTransform>(), false);
            UiManager.Instance.SetActive(ProfileButton.gameObject.GetComponent<RectTransform>(), false);

            SettingsButton.gameObject.GetComponent<Image>().enabled = false;
            SettingsButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            ButtonClickSound();
        });

        SettingsCloseButton.onClick.AddListener(() =>
        {
            UiManager.Instance.ToggleRoll(SettingsButton.transform.position);

            UiManager.Instance.SetActive(LeaderBoardButton.gameObject.GetComponent<RectTransform>(), true);
            UiManager.Instance.SetActive(StoreButton.gameObject.GetComponent<RectTransform>(), true);
            UiManager.Instance.SetActive(ProfileButton.gameObject.GetComponent<RectTransform>(), true);

            SettingsButton.gameObject.GetComponent<Image>().enabled = true;
            SettingsButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);

            ButtonClickSound();
        });

        SoundToggleButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.AutoToggleSound();

            AudioManager.Instance.PlaySound("ButtonClick");
        });

        VibrationToggleButton.onClick.AddListener(() =>
        {
            VibrationManager.Instance.ToggleVibration();
            //VibrationManager.Instance.Vibrate(500);

            AudioManager.Instance.PlaySound("ButtonClick");
        });

        RemoveAds.onClick.AddListener(() =>
        {
            IAP.Instance.BuyConsumableRemove_Ads();

            AudioManager.Instance.PlaySound("ButtonClick");
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
        ButtonClickSound();
    };

    ///Supporting function;
    public static void ButtonClickSound()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("ButtonClick");
    }
}


