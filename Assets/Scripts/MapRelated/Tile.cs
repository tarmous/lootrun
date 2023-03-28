using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour {

	//distance and previous is always re-set after each move
	//private int tileX, tileZ;
	public int movePoints;
	public int tileType;
	private bool steppedOnByPlayer=false;
	public GameObject onTile;
	public List<Tile> neighbours= new List<Tile>();

	public void setNeighbours(int i, int j, Tile[,] graph){
		//4-way
		if (i > 0)
			neighbours.Add (graph [i - 1, j]);
		
		if (i < graph.GetLength(0) -1)
			neighbours.Add (graph [i + 1, j]);

		if (j > 0)
			neighbours.Add (graph [i, j - 1]);

		if (j < graph.Length/graph.GetLength(0)-1)
			neighbours.Add (graph [i, j + 1]);
	}

	public void setNeighbours (Tile t){
		neighbours.Add (t);


	}

	public void Update(){
		if (tileType == 3) {
			if (onTile) {
				if (!steppedOnByPlayer) {
					if (onTile.tag == "Player") {
						Scoreboard.updateAccumulatedXP ();
						steppedOnByPlayer = true;
						foreach (Tile t in neighbours) {
							if (t.tileType == 3) {
								t.steppedOnByPlayer = true;
							}
						}

					}
				}
			}
		}
	}

	//public void setX(int i){this.tileX = i;}
	//public void setZ(int i){this.tileZ = i;}
	public void setOnTile(GameObject i){ this.onTile = i;}
	public void setMovePoints(int i){this.movePoints=i;}


	//public int getX(){return tileX;}
	//public int getZ(){return tileZ;}
	public GameObject getOnTile(){return this.onTile;}
	public int getMovePoints(){return this.movePoints;}
	public List<Tile> getNeighbours(){return this.neighbours;}

}
