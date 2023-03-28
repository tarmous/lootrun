using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	//private float cameraRotateSpeed=100f ,cameraMoveSpeed = 10f;
	Vector3 cameraPlaneSpeed, cameraRot, cameraVerticalSpeed;
	//private CharacterController cc;
	public GameObject objectToFollow;


	// Use this for initialization
	void Start () {
		//cc = GetComponent<CharacterController> ();
		objectToFollow = transform.parent.gameObject;
		transform.parent = null;
	}
		

	void LateUpdate(){
		if(transform.position.y <= objectToFollow.transform.position.y){
		transform.position = new Vector3 (
			Mathf.Lerp(transform.position.x,objectToFollow.transform.position.x,0.05f),
				Mathf.Lerp(transform.position.y,objectToFollow.transform.position.y,0.05f),
			transform.position.z);
		}else{
			transform.position = new Vector3 (
				Mathf.Lerp(transform.position.x,objectToFollow.transform.position.x,0.05f),
				transform.position.y,
				transform.position.z);
		}
	}
}
