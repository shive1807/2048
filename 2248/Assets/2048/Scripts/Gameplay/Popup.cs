using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [HideInInspector] public bool buttonPressed = false;
    public int newBlockReward = 50;
    public TextMeshProUGUI rewardTxt;
    public Image newBlockImg;
    public TextMeshProUGUI newBlockTxt;
    public IEnumerator PopupConfirmation(Action<Num> function , GameObject popup, Num min, Num max)
    {
        rewardTxt.text = newBlockReward.ToString();
        newBlockImg.color = DataManager.GetColor(max);
        newBlockTxt.text = max.txt;

        popup.SetActive(true);

        buttonPressed = false;

        yield return new WaitUntil (() => buttonPressed);
        Debug.Log("pop");
        popup.SetActive(false);

        function.Invoke(min);
    }
    public void OnConfirmButtonPressed()
    {
        buttonPressed = true;
        GemsManager.Instance.AddGems(newBlockReward);
    }
}
