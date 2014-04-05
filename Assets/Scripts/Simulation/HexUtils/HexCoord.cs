using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexCoord
{
	public const float DEFAULT_SIZE = 1.0f;

	/// <summary>HexCoords are stored in Axial coordinates</summary> 
	private AxialCoord coordinate;

	public HexCoord(int q, int r) {
		coordinate = new AxialCoord(q,r);
	}

	/// <summary>Construct a HexCoord object from Cube coordinates.</summary>
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

	/// <summary>
	/// Converts a HexCoord object to a Vector3 position, for which Unity GameObjects can be Instantiated.
	/// </summary>
	/// <returns>The Vector3 position that the center of the hexcoord represents.</returns>
	/// <param name="size">The radius of the hex tiles.</param>
	public Vector3 toPosition(float size) {
		return new Vector3(size * Mathf.Sqrt(3) * (coordinate.Q + (float)coordinate.R / 2), 0, size * (3.0f/2.0f) * coordinate.R);
	}

	public Vector3 toPosition() {
		return toPosition(DEFAULT_SIZE);
	}

	/// <summary>Returns a HashSet containing the size neighboring HexCoord objects.</summary>
	public HashSet<HexCoord> getNeighbors() {
		HashSet<HexCoord> neighbors = new HashSet<HexCoord>();

		neighbors.Add(new HexCoord(coordinate.Q + 1, coordinate.R));
		neighbors.Add(new HexCoord(coordinate.Q + 1, coordinate.R - 1));
		neighbors.Add(new HexCoord(coordinate.Q, coordinate.R - 1));
		neighbors.Add(new HexCoord(coordinate.Q - 1, coordinate.R));
		neighbors.Add(new HexCoord(coordinate.Q - 1, coordinate.R + 1));
		neighbors.Add(new HexCoord(coordinate.Q, coordinate.R + 1));

		return neighbors;
	}
}

