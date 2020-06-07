using UnityEngine;
using System.Collections;

public class MeshRendFix2 : MonoBehaviour {

	void Start () {
        GetComponent<MeshRenderer>().sortingLayerName = "Layer2";
        GetComponent<MeshRenderer>().sortingOrder = 0;	
	}
}
