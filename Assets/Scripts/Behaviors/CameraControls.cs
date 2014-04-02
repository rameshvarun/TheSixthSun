	using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	enum CameraMode {
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

	private CameraMode mode = CameraMode.Panning;
	private GameObject inspectTarget;

	private GameObject gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		if(mode == CameraMode.Panning) {
			//Set Camera position
			transform.position = new Vector3(gridPosX + xDisplacement, transform.position.y, gridPosY);
			Vector3 targetPosition = new Vector3(gridPosX + xDisplacement, cameraHeight, gridPosY);
			transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
			transform.LookAt(new Vector3(gridPosX, 0, gridPosY));

			#if UNITY_EDITOR
				//Mouse-wheel scrolling
				cameraHeight -= scrollSpeed*Input.GetAxis("Mouse ScrollWheel");
			#endif
			#if UNITY_ANDROID
				//Multi-touch scroll
				if(Input.touchCount == 2 ) {
					Vector2 touchDifference = Input.GetTouch(0).position - Input.GetTouch(1).position;
					Vector2 previousTouchDifference = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
					float scale = touchDifference.magnitude / previousTouchDifference.magnitude;
					
					cameraHeight /= scale*scale*scale;
				}
			#endif
			cameraHeight = Mathf.Clamp( cameraHeight, minimumHeight, maximumHeight );


			//Get Panning Speed
			float moveSpeed = baseMoveSpeed*cameraHeight;
			moveSpeed = Mathf.Max(minimumMoveSpeed, moveSpeed)*Time.deltaTime;

			#if UNITY_ANDROID
				//Touch based panning
				if(Input.touchCount == 1) {
					Vector2 move = -Input.GetTouch(0).deltaPosition;
					gridPosY += move.x*moveSpeed;
					gridPosX -= move.y*moveSpeed;
				}
			#endif
			#if UNITY_EDITOR
				//Mouse Based Panning
				if( Input.mousePosition.x < mouseMargin || Input.mousePosition.x > Screen.width - mouseMargin ||
				   Input.mousePosition.y < mouseMargin || Input.mousePosition.y > Screen.height - mouseMargin ) {
					Vector2 move = new Vector3( Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
					move.Normalize();
					gridPosY += move.x*moveSpeed;
					gridPosX -= move.y*moveSpeed;
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
				}
			}
		}

		if( mode == CameraMode.Planet ) {

			transform.position = Vector3.Lerp(transform.position, inspectTarget.transform.TransformPoint(inspectDisplacement*rotateRadius), 0.2f);

			Quaternion q = Quaternion.LookRotation(inspectTarget.transform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, q, 0.5f);

			Vector2 move = new Vector2();
			#if UNITY_ANDROID
				//Touch based panning
				if(Input.touchCount == 1) {
					move = -Input.GetTouch(0).deltaPosition;
				}
			#endif
			#if UNITY_EDITOR
				//Mouse Based Panning
				if( Input.mousePosition.x < mouseMargin || Input.mousePosition.x > Screen.width - mouseMargin ||
				   Input.mousePosition.y < mouseMargin || Input.mousePosition.y > Screen.height - mouseMargin ) {
					move = new Vector3( Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
					move.Normalize();
				}
			#endif
			inspectDisplacement = Quaternion.AngleAxis(-move.x, Vector3.up)*inspectDisplacement;
			inspectDisplacement = Quaternion.AngleAxis(-move.y, Vector3.Cross(Vector3.up, inspectDisplacement))*inspectDisplacement;

			#if UNITY_ANDROID
				if(Input.touchCount == 2 ) {
					//Exit with a pinch gesture
					Vector2 touchDifference = Input.GetTouch(0).position - Input.GetTouch(1).position;
					Vector2 previousTouchDifference = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
					float scale = touchDifference.magnitude / previousTouchDifference.magnitude;
					rotateRadius /= scale*scale*scale;
				}
			#endif

			#if UNITY_EDITOR
				rotateRadius -= Input.GetAxis("Mouse ScrollWheel");
			#endif

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

		if(mode == CameraMode.Move) {
			transform.LookAt(moveTarget.transform);
			transform.position = moveTarget.GetComponent<UnitBehavior>().getInspectCameraPosition();
		}

		if( mode == CameraMode.Returning ) {
			Vector3 targetPosition = new Vector3(gridPosX + xDisplacement, cameraHeight, gridPosY);
			Quaternion targetOrientation = Quaternion.LookRotation(new Vector3(gridPosX, 0, gridPosY) - targetPosition);

			transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f*Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetOrientation, 5.0f*Time.deltaTime);

			if(Vector3.Distance(transform.position, targetPosition) < 0.1f)
				mode = CameraMode.Panning;
		}
	}

	void OnGUI() {
		if(mode == CameraMode.Move) {
			if (GUI.Button(new Rect(10, 10, 150, 100), "Back"))
				mode = previousMode;

			if(moveTarget.GetComponent<UnitBehavior>().MoveGUI())
				mode = previousMode;
		}
	}

	private GameObject moveTarget;
	private CameraMode previousMode;

	Vector3 inspectDisplacement;
	float rotateRadius;
}
