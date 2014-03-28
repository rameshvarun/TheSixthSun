using UnityEngine;
using System.Collections;

public struct CubeCoord {
	private int x, y, z;
	public CubeCoord(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public AxialCoord toAxialCoord() {
		return new AxialCoord(x, y);
	}

	//TODO: Implement Equals
	//TODO: Implement GetHashCode

	public override string ToString() {
		return string.Format ("<{0}, {1}, {2}>", x, y, z);
	}
}
