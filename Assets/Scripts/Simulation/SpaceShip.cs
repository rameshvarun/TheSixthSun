using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>Class that represents a spaceship unit.</summary>
public class Spaceship : ISpaceUnit {
	public static string TestShip = "testship";

	/// <summary>Store the type of spaceship</summary>
	public string type;

	/// <summary>Location of the spaceship in space.</summary>
	public HexCoord coordinate { get; set; }

	/// <summary>The player that has control over the current unit.</summary>
	public Player owner { get; set; }

	/// <summary>Whether or not the unit has already performed an action this turn. </summary>
	public bool hasMoved;

	public bool canMove() { return !hasMoved; }

	public Spaceship(HexCoord coordinate, string type, Player owner) {
		this.coordinate = coordinate;
		this.type = type;
		this.owner = owner;
	}

	private static JsonData balance = null;

	/// <summary>
	/// Gets the balance data, loaded from the JSON file "Balance/space_units." This contains balance data for space units, such as movement ranges, hp, and attack values.
	/// </summary>
	/// <value>The balance.</value>
	public static JsonData Balance {
		get {
			if(balance == null) {
				TextAsset jsonString = Resources.Load<TextAsset>("Balance/space_units");
				balance = JsonMapper.ToObject(jsonString.text);
			}
			return balance;
		}
	}
}

