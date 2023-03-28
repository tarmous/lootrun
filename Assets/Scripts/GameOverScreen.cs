using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	public static int goldAmount; //max gold gained
	public static int XPAmount;//max xp gained
	public static int levelbeatXP;//xp gained from beating level
	public static int levelProgressXP;//xp gaining by progressing through modules
	public static int XPFromGold;//xp gained from gold
	public static bool beatlevel;

	public GameObject WinText;
	public GameObject LoseText;

	public GameObject goGoldAmount; //max gold gained
	public GameObject goXPAmount;//max xp gained
	public GameObject goLevelBeatXP;//xp gained from beating level
	public GameObject goLevelProgressXP;//xp gaining by progressing through modules
	public GameObject goXPFromGold;//xp gained from gold

	// Use this for initialization
	void Start () {
		if (beatlevel) {
			LoseText.SetActive (false);
		} else {
			WinText.SetActive (false);
		}
		goGoldAmount.GetComponent<Text> ().text = "Gold: " + goldAmount;
		goXPAmount.GetComponent<Text> ().text = "EXP: " + XPAmount;
		goLevelBeatXP.GetComponent<Text> ().text = "Clear: " + levelbeatXP;
		goLevelProgressXP.GetComponent<Text> ().text = "Progress: " + levelProgressXP;
		goXPFromGold.GetComponent<Text> ().text = "Gold EXP: " + XPFromGold;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
