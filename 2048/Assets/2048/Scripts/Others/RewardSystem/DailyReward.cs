using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DailyReward : MonoBehaviour
{
    public GameObject inActiveImg;
    public GameObject collectedImg;

    public int dayNumber = 1;
    public bool isActive = false;
    public bool isCollected = false;
    private void OnEnable()
    {
        inActiveImg = gameObject.transform.GetChild(3).gameObject;
        collectedImg = gameObject.transform.GetChild(4).gameObject;
    }
}
