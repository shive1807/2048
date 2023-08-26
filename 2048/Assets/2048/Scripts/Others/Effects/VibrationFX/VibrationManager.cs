using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VibrationManager : Singleton<VibrationManager>
{
    [HideInInspector] public int vibration = 1;
    [SerializeField] private GameObject vibOn;
    [SerializeField] private GameObject vibOff;

    public void Vibrate(int duration)
    {
        if (vibration == 1)
        {
            Vibrator.vibrate(duration);
        }
        else
        {
            return;
        }
    }

    public void ToggleVibration()
    {
        if(vibration == 1)
        {
            vibration = 0;
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
        else if(vibration == 0)
        {
            vibOff.SetActive(false);
            vibOn.SetActive(true);
            vibration = 1;
        }
    }
}
