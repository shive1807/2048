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
        DependencyManager.Instance.gameController.BlockRaycast(true);

        yield return new WaitUntil (() => buttonPressed);
        popup.SetActive(false);

        function.Invoke(min);

        DependencyManager.Instance.gameController.BlockRaycast(false);
    }
    public void OnConfirmButtonPressed()
    {
        buttonPressed = true;
        GemsManager.Instance.AddGems(newBlockReward);
    }
}
