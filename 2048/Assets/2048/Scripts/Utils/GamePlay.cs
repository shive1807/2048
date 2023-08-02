using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;

public class GamePlay : MonoBehaviour
{
    private GameObject ButtonsContainer;

    private Button StoreButton;
    private Button PauseButton;
    private Button SmashButton;
    private Button SwapButton;

    private Button PlusButton;
    private Button StoreBackButton;

    private Button RankButton;
    private Button RankBackButton;

    private Button ResumeButton;
    private Button RestartButton;
    private Button HomeButton;


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
    }

    private void StoreButtonLogic()
    {
        var topUiPanel  = ButtonsContainer.transform.GetChild(1);
        PlusButton      = topUiPanel.GetChild(1).GetComponent<Button>();
        StoreButton     = ButtonsContainer.transform.GetChild(0).GetChild(3).GetComponent<Button>();

        UnityAction OpenStore = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.Store);
            ButtonClickSound();
            this.gameObject.SetActive(false);
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
        var topUiPanel = ButtonsContainer.transform.GetChild(1);
        RankButton = topUiPanel.GetChild(0).GetComponent<Button>();

        UnityAction OpenLeaderBoard = () =>
        {
            UiManager.Instance.PanelOpenAnimation(Panel.LeaderBoard);
            ButtonClickSound();
            gameObject.SetActive(false);
        };

        RankButton.onClick.AddListener(OpenLeaderBoard);

        LeaderBoardBackButtonLogic();
    }

    private void LeaderBoardBackButtonLogic()
    {
        RankBackButton = UiManager.Instance.LeaderBoard.transform.GetChild(1).GetComponent<Button>();

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
            ButtonsContainer.transform.GetChild(5).gameObject.SetActive(true);
            DependencyManager.Instance.gameController.BlockRaycast(true);
        });
        ResumeButton.onClick.AddListener(() =>
        {
            ButtonsContainer.transform.GetChild(5).gameObject.SetActive(false);
            DependencyManager.Instance.gameController.BlockRaycast(false);
        });
        RestartButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.pauseMenu.OnRestartPressed();
            DependencyManager.Instance.gameController.BlockRaycast(false);
        });
        HomeButton.onClick.AddListener(() =>
        {
            DependencyManager.Instance.pauseMenu.OnHomePressed();
            DependencyManager.Instance.gameController.BlockRaycast(false);
        });
    }
    ///Supporting function;
    private void ButtonClickSound()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound("ButtonClick");
    }
}
