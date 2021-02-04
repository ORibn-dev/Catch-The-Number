using UnityEngine;
using System.Collections;

public class MeshRendFix : MonoBehaviour {

	void Start () {
        GetComponent<MeshRenderer>().sortingLayerName = "Layer1";
        GetComponent<MeshRenderer>().sortingOrder = 1;	    
    }
	
}
