﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundUnitBehavior : UnitBehavior {

	/// <summary>The simulation GroundUnit object.</summary>
	public GroundUnit groundUnit;

	/// <summary> The Game Object representing the Planet that this unit is on.</summary>
	public Transform planet;
	public HexPlanet hexPlanet;

	public Transform moveSelector;

	private List<Transform> moveSelectors = new List<Transform>();

	// Use this for initialization
	void Start () {
		hexPlanet = planet.GetComponent<HexPlanet>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.Lerp(transform.localPosition, hexPlanet.getNodePosition(groundUnit.node), 5*Time.deltaTime );
		transform.localRotation = Quaternion.Lerp(transform.localRotation, hexPlanet.getNodeOrientation(groundUnit.node), 5*Time.deltaTime);
	}

	public override Vector3 getInspectCameraPosition ()
	{
		return transform.TransformPoint(new Vector3(0,5,-1));
	}

	public void clearMoveSelectors() {
		foreach(Transform selector in moveSelectors) {
			Destroy(selector.gameObject);
		}
		moveSelectors.Clear();
	}

	public override bool MoveGUI() {
		if(groundUnit.canMove()) {
			if (GUI.Button(new Rect(200, 10, 150, 100), "Move")) {
				int movement_range = (int)(GroundUnit.Balance[groundUnit.type]["move"]);
				foreach(int node in hexPlanet.getMovementRange(groundUnit.node, movement_range)) {
					Transform selectorTransform = (Transform)Instantiate(moveSelector);
					selectorTransform.parent = planet;
					selectorTransform.localPosition = hexPlanet.getNodePosition(node);
					selectorTransform.localRotation = hexPlanet.getNodeOrientation(node);
					moveSelectors.Add(selectorTransform);

					int n = node;
					selectorTransform.GetComponent<SelectableBehavior>().onClick += () => {
						this.groundUnit.node = n;
						this.groundUnit.hasMoved = true;
						this.clearMoveSelectors();
					};
				}
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