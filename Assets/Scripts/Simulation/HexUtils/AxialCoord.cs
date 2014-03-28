using UnityEngine;
using System.Collections;

public struct AxialCoord {
	private int q, r;

	public int Q { get { return q; } }
	public int R { get { return r; } }

	public AxialCoord(int q, int r) {
		this.q = q;
		this.r = r;
	}
	public CubeCoord toCubeCoord() {
		return new CubeCoord(q, r, -q - r);
	}

	public override bool Equals(object obj) {
		if(obj is AxialCoord) {
			AxialCoord c = (AxialCoord)obj;
			return (q == c.Q) && (r == c.R);
		}
		else {
			return false;
		}
	}
	
	public override int GetHashCode() {
		return q.GetHashCode() ^ r.GetHashCode();
	}

	public override string ToString () {
		return string.Format ("<{0}, {1}>", q, r);
	}
}
