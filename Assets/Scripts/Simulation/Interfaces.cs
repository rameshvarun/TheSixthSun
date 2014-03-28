using UnityEngine;
using System.Collections;

public interface ISpaceObject {
	HexCoord coordinate { get; }
}

public interface ILandObject {
	int vertex { get; }
}

public interface IInspectable {
}