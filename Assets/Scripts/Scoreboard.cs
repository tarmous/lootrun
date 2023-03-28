using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard: MonoBehaviour  {

	//public static Scoreboard instance;
	public static int goldAmount;
	public static int XPAmount;//max xp gained
	public static int levelbeatXP=200;//xp gained from beating level
	private static int levelProgressPerModuleXP=10;//xp gaining by progressing through modules
	public static int levelProgressXP;//xp gaining by progressing through modules
	//public static int XPFromGold;//xp gained from gold
	//public static bool beatlevel;
	private static Text text;

	// Use this for initialization
	void Start () {
		goldAmount = 0;
		levelProgressXP = 0;
		//instance = this;
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Gold: " + goldAmount;
		
	}

	public static void updateAccumulatedXP (){
		levelProgressXP += levelProgressPerModuleXP;
	}

	public static void BeatLevel (){
		UpdateProfileStatistics ups = new UpdateProfileStatistics ();

		GameOverScreen.goldAmount=goldAmount; //max gold gained
		GameOverScreen.levelbeatXP=levelbeatXP;//xp gained from beating level
		GameOverScreen.XPFromGold=ups.goldConversion(goldAmount);//xp gained from gold
		GameOverScreen.levelProgressXP=levelProgressXP;//xp gaining by progressing through modules
		GameOverScreen.XPAmount=GameOverScreen.levelbeatXP + GameOverScreen.XPFromGold + GameOverScreen.levelProgressXP;//max xp gained
		GameOverScreen.beatlevel=true;

		ups.updateGold (goldAmount);
		ups.updateExp (GameOverScreen.XPAmount);
		
	}

	public static void gameOver (){
		UpdateProfileStatistics ups = new UpdateProfileStatistics ();

		GameOverScreen.goldAmount=0; //max gold gained
		GameOverScreen.levelbeatXP=0;//xp gained from beating level
		GameOverScreen.XPFromGold=0;//xp gained from gold
		GameOverScreen.levelProgressXP=levelProgressXP;//xp gaining by progressing through modules
		GameOverScreen.XPAmount=GameOverScreen.levelProgressXP;//max xp gained
		GameOverScreen.beatlevel=false;

		ups.updateExp (GameOverScreen.XPAmount);
		
	}
}
