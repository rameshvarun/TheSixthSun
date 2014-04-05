using UnityEngine;
using System.Collections;
using LitJson;

public class GroundUnit : ILandObject, IUnit
{
	/// <summary>Colonists serve to found cities.</summary>
	public static string Colonist = "colonist";
	/// <summary>Basic ground infantry unit.</summary>
	public static string Marine = "marine";

	/// <summary>This is the node on the planets surface that a ground unit is on. Set to null if on a cargo ship.</summary> 
	public int node { get; set; }
	public Player owner { get; set; }

	/// <summary>Whether or not the unit has already performed an action this turn. </summary>
	public bool hasMoved;

	/// <summary>Used to store the type of ground unit.</summary>
	public string type;

	public GroundUnit(int node, string type, Player owner) {
		this.node = node;
		this.type = type;
		this.owner = owner;

		hasMoved = false;
	}

	public bool canMove() { return !hasMoved; }
	
	private static JsonData balance = null;

	/// <summary>
	/// Gets the balance data, loaded from the JSON file "Balance/ground_units." This contains balance data for ground units, such as movement ranges, hp, and attack values.
	/// </summary>
	/// <value>The balance.</value>
	public static JsonData Balance {
		get {
			if(balance == null) {
				TextAsset jsonString = Resources.Load<TextAsset>("Balance/ground_units");
				balance = JsonMapper.ToObject(jsonString.text);
			}
			return balance;
		}
	}
}