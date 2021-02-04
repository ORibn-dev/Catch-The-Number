using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GooglePlayAds : MonoBehaviour
{
    #region DeclaredFields

    private BannerView bannerv;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd reward_video;

    #endregion

    #region Inititialization

    private Statistics stat;
    public static GooglePlayAds Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        stat = Statistics.Instance;
        reward_video = RewardBasedVideoAd.Instance;
        RequestBanner();
        RequestInterstitial();
        RequestRewardVideo();
    }

    #endregion

    #region RequestAds

    public void RequestBanner()
    {
        string AdId = "Insert your BannerAdID here.";
        bannerv = new BannerView(AdId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerv.LoadAd(request);
        bannerv.OnAdLoaded += HandleOnAdLoaded;
    }
    public void RequestInterstitial()
    {
        string AdId = "Insert your InterstitialAdID here.";
        interstitial = new InterstitialAd(AdId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    public void RequestRewardVideo()
    {
        string AdId = "Insert your RewardAdID here.";
        reward_video.LoadAd(new AdRequest.Builder().Build(), AdId);
    }

    #endregion

    #region ShowAds

    void HandleOnAdLoaded(object a, System.EventArgs args)
    {
        bannerv.Show();
    }

    public void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
    public void ShowRewardVideo()
    {
        stat.best_score += 200;
        MainMenu.Instance.MMBestScore.text = "Best Score: " + stat.best_score.ToString();
        stat.SaveBestScore();
        if (reward_video.IsLoaded())
        {
            reward_video.Show();
        }
    }
    #endregion
}
