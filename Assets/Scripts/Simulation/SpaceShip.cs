using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>Class that represents a spaceship unit.</summary>
public class Spaceship : ISpaceUnit {
	public static string TestShip = "testship";

	/// <summary>Store the type of spaceship</summary>
	public string type;

	/// <summary>Location of the spaceship in space.</summary>
	public HexCoord coordinate { get; set; }

	/// <summary>The player that has control over the current unit.</summary>
	public Player owner { get; set; }

	/// <summary>Whether or not the unit has already performed an action this turn. </summary>
	public bool hasMoved;

	public bool canMove() { return !hasMoved; }

	public Spaceship(HexCoord coordinate, string type, Player owner) {
		this.coordinate = coordinate;
		this.type = type;
		this.owner = owner;
	}
}

public class Fleet : ISpaceUnit {
	/// <summary>A list of ships that are part of the given fleet.</summary>
	List<Spaceship> ships = new List<Spaceship>();

	/// <summary>Location of the fleet in space.</summary>
	public HexCoord coordinate { get; set; }

	/// <summary>The player that has control over the current unit.</summary>
	public Player owner { get; set; }
	
	/// <summary>Whether or not the unit has already performed an action this turn. </summary>
	public bool hasMoved;
	
	public bool canMove() { return !hasMoved; }
}