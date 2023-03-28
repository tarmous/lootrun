using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public GameObject selectedObject=null;
	public bool isSwipeObjectSet=false;

	//_______________________________________________________
	//_______________________________________________________
	//_______________________________________________________
	public void CheckMove(GameObject toMove, GameObject selected, float towardsV, float towardsH)
	{
		if (toMove == null) { //an einai na kinithei
			selected.GetComponent<Pawn>().lockPlayerMovement=true;
			toMove = selected.GetComponent<Pawn> ().getTileOn (); //pernei to tile pou vriskete to adikeimeno pou einai na kinithei
			List<Tile> neighbours = new List<Tile> (); //ftiaxe nea lista neighbours
			neighbours = toMove.GetComponent<Tile> ().getNeighbours (); //pernei olous tous neighbours sto simeio pou vriskete to Pawn

			foreach (Tile t in neighbours) { //kita se pio tile tha metakinithei

				if (towardsV != 0) { 
					if (t.transform.position.y > toMove.transform.position.y) {
						if (towardsV >= 0) {
							toMove = t.gameObject; //metakinisou pros ta epano
							break;
						}
					} else if (t.transform.position.y < toMove.transform.position.y) {
						if (towardsV <= 0) {
							toMove = t.gameObject; //metakinisou pros ta kato
							break;
						}
					}
				}

				if (towardsH != 0) {
					if (t.transform.position.x > toMove.transform.position.x) {
						if (towardsH >= 0) {
							toMove = t.gameObject; //metakinisou deksia
							break;
						}
					} else if (t.transform.position.x < toMove.transform.position.x) {
						if (towardsH <= 0) {
							toMove = t.gameObject; //metakinisou aristera
							break;
						}
					}
				}
			}
			selected.GetComponent<Pawn> ().currentPath.Clear (); //kane clear oti eixe prin
			selected.GetComponent<Pawn> ().currentPath.Add (toMove.GetComponent<Tile>()); 

			//if player moves on enemy kill him
			if(selected.GetComponent<Pawn> ().currentPath [0].getOnTile()){
			if (selected.GetComponent<Pawn> ().currentPath [0].getOnTile ().tag == "Pawn" && selected.tag=="Player") {
					TileMap.instance.DestroyPawn (selectedObject.GetComponent<Pawn>());
					//Destroy (selectedObject);
					//KILL PLAYER ?!
				}}

			selected.GetComponent<Pawn> ().getTileOn ().GetComponent<Tile> ().setOnTile (null); 
			selected.GetComponent<Pawn> ().setTileOn (selected.GetComponent<Pawn> ().currentPath [0].gameObject); 
			selected.GetComponent<Pawn> ().currentPath [0].setOnTile (selected); 
			selected.GetComponent<Pawn> ().startMoving (); 
			return;
		}
		DijkstrasAlgorithmWalk (selected.GetComponent<Pawn>().getTileOn(), toMove, selected); //ektelese to Dijkstra algorithmo gia ta enemies
	}

	//_______________________________________________________
	//_______________________________________________________
	//_______________________________________________________
	public void DijkstrasAlgorithmWalk(GameObject s, GameObject t, GameObject ObjectToMove){

		if(ObjectToMove.GetComponent<Pawn>().currentPath.Count==0){
		Tile source = s.GetComponent<Tile> (); 
		Tile target = t.GetComponent<Tile> (); 
		Dictionary<Tile, float> dist = new Dictionary<Tile, float> (); 
		Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile> (); 
		List<Tile> notVisited = new List<Tile> (); 
		dist [source] = 0; 
		prev [source] = null; 

		//distance=infinity
		//some nodes might not be able to be reached
		foreach (Tile v in TileMap.instance.tilesRef) {
			if (v != source) { 
				dist [v] = Mathf.Infinity; 
				prev [v] = null; 
			}
			notVisited.Add (v); 
		}

		while (notVisited.Count > 0) { 

			Tile u = null; 
			foreach (Tile tile in notVisited) { 
				if (u == null || dist [tile] < dist [u]) { 
					u = tile;
				}
			}
				
			if (u == target) {
				break;
			}

			notVisited.Remove (u); 
			//if ((!u.getOnTile () && u != source) || (u.getOnTile () && u == source)){
			if ((u != source) || (u == source)) {
				
				foreach (Tile v in u.getNeighbours()) { 
					//if (!v.getOnTile ()) {
					int alt = (int)dist [u] + v.getMovePoints (); 
					if (alt < dist [v]) { 
						dist [v] = alt; 
						prev [v] = u; 
					}
					//}
				}
			}

		}//end_While

		if (prev [target] == null) {
			//no route to target
			//Debug.Log("No Route");
			return;
		}

		//create the path
		ObjectToMove.GetComponent<Pawn> ().currentPath.Clear (); 
		Tile current = target; 
		while (current) {
			ObjectToMove.GetComponent<Pawn> ().currentPath.Add (current);
			current = prev [current];
		}

		ObjectToMove.GetComponent<Pawn> ().currentPath.Reverse ();
		ObjectToMove.GetComponent<Pawn> ().currentPath.RemoveAt (0);

		//Pawns cant walk over other pawns
		//pawns kill player wilst walking on them
		if (ObjectToMove.GetComponent<Pawn> ().currentPath [0].getOnTile ()) {
			if (ObjectToMove.GetComponent<Pawn> ().currentPath [0].getOnTile ().tag == "Pawn") {
				ObjectToMove.GetComponent<Pawn> ().currentPath.Clear ();
				return;
			} else if (ObjectToMove.GetComponent<Pawn> ().currentPath [0].getOnTile ().tag == "Player") {
				TileMap.instance.DestroyPawn (selectedObject.GetComponent<Pawn> ());
				//Destroy (selectedObject);
				//KILL PLAYER ?!
			}
		}
	}
		if(ObjectToMove.GetComponent<Pawn> ().getSawPlayer()){
		ObjectToMove.GetComponent<Pawn> ().getTileOn ().GetComponent<Tile> ().setOnTile (null);  //N:bgazoume to tile pou kathete o enemy
		ObjectToMove.GetComponent<Pawn> ().setTileOn (ObjectToMove.GetComponent<Pawn> ().currentPath [0].gameObject); //N:pes tou tile oti kathete epanosou o enemy
		ObjectToMove.GetComponent<Pawn> ().currentPath [0].setOnTile (ObjectToMove); //N:bazoume to tile pou tha kinithei o enemy

		ObjectToMove.GetComponent<Pawn> ().startMoving ();//N:Metakinhse ton enemy
		}else{
			
		}
		return;
	}
		

	//_______________________________________________________
	//_______________________________________________________
	//_______________________________________________________
	public void LineOfSight(Pawn p){
			RaycastHit hit; //N: girna oti vrikes kati
			//x1->x2 ||x2-x1,y2-y1
			if( Mathf.Sqrt ( Mathf.Pow((p.transform.position.x-selectedObject.transform.position.x),2f) + Mathf.Pow((p.transform.position.y-selectedObject.transform.position.y),2f) ) <3.5f ){
			Debug.Log ("Close");
			//Debug.Log (Vector3.Angle(selectedObject.transform.position-p.transform.position, p.lookDirection));
			if (Vector3.Angle(selectedObject.transform.position-p.transform.position, p.lookDirection) < 30f ){
				//Debug.Log ("can see");
				if (Physics.Raycast (p.gameObject.transform.Find("Soldier").transform.position, selectedObject.transform.position - p.transform.position, out hit, 3.5f)){
					//print(hit.collider.gameObject.tag);
					if (hit.collider.gameObject.tag == "Player") 
					{
						//print ("game object tag is player");
						p.setSawPlayer(true);
					}

				}
			}
		}
	}


	//_______________________________________________________
	//_______________________________________________________
	//_______________________________________________________
	public void MoveEnemies(){
		foreach(Pawn p in TileMap.instance.pawnRef){ 

			if (p.tag != "Player") { 

				if (!p.getSawPlayer ()) {
					LineOfSight (p);
					//start moving the next turn
					continue;
				}
				
				if (p.getSawPlayer()) {
				//if pawn has seen the player
					CheckMove (
						selectedObject.GetComponent<Pawn> ().getTileOn (),
						p.gameObject,
						0,
						0);
				} else {
				//patrol
				}
			}
		}
	}

	public void ReloadLevel(){
		SceneManager.LoadSceneAsync (1);
		//Application.LoadLevel (Application.loadedLevel);
	}

	// Use this for initialization
	void Start () {
		if (instance == null) //an den uparxei game manager
		instance = this; //ftiaxe ton game manager me auton ton kodika ws vash
	}

	// Update is called once per frame
	void Update () {

		if (!isSwipeObjectSet) { 
			if (SwipeTest.instance) { 
				SwipeTest.instance.player = selectedObject.transform; 
				isSwipeObjectSet = true; 
			}
		}


		//RELOAD LEVEL
		if (Input.GetKey (KeyCode.R)) {
			ReloadLevel ();
		}

		if (!selectedObject.GetComponent<Pawn> ().lockPlayerMovement) {
			//MOVE-UP-DOWN
			float deltaV = Input.GetAxis ("Vertical"); //N:pare to input oti metakinite katheta
			if (Input.GetButtonDown ("Vertical")) {

				CheckMove (
					null,
					selectedObject,
					deltaV,
					0);
			
				//MoveEnemies ();
			}

			//MOVE-LEFT-RIGHT
			float deltaH = Input.GetAxis ("Horizontal"); //N:pare to input oti metakinite orizodia
			if (Input.GetButtonDown ("Horizontal")) {

				CheckMove (
					null,
					selectedObject,
					0,
					deltaH);
			
				//MoveEnemies ();

			}
		}
		}
		
}

