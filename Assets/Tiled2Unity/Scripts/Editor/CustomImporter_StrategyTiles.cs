using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_StrategyTiles : Tiled2Unity.ICustomTiledImporter {

	private int goldAmount=0;
	private List<GameObject> goldSpawnLocations = new List<GameObject>();
	private List<GameObject> goldType = new List<GameObject>();
	private List<GameObject> trapSpawnLocations =new List<GameObject>();
	private List<int> trapDirection= new List<int>();
	private List<float> trapTimer = new List<float> ();
	private List<int> trapSize = new List<int> ();
	private List<GameObject> trapType = new List<GameObject>();
	private List<GameObject> enemySpawnLocations = new List<GameObject>();
	private List<GameObject> enemyType = new List<GameObject>();
	private GameObject playerStart=null;
	private GameObject playerPrefab = null;
	private List<int> enemyDirection = new List<int> ();
	private List<int> switchesToTraps = new List<int> ();
	private List<int> switchesToTrapsID = new List<int> ();
	private List<int> switchesID = new List<int> ();
	private List<bool> switchesShouldStartActive= new List<bool>();
	private List<GameObject> switchesSpawnLocations = new List<GameObject> ();
	private List<GameObject> switchType = new List<GameObject>();
	//private Dictionary<int, GameObject> patrolPoints= new Dictionary<int, GameObject>(); 


	public void HandleCustomProperties(GameObject go, IDictionary<string, string> customProperties){


		if(customProperties.ContainsKey("tileType")){
			//add the tiletype game object
			go.tag="Tile";
			Tile tile = go.AddComponent<Tile>();
			tile.tileType = int.Parse(customProperties["tileType"]);
			tile.setMovePoints (int.Parse(customProperties["movePoints"]));

			if (customProperties.ContainsKey ("patrolPoint")) {
				go.AddComponent<PatrolPoint>() ;
				go.GetComponent<PatrolPoint> ().patrolPointID = int.Parse (customProperties ["patrolPoint"]);

				if (customProperties.ContainsKey ("patrolGroup")) {
					go.GetComponent<PatrolPoint> ().patrolGroup = int.Parse (customProperties ["patrolGroup"]);
				} else {
					go.GetComponent<PatrolPoint> ().patrolGroup = 1;
				}
				
				if (customProperties.ContainsKey ("isLast")) {
					go.GetComponent<PatrolPoint> ().isLast = true;
				}
				//patrolPoints [int.Parse (customProperties ["patrolPoint"])] = go;
			}

			if (customProperties.ContainsKey ("isHazard")) {
				MonoBehaviour.DestroyImmediate (go.GetComponent<BoxCollider2D> ());
				go.AddComponent<BoxCollider> ();
				go.GetComponent<BoxCollider> ().size = new Vector3 (0.8f,0.8f,2.0f);
				go.GetComponent<BoxCollider> ().center = go.GetComponent<BoxCollider> ().center + new Vector3 (0.5f,-0.5f,0);
				go.GetComponent<BoxCollider> ().isTrigger = true;
				go.AddComponent<Hazard> ();

			}
			if (customProperties.ContainsKey ("levelEnd")) {
				MonoBehaviour.DestroyImmediate (go.GetComponent<BoxCollider2D> ());
				go.AddComponent<BoxCollider> ();
				go.GetComponent<BoxCollider> ().size = new Vector3 (0.8f,0.8f,2.0f);
				//go.GetComponent<BoxCollider> ().center = go.GetComponent<BoxCollider> ().center + new Vector3 (0.5f,-0.5f,-1f);
				go.GetComponent<BoxCollider> ().isTrigger = true;
				go.AddComponent<PlayerEnd> ();

			}

			if (customProperties.ContainsKey ("levelStart")) {
				playerStart = go;

				Object[] loadedAssets = Resources.LoadAll ("Pawns", typeof(GameObject));
				foreach (Object o in loadedAssets) {
					if (o.ToString ().StartsWith("PlayerWrapper")) {
						//THIS SHOULD NEVER FAIL
						//CHECK IF NAME CORRESPONDS CORRECLTY IF IT DOES
						playerPrefab = (GameObject)o;
					}
				}
				
			}

		}

		if (customProperties.ContainsKey("isWall")){
			MonoBehaviour.DestroyImmediate (go.GetComponent<BoxCollider2D> ());
			go.AddComponent<BoxCollider> ();
			go.GetComponent<BoxCollider> ().size = new Vector3 (1.0f,1.0f,2.0f);
			go.GetComponent<BoxCollider> ().center = go.GetComponent<BoxCollider> ().center + new Vector3 (0.5f,-0.5f,-1f);
		}

		if (customProperties.ContainsKey ("hasGold")) {
			//Debug.Log (int.Parse(customProperties ["goldAmount"]));
			//goldAmount += int.Parse(customProperties ["goldAmount"]);
			goldSpawnLocations.Add(go);

			Object[] loadedAssets = Resources.LoadAll ("Gold", typeof(GameObject));
			foreach (Object o in loadedAssets) {
				GameObject tmpGO = (GameObject)o;
				if (tmpGO.GetComponentInChildren<Gold> ().goldAmount == int.Parse (customProperties ["goldAmount"])) {
					goldType.Add (tmpGO);
					break;
				}
			}
		}



		if (customProperties.ContainsKey ("hasTrap")) {
			//if trap has a switch mechanism
			if (customProperties.ContainsKey ("switchID")) {
				//go.GetComponent<Switch> ().hasTrap = true;
				switchesToTraps.Add (trapType.Count);
				switchesToTrapsID.Add(int.Parse (customProperties ["switchID"]));
				if (customProperties.ContainsKey ("shouldStartActive")) {
					switchesShouldStartActive.Add (bool.Parse (customProperties ["shouldStartActive"]));
				} else {
					switchesShouldStartActive.Add (false);
				}
			}

			trapSpawnLocations.Add(go);
			Object[] loadedAssets = Resources.LoadAll ("Traps", typeof(GameObject));
			foreach (Object o in loadedAssets) {
				if (o.ToString ().StartsWith(customProperties ["trapType"]) ) {
					
					trapType.Add((GameObject)o);

					if (customProperties.ContainsKey ("trapDirection")) {
						trapDirection.Add(int.Parse (customProperties ["trapDirection"]));
					} else {
						trapDirection.Add(0);
					}
					if (customProperties.ContainsKey ("trapSize")) {
						//trapType [trapType.Count - 1].GetComponent<FlameTrap> ().trapSize = int.Parse (customProperties ["trapSize"]);
						trapSize.Add (int.Parse (customProperties ["trapSize"]));
					} else {
						trapSize.Add (0);
					}
					if (customProperties.ContainsKey ("trapTimer")) {
						trapTimer.Add (float.Parse (customProperties ["trapTimer"]));
					} else {
						trapTimer.Add (0);
					}
					}
				}
		}else if (customProperties.ContainsKey ("switchID")) {

			switchesSpawnLocations.Add (go);
			Object[] loadedAssets = Resources.LoadAll ("Misc", typeof(GameObject));
			foreach (Object o in loadedAssets) {
				if (o.ToString ().StartsWith (customProperties ["switchType"])) {
					switchType.Add ((GameObject)o);
					switchesID.Add (int.Parse (customProperties["switchID"]));
					break;
				}
			}
			/*
			MonoBehaviour.DestroyImmediate (go.GetComponent<BoxCollider2D> ());
			go.AddComponent<BoxCollider> ();
			go.GetComponent<BoxCollider> ().size = new Vector3 (0.8f,0.8f,2.0f);
			go.GetComponent<BoxCollider> ().center = go.GetComponent<BoxCollider> ().center + new Vector3 (0.5f,-0.5f,0);
			go.GetComponent<BoxCollider> ().isTrigger = true;
			go.AddComponent<Switch>();
			//update variables
			go.GetComponent<Switch>().switchID= int.Parse(customProperties["switchID"]);
			*/
		}


		if (customProperties.ContainsKey ("hasEnemy")) {
			enemySpawnLocations.Add(go);
			enemyDirection.Add (int.Parse(customProperties["enemyDirection"]));
			Object[] loadedAssets = Resources.LoadAll ("Pawns", typeof(GameObject));
			foreach (Object o in loadedAssets) {
				
				if (o.ToString ().StartsWith(customProperties ["enemyType"])) {
					enemyType.Add((GameObject)o);
				}
			}
		}

	}

	public void CustomizePrefab(GameObject go){
		//Do Nothing
		Module mo = go.AddComponent<Module> ();
		go.tag="Module";
		//if (!go.GetComponent<Module> ()) {
		//	mo = go.AddComponent<Module> ();
		//} else {
		//	mo = go.GetComponent<Module> ();
		//}

		if (goldType.Count > 0) {
			foreach (GameObject tmpGO in goldType) {
				Gold g = tmpGO.GetComponentInChildren<Gold> ();
				goldAmount+=g.goldAmount;
			}
			mo.moduleGold = goldAmount;
			mo.goldSpawnLocations = goldSpawnLocations;
			mo.goldType = goldType;
		}

		if (trapSpawnLocations != null) {
			mo.trapSpawnLocations=trapSpawnLocations;
			mo.trapType=trapType;
			mo.trapDirection = trapDirection;
			mo.trapTimer = trapTimer;
			mo.trapSize = trapSize;
		}

		if (enemySpawnLocations != null) {
			mo.enemySpawnLocations=enemySpawnLocations;
			mo.enemyType=enemyType;
			mo.enemyDirection = enemyDirection;
		}

		if (playerStart != null) {
			mo.playerStart = playerStart;
			mo.playerPrefab = playerPrefab;
		}

		if (switchesToTraps.Count > 0) {
			mo.switchesToTraps = switchesToTraps;
			mo.switchesToTrapsID = switchesToTrapsID;
			mo.switchesID = switchesID;
			mo.switchesSpawnLocations = switchesSpawnLocations;
			mo.switchType = switchType;

			if (switchesShouldStartActive.Count > 0) {
				mo.switchesShouldStartActive = switchesShouldStartActive;
			}
		}
		//if(patrolPoints.Count>0){
		//	mo.patrolPoints = patrolPoints;
		//}
	}

}


