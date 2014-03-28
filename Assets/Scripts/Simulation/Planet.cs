using UnityEngine;
using System.Collections;

public class Planet : ISpaceObject {
	public HexCoord coordinate { get; set; }

	public string name;
	public string description;

	public Planet() {
	}
}
