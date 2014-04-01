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

	public override bool Equals(object obj) {
		if(obj is CubeCoord) {
			CubeCoord c = (CubeCoord)obj;
			return (x == c.x) && (y == c.y) && (z == c.z);
		}
		else {
			return false;
		}
	}

	public override int GetHashCode() {
		return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
	}

	public override string ToString() {
		return string.Format ("<{0}, {1}, {2}>", x, y, z);
	}
}
