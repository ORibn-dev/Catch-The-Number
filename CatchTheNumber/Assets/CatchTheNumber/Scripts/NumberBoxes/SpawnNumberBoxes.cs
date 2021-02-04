using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ScoreUpdater(int scoretoadd);
public delegate void SoundPlayer(int soundtoplay);
public delegate void ObserverActions(IObserver observ);

public class SpawnNumberBoxes : MonoBehaviour
{
    #region DeclaredFields

    public bool ingame;
    private float step;
    private int generateanswer_chance;

    private List<IObserver> observers_list = new List<IObserver>();

    #endregion

    #region SingletonReferences

    public static SpawnNumberBoxes Instance { get; private set; }
    private NumberBox numbox;
    private Statistics stat;
    private GameSounds gamesounds;

    #endregion

    #region Initialization

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        stat = Statistics.Instance;
        gamesounds = GameSounds.Instance;
    }

    #endregion

    #region SpawnBoxes

    void Update () 
    {
        if (ingame == true)
        {
            step += Time.deltaTime;
            if (step >= 2)
            {
                SpawnBoxes();
            }
        }
	}
    private void SpawnBoxes()
    {
        generateanswer_chance = Random.Range(1, 100);
        numbox = BoxandExplosionsPool.Instance.GetStuff<NumberBox>(BoxandExplosionsPool.Instance.pooled_boxes);
        numbox.transform.position = new Vector2(Random.Range(17.5f, 142.5f), transform.position.y);
        numbox.gameObject.SetActive(true);
        numbox.number = (generateanswer_chance <= 35) ? stat.catch_number : Random.Range(0, 20);
        numbox.SetNumBox(stat.speed);
        AddObserver(numbox);
        AddEvents();
        step = 0;
    }

    private void AddEvents()
    {
        /////////////RemoveEvents/////////////////
        numbox.UpdateScore -= stat.UpdateScore;
        numbox.PlaySound -= gamesounds.PlaySounds;
        numbox.RemoveObserver -= RemoveObserver;
        /////////////AddEvents////////////////////
        numbox.UpdateScore += stat.UpdateScore;
        numbox.PlaySound += gamesounds.PlaySounds;
        numbox.RemoveObserver += RemoveObserver;
    }

    #endregion

    #region ObserverPatternOperations

    private void AddObserver(IObserver observer)
    {
        observers_list.Add(observer);
    }
    public void RemoveObserver(IObserver observer)
    {
        observers_list.Remove(observer);
    }
    public void DestroyAllBoxes()
    {
        foreach (IObserver iob in observers_list)
        {
            iob.Notify();
        }
        observers_list.Clear();
    }

    #endregion
}
