using UnityEngine;
using System.Collections;
using LitJson;

public class GroundUnit : ILandObject, IUnit
{
	/// <summary>Colonists serve to found cities.</summary>
	public static string Colonist = "colonist";

	/// <summary>Enum for listing all possible ground unit types. </summary>
	public static string Marine = "marine";

	public int node { get; set; }
	public Player owner { get; set; }
	public float hp { get; set; }

	public string type;

	public GroundUnit(int node, string type, Player owner) {
		this.node = node;
		this.type = type;
		this.owner = owner;
	}
}