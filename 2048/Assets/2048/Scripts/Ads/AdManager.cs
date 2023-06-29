using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdManager : MonoBehaviour
{
    public static AdManager instanceAdManager;
    
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    [SerializeField] private AdPosition adPos;
    public bool showAD = true;

    [Header("These are Samples , Please Add Your Ids")]
    [SerializeField] private string bannerUnitId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] private string rewardedUnitId = "ca-app-pub-3940256099942544/5224354917";


    private void Awake()
    {
        if (instanceAdManager == null)
            instanceAdManager = this;
    }
    private void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #region Banner AD
    public void RequestBanner()
    {
        if (!showAD)
            return;

        this.bannerAd = new BannerView(bannerUnitId, AdSize.SmartBanner, adPos);
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

        InterstitialAd.Load(interstitialUnitId, CreateAdRequest(), (InterstitialAd ad, LoadAdError loadAdError) =>
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
        RewardedAd.Load(rewardedUnitId, CreateAdRequest(),
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
