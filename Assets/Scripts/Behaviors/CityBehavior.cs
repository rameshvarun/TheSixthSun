using UnityEngine;
using System.Collections;

public class CityBehavior : UnitBehavior {
	City city;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override Vector3 getInspectCameraPosition ()
	{
		return transform.TransformPoint(new Vector3(0,5,-1));
	}

	public void AnimateConstruction() {
		//TODO: Implement
	}

	public void Link(City city) {
		this.city = city;
		city.behavior = this;
	}

	public override void cleanUp() {
	}

	public override bool MoveGUI() {
		if(city.canMove() && city.owner == GameState.Instance.currentPlayer && GameState.isMyTurn()) {
			if (GUI.Button(new Rect(400, 10, 150, 100), "Skip Turn")) {
				//groundUnit.hasMoved = true;
				return true;
			}
		}

		return false;
	}
}
