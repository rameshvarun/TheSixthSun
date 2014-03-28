using UnityEngine;
using System.Collections;

public interface ISpaceObject {
	/// <summary>The HexCoord of the object in space</summary> 
	HexCoord coordinate { get; }
}

public interface ILandObject {
	/// <summary>Planet surfaces are modeled as a node-edge graph. This is the node that the object is on</summary> 
	int node { get; }
}

public interface IInspectable {
}