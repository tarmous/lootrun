using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountProfileInfoFetcher : MonoBehaviour {
	public UpdateProfileStatistics ups;

	// Use this for initialization
	void Start () {
		ups = new UpdateProfileStatistics ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate(){
		if (ups ==null) {
			ups = new UpdateProfileStatistics ();
		}

		if (gameObject.name == "AccountGold") {
			GetComponent<Text>().text="Account Gold: "+PlayerPrefs.GetInt ("goldAmount");
			//PlayerPrefs.SetInt ();
			//PlayerPrefs.GetInt ();
		}else if (gameObject.name == "AccountLevel") {
			GetComponent<Text>().text="Account Level: "+ups.getPlayerLevel();
		}else if (gameObject.name == "AccountCurrentXP") {
			GetComponent<Text>().text="Account Experience: "+PlayerPrefs.GetInt ("currentXP");
		}else if (gameObject.name == "AccountRequiredXP") {
			GetComponent<Text>().text="Account Required XP: "+ups.getRequiredXP();
		}
	}
}
