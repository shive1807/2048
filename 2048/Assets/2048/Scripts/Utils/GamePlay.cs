using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    private GameObject ButtonsContainer;

    private Button StoreButton;
    private Button PlusButton;
    private Button StoreBackButton;

    private Button RankButton;
    private Button RankBackButton;

    private Button PauseButton;
    private Button ResumeButton;
    private Button RestartButton;
    private Button HomeButton;

    private Button SmashButton;
    private Button SwapButton;

    private void Start()
    {
        ButtonsContainer = transform.GetChild(1).gameObject;

        ButtonsLogic();
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
        PlusButton = ButtonsContainer.transform.GetChild(2).GetChild(3).GetComponent<Button>();
        StoreButton = ButtonsContainer.transform.GetChild(4).GetComponent<Button>();

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
        RankButton = ButtonsContainer.transform.GetChild(1).GetChild(2).GetComponent<Button>();

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
    UnityAction<Panel, GameObject> Back = (panel, gameObject) =>
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
        PauseButton = ButtonsContainer.transform.GetChild(3).GetComponent<Button>();
        ResumeButton = ButtonsContainer.transform.GetChild(5).GetChild(4).GetComponent<Button>();
        RestartButton = ButtonsContainer.transform.GetChild(5).GetChild(3).GetComponent<Button>();
        HomeButton = ButtonsContainer.transform.GetChild(5).GetChild(2).GetComponent<Button>();

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
