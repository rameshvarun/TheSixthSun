using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * The purpose of this script is to look at the GameState singleton and actually construct the visual scene from the provided Prefabs
 */
public class SceneBuilder : MonoBehaviour {
	public Transform hexTile;
	public Transform planet;

	public Transform star;

	public Transform cityTransform;

	public List<string> groundUnits_keys;
	public List<Transform> groundUnits_values;
	private Dictionary<string, Transform> groundUnits = new Dictionary<string, Transform>();

	public List<string> spaceUnits_keys;
	public List<Transform> spaceUnits_values;
	private Dictionary<string, Transform> spaceUnits = new Dictionary<string, Transform>();

	public Transform CreateCity(City city) {
		Debug.Log("City on node " + city.node);
		Transform unitTransform = (Transform)Instantiate(cityTransform);
		unitTransform.parent = city.planet.hexPlanet.transform;
		unitTransform.localPosition = city.planet.hexPlanet.getNodePosition(city.node);
		unitTransform.localRotation = city.planet.hexPlanet.getNodeOrientation(city.node);

		unitTransform.GetComponent<CityBehavior>().Link(city);

		return unitTransform;
	}

	public Transform CreateGroundUnit(GroundUnit groundUnit) {
		Debug.Log("Ground unit on node " + groundUnit.node);
		Transform unitTransform = (Transform)Instantiate(groundUnits[groundUnit.type]);
		unitTransform.parent = groundUnit.planet.hexPlanet.transform;
		unitTransform.localPosition = groundUnit.planet.hexPlanet.getNodePosition(groundUnit.node);
		unitTransform.localRotation = groundUnit.planet.hexPlanet.getNodeOrientation(groundUnit.node);

		unitTransform.GetComponent<GroundUnitBehavior>().Link(groundUnit);
		return unitTransform;
	}

	// Use this for initialization
	void Start () {
		//Sew up dictionaries from lists
		for(int i = 0; i < groundUnits_keys.Count; ++i) groundUnits[groundUnits_keys[i]] = groundUnits_values[i];
		for(int i = 0; i < spaceUnits_keys.Count; ++i) spaceUnits[spaceUnits_keys[i]] = spaceUnits_values[i];

		//Create empty game state with three players
		GameState.Instance = new GameState();
		GameState.Instance.players.Add(new Player());
		GameState.Instance.players.Add(new Player());
		GameState.Instance.players.Add(new Player());
		GameState.Instance.currentPlayer = GameState.Instance.players[0];

		//Populate game state
		MapGenerators.BasicMap(GameState.Instance);

		//Create empty space grid
		foreach(KeyValuePair<HexCoord, int> tile in GameState.Instance.grid.tiles) {
			Instantiate(hexTile,tile.Key.toPosition(1.0f),Quaternion.identity);
		}

		//Spawn in planets
		foreach(Planet p in GameState.Instance.planets) {
			Debug.Log("Planet at " + p.coordinate, this);
			Transform planetTransform = (Transform)Instantiate(planet, p.coordinate.toPosition(1.0f) + new Vector3(0, 0.5f, 0),Quaternion.identity);
			planetTransform.GetComponent<PlanetBehavior>().Link(p);
			planetTransform.GetComponent<HexPlanet>().CreatePlanet();

			//Spawn all ground units on the surface
			foreach(ILandObject landObject in p.landObjects) {
				if(landObject is GroundUnit) CreateGroundUnit((GroundUnit)landObject);

				if(landObject is City) CreateCity((City)landObject);
			}
		}
		
		//Spawn in Stars
		foreach(Star s in GameState.Instance.stars) {
			Debug.Log("Star at " + s.coordinate, this);
			Transform transform = (Transform)Instantiate(star, s.coordinate.toPosition(1.0f) + new Vector3(0, 0.5f, 0),Quaternion.identity);
			transform.GetComponent<StarBehavior>().star = s; //Give behavior a link to the star object
		}

		//Spawn in spaceunits
		foreach(ISpaceUnit spaceUnit in GameState.Instance.spaceUnits) {
			if(spaceUnit is Spaceship) {
				Spaceship spaceship = (Spaceship)spaceUnit;

				Transform spaceshipTransform = (Transform)Instantiate(spaceUnits[spaceship.type], spaceship.coordinate.toPosition(1.0f), Quaternion.identity);
				spaceshipTransform.GetComponent<SpaceshipBehavior>().spaceship = spaceship;
			}
			else if(spaceUnit is Fleet) {
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
