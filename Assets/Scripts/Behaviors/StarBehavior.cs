using UnityEngine;
using System.Collections;

public class StarBehavior : MonoBehaviour
{
	public Star star;

	public float rotateSpeed;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(0, rotateSpeed*Time.deltaTime, 0);
	}
}

