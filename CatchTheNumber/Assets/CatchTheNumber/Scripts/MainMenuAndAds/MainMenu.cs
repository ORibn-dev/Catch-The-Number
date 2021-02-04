using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void UpdateStuff();

public class MainMenu : MonoBehaviour
{
    #region DeclaredFields

    public GameObject GameUI;
    public Text MMBestScore, FSBestScore;
    [SerializeField] private GameObject MainMenuUI, FailScreenUI;
    private float bestscore;

    #endregion

    #region Events

    public event UpdateStuff SaveBestScore;
    public event UpdateStuff SetScore;

    #endregion

    #region Initialization

    public static MainMenu Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    #region SetMainManuAndShowGameUI

    public void SetMainMenu(float bs)
    {
        bestscore = bs;
        MMBestScore.text = "Best Score: " + bestscore.ToString();
    }

    private void InGameUI()
    {
        GameUI.SetActive(true);
        GameUI.GetComponent<Animator>().Play("IGUI_in");
        SpawnNumberBoxes.Instance.ingame = true;
    }

    #endregion

    #region FailScreen

    public void ShowFailScreen()
    {
        StartCoroutine(SFS());
    }
    private IEnumerator SFS()
    {
        SaveBestScore();
        SpawnNumberBoxes.Instance.ingame = false;
        GameUI.GetComponent<Animator>().Play("IGUI_out");
        yield return new WaitForSeconds(1f);
        FSBestScore.text = "Best Score: " + bestscore.ToString();
        FailScreenUI.SetActive(true);
        FailScreenUI.GetComponent<Animator>().Play("FailScreen_in");
        yield return new WaitForSeconds(1f);
        Statistics.Instance.Ground.SetActive(false);
        GooglePlayAds.Instance.ShowInterstitial();
    }

    #endregion

    #region StartGame

    public void StartGame()
    {
        StartCoroutine(SG());
    }
    private IEnumerator SG()
    {
        SetScore();
        MainMenuUI.GetComponent<Animator>().Play("MMAnim_in");
        yield return new WaitForSeconds(1f);
        InGameUI();
        MainMenuUI.SetActive(false);
    }

    #endregion

    #region RestartGame

    public void RestartGame()
    {
        StartCoroutine(RG());
    }
    private IEnumerator RG()
    {
        SetScore();
        FailScreenUI.GetComponent<Animator>().Play("FailScreen_out");
        yield return new WaitForSeconds(1f);
        InGameUI();
        FailScreenUI.SetActive(false);
    }

    #endregion

    #region MainMenuAndExit

    public void BackToMainMenu()
    {
        StartCoroutine(BackToMM());
    }
    private IEnumerator BackToMM()
    {
        GameSounds.Instance.PlaySounds(0);
        MainMenuUI.SetActive(true);
        MMBestScore.text = "Best Score: " + bestscore.ToString();
        FailScreenUI.GetComponent<Animator>().Play("FailScreen_toMM");
        MainMenuUI.GetComponent<Animator>().Play("MMAnim_toMM");
        yield return new WaitForSeconds(1f);
        FailScreenUI.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
