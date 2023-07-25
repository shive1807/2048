using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

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

        // UI update
        UIUpdate();
    }

    private void UIUpdate()
    {
        for (int i = 0; i < Days.Length; i++)
        {
            if (currentStreak == i + 1)
            {
                Days[i].inActiveImg.SetActive(false);
                Days[i].collectedImg.SetActive(false);
            }
            else
            {
                if (i + 1 < currentStreak)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(true);
                }
                else if (i + 1 > currentStreak)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(false);
                }
            }
        }
    }
    public void ClaimDailyReward()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimDate;

        if (timeSinceLastClaim.TotalDays >= 1)
        {
            if (timeSinceLastClaim.TotalDays == 1)
            {
                currentStreak++;
            }
            else
            {
                currentStreak = 1;
            }
            UIUpdate();
            SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, DateTime.Now);
        }
        else
        {
            Debug.Log("You can claim the reward again tomorrow.");
        }
    }
}
