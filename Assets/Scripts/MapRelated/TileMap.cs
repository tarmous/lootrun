using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TileMap : MonoBehaviour {

	public static TileMap instance;

	//_____Important______
	//tiletypes:
		//0: fall-pit
		//1: walkway
		//2: entrance
		//3: exit

	public static int numToLoad, typeX, typeY; //Variables for the Module. (numToLoad: number of modules. typeX and typeY are used for the name of the module "typeX_typeY_Module")
	public List<Module> module =new List<Module>(); //A list of files for all the modules.
	public List<GameObject> moduleGO = new List<GameObject>(); //the list of the modules currently running.
	//public GameObject[] pawns; 
	public List<Tile> tilesRef= new List<Tile>(); //the list of all the tiles currently running. 
	public List<Pawn> pawnRef= new List<Pawn>(); //the list of all the pawns currently running.
	private Vector3 tileOffset= new Vector3(0.5f, -0.5f, -0.5f);//used to position prefabs on center of tile
	public GameOver gameOverText ;

	//get Modules from Resources/Modules as Module Objects
	private List<Module> GetModules(){
		
		ModuleSelector ms = new ModuleSelector (); //Create the moduleSelector class
		List<Module> mo = new List<Module> (); 
		List<GameObject> moduleFile = ms.SelectModules (); 

		foreach (GameObject go in moduleFile) {
			mo.Add (go.GetComponent<Module> ()); 
		}
		return mo;
	}



	//Setup and spawn
	private void SetupModule(){

		//get modules
		module.Clear(); //empty the module file
		module = GetModules (); //run the GetModule function
		Tile[] list; //create the tile list array

		foreach (Module m in module) {
			
			GameObject tmpGO= Instantiate (m.gameObject, new Vector3 (0f,0f,0f), Quaternion.identity) as GameObject; //create the module in game
			list = tmpGO.GetComponentsInChildren<Tile> ();	//add the tiles of the module in the list.
			moduleGO.Add (tmpGO); //add the module on the active module list.

			//gia kathe tile stin lista twn tiles
			foreach (Tile t in list) {
				t.getNeighbours ().Clear (); //katharizei tois prohgoumenes times. Failsafe
			}

			//an h lista twn tiles uparxei. Tote gia kathe tile stin lista.
			if (list.Length > 0) {
				foreach (Tile t1 in list) {
					
					tilesRef.Add (t1); //valto sto tiles refrences
					if (t1.tileType == 2) //an einai entrance
						tmpGO.GetComponent<Module>().entrance = t1; //valto stin lista ws entrance
					if (t1.tileType == 3) //an einai exit
						tmpGO.GetComponent<Module>().exit = t1; //valto stin lista ws exit
					
					//sigkrine to tile me akthe all stin lista
					foreach (Tile t2 in list) {
						if( // if it's adjacent (distance = 1)
						(Mathf.Abs(t2.gameObject.transform.localPosition.x - t1.gameObject.transform.localPosition.x) + Mathf.Abs(t2.gameObject.transform.localPosition.y - t1.gameObject.transform.localPosition.y) == 1)
							&&
							(t1!=t2) //not the same tile
						)
							{
								t1.setNeighbours (t2); 
							}

					}
				}
			}


		}


	}




	//Align modules between entrance and exit (vertical connection only)
	private void AlignModules(){
		List<Module> tmpModule = new List<Module> (); //temporary list of modules
		foreach (GameObject m in moduleGO) {
			tmpModule.Add (m.GetComponent<Module> ()); //add in the temporary list. The module from the in-game list
		}
	
		//mexri na ftash sto proteleuteo module
		for (int i = 0; i < tmpModule.Count - 1; i++) {
			//vazei to position tou epomenou module. Vriskei tin thesi aferodas tin thesi tou entrance tou epomenou me tin thesi tou exit tou torinou kai meta to anevazei ena epano.
			tmpModule [i + 1].transform.position = Vector3.up * 1f + (tmpModule [i].exit.transform.position - tmpModule [i + 1].entrance.transform.position);

			tmpModule [i + 1].entrance.setNeighbours (tmpModule [i].exit);//theorise to exit tou torinou module ws neighbour sto entrance tou epomenou module
			tmpModule [i].exit.setNeighbours (tmpModule [i + 1].entrance);//theorise to entrance tou epomenou module ws neighbour tou entrance tou epomenou module
		}

		foreach (GameObject tmpGO in moduleGO) {
			spawnModuleMisc (tmpGO);
			setupPatrolPoints (tmpGO);
			SetupPawns (tmpGO);

		
		}
	}

	private void spawnModuleMisc(GameObject moduleref){
		List<Switch> moduleSwitches = new List<Switch> (); moduleSwitches.Clear ();
		List<Switch> moduleTraps = new List<Switch> (); moduleTraps.Clear ();

		//spawn gold
		for (int i = 0; i < moduleref.GetComponent<Module> ().goldSpawnLocations.Count; i++) {
			if (moduleref.GetComponent<Module> ().goldType [i]) {
				GameObject go = Instantiate (moduleref.GetComponent<Module> ().goldType [i], moduleref.GetComponent<Module> ().goldSpawnLocations [i].transform.position + tileOffset, Quaternion.identity) as GameObject;
				go.transform.parent = moduleref.transform;
			}
		}

		//spawns switches
		for (int i=0; i<moduleref.GetComponent<Module>().switchesSpawnLocations.Count; i++){
			if (moduleref.GetComponent<Module> ().switchType [i]) {
				GameObject go = Instantiate (moduleref.GetComponent<Module> ().switchType [i], moduleref.GetComponent<Module> ().switchesSpawnLocations [i].transform.position + tileOffset, Quaternion.identity) as GameObject;
				go.GetComponentInChildren<Switch> ().switchID = moduleref.GetComponent<Module> ().switchesID [i];
				moduleSwitches.Add (go.GetComponentInChildren<Switch> ());//get a reference for connecting switches and traps
			}
		}

		//spawn traps
		for (int i = 0; i < moduleref.GetComponent<Module> ().trapSpawnLocations.Count; i++) {
			if (moduleref.GetComponent<Module> ().trapType [i]) {
				GameObject go = Instantiate (moduleref.GetComponent<Module> ().trapType [i], moduleref.GetComponent<Module> ().trapSpawnLocations [i].transform.position + tileOffset, Quaternion.Euler(0f,0f,-90f*moduleref.GetComponent<Module> ().trapDirection [i])) as GameObject;
				//switch related
				if (moduleref.GetComponent<Module> ().switchesToTraps.Contains (i)) {
					go.AddComponent<Switch> ();
					go.GetComponent<Switch> ().hasTrap = true;
					moduleTraps.Add (go.GetComponent<Switch> ());//get a reference for connecting switches and traps
					for(int j=0; j<moduleref.GetComponent<Module> ().switchesToTraps.Count;j++){
						if(moduleref.GetComponent<Module> ().switchesToTraps[j]==i){
							go.GetComponent<Switch> ().switchID = moduleref.GetComponent<Module> ().switchesToTrapsID [j];
							if (moduleref.GetComponent<Module> ().switchesShouldStartActive.Count > j) {
								go.GetComponent<Switch> ().shouldStartActivated = moduleref.GetComponent<Module> ().switchesShouldStartActive [j];
							}
							break;
						}
					}

				}

				//trap timers
				//for spike traps
				if (go.GetComponentInChildren<SpikeTrap> ()) {
					if(moduleref.GetComponent<Module> ().trapTimer.Count >= i+1) {
						go.GetComponentInChildren<SpikeTrap> ().trapTimer = moduleref.GetComponent<Module> ().trapTimer [i];
					} else {
						go.GetComponentInChildren<SpikeTrap> ().trapTimer = 0f;
					}
				}
				//for flame traps
				if (go.GetComponent<FlameTrap> ()) {
					if(moduleref.GetComponent<Module> ().trapTimer.Count >= i+1) {
						go.GetComponentInChildren<FlameTrap> ().trapTimer = moduleref.GetComponent<Module> ().trapTimer [i];

					} else {
						go.GetComponentInChildren<FlameTrap> ().trapTimer = 0f;
					}

					if (moduleref.GetComponent<Module> ().trapSize.Count >= i + 1) {
						go.GetComponentInChildren<FlameTrap> ().trapSize = moduleref.GetComponent<Module> ().trapSize [i];
					}
				}
			}
		}

		//connect switches with traps

		for(int i = 0; i < moduleSwitches.Count; i++){
			int k=0;
			for (int j = 0; j < moduleTraps.Count; j++) {
				if (moduleSwitches [i].switchID == moduleTraps [j].switchID) {
					moduleSwitches [i].trapReference.Add (moduleTraps [j]);
					if(!moduleSwitches [i].trapReference [k].shouldStartActivated){
						moduleSwitches [i].trapReference [k].deActivate ();
					}else{
					moduleSwitches [i].trapReference [k].activate ();
					}
					k++;
				}
				//moduleSwitches
				//moduleTraps
			}
		}

		//spawn player End trigger?!
		//already created in import script

	}

	private void SetupPawns(GameObject moduleref)
	{
		//spawn enemies
		for (int i = 0; i < moduleref.GetComponent<Module> ().enemySpawnLocations.Count; i++) {
			if (moduleref.GetComponent<Module> ().enemyType [i]) {
				GameObject go = Instantiate (moduleref.GetComponent<Module> ().enemyType [i], moduleref.GetComponent<Module> ().enemySpawnLocations [i].transform.position + tileOffset, Quaternion.Euler(-90f,0,0)) as GameObject;
				//go.transform.parent = moduleref.transform;
				pawnRef.Add (go.GetComponent<Pawn> ());

				foreach (Tile t in moduleref.GetComponentsInChildren<Tile>()) {
					if (t.transform.localPosition == moduleref.GetComponent<Module> ().enemySpawnLocations [i].transform.localPosition) {
						moduleref.GetComponent<Module> ().enemySpawnLocations [i] = t.gameObject;
						
						break;
					}
				}
					
				moduleref.GetComponent<Module> ().enemySpawnLocations[i].GetComponent<Tile> ().setOnTile (go.gameObject);
				go.GetComponent<Pawn> ().setTileOn (moduleref.GetComponent<Module> ().enemySpawnLocations[i]);

				if (moduleref.GetComponent<Module> ().enemyDirection.Count >= i + 1) {
					switch (moduleref.GetComponent<Module> ().enemyDirection [i]) {
					case 0:
						go.GetComponent<Pawn> ().startDirection = new Vector3 (0, 1, 0);
						break;
					case 1:
						go.GetComponent<Pawn> ().startDirection = new Vector3 (1, 0, 0);
						break;
					case 2:
						go.GetComponent<Pawn> ().startDirection = new Vector3 (0, -1, 0);
						break;
					case 3:
						go.GetComponent<Pawn> ().startDirection = new Vector3 (-1, 0, 0);
						break;
					default:
						break;
					}
				} else {
					go.GetComponent<Pawn> ().startDirection = new Vector3 (0, 1, 0);
				}
			}
		}

		//spawn player
		if (moduleref.GetComponent<Module> ().playerStart) {
			GameObject go = Instantiate (moduleref.GetComponent<Module> ().playerPrefab, moduleref.GetComponent<Module> ().playerStart.transform.position + tileOffset, Quaternion.Euler(-90f,0,0)) as GameObject;
			//go.transform.parent = moduleref.transform;
			pawnRef.Add (go.GetComponent<Pawn> ());

			moduleref.GetComponent<Module> ().playerStart.GetComponent<Tile> ().setOnTile (go.gameObject);
			go.GetComponent<Pawn> ().setTileOn (moduleref.GetComponent<Module> ().playerStart);
		}
	}

	private void setupPatrolPoints(GameObject go){

		PatrolPoint[] tmpArray = go.GetComponentsInChildren<PatrolPoint> ();
		
		if (tmpArray.Length > 1) {
			//Sort them
			for (int i = 0; i <= tmpArray.Length - 2; i++) {
				for (int j = i+1; j <= tmpArray.Length - 1; j++) {
					if (tmpArray [i].patrolPointID > tmpArray [j].patrolPointID) {
						PatrolPoint tmpPatrolPoint = tmpArray [i];
						tmpArray [i] = tmpArray [j];
						tmpArray [j] = tmpPatrolPoint;
					}
					
				}
			}

			//set next and previous points
			for (int i = 0; i <= tmpArray.Length - 1; i++) {
				if (tmpArray [i].isLast) {
					//dont connect anything.
					//jump to the next
					continue;
				}

				//if we didnt find next then "i" is last in a circle type patrol.
				//if isLast existed we would have backtracking type patrol already setup from code bellow.
				bool foundNext = false;
				if (i + 1 <= tmpArray.Length - 1) {
					for (int j = i + 1; j <= tmpArray.Length - 1; j++) {
						if (tmpArray [i].patrolGroup != tmpArray [j].patrolGroup) {
							continue;
						} else {
							//if they are on same group: connect them;
							foundNext = true;
							tmpArray [i].next = tmpArray [j].gameObject;
							tmpArray [j].prev = tmpArray [i].gameObject;
							break;
						}
					}
				}

				if (!foundNext) {
					for (int k = i-1; k >= 0; k--) {
						if (tmpArray [k].patrolPointID == 0 && tmpArray [i].patrolGroup == tmpArray [k].patrolGroup) {
							tmpArray [i].next = tmpArray [k].gameObject;
							tmpArray [k].prev = tmpArray [i].gameObject;
							break;
						}
				}
			}
		}
	}
	}

	public void DestroyPawn(Pawn p){

		if (pawnRef.Contains (p)) //an to pawn einai to analogo
		{	
			pawnRef.Remove (p); //aferese to refrence apo tin lista
			if(p.tag=="Player"){
				Debug.Log ("You died");
				Scoreboard.gameOver ();//give player xp and update end/win screen
				SceneManager.LoadSceneAsync (2);
				//gameOverText.gameObject.SetActive( true );
				//spawnUI
			}
			Destroy (p.gameObject); //destroy ton pekth

		}

	}



	// Use this for initialization
	void Start () {
		instance = this;
		SetupModule (); //Setups the module
		AlignModules (); //aligns this module with the previous one
		//SetupPawns (); //setups pawns

		gameOverText = FindObjectOfType<GameOver> ();
		gameOverText.gameObject.SetActive (false);
	}
		
}
