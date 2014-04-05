using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Camera controls. Details the camera modes necessary for close-up interaction with planets and ground units, and space units.
/// </summary>
public class CameraControls : MonoBehaviour {

	/// <summary>Camera mode enumeration of those currently in use.</summary>
	public enum CameraMode {
		/// <summary>Panning in the global space view.</summary>
		Panning,
		Planet,
		Returning,
		Move
	};

	//Panning mode values
	public float gridPosX = 0.0f;
	public float gridPosY = 0.0f;
	public float cameraHeight = 10.0f;
	public float xDisplacement = 5.0f;
	public float baseMoveSpeed = 1.0f;
	public float minimumMoveSpeed = 6.0f;
	public float mouseMargin = 100;
	public float scrollSpeed = 10.0f;
	public float minimumHeight = 2.0f;
	public float maximumHeight = 35.0f;
	public float minRotateRadius = 2.0f;

	private float maxTileRadius;

	public CameraMode mode = CameraMode.Panning;
	private GameObject inspectTarget;

	private GameObject gameController;

	// Use this for initialization
	/// <summary>
	/// Initializes the game controller and defines the outer limits of the gameplay area.
	/// </summary>
	void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController");
		maxTileRadius = 0;
		foreach (KeyValuePair<HexCoord, int> tile in GameState.Instance.grid.tiles) {
			if(tile.Key.toPosition(1).magnitude > maxTileRadius) maxTileRadius = tile.Key.toPosition(1).magnitude;
		}
	}
	
	// Update is called once per frame
	/// <summary>
	/// Regulates camera behavior and movement based on the mode in which the camera is in, also providing for transitions between different modes.
	/// </summary>
	void Update () {

		//Panning mode: used to view overall hex tile map in space.
		if(mode == CameraMode.Panning) {
			//Set Camera position
			transform.position = new Vector3(gridPosX + xDisplacement, transform.position.y, gridPosY);
			Vector3 targetPosition = new Vector3(gridPosX + xDisplacement, cameraHeight, gridPosY);
			transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
			transform.LookAt(new Vector3(gridPosX, 0, gridPosY));

			Vector3 newPosition = new Vector3(gridPosX, 0, gridPosY);
			//limit camera to the game board
			if(newPosition.magnitude > maxTileRadius){
				newPosition.Normalize();
				newPosition *= maxTileRadius;
				gridPosX = newPosition.x;
				gridPosY = newPosition.z;
			}

			cameraHeight = Mathf.Clamp( cameraHeight, minimumHeight, maximumHeight );

			//Get Panning Speed
			float moveSpeed = baseMoveSpeed*cameraHeight;
			moveSpeed = Mathf.Max(minimumMoveSpeed, moveSpeed)*Time.deltaTime;

			Vector2 move = new Vector2();

			#if UNITY_EDITOR
				//Mouse Based Panning
				mousePanning(ref move);

				//Mouse-wheel scrolling
				cameraHeight -= scrollSpeed*Input.GetAxis("Mouse ScrollWheel");
			#endif
			#if UNITY_ANDROID
				//Touch based panning
				if(Input.touchCount == 1) {
					move = -Input.GetTouch(0).deltaPosition;
					gridPosY += move.x*moveSpeed;
					gridPosX -= move.y*moveSpeed;
				}

				//Multi-touch scroll
				if(Input.touchCount == 2 ) {
					cameraHeight /= (float)(Math.Pow(touchScroll(),3.0));
				}
			#endif

			//Selecting an object
			if(Input.GetMouseButtonUp(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if( Physics.Raycast(ray, out hit) ) {
					//Planets can be inspected from space
					if(hit.collider.gameObject.tag == "Planet") {
						inspectTarget = hit.collider.gameObject;
						mode = CameraMode.Planet;
						inspectDisplacement = inspectTarget.transform.InverseTransformPoint(transform.position).normalized;
						rotateRadius = 2.0f;
					}

					if(hit.collider.gameObject.tag == "SpaceUnit") {
						mode = CameraMode.Move;
						previousMode = CameraMode.Panning;
						moveTarget = hit.collider.gameObject;
					}
				}
			}
		}

		//Planet mode: camera rotates around the planet to view ground hex map for a specific planet.
		if( mode == CameraMode.Planet ) {

			transform.position = Vector3.Lerp(transform.position, inspectTarget.transform.TransformPoint(inspectDisplacement*rotateRadius), 0.2f);

			Quaternion q = Quaternion.LookRotation(inspectTarget.transform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, q, 0.5f);

			Vector2 move = new Vector2();

			#if UNITY_EDITOR
				//Mouse Based Panning
				mousePanning(ref move);

				//Mouse based scrolling
				rotateRadius -= Input.GetAxis("Mouse ScrollWheel");
			#endif
			#if UNITY_ANDROID
			//Touch based panning
			if(Input.touchCount == 1) {
				move = -Input.GetTouch(0).deltaPosition;
			}
			
			//Touch based scrolling
			if(Input.touchCount == 2 ) {
				rotateRadius /= (float)(Math.Pow(touchScroll(),3.0));
			}
			#endif

			inspectDisplacement = Quaternion.AngleAxis(-move.x, Vector3.up)*inspectDisplacement;
			inspectDisplacement = Quaternion.AngleAxis(-move.y, Vector3.Cross(Vector3.up, inspectDisplacement))*inspectDisplacement;

			if(rotateRadius > 3.5f) mode = CameraMode.Returning;
			if(rotateRadius < minRotateRadius) rotateRadius = minRotateRadius;

			//Selecting an object
			if(Input.GetMouseButtonUp(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit)) {
					if(hit.collider.gameObject.tag == "GroundUnit") {
						mode = CameraMode.Move;
						previousMode = CameraMode.Planet;
						moveTarget = hit.collider.gameObject;
					}
				}
			}
		}

		//Move mode: used to select units; activates the GUI with specific action options
		if(mode == CameraMode.Move) {
			Quaternion targetRotation = Quaternion.LookRotation(moveTarget.transform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10.0f*Time.deltaTime);

			Vector3 targetPosition = moveTarget.GetComponent<UnitBehavior>().getInspectCameraPosition();
			transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f*Time.deltaTime);
		}

		//Returning mode: Used to move back from a planet.
		if( mode == CameraMode.Returning ) {
			Vector3 targetPosition = new Vector3(gridPosX + xDisplacement, cameraHeight, gridPosY);
			Quaternion targetOrientation = Quaternion.LookRotation(new Vector3(gridPosX, 0, gridPosY) - targetPosition);

			transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f*Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetOrientation, 5.0f*Time.deltaTime);

			if(Vector3.Distance(transform.position, targetPosition) < 0.1f)
				mode = CameraMode.Panning;
		}
	}

	/// <summary>
	/// Adds GUI options on top of the camera - allows the user to go back a mode.
	/// </summary>
	void OnGUI() {
		if(mode == CameraMode.Move) {
			if ( GUI.Button(new Rect(10, 10, 150, 100), "Back") || moveTarget.GetComponent<UnitBehavior>().MoveGUI() ) {
				mode = previousMode;
				moveTarget.GetComponent<UnitBehavior>().cleanUp();
			}
		}
	}

	/// <summary>
	/// Pans over the map using the mouse if it's in the margin of the screen.
	/// </summary>
	/// <param name="move">Vector that controls direction of movement - values modified/created by function</param>
	void mousePanning(ref Vector2 move){

		if(Input.mousePosition.x < mouseMargin || Input.mousePosition.x > Screen.width - mouseMargin ||
		   Input.mousePosition.y < mouseMargin || Input.mousePosition.y > Screen.height - mouseMargin ) {

			move = new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
			move.Normalize();

			if(mode == CameraMode.Panning){

				//Get Panning Speed
				float moveSpeed = baseMoveSpeed*cameraHeight;
				moveSpeed = Mathf.Max(minimumMoveSpeed, moveSpeed)*Time.deltaTime;

				//move grid position
				gridPosY += move.x*moveSpeed;
				gridPosX -= move.y*moveSpeed;
			}
		}
	}

	/// <summary>
	/// Scrolls in and out of the map using touch controls.
	/// </summary>
	/// <returns>The scale of the scroll to be used to edit camera height</returns>
	double touchScroll(){

		Vector2 touchDifference = Input.GetTouch(0).position - Input.GetTouch(1).position;
		Vector2 previousTouchDifference = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
		return touchDifference.magnitude / previousTouchDifference.magnitude;
	}

	private GameObject moveTarget;
	private CameraMode previousMode;

	private Vector3 inspectDisplacement;
	private float rotateRadius;
}
