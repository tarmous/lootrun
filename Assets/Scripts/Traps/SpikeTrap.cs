using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap {
	public bool isActive=false;
	private BoxCollider bc;
	private Material mat;
	private Animator anim;

	public override void activateSwitch(){
		shouldStopCoroutine = true;
		if(!anim)
			anim = GetComponent<Animator> ();
		if(!bc)
			bc = GetComponent<BoxCollider> ();
		if(!mat)
			mat = GetComponent<Renderer> ().material;
		bc.enabled = true;
		isActive = true;
		mat.color = Color.red;
	}
	public override void deActivateSwitch(){
		shouldStopCoroutine = true;
		isActive = false;
		if(!anim)
			anim = GetComponent<Animator> ();
		if(!bc)
			bc = GetComponent<BoxCollider> ();
		if(!mat)
			mat = GetComponent<Renderer> ().material;
		bc.enabled = false;
		mat.color = Color.blue;
	}


	public float trapTimer=0f;

	IEnumerator ToggleTrap(){

		while (true && !shouldStopCoroutine) {
			//Debug.Log ("Ran routine");
			//bc.isTrigger = !bc.isTrigger;
			bc.enabled= !bc.enabled;
			if (bc.enabled) {
				mat.color = Color.red;
				isActive = true;
			} else {
				mat.color = Color.blue;
				isActive = false;
			}
			yield return new WaitForSeconds(1.3f);
		}

	}



	// Use this for initialization
	IEnumerator Start () {
		anim = GetComponent<Animator> ();
		bc = GetComponent<BoxCollider> ();
		mat = GetComponent<Renderer> ().material;
		yield return new WaitForSeconds (trapTimer);
		if (!GetComponent<Switch> () || !GetComponentInParent<Switch> () || !GetComponentInChildren<Switch> ()) {
			StartCoroutine (ToggleTrap ());
		} else {
			deActivateSwitch ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("isActive", isActive);
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
