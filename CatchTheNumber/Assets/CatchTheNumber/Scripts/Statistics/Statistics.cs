using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    #region DeclaredFields

    public GameObject Ground;
    [SerializeField] private GameObject smiley, BestScore_notif;
    [SerializeField] private Text score_txt, catchthenumber_txt;

    public int score, best_score;
    public int catch_number;
    public float speed = 20f;

    private float each100 = 100;
    private bool bestscore = false;
    private GameObject smiley_obj, bestscore_obj;

    #endregion

    #region SingletonReferences

    public static Statistics Instance { get; private set; }
    private SpawnNumberBoxes SNB;
    private GameSounds GS;
    private MainMenu MM;

    #endregion

    #region Initialization

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SNB = SpawnNumberBoxes.Instance;
        GS = GameSounds.Instance;
        MM = MainMenu.Instance;
        LoadBestScore();
        catch_number = Random.Range(0, 20);
        SetScoreAndCatchNumberTxt();

        MM.SetMainMenu(best_score);
        MM.SaveBestScore += SaveBestScore;
        MM.SetScore += SetScore;
    }

    #endregion

    #region SetScore

    private void SetScoreAndCatchNumberTxt()
    {
        score_txt.text = "Score: " + score.ToString();
        catchthenumber_txt.text = "Catch the number: " + catch_number.ToString();
    }
    private void SetScore()
    {
        GS.PlaySounds(0);
        Ground.SetActive(true);
        bestscore = false;
        score = 0;
        each100 = 100;
        SetScoreAndCatchNumberTxt();
    }
    public void UpdateScore(int number)
    {
        score += number;
        score_txt.gameObject.GetComponent<Animator>().Play("ScorePlusAnim", -1, 0f);
        MM.GameUI.GetComponent<Animator>().Play("CatchNumberAnim", -1, 0f);
        catch_number = Random.Range(0, 20);
        SetScoreAndCatchNumberTxt();

        if (score >= each100)
        {
            ComboScore();
        }
        if (score > best_score)
        {
            UpdateBestScore();
        }
    }

    #endregion

    #region UpdateComboAndBestScore

    private void ComboScore()
    {
        ScoreTypes.Instance.ShowComboScore();
        score_txt.gameObject.GetComponent<Animator>().Play("Score100Anim", -1, 0f);
        speed += 4f;
        each100 += 100;
    }
    private void UpdateBestScore()
    {
        best_score = score;
        if (bestscore == false)
        {
            ScoreTypes.Instance.ShowBestScore();
            bestscore = true;
        }
    }

    #endregion

    #region SaveLoadBestScore

    public void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", best_score);
    }

    private void LoadBestScore()
    {
        best_score = PlayerPrefs.GetInt("BestScore");
    }

    #endregion
}
