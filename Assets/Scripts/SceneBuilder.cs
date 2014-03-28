using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * The purpose of this script is to look at the GameState singleton and actually construct the visual scene from the provided Prefabs
 */
public class SceneBuilder : MonoBehaviour {
	public Transform hexTile;
	public Transform planet;

	// Use this for initialization
	void Start () {
		GameState.Instance = new GameState();
		MapGenerators.TestMap(GameState.Instance);

		//Create empty space grid
		foreach(KeyValuePair<HexCoord, int> tile in GameState.Instance.grid.tiles) {
			Debug.Log("HexTile at " + tile.Key, this);
			Instantiate(hexTile,tile.Key.toPosition(1.0f),Quaternion.identity);
		}

		//Spawn in planets
		foreach(Planet p in GameState.Instance.planets) {
			Debug.Log("Planet at " + p.coordinate, this);
			Instantiate(planet, p.coordinate.toPosition(1.0f) + new Vector3(0, 0.5f, 0),Quaternion.identity);
		}

		//TODO: Spawn in Stars
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
