using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [HideInInspector] public bool buttonPressed = false;
    public IEnumerator PopupConfirmation(Action<Num> function , GameObject popup, Num min)
    {
        popup.SetActive(true);

        DependencyManager.Instance.gameController.BlockRaycast(true);

        yield return new WaitUntil (() => buttonPressed);
        popup.SetActive(false);

        function.Invoke(min);

        DependencyManager.Instance.gameController.BlockRaycast(false);
    }
    public void OnConfirmButtonPressed()
    {
        buttonPressed = true;
    }
}
