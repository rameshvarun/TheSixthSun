using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
