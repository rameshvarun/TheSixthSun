using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	CameraControls cameraControls;

	// Use this for initialization
	void Start ()
	{
		cameraControls = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControls>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnGUI() {
		if(cameraControls != CameraControls.CameraMode.Move) {
			GUI.TextArea(new Rect(Screen.width - 250, Screen.height - 250, 150, 50), GameState.Instance.unmovedUnits().Count + " Units Awaiting Orders.");
			if(GUI.Button(new Rect(Screen.width - 250, Screen.height - 200, 150, 100), "End My Turn")) {
				GameState.Instance.advanceTurn();
			}
		}
	}
}

