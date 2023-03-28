using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProfileStatistics {
	TextAsset levelData;
	string[] levelDataString;
	string[][] levelDataRow; //string[Row][Rowvar]
	private float goldToXPRatio=0.01f;

	public UpdateProfileStatistics (){
		levelData = Resources.Load<TextAsset>("PlayerProgressionTable"); //load CSV
		levelDataString = levelData.text.Split (new char[]{'\n'}); //split CSV into rows
		levelDataRow = new string[levelDataString.Length-1][]; 

		for (int i = 1; i < levelDataString.Length; i++) {
			string[] row = levelDataString[i].Split (new char[]{','}); //get the row from excel
			levelDataRow [i - 1] = new string[row.Length];
			for (int j = 0; j < row.Length; j++){
				levelDataRow [i - 1] [j] = row [j]; // store row variables in a huge public array
			}
		}
	}

	public void updateGold(int gold){
		PlayerPrefs.SetInt ("goldAmount",PlayerPrefs.GetInt("goldAmount")+gold);
		updateExp (goldConversion(gold));
	}

	public int goldConversion(int gold){
		 return Mathf.RoundToInt (gold * goldToXPRatio);
	}
	public void updateExp(float exp){
		//if currentXP > requiredXP
		if (PlayerPrefs.GetInt ("currentXP") + Mathf.RoundToInt (exp) > int.Parse (levelDataRow [PlayerPrefs.GetInt ("playerLevel")] [1])) {
			//if player level < list of levels
			if (PlayerPrefs.GetInt ("playerLevel") < levelDataRow.Length-2) {
				
				PlayerPrefs.SetInt ("currentXP", PlayerPrefs.GetInt ("currentXP") + Mathf.RoundToInt (exp));
				levelUp ();
			}
		} else {
			PlayerPrefs.SetInt ("currentXP", PlayerPrefs.GetInt("currentXP") + Mathf.RoundToInt(exp));
			levelUp ();
		}


		//levelDataRow [PlayerPrefs.GetInt ("playerLevel")] [1];
		//PlayerPrefs.SetInt ("requiredXP",0);
	}


	public void calculateExp(){
		PlayerPrefs.SetInt ("currentXP", PlayerPrefs.GetInt("currentXP") - int.Parse(levelDataRow [PlayerPrefs.GetInt ("playerLevel")] [1]));

	}

	protected void levelUp(){
		if (PlayerPrefs.GetInt ("currentXP") >= int.Parse(levelDataRow [PlayerPrefs.GetInt ("playerLevel")] [1])) {

			if (PlayerPrefs.GetInt ("playerLevel") < 0) {
				PlayerPrefs.SetInt ("playerLevel", 0);
			} else {
				if(PlayerPrefs.GetInt ("playerLevel") < levelDataRow.Length-1){
				calculateExp ();
				PlayerPrefs.SetInt ("playerLevel", PlayerPrefs.GetInt ("playerLevel")+1);
					levelUp ();
				}
			}
				
		}
	}

	public int getPlayerLevel(){
		if (PlayerPrefs.GetInt ("playerLevel") < 0) {
			PlayerPrefs.SetInt ("playerLevel", 0);
		} 
		return PlayerPrefs.GetInt ("playerLevel");
	}

	public int getRequiredXP(){
		return int.Parse (levelDataRow [PlayerPrefs.GetInt ("playerLevel")] [1]);
	}
}
