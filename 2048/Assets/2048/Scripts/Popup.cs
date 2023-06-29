using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Popup
{
    public bool buttonPressed = false;
    public IEnumerator PopupConfirmation(Action function , GameObject popup)
    {
        popup.SetActive(true);
        yield return new WaitUntil (() => buttonPressed);
        popup.SetActive(false);

        function.Invoke();
    }

    public void OnConfirmButtonPressed()
    {
        buttonPressed = true;
    }
}
