using UnityEngine;
using System.Collections;

public class GroundUnitBehavior : UnitBehavior {

	/// <summary>The simulation GroundUnit object.</summary>
	public GroundUnit groundUnit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override Vector3 getInspectCameraPosition ()
	{
		return transform.TransformPoint(new Vector3(0,5,-2));
	}

	public override bool MoveGUI() {
		if(groundUnit.canMove()) {
			if (GUI.Button(new Rect(200, 10, 150, 100), "Move")) {
			}

			if (GUI.Button(new Rect(400, 10, 150, 100), "Skip Turn")) {
				groundUnit.hasMoved = true;
				return true;
			}
			
			if(groundUnit.type == GroundUnit.Colonist) {
				if (GUI.Button(new Rect(600, 10, 150, 100), "Found City")) {
					groundUnit.hasMoved = true;
					return true;
				}
			}
		}

		return false;

	}
}
