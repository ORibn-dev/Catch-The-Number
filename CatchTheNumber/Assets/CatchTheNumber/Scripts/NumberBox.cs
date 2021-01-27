using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumberBox : MonoBehaviour {

    public int number;
    public TextMesh num_text;
    public float boxspeed;
    public AudioClip expsound_right, expsound_wrong, expsound_ground;
    public AudioSource audios;
    public Statistics inst;
    public BoxandExplosionsPool BEP;
    private NumberBox[] allnumboxes;
    private GameObject dp, dp_g, dp_c;

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * boxspeed);
        if (gameObject.transform.position.y <= 12)
        {
            PlayExpSound(expsound_ground);
            if (number != inst.catch_number)
            {
                BEP.InitiateExplosion(2, transform.position, transform.rotation);
                TurnOffBoxandTrail();
            }
            else if (number == inst.catch_number)
            {
                FailScreen();
            }
        }
    }

    void OnMouseDown()
    {
        if (number == inst.catch_number)
        {
            PlayExpSound(expsound_right);
            inst.UpdateScore(number);
            BEP.InitiateExplosion(1, transform.position, transform.rotation);
            TurnOffBoxandTrail();
        }
        else if (number != inst.catch_number)
        {
            PlayExpSound(expsound_wrong);
            FailScreen();
        }
    }

    private void PlayExpSound(AudioClip ac)
    {
        audios.clip = ac;
        audios.Play();
    }
    private void FailScreen()
    {
        inst.ShowFailScreen();
        BEP.DestroyAllBoxes();
    }
    private void TurnOffBoxandTrail()
    {
        GetComponent<TrailRenderer>().Clear();
        gameObject.SetActive(false);
    }
}
