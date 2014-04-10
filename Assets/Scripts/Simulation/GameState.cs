using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {
	/// <summary>The hex grid that stores all possible space locations.</summary>
	public HexGrid grid = new HexGrid();
	
	public List<Player> players = new List<Player>();

	/// <summary>Stores the player who is currently taking their turn.</summary>
	public Player currentPlayer;

	/// <summary>List of planet objects.</summary>
	public List<Planet> planets = new List<Planet>();

	/// <summary>List of star objects.</summary>
	public List<Star> stars = new List<Star>();

	/// <summary>List of space units.</summary>
	public List<ISpaceUnit> spaceUnits = new List<ISpaceUnit>();

	public GameState() {
	}

	/// <summary>
	/// Advances the turn. This advances the current player.
	/// </summary>
	public void advanceTurn() {
		//currentPlayer advances one
		int current_player_index = players.IndexOf(currentPlayer);
		currentPlayer = players[(current_player_index + 1) % players.Count];

		if(current_player_index == players.Count - 1) {
			foreach(Planet planet in planets) {
				foreach(ILandObject landObject in planet.landObjects) {
					if(landObject is GroundUnit) ((GroundUnit)landObject).hasMoved = false;
				}
			}

			//TODO: Make it so that all units can move again
		}
	}

	/// <summary>
	/// Get a list of all Units, owned by the currentPlayer, that have yet to make their move this turn.
	/// </summary>
	public List<IUnit> unmovedUnits() {
		List<IUnit> units = new List<IUnit>();

		foreach(Planet planet in planets) {
			foreach(ILandObject landObject in planet.landObjects) {
				if(landObject is IUnit && ((IUnit)landObject).canMove() && ((IUnit)landObject).owner == GameState.Instance.currentPlayer) {
					units.Add((IUnit)landObject);
				}
			}
		}

		return units;
	}

	/// <summary>Should be used to determine if the current user can take actions on
	/// their units. For a pass-and-play game, this is always true.</summary>
	public static bool isMyTurn() {
		return true;
	}

	//Singleton that refers to the currently drawn GameState
	private static GameState instance;
	public static GameState Instance {
		get { return instance; }
		set { instance = value; }
	}
}
