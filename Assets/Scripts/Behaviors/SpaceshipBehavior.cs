using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceshipBehavior : UnitBehavior {

	/// <summary>Reference back to the spaceship simulation object.</summary>
	public Spaceship spaceship;

	/// <summary>Should refer to the prefab that will be instantiated as a move selector.</summary>
	public Transform moveSelector;

	private List<Transform> moveSelectors = new List<Transform>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, spaceship.coordinate.toPosition(), 5*Time.deltaTime );
	}

	public void clearMoveSelectors() {
		foreach(Transform selector in moveSelectors) {
			Destroy(selector.gameObject);
		}
		moveSelectors.Clear();
	}

	public override Vector3 getInspectCameraPosition() {
		return transform.position + new Vector3(2, 2, 0);
	}
	
	public override bool MoveGUI() {

		if(spaceship.canMove()) {
			if (GUI.Button(new Rect(200, 10, 150, 100), "Move")) {
				//Delete move selectors if there were any
				this.clearMoveSelectors();

				int movement_range = (int)(Spaceship.Balance[spaceship.type]["move"]);

				//Construct movement selectors
				foreach(HexCoord coordinate in GameState.Instance.grid.getMovementRange(spaceship.coordinate, movement_range)) {
					Transform selectorTransform = (Transform)Instantiate(moveSelector, coordinate.toPosition(), Quaternion.identity);
					moveSelectors.Add(selectorTransform);

					HexCoord target_coordinate = coordinate;
					selectorTransform.GetComponent<SelectableBehavior>().onClick += () => {
						this.spaceship.coordinate = target_coordinate;
						this.spaceship.hasMoved = true;
						this.clearMoveSelectors();
					};
				}

			}

			//Skipping turn
			if (GUI.Button(new Rect(400, 10, 150, 100), "Skip Turn")) {
				spaceship.hasMoved = true;
				return true;
			}

			//Attack
			if (GUI.Button(new Rect(600, 10, 150, 100), "Attack")) {
				spaceship.hasMoved = true;
				return true;
			}

			//Join Fleet
			if (GUI.Button(new Rect(800, 10, 150, 100), "Join Fleet")) {
				spaceship.hasMoved = true;
				return true;
			}
		}


		return false;
	}
}
