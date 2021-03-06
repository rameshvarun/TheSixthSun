﻿using UnityEngine;
using System.Collections;

/// <summary>Interface for an object that is in space.</summary>
public interface ISpaceObject {
	/// <summary>The HexCoord of the object in space</summary> 
	HexCoord coordinate { get; }
}

/// <summary>Interface for an object that is on the surface of a planet.</summary>
public interface ILandObject {
	/// <summary>Maintain a link back to the planet that this object is on.</summary>
	Planet planet { get; }

	/// <summary>Planet surfaces are modeled as a node-edge graph. This is the node that the object is on</summary> 
	int node { get; }
}

/// <summary>Basic interface for a unit with health and a controlling player</summary>
public interface IUnit {
	Player owner { get; }

	/// <summary>
	/// This function should return if the unit is able to make a new move this turn.
	/// This can be used to cycle through all units, ensuring that the player has made all possible moves.
	/// </summary>
	bool canMove();
}

/// <summary>Interface that represents all movable units in space. This can either be an individual ship, or a fleet. </summary>
public interface ISpaceUnit : IUnit, ISpaceObject {
}

public interface IInspectable {
}