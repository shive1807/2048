using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdManager : Singleton<AdManager> 
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    [SerializeField] private AdPosition adPos;
    public bool showAD = true;

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(InitializationStatus => { });
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #region Banner AD
    public void RequestBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;
        if (!showAD)
            return;

        this.bannerAd = new BannerView(GameSettings.BannerUnitId, AdSize.SmartBanner, adPos);
        this.bannerAd.LoadAd(this.CreateAdRequest());
    }
    #endregion

    #region Interstital AD 

    public void RequestInterstitial()
    {
        if (!showAD)
            return;

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        InterstitialAd.Load(GameSettings.InterstitialUnitId, CreateAdRequest(), (InterstitialAd ad, LoadAdError loadAdError) =>
        {
            if (loadAdError != null)
            {
                return;
            }
            else
            if (ad == null)
            {
                return;
            }
            interstitialAd = ad;

        });
    }
    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            RegisterReloadHandler(interstitialAd);
        }
    }
    private void RegisterReloadHandler(InterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            RequestInterstitial();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestInterstitial();
        };
    }
    #endregion

    #region Rewarded AD 

    public void RequestRewardedAd()
    {
        if (!showAD)
            return;

        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");


        // send the request to load the ad.
        RewardedAd.Load(GameSettings.RewardedUnitId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });


    }
    public void ShowRewardedAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
            RegisterReloadHandler(rewardedAd);
        }
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardedAd();
        };
    }
    #endregion
}
