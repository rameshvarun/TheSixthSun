using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {
	//Information about map
	public HexGrid grid = new HexGrid();
	public List<Planet> planets = new List<Planet>();
	public List<Player> players = new List<Player>();

	public GameState() {
	}

	//Singleton that refers to the currently drawn GameState
	private static GameState instance;
	public static GameState Instance {
		get { return instance; }
		set { instance = value; }
	}
}
