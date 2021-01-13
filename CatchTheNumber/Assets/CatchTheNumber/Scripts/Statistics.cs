using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class Statistics : MonoBehaviour {

    public GameObject MainMenu, GameUI, FailScreen, smiley, BestScore_notif, Ground, canvas;
    public float score, best_score;
    public int catch_number;
    public SpawnNumberBoxes SNB;
    public Text score_txt, catchthenumber_txt, MMBestScore, FSBestScore;
    public float speed = 20f;
    public AudioSource asource;
    public AudioClip button_pressed, best_score_notif, smiley_notif;
    public static Statistics Instance { get; private set; }
    private float each100 = 100;
    private GameObject smiley_obj, bestscore_obj;
    private bool bestscore = false;
    private BannerView bannerv;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd reward_video;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        reward_video = RewardBasedVideoAd.Instance;
        LoadBestScore();
        catch_number = Random.Range(0, 20);
        UpdateScoreAndCatchNumber();
        MMBestScore.text = "Best Score: " + best_score.ToString();
        RequestBanner();
        RequestInterstitial();
        RequestRewardVideo();
    }

    void Update()
    {
        if (score >= each100)
        {
            ComboScore();
        }
        if (score > best_score)
        {
            UpdateBestScore();
        }
    }
    private void ComboScore()
    {
        PlaySounds(smiley_notif);
        score_txt.gameObject.GetComponent<Animator>().Play("Score100Anim", -1, 0f);
        smiley_obj = Instantiate(smiley) as GameObject;
        smiley_obj.transform.SetParent(canvas.transform, false);
        smiley_obj.GetComponent<Animator>().Play("Smiley_anim");
        Destroy(smiley_obj, 1f);
        speed += 4f;
        each100 += 100;
    }
    private void UpdateBestScore()
    {
        best_score = score;
        if (bestscore == false)
        {
            PlaySounds(best_score_notif);
            bestscore_obj = Instantiate(BestScore_notif) as GameObject;
            bestscore_obj.transform.SetParent(canvas.transform, false);
            bestscore_obj.GetComponent<Animator>().Play("BestScore_anim");
            Destroy(bestscore_obj, 1f);
            bestscore = true;
        }
    }

    public void UpdateScoreAndCatchNumber()
    {
        score_txt.text = "Score: " + score.ToString();
        catchthenumber_txt.text = "Catch the number: " + catch_number.ToString();
    }

    public void ShowFailScreen()
    {
        StartCoroutine(SFS());
    }
    private IEnumerator SFS()
    {
        each100 = 100;
        SaveBestScore();
        SNB.ingame = false;
        GameUI.GetComponent<Animator>().Play("IGUI_out");
        yield return new WaitForSeconds(1f);
        FSBestScore.text = "Best Score: " + best_score.ToString();
        FailScreen.SetActive(true);
        FailScreen.GetComponent<Animator>().Play("FailScreen_in");
        yield return new WaitForSeconds(1f);
        Ground.SetActive(false);
        ShowInterstitial();
    }
    public void StartGame()
    {
        StartCoroutine(SG());
    }
    private IEnumerator SG()
    {
        SetScore();
        MainMenu.GetComponent<Animator>().Play("MMAnim_in");
        yield return new WaitForSeconds(1f);
        InGameUI();
        MainMenu.SetActive(false);
    }
    public void RestartGame()
    {
        StartCoroutine(RG());
    }
    private IEnumerator RG()
    {
        SetScore();
        FailScreen.GetComponent<Animator>().Play("FailScreen_out");
        yield return new WaitForSeconds(1f);
        InGameUI();
        FailScreen.SetActive(false);
    }
    public void BackToMainMenu()
    {
        StartCoroutine(BackToMM());
    }
    private IEnumerator BackToMM()
    {
        PlaySounds(button_pressed);
        MainMenu.SetActive(true);
        MMBestScore.text = "Best Score: " + best_score.ToString();
        FailScreen.GetComponent<Animator>().Play("FailScreen_toMM");
        MainMenu.GetComponent<Animator>().Play("MMAnim_toMM");
        yield return new WaitForSeconds(1f);
        FailScreen.SetActive(false);
    }
    private void InGameUI()
    {
        GameUI.SetActive(true);
        GameUI.GetComponent<Animator>().Play("IGUI_in");
        SNB.ingame = true;
    }
    private void SetScore()
    {
        PlaySounds(button_pressed);
        Ground.SetActive(true);
        bestscore = false;
        score = 0;
        UpdateScoreAndCatchNumber();
    }
    public void SaveBestScore()
    {
        PlayerPrefs.SetFloat("BestScore", best_score);
    }

    public void LoadBestScore()
    {
        best_score = PlayerPrefs.GetFloat("BestScore");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

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
        best_score += 200;
        SaveBestScore();
        MMBestScore.text = "Best Score: " + best_score.ToString();
        if (reward_video.IsLoaded())
        {
            reward_video.Show();
        }
    }

    private void PlaySounds(AudioClip soundd)
    {
        asource.clip = soundd;
        asource.Play();
    }
}
