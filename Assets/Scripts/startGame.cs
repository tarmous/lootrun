using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour {
	public InputField Type_x;
	public InputField Type_y;
	public InputField Type_numToLoad;

	public int x, y, numToLoad;

	// Use this for initialization
	void Start () {
		
	}

	void Update(){
		if(Input.GetKeyDown (KeyCode.E)){
			Debug.Log ("Pressed");
			UpdateProfileStatistics ups = new UpdateProfileStatistics ();
			ups.updateExp (1000);
		}
		if(Input.GetKeyDown (KeyCode.T)){
			PlayerPrefs.SetInt ("playerLevel",0);
			PlayerPrefs.SetInt ("currentXP",0);
			PlayerPrefs.SetInt ("goldAmount",0);
		}
	}

	public void updateValues(){
		if(Type_x.text!=null && Type_x.text!="" )
		this.x =int.Parse(Type_x.text);
		if(Type_y.text!=null && Type_y.text!="")
		this.y =int.Parse(Type_y.text);

		if(Type_numToLoad.text!=null && Type_numToLoad.text!="")
			this.numToLoad =int.Parse(Type_numToLoad.text);
	}


	public void loadLevel()
	{	//if (this.x != 0 && this.y != 0) {
			
			///TileMap.typeY=this.y;
			//TileMap.typeX=this.x;
			//TileMap.numToLoad = this.numToLoad;
			//Debug.Log (TileMap.typeY);
			//Debug.Log (TileMap.typeX);
			//Debug.Log (TileMap.numToLoad);
			SceneManager.LoadSceneAsync (1);
		//}
		//Application.LoadLevel (1);
	}

	public void reloadLevel(){
		SceneManager.LoadSceneAsync (1);	
	}

	public void closeGame()
	{
		Application.Quit ();
		Debug.Log ("pata");
	}
}
