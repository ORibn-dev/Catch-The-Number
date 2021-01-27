using UnityEngine;
using System.Collections;

public class SpawnNumberBoxes : MonoBehaviour {

    public AudioSource saudios;
    public bool ingame;
    public BoxandExplosionsPool BaEPool;
    private float step;
    private int generateanswer_chance;
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
        nb = BaEPool.GetStuff<NumberBox>(BaEPool.pooled_boxes);
        nb.transform.position = new Vector2(Random.Range(17.5f, 142.5f), transform.position.y);
        nb.gameObject.SetActive(true);
        nb.inst = st;
        nb.BEP = BaEPool;
        nb.audios = saudios;
        nb.number = (generateanswer_chance <= 35) ? st.catch_number : Random.Range(0, 20);
        nb.num_text.text = nb.number.ToString();
        nb.boxspeed = st.speed;
        step = 0;
    }
}
