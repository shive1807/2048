using UnityEngine;

public class ADTest : MonoBehaviour
{
    void Start()
    {
        AdManager.instanceAdManager.RequestInterstitial();
        AdManager.instanceAdManager.RequestRewardedAd();
        AdManager.instanceAdManager.RequestBanner();
    }
    public void LoadInterstitialAd()
    {
        AdManager.instanceAdManager.ShowInterstitial();
    }
    public void LoadRewardedAd()
    {
        AdManager.instanceAdManager.ShowRewardedAd();
    }
}
