using UnityEngine;
using System.Collections;

public class HexCoord
{
	/// <summary>HexCoords are stored in Axial coordinates</summary> 
	private AxialCoord coordinate;

	public HexCoord(int q, int r) {
		coordinate = new AxialCoord(q,r);
	}

	public HexCoord(int x, int y, int z) {
		coordinate = new CubeCoord(x, y, z).toAxialCoord();
	}

	public override bool Equals(object obj) {
		if(obj is HexCoord) {
			HexCoord hexCoord = (HexCoord)obj;
			return coordinate.Equals(hexCoord.coordinate);
		}
		else {
			return false;
		}
	}

	public override int GetHashCode() {
		return coordinate.GetHashCode();
	}

	public override string ToString ()
	{
		return coordinate.ToString();
	}

	public Vector3 toPosition(float size) {
		return new Vector3(size * Mathf.Sqrt(3) * (coordinate.Q + (float)coordinate.R / 2), 0, size * (3.0f/2.0f) * coordinate.R);
	}
}

