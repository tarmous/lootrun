using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

	public GameObject tileOn;
	public GameObject goldAnimation;//prefab
	public GameObject goldAnimationRef;//Reference
	public List<Tile> currentPath = new List<Tile> ();

	private int tilesToMove;
	private int maxMovePoints;
	private int attackRange;
	private bool sawPlayer = false;

	private float interpolateSpeed=0.7f;
	private bool startMovement;
	private bool startPatrolling = false;
	private bool isBacktracking = false;
	private float tileOffset= 0.5f;//used to position prefabs on center of tile

	public void setTileOn(GameObject i){this.tileOn = i;}
	public void setTilesToMove(int i){this.tilesToMove = i;}
	public void setSawPlayer(bool i){this.sawPlayer = i; print("saw player run");}
	public void setAttackRange(int i){this.attackRange = i;}
	public void startMoving(){startMovement = true;}

	public int getAttackRange(){return this.attackRange;}
	public GameObject getTileOn(){return this.tileOn;}
	public int getTilesToMove(){return this.tilesToMove;}
	public bool getSawPlayer(){return this.sawPlayer;}

	public Vector3 startDirection;
	public Vector3 lookDirection= new Vector3(0,1,0);
	public Vector3 prevLocation;
	private bool checkForDir = true;
	public bool lockPlayerMovement = false;


	public IEnumerator startCoinFlip(){
		goldAnimationRef.SetActive (true);
		yield return new WaitForSeconds (2f);
		goldAnimationRef.SetActive (false);
		StopCoroutine (startCoinFlip());
	}
		
	public void ResetMovePoints()
	{
		tilesToMove = maxMovePoints;
	}

	private void setLookDirection(){
		//y-x
		if (gameObject.transform.position.y > prevLocation.y) {
			lookDirection = new Vector3 (0,1,0);
		} else if (gameObject.transform.position.y < prevLocation.y) {
			lookDirection = new Vector3 (0,-1,0);
		} else if (gameObject.transform.position.x > prevLocation.x) {
			lookDirection = new Vector3 (1,0,0);
		} else if (gameObject.transform.position.x < prevLocation.x) {
			lookDirection = new Vector3 (-1,0,0);
		}
		prevLocation = transform.position;
	}

	private IEnumerator patrolGameObject(){
			while (currentPath.Count > 0) { 
				if ((Mathf.Abs (transform.position.x - (currentPath [0].transform.position.x + tileOffset)) >= 0.05f || Mathf.Abs (transform.position.y - (currentPath [0].transform.position.y - tileOffset)) >= 0.05f)) {

					transform.position = new Vector3 (
						Mathf.Lerp (transform.position.x, currentPath [0].transform.position.x + tileOffset, interpolateSpeed),
						Mathf.Lerp (transform.position.y, currentPath [0].transform.position.y - tileOffset, interpolateSpeed),
						transform.position.z);

				if (checkForDir) {
					setLookDirection ();
					checkForDir = false;
				}

				yield return new WaitForSeconds (interpolateSpeed/100f);
				
				} else {

					tileOn.GetComponent<Tile> ().onTile = null;
					tileOn = currentPath [0].gameObject;
					currentPath [0].onTile = this.gameObject;
					
				transform.position = new Vector3 (
					currentPath [0].transform.position.x + tileOffset,
					currentPath [0].transform.position.y - tileOffset,
					transform.position.z);
					
					setLookDirection ();
					currentPath.RemoveAt (0);
					checkForDir = true;

				GameManager.instance.LineOfSight (this);

				if (sawPlayer) {
					currentPath.Clear ();
					break;
				}
					
				if (tileOn.GetComponent<PatrolPoint> ()) {

					if (!isBacktracking) {
						if (tileOn.GetComponent<PatrolPoint> ().next) {
							isBacktracking = false;
						} else {
							isBacktracking = true;
							continue;
						}

						GameManager.instance.CheckMove (
							tileOn.GetComponent<PatrolPoint> ().next,
							this.gameObject,
							0,
							0);
						


					} else {
						if (tileOn.GetComponent<PatrolPoint> ().prev) {
							isBacktracking = true;
						}else {
							isBacktracking = false;
							continue;
						}

						GameManager.instance.CheckMove (
							tileOn.GetComponent<PatrolPoint> ().prev,
							this.gameObject,
							0,
							0);
					}

				}
					yield return new WaitForSeconds (1f);

					
				}
			}//end_if

	}


	private void moveGameObject(){

		if (startMovement) {
			if (currentPath.Count > 0){ 
				if ((Mathf.Abs (transform.position.x - (currentPath [0].transform.position.x + tileOffset)) >= 0.05f || Mathf.Abs (transform.position.y - (currentPath [0].transform.position.y - tileOffset)) >= 0.05f)) {


					//elipctic math function (jump effect)
					float x = (transform.position.x - currentPath [0].transform.position.x - tileOffset );
					float y = (transform.position.y - currentPath [0].transform.position.y + tileOffset );
					 
						transform.position = new Vector3 (
						Mathf.Lerp (transform.position.x, currentPath [0].transform.position.x + tileOffset, interpolateSpeed),
						Mathf.Lerp (transform.position.y, currentPath [0].transform.position.y - tileOffset, interpolateSpeed),
						Mathf.Lerp(transform.position.z, - 2*(Mathf.Pow(x,2) + Mathf.Pow(y,2)) - tileOffset + 0.1f, interpolateSpeed ));

					//y = -4x^2 + 4x





					/*
					transform.position = new Vector3 (
						Mathf.Lerp (transform.position.x, currentPath [0].transform.position.x + tileOffset, interpolateSpeed),
						Mathf.Lerp (transform.position.y, currentPath [0].transform.position.y - tileOffset, interpolateSpeed),
						transform.position.z);
					*/

					if (checkForDir) {
						setLookDirection ();
						checkForDir = false;
					}

				} else {
					
					transform.position = new Vector3 (
						currentPath [0].transform.position.x + tileOffset,
						currentPath [0].transform.position.y - tileOffset,
						transform.position.z);

					setLookDirection ();
					currentPath.Clear ();
					startMovement = false;
					checkForDir = true;


					if (gameObject.tag == "Player") {
						GameManager.instance.MoveEnemies ();
						lockPlayerMovement = false;
					}

					if (gameObject.tag == "Pawn") {
						foreach (Tile t in getTileOn().GetComponent<Tile>().getNeighbours()) {
							if (t.getOnTile ()) {
								if (t.getOnTile ().tag == "Player") {

									bool isBeingChased = false;
									foreach (Pawn p in TileMap.instance.pawnRef) {
										if (p != this && p.gameObject.tag!= "Player") {
											if (p.currentPath.Count > 0) {
												if (p.currentPath [0] == t) {
													isBeingChased = true;
													break;
												}
											}
										}
									}//end_foreach
									if (!isBeingChased) {
										currentPath.Add (t);
										break;
									}
								}
							}
						}

					}
				}
			}
		}
	}

	// Use this for initialization
	void Start () {

		//Debug.Log( Resources.Load<GameObject>("Misc/Switch").GetComponentInChildren<Switch>());

		lookDirection= startDirection;
		prevLocation = transform.position;
		startMovement = false;
		maxMovePoints = 1;
		tilesToMove = maxMovePoints;
		attackRange = 3;

		if (gameObject.tag == "Player")
		{
			sawPlayer = true;
			GameManager.instance.selectedObject = this.gameObject;
			if (GameManager.instance.selectedObject) {
			}

			//coin animation
			GameObject go=Instantiate (goldAnimation, gameObject.transform.position,Quaternion.identity) as GameObject;
			go.transform.SetParent (gameObject.transform);
			go.transform.position += new Vector3(0,1f,0);
			go.SetActive (false);
			goldAnimationRef = go;
		}

		if (gameObject.tag == "Pawn")
		{
			
		}


	}
	
	// Update is called once per frame
	void Update () {
		
		if (gameObject.tag == "Player") {
			if (getTileOn ().GetComponent<Tile> ().tileType == 3) {
				int moduleCounter = -1;

				foreach(GameObject tmpGO in TileMap.instance.moduleGO){
					moduleCounter++;
					if (tmpGO.GetComponent<Module> ().exit == getTileOn ().GetComponent<Tile> ()) {
						break;
					}
				}
				if(moduleCounter >= 1 && moduleCounter < TileMap.instance.moduleGO.Count){
				TileMap.instance.moduleGO [moduleCounter-1].SetActive(false);
				}
			}
		}

		//patrol
		if (!sawPlayer) {
			if (!startPatrolling) {
				startPatrolling = true;
				if (tileOn.GetComponent<PatrolPoint>()) {
					GameManager.instance.CheckMove (
						tileOn.GetComponent<PatrolPoint> ().next,
						this.gameObject,
						0,
						0);
				}
				StartCoroutine (patrolGameObject ());
			}

		}else{
			StopCoroutine (patrolGameObject ());
			//move game object
			moveGameObject();
		}


	}

	void LateUpdate(){

	}



}
