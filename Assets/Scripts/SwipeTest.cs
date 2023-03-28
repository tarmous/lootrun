using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeTest : MonoBehaviour {
	public static SwipeTest instance;

	public Swipe swipeControls;
	public Transform player;
	private Vector3 desiredPosition;


	void Start(){
		instance = this;
		if (GameManager.instance.selectedObject) {
			player = GameManager.instance.selectedObject.transform;
			GameManager.instance.isSwipeObjectSet = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (swipeControls.SwipeLeft){
			GameManager.instance.CheckMove(null,player.gameObject,0f,-1f);//desiredPosition += Vector3.left;
			//GameManager.instance.MoveEnemies ();//N:metakinhse tous exthrous
		}
		if (swipeControls.SwipeRight){
			GameManager.instance.CheckMove(null,player.gameObject,0f,1f);//desiredPosition += Vector3.right;
			//GameManager.instance.MoveEnemies ();//N:metakinhse tous exthrous
		}
			if (swipeControls.SwipeUp){
			GameManager.instance.CheckMove(null,player.gameObject,1f,0f);//desiredPosition += Vector3.forward;
			//GameManager.instance.MoveEnemies ();//N:metakinhse tous exthrous
		}
				if (swipeControls.SwipeDown){
			GameManager.instance.CheckMove(null,player.gameObject,-1f,0f);//desiredPosition += Vector3.back;
			//GameManager.instance.MoveEnemies ();//N:metakinhse tous exthrous
		}

		//player.transform.position = Vector3.MoveTowards (player.transform.position, desiredPosition, 3f * Time.deltaTime);
	}
}
