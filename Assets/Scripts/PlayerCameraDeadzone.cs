using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraDeadzone : MonoBehaviour {


	void OnTriggerExit(Collider other){
		//Destroy only what is under the box
		if (other.gameObject.transform.position.y > this.gameObject.transform.position.y) {
			return;
		}

		if (other.tag == "Pawn" || other.tag=="Player") {
			TileMap.instance.DestroyPawn (other.GetComponentInParent<Pawn> ());
			return;
		}

		if (other.tag == "Tile" || other.tag == "Module") {
			Destroy(other.gameObject);
			return;
		}

		if (other.transform.parent != null) {
			//Debug.Log (other.name);
			Destroy(other.gameObject.transform.parent.gameObject);
			return;
		}
		//Destroy(other.gameObject);
	}
		
}
