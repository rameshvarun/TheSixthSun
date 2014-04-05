using UnityEngine;
using System.Collections;

public class SpaceshipBehavior : UnitBehavior {

	/// <summary>Reference back to the spaceship simulation object.</summary>
	public Spaceship spaceship;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override Vector3 getInspectCameraPosition() {
		return transform.position + new Vector3(2, 2, 0);
	}
	
	public override bool MoveGUI() {
		return false;
	}
}
