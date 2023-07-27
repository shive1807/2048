using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Runtime.Serialization.Json;

public class RewardManager : Singleton<RewardManager>
{
    public DailyReward[] Days;

    public float popupTimer = 2f;
    public float popupOpenDuration = .2f;

    public int rewardAmount = 30;
    public int rewardMultiplier = 10;

    private DateTime lastClaimDate;
    private int currentStreak = 1;
    void Start()
    {
        StartCoroutine(StartDailyRewardPopup());
        Setup();
    }
    IEnumerator StartDailyRewardPopup()
    {
        yield return new WaitForSeconds(popupTimer);
        this.gameObject.SetActive(true);
        RectTransform rect = this.gameObject.gameObject.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
        rect.DOScale(1, popupOpenDuration).SetEase(Ease.OutBounce);
    }
    void Setup()
    {
        lastClaimDate = GameManager.Instance.gameData.LastClaimRewardDate;
        currentStreak = GameManager.Instance.gameData.RewardClaimStreak;

        int reward = rewardAmount * currentStreak * rewardMultiplier;
        StreakCheck();
        // UI update
        UIUpdate();
    }
    private void StreakCheck()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimDate;

        if (timeSinceLastClaim.TotalDays >= 1)
        {
            if(timeSinceLastClaim.TotalDays > 1)
            {
                currentStreak = 1;
                SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, DateTime.Now);
            }
            UIUpdate();
        }
    }
    private void UIUpdate()
    {
        for (int i = 0; i < Days.Length; i++)
        {
            if (currentStreak == i + 1)
            {
                Days[i].inActiveImg.SetActive(false);
                Days[i].collectedImg.SetActive(false);
                Days[i].isActive = true;
            }
            else
            {
                if (i + 1 < currentStreak)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(true);
                    Days[i].isActive = true;
                }
                else if (i + 1 > currentStreak)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(false);
                    Days[i].isActive = true;
                }
            }
        }
    }
    public void ClaimDailyReward(DailyReward dailyReward)
    {
        if(dailyReward.isActive)
        {
            if (!dailyReward.isCollected)
            {
                currentStreak++;
                UIUpdate();
                dailyReward.isCollected = true;
                dailyReward.isActive = false;
                SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, DateTime.Now);
                GemsManager.Instance.AddGems(rewardAmount + (currentStreak * rewardMultiplier));
            }
            else
            {
                Debug.Log("You can claim the reward again tomorrow.");
            }
        }
    }
}
