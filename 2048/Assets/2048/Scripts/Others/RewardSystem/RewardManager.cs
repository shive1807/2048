using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;


public class RewardManager : Singleton<RewardManager>
{
    public DailyReward[] Days;

    //popup variables
    public float popupTimer = 2f;
    public float popupOpenDuration = .2f;

    //reward variables
    public int rewardAmount = 30;
    public int rewardMultiplier = 10;

    //collection check
    private DateTime lastClaimDate = DateTime.Now;
    public int currentStreak = 0;
    public int rewardCollected = 0;

    #region private functions
    private void Start()
    {
        if(rewardCollected == 0)
        {
            StartCoroutine(StartDailyRewardPopup());
        }
        Setup();
    }
    private IEnumerator StartDailyRewardPopup()
    {
        if (GameManager.Instance.gameData.FirstLogin == 1)
        {
            yield return new WaitUntil(() => DependencyManager.Instance.login.login);
        }

        yield return new WaitForSeconds(popupTimer);
        this.gameObject.SetActive(true);
        RectTransform rect = this.gameObject.gameObject.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
        rect.DOScale(1, popupOpenDuration).SetEase(Ease.OutBounce);

        transform.parent.GetChild(2).gameObject.SetActive(true);
    }
    private void Setup()
    {
        GameData gamedata = GameManager.Instance.gameData;
        if(gamedata != null)
        {
            lastClaimDate = GameManager.Instance.gameData.LastClaimRewardDate;
            currentStreak = GameManager.Instance.gameData.RewardClaimStreak;
            rewardCollected = GameManager.Instance.gameData.Collected;
        }

        if(currentStreak >= Days.Length)
        {
            currentStreak = 0;
            SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, currentStreak);
        }
        DayCheck();
    }
    private void DayCheck()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimDate;

        if (timeSinceLastClaim.TotalDays >= 1)
        {
            rewardCollected = 0;
            if (timeSinceLastClaim.TotalDays > 2)
            {
                currentStreak = 0;
                SaveSystem.SaveGame(-1, false, null, -1, -1, -1, -1, currentStreak);
            }
        }

        UIUpdate(currentStreak);
    }
    private void UIUpdate(int streak)
    {
        for (int i = 0; i < Days.Length; i++)
        {
            if (streak == i)
            {
                if(rewardCollected == 1)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(false);
                    Days[i].isActive = false;
                    Days[i].isCollected = false;
                }
                else if(rewardCollected == 0)
                {
                    Days[i].inActiveImg.SetActive(false);
                    Days[i].collectedImg.SetActive(false);
                    Days[i].isActive = true;
                    Days[i].isCollected = false;
                }
            }
            else
            {
                if (streak > i)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(true);
                    Days[i].isActive = false;
                    Days[i].isCollected = true;
                }
                else if (streak < i)
                {
                    Days[i].inActiveImg.SetActive(true);
                    Days[i].collectedImg.SetActive(false);
                    Days[i].isActive = false;
                    Days[i].isCollected = false;
                }
            }
        }
    }

    #endregion

    #region public functions
    public void ClaimDailyReward(DailyReward dailyReward)
    {
        if(dailyReward.isActive)
        {
            if (!dailyReward.isCollected)
            {
                // UI update
                dailyReward.isCollected = true;
                dailyReward.isActive = false;
                dailyReward.collectedImg.SetActive(true);
                dailyReward.inActiveImg.SetActive(true);

                rewardCollected = 1;
                GemsManager.Instance.AddGems(rewardAmount + (currentStreak * rewardMultiplier));

                // Streak update
                currentStreak++;

                SaveSystem.SaveGame(GemsManager.Instance.gems, false, null, -1, -1, -1, -1, currentStreak, DateTime.Now, rewardCollected);
            }
            else
            {
                Debug.Log("You can claim the reward again tomorrow.");
            }
        }
    }

    #endregion
}
