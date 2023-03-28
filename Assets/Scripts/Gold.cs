using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {
	public int goldAmount;



	void GivePlayerGold(Collider other){
		Scoreboard.goldAmount += goldAmount;
		Debug.Log ("Got gold coins: "+goldAmount);

		other.transform.parent.gameObject.GetComponent<Pawn> ().StartCoroutine(other.transform.parent.gameObject.GetComponent<Pawn> ().startCoinFlip());
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.transform.parent.tag == "Player") {
			GivePlayerGold (other);
			gameObject.transform.parent.gameObject.SetActive(false);
		}
	}

}
