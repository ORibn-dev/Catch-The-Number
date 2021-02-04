using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumberBox : MonoBehaviour, IObserver
{
    #region Events

    public event ScoreUpdater UpdateScore;
    public event SoundPlayer PlaySound;
    public event ObserverActions RemoveObserver;

    #endregion

    #region DeclaredFields 

    public int number;
    public float boxspeed;
    [SerializeField] private TextMesh num_text;

    #endregion

    #region SetNumBox

    private Statistics stat;
    private BoxandExplosionsPool BEP;

    public void SetNumBox(float speed)
    {
        stat = Statistics.Instance;
        BEP = BoxandExplosionsPool.Instance;
        boxspeed = speed;
        num_text.text = number.ToString();
    }

    #endregion

    #region NumBoxMovement

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * boxspeed);
        if (gameObject.transform.position.y <= 12)
        {
            PlaySound(3);
            if (number != stat.catch_number)
            {
                RemoveObserver(this);
                DestroyBox(2);
            }
            else if (number == stat.catch_number)
            {
                FailScreen();
            }
        }
    }
    #endregion

    #region NumBoxActions 

    void OnMouseDown()
    {
        if (number == stat.catch_number)
        {
            PlaySound(4);
            UpdateScore(number);
            RemoveObserver(this);
            DestroyBox(1);
        }
        else if (number != stat.catch_number)
        {
            PlaySound(5);
            FailScreen();
        }
    }

    private void FailScreen()
    {
        MainMenu.Instance.ShowFailScreen();
        SpawnNumberBoxes.Instance.DestroyAllBoxes();
    }

    #endregion

    #region DestroyThisNumBox

    public void Notify()
    {
        DestroyBox(0);
    }

    private void TurnOffBoxandTrail()
    {
        GetComponent<TrailRenderer>().Clear();
        gameObject.SetActive(false);
    }
    private void DestroyBox(int exp_indx)
    {
        BEP.InitiateExplosion(exp_indx, transform.position, transform.rotation);
        TurnOffBoxandTrail();
    }

    #endregion
}
