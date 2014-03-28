using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	enum CameraMode {
		/// <summary>Panning in the global space view.</summary>
		Panning,
		Planet
	};

	private CameraMode mode = CameraMode.Panning;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(mode == CameraMode.Panning) {
		}
	}
}
