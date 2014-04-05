using UnityEngine;
using System.Collections;
using System;

public class SelectableBehavior : MonoBehaviour {
	public Action onClick;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Selecting an object
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				if(hit.collider.gameObject.transform == transform) {
					onClick();
				}
			}
		}
	}
}
