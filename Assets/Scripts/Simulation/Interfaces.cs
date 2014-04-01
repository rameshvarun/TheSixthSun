using UnityEngine;
using System.Collections;

/// <summary>Interface for an object that is in space.</summary>
public interface ISpaceObject {
	/// <summary>The HexCoord of the object in space</summary> 
	HexCoord coordinate { get; }
}

/// <summary>Interface for an object that is on the surface of a planet.</summary>
public interface ILandObject {
	/// <summary>Planet surfaces are modeled as a node-edge graph. This is the node that the object is on</summary> 
	int node { get; }
}

/// <summary>Basic interface for a unit with health and a controlling player</summary>
public interface IUnit {
	Player owner { get; }
	float hp { get; }
}

public interface IInspectable {
}