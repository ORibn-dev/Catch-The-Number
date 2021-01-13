using UnityEngine;
using System.Collections;

public class SpawnNumberBoxes : MonoBehaviour {

    public GameObject number_box;
    public AudioSource saudios;
    public bool ingame;
    private float step;
    private int generateanswer_chance;
    private GameObject number_b;
    private NumberBox nb;
    private Statistics st;

    void Start()
    {
        st = Statistics.Instance;
    }
	
	void Update () {
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
        number_b = Instantiate(number_box, new Vector2(Random.Range(17.5f, 142.5f), transform.position.y), transform.rotation) as GameObject;
        nb = number_b.GetComponent<NumberBox>();
        nb.inst = st;
        nb.audios = saudios;
        nb.number = (generateanswer_chance <= 35) ? st.catch_number : Random.Range(0, 20);
        nb.num_text.text = nb.number.ToString();
        nb.boxspeed = st.speed;
        step = 0;
    }
}
