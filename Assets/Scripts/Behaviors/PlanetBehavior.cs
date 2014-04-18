using UnityEngine;
using System.Collections;

public class PlanetBehavior : MonoBehaviour
{
	public Planet planet;

	public float rotateSpeed;

	// Use this for initialization
	void Start ()
	{

	}

	public void Link(Planet planet) {
		this.planet = planet;

		planet.hexPlanet = GetComponent<HexPlanet>();
		planet.behavior = this;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(0, rotateSpeed*Time.deltaTime, 0);
	}
}

