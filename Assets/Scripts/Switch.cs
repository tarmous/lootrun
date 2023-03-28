using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
	public int switchID;
	public bool hasTrap=false;
	public bool shouldStartActivated = false;
	public List<Switch> trapReference = new List<Switch> ();
	//private bool foundTrap=false;
	private enum switchTypes {FlameTrap, SpikeTrap};

	public void activate(){
		//flame trap
		if (GetComponent<FlameTrap>()) {
			GetComponent<FlameTrap> ().activateSwitch ();
			return;
		} else if (GetComponentInChildren<FlameTrap>()) {
			GetComponentInChildren<FlameTrap> ().activateSwitch ();
			return;
		}

		//spike trap
		if (GetComponent<SpikeTrap>()) {
			GetComponent<SpikeTrap> ().activateSwitch ();
			return;
		} else if (GetComponentInChildren<SpikeTrap>()) {
			GetComponentInChildren<SpikeTrap> ().activateSwitch ();
			return;
		}

	}

	public void deActivate(){
		//flame trap
		if (GetComponent<FlameTrap>()) {
			GetComponent<FlameTrap> ().deActivateSwitch ();
			return;
		} else if (GetComponentInChildren<FlameTrap>()) {
			GetComponentInChildren<FlameTrap> ().deActivateSwitch ();
			return;
		}

		//spike trap
		if (GetComponent<SpikeTrap>()) {
			GetComponent<SpikeTrap> ().deActivateSwitch ();
			return;
		} else if (GetComponentInChildren<SpikeTrap>()) {
			GetComponentInChildren<SpikeTrap> ().deActivateSwitch ();
			return;
		}
	}

	// Use this for initialization
	void Start () {
		/*
		trapReference.Clear ();
		if (!hasTrap) {
			Switch[] switches = FindObjectsOfType<Switch> ();
			int i = 0;
			foreach (Switch s in switches) {
				if (s != this && s.switchID == this.switchID) {
					trapReference.Add(s);
					foundTrap = true;
					trapReference[i].deActivate ();
					i++;
				}
			}
		} else {
			foundTrap = true;
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (!foundTrap) {
			Switch[] switches = FindObjectsOfType<Switch> ();
			int i = 0;
			foreach (Switch s in switches) {
				if (s != this && s.switchID == this.switchID) {
					trapReference.Add(s);
					foundTrap = true;
					trapReference[i].deActivate ();
					i++;
				}
			}
		}
		*/
	}

	private bool allowedColliders(Collider other){
		if (other.tag == "Pawn" || other.tag == "Player") {
			return true;
		}
		return false;
	}

	void OnTriggerEnter(Collider other){
		//activate switch
		if (allowedColliders(other)) {
			if (trapReference.Count > 0) {
				foreach (Switch s in trapReference) {
					if (!s.shouldStartActivated) {
						s.activate ();
						continue;
					}
					s.deActivate ();
				}
			}
		}
		
	}

	void OnTriggerExit(Collider other){
		//deactivate switch
		if (allowedColliders(other)) {
			if (trapReference.Count > 0) {
				foreach (Switch s in trapReference) {
					if (!s.shouldStartActivated) {
						s.deActivate ();
						continue;
					}
					s.activate ();
				}
				
			}
		}
	}
}
