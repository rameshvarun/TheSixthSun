using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {
	/// <summary>The hex grid that stores all possible space locations.</summary>
	public HexGrid grid = new HexGrid();
	
	public List<Player> players = new List<Player>();

	public List<Planet> planets = new List<Planet>();
	public List<Star> stars = new List<Star>();

	public GameState() {
	}

	//Singleton that refers to the currently drawn GameState
	private static GameState instance;
	public static GameState Instance {
		get { return instance; }
		set { instance = value; }
	}
}
