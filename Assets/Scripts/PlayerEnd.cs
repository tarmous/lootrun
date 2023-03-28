using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnd : MonoBehaviour {


	//private int finishXPBonus = 200;



	void OnTriggerEnter(Collider other){
		Debug.Log ("You beat the level");

		//UpdateProfileStatistics ups = new UpdateProfileStatistics ();
		//ups.updateGold (GoldScore.goldAmount);
		//ups.updateExp (finishXPBonus);
		//ups.updateExp (GoldScore.getAccumulatedXP());

		//PlayerPrefs.SetInt ("goldAmount",PlayerPrefs.GetInt("goldAmount")+GoldScore.goldAmount);
		//Application.LoadLevel (2);
		Scoreboard.BeatLevel();
		SceneManager.LoadSceneAsync (2);
	}
}
