using UnityEngine;
using System.Collections;

public interface ISpaceObject {
	HexCoord coordinate { get; }
}

public interface ILandObject {
}

public interface IInspectable {
}