using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		//Debug.Log ("enter");
		TileMap.instance.DestroyPawn (other.gameObject.transform.parent.GetComponent<Pawn> ());
	}
}
