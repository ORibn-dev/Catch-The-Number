using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumberBox : MonoBehaviour {

    public int number;
    public TextMesh num_text;
    public GameObject destruction_particle;
    public GameObject destruction_particle_ground;
    public GameObject destruction_particle_catch;
    public float boxspeed;
    public AudioClip expsound_right;
    public AudioClip expsound_wrong;
    public AudioClip expsound_ground;
    public Statistics stat;
    public AudioSource audios;
    private GameObject[] allnumboxes;
    private GameObject dp;
    private GameObject dp_g;
    private GameObject dp_c;

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * boxspeed);
        if (gameObject.transform.position.y <= 12)
        {
            PlayExpSound(expsound_ground);
            if (number != stat.catch_number)
            {
                dp_g = Instantiate(destruction_particle_ground, transform.position, transform.rotation) as GameObject;
                DestroyParticles(ref dp_g);
            }
            else if (number == stat.catch_number)
            {
                FailScreen();
            }
        }
    }

    void OnMouseDown()
    {
        if (number == stat.catch_number)
        {
            stat.score += number;
            PlayExpSound(expsound_right);
            stat.score_txt.gameObject.GetComponent<Animator>().Play("ScorePlusAnim", -1, 0f);
            stat.GameUI.GetComponent<Animator>().Play("CatchNumberAnim", -1, 0f);
            stat.catch_number = Random.Range(0, 20);
            stat.UpdateScoreAndCatchNumber();
            dp_c = Instantiate(destruction_particle_catch, transform.position, transform.rotation) as GameObject;
            DestroyParticles(ref dp_c);
        }
        else if (number != stat.catch_number)
        {
            PlayExpSound(expsound_wrong);
            FailScreen();
        }
    }

    private void DestroyAllBoxes()
    {
        allnumboxes = GameObject.FindGameObjectsWithTag("numbox");
        foreach (GameObject numbox in allnumboxes)
        {
            numbox.GetComponent<NumberBox>().DestoryThisBox();
            Destroy(numbox);
        }
    }

    private void PlayExpSound(AudioClip ac)
    {
        audios.clip = ac;
        audios.Play();
    }
    private void FailScreen()
    {
        stat.ShowFailScreen();
        DestroyAllBoxes();
    }

    public void DestoryThisBox()
    {
        dp = Instantiate(destruction_particle, transform.position, transform.rotation) as GameObject;
        Destroy(dp, 1f);
    }

    private void DestroyParticles(ref GameObject partexp)
    {
        Destroy(partexp, 2f);
        Destroy(gameObject);
    }
}
