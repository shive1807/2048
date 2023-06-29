using System;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    private List<Element> reshuffleList;
    private Action reshuffleCallback;

    public void ShowReshuffleConfirmation(List<Element> list, Action callback)
    {
        reshuffleList = list;
        reshuffleCallback = callback;

        // Show the reshuffle confirmation popup
        // Set up UI elements, buttons, etc.
    }

    public void OnConfirmButtonPressed()
    {
        // Hide the reshuffle confirmation popup

        // Invoke the reshuffle callback to perform the reshuffle logic
        reshuffleCallback?.Invoke();

        // Clear the callback reference
        reshuffleCallback = null;
    }
}
