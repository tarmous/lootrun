using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Module : MonoBehaviour{

	//public string Name;

	public Tile entrance;
	public Tile exit;

	public int moduleGold=0;
	//public int numTraps=0, numEnemies;

	public GameObject playerStart = null;
	public GameObject playerPrefab = null;
	//public GameObject playerEnd = null;
	//public GameObject playerEndPrefab = null;

	public List<GameObject> goldSpawnLocations = new List<GameObject> ();
	public List<GameObject> goldType = new List<GameObject> ();

	public List<GameObject> trapSpawnLocations = new List<GameObject> ();
	public List<int> trapDirection = new List<int> ();
	public List<float> trapTimer = new List<float> ();
	public List<int> trapSize = new List<int> ();
	public List<GameObject> enemySpawnLocations = new List<GameObject> ();
	public List<GameObject> switchesSpawnLocations = new List<GameObject> ();//for independed switch

	public List<GameObject> trapType = new List<GameObject> ();
	public List<GameObject> enemyType = new List<GameObject> ();
	public List<GameObject> switchType = new List<GameObject> ();//for independed switch
	public List<int> enemyDirection = new List<int> ();

	public List<int> switchesID = new List<int> ();//for independed switch
	public List<int> switchesToTraps = new List<int> ();
	public List<int> switchesToTrapsID = new List<int> ();
	public List<bool> switchesShouldStartActive= new List<bool>();

	//public Dictionary<int, GameObject> patrolPoints= new Dictionary<int, GameObject>(); 

	//public Tile parentGameObject;

}
