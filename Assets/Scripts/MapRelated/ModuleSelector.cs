using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSelector {
	TextAsset levelData;
	string[] levelDataString;
	string[][] levelDataRow; //string[Row][Rowvar]
	public List<GameObject> pickedObjects = new List<GameObject> (); //N: dimiourgise lista aop game objects
	int typeX,typeY,numToLoad;

	public ModuleSelector(){
		levelData = Resources.Load<TextAsset>("LevelBasedMapProgressionTable"); //load CSV
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
	


	private void levelBasedTypes ( out int typeX, out int typeY, out int numToLoad){
		UpdateProfileStatistics ups = new UpdateProfileStatistics ();
		int playerLevel = ups.getPlayerLevel ();
		typeX = int.Parse(levelDataRow [playerLevel] [1]);
		typeY = int.Parse(levelDataRow [playerLevel] [2]);
		numToLoad=int.Parse(levelDataRow [playerLevel] [3]);
	}
	//int numToLoad, int typeX, int typeY
	public List<GameObject> SelectModules(){
		levelBasedTypes(out typeX,out typeY,out numToLoad);

		pickedObjects.Clear (); //N: adiase tin lista me ta epilegmena modules
		Object[] loadedAssetsConnectors = Resources.LoadAll ("Modules", typeof(GameObject)); //N: get info for all modules
		Object[] loadedAssetsStart = Resources.LoadAll ("Modules/Start", typeof(GameObject)); //N: get info for all start modules
		Object[] loadedAssetsEnd= Resources.LoadAll ("Modules/End", typeof(GameObject)); //N: get info for all end modules
		int k = 0;
		int randomCounter=0;

		//Start Module
		randomCounter= Random.Range (0, loadedAssetsStart.Length);
		pickedObjects.Add ((GameObject)loadedAssetsStart[randomCounter]);

		//Connector Modules
		for (int i = 0; i < numToLoad-2;) {
			int j = Random.Range (0, loadedAssetsConnectors.Length); //N: get random from loaded modules
			//k++; //N: failsafe
			if ( loadedAssetsConnectors[j].ToString().StartsWith(typeX.ToString()+"_"+typeY.ToString()) ){ //N: look for module with certain name
				pickedObjects.Add ((GameObject)loadedAssetsConnectors [j]); //N: add loaded module to selected modules 
				i++;
			}
			if (k > numToLoad + 10) {
				// we should never reach here
				// something went wrong
				// cant read files correctly
				break;
			}
		}

		//End Module
		randomCounter= Random.Range (0, loadedAssetsEnd.Length);
		pickedObjects.Add ((GameObject)loadedAssetsEnd[randomCounter]);

		return pickedObjects;
	}

}
