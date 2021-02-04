using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTypes : MonoBehaviour
{
    #region DeclaredFields

    [SerializeField] private GameObject smiley, BestScore_notif, canvas;
    private GameObject smiley_obj, bestscore_obj;

    #endregion

    #region Initialization

    public static ScoreTypes Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    #region TypesOfScore

    public void ShowComboScore()
    {
        GameSounds.Instance.PlaySounds(2);
        smiley_obj = Instantiate(smiley) as GameObject;
        smiley_obj.transform.SetParent(canvas.transform, false);
        smiley_obj.GetComponent<Animator>().Play("Smiley_anim");
        Destroy(smiley_obj, 1f);
    }
    public void ShowBestScore()
    {
        GameSounds.Instance.PlaySounds(1);
        bestscore_obj = Instantiate(BestScore_notif) as GameObject;
        bestscore_obj.transform.SetParent(canvas.transform, false);
        bestscore_obj.GetComponent<Animator>().Play("BestScore_anim");
        Destroy(bestscore_obj, 1f);
    }

    #endregion
}
