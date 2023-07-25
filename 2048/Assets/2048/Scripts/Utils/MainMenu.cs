using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private GameObject ButtonsContainer;
    private GameObject GemsStatus;

    private Button PlusButton;
    private Button PlayButton;

    private TextMeshProUGUI GemsText;

    private void OnEnable()
    {
        //UpdateCurrency();
    }

    private void Start()
    {
        ButtonsContainer = transform.GetChild(1).gameObject;

        PlayButton = ButtonsContainer.transform.GetChild(0).GetComponent<Button>();

        PlayButtonLogic();
        GetCurrencyReference();
    }

    private void GetCurrencyReference()
    {
        GemsStatus = ButtonsContainer.transform.GetChild(1).gameObject;

        PlusButton = GemsStatus.transform.GetChild(3).GetComponent<Button>();
        GemsText = GemsStatus.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        PlusButton.onClick.AddListener(() => {
            UiManager.Instance.PanelOpenAnimation(Panel.Store);
            PlayButtonSound();
            gameObject.SetActive(false);
        });
    }

    private void PlayButtonLogic()
    {
        PlayButton.onClick.AddListener(()=> {
            GameManager.Instance.LoadScene("GameScene");
            PlayButtonSound();
        });
    }

    private void UpdateCurrency()
    {
        if(GemsText == null)
            GetCurrencyReference();

        GemsText.text = DataManager.Gems.ToString();
    }

    ///Supporting function;
    private void PlayButtonSound()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
    }
}


