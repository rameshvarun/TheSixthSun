using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * The purpose of this script is to look at the GameState singleton and construct the visible scene
 */
public class SceneBuilder : MonoBehaviour {
	public Transform hexTile;

	// Use this for initialization
	void Start () {
		GameState.Instance = new GameState();
		MapGenerators.TestMap(GameState.Instance);

		foreach(KeyValuePair<HexCoord, int> tile in GameState.Instance.grid.tiles) {
			Debug.Log("HexTile at " + tile.Key.ToString(), this);
			Instantiate(hexTile,tile.Key.toPosition(1.0f),Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
