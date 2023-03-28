using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : Trap {

	public override void activateSwitch(){
		shouldStopCoroutine = true;
		//if for some reason start doesnt activate first (failsafe)
		if (bc.Length==0) {
			Start ();
		}
		for (int i = 0; i < bc.Length; i++) {
			bc [i].enabled = true;
			mat[i].color = Color.red;
		}
	}
	public override void deActivateSwitch(){
		shouldStopCoroutine = true;
		//if for some reason start doesnt activate first (failsafe)
		if (bc.Length==0) {
			Start ();
		}
		for (int i = 0; i < bc.Length; i++) {


			bc [i].enabled = false;
			mat[i].color = Color.blue;
		}
	}

	public int trapSize = 2;
	public GameObject childPrefab;
	private GameObject[] children; 
	public float trapTimer=0f;

	private BoxCollider[] bc;
	private Material[] mat;

	IEnumerator ToggleTrap(){

		while (true && !shouldStopCoroutine) {
			//Debug.Log ("Ran routine");
			//bc.isTrigger = !bc.isTrigger;
			for (int i = 0; i < bc.Length; i++) {
				bc[i].enabled = !bc[i].enabled;
				if (bc[i].enabled) {
					mat[i].color = Color.red;
				} else {
					mat[i].color = Color.blue;
				}
			}
			yield return new WaitForSeconds (1.3f);
		}

	}


	// Use this for initialization
	IEnumerator Start () {
		children = new GameObject[trapSize];
		children [0] = childPrefab;
		bc= new BoxCollider[trapSize];
		mat= new Material[trapSize];

		for(int i=1;i<bc.Length;i++){
			children[i]=Instantiate (childPrefab,gameObject.transform.position,Quaternion.identity) as GameObject;
			children[i].transform.parent = gameObject.transform;
			children[i].transform.localPosition = new Vector3 (0, i, 0);
			children [i].transform.localRotation= Quaternion.identity;

		}

		for (int i=0; i<bc.Length;i++) {
			bc[i] = children[i].GetComponent<BoxCollider> ();
			mat[i] = children[i].GetComponent<Renderer> ().material;
		}
		yield return new WaitForSeconds (trapTimer);
		if (!GetComponent<Switch> () || !GetComponentInParent<Switch> () || !GetComponentInChildren<Switch> ()) {
			StartCoroutine (ToggleTrap ());
		} else {
			deActivateSwitch ();
		}	
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other){
		//Debug.Log ("stay");
	}
	void OnTriggerEnter(Collider other){
		//Debug.Log ("enter");
		TileMap.instance.DestroyPawn (other.gameObject.transform.parent.GetComponent<Pawn> ());
	}
	void OnTriggerExit(Collider other){
		//Debug.Log ("exit");
	}



}
