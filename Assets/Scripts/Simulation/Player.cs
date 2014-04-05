using UnityEngine;
using System.Collections;

public class Player {
	public const string Aztec = "aztec";

	/// <summary>
	/// Will store the Google Play Games API Participant ID.
	/// </summary>
	public string participantId;

	/// <summary>Stores the faction that the player represents. For now, only aztecs.</summary>
	public string factionName = Aztec;

	/// <summary>Store a color that represents the player. This can be used for colored outlines, etc.</summary>
	public Color color;

	/// <summary>Ideally, this can store a player's name during a pass and play game.</summary>
	public string name;
}
