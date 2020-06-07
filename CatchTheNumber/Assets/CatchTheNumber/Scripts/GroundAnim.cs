using UnityEngine;
using System.Collections;

public class GroundAnim : MonoBehaviour {

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

	void Update () {
        rend.material.mainTextureOffset = new Vector2(Time.time / 1.5f, 0);
	}
}
