using UnityEngine;

public class ADTest : Singleton<ADTest>
{
    private void Start()
    {
        AdManager.Instance.RequestInterstitial();
        AdManager.Instance.RequestRewardedAd();
        AdManager.Instance.RequestBanner();
    }

    public void LoadInterstitialAd()
    {
        AdManager.Instance.ShowInterstitial();
    }

    public void LoadRewardedAd()
    {
        AdManager.Instance.ShowRewardedAd();
    }   

}
