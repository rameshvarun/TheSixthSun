using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class City : ILandObject, IUnit
{
	/// <summary>Store the current, in-progress project that the city is working on. null if the city is not building anything.</summary>
	public string currentProject = null;

	/// <summary>Number of turns left to construct the current project.</summary>
	public int projectTurns = 0;

	public Planet planet { get; set; }

	/// <summary>This is the node on the planets surface that the city is on.</summary> 
	public int node { get; set; }

	/// <summary>The player who owns this city </summary>
	public Player owner { get; set; }

	/// <summary>The city can make a move if it is not building anything.</summary>
	public bool canMove() { return currentProject == null; }

	public CityBehavior behavior;

	public City(int node, Player owner, Planet planet) {
		this.node = node;
		this.owner = owner;
		this.planet = planet;
	}

	private static JsonData balance = null;

	/// <summary>
	/// Gets the balance data, loaded from "Balance/ground_building"
	/// </summary>
	/// <value>The balance.</value>
	public static JsonData Balance {
		get {
			if(balance == null) {
				TextAsset jsonString = Resources.Load<TextAsset>("Balance/ground_building");
				balance = JsonMapper.ToObject(jsonString.text);
			}
			return balance;
		}
	}
}

