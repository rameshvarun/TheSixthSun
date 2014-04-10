using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The main HexPlanet script.
/// </summary>
public class HexPlanet : MonoBehaviour {
	/// <summary>The planet mesh built procedurally.</summary>
	Mesh mesh;

	/// <summary>Dictionary used to store graph in an edge-list format.</summary>
	public Dictionary<int, HashSet<int>> graph;

	/// <summary>Dictionary used to lookup the node number from a cartesian position</summary>
	public Dictionary<Vector3, int> vertex_to_node;

	/// <summary>Dictionary used to convert node ids to a position</summary>
	public Dictionary<int, Vector3> node_to_vertex;

	/// <summary>
	/// Gets the node that is neares to a given position.
	/// </summary>
	/// <returns>The nearest node.</returns>
	/// <param name="position">The position to be converted to a node number.</param>
	public int getNearestNode(Vector3 position) {
		int nearest_node = -1;
		float nearest_distance = 0.0f;
		foreach(KeyValuePair<Vector3, int> entry in vertex_to_node) {
			float distance = Vector3.Distance(position, entry.Key);
			if(nearest_node < 0 || distance < nearest_distance) {
				nearest_node = entry.Value;
				nearest_distance = distance;
			}
		}
		return nearest_node;
	}

	/// <summary>Converts a node index to a position. Should be used for placing units and buildings.</summary>
	public Vector3 getNodePosition(int node) {
		return node_to_vertex[node];
	}

	/// <summary> Returns the Orientation that an object stationed on a particular node should have. </summary>
	public Quaternion getNodeOrientation(int node) {
		return Quaternion.LookRotation(Vector3.Cross(getNodePosition(node).normalized, Vector3.forward), getNodePosition(node).normalized);
	}

	/// <summary>
	/// Gets the indices of the nodes that neigbor the current node index.
	/// </summary>
	/// <returns>An integer array of all of the neigboring node indices.</returns>
	/// <param name="node">The initial node index.</param>
	public int[] getNodeNeigbors(int node) {
		int[] neighbors = new int[graph[node].Count];
		graph[node].CopyTo(neighbors);
		return neighbors;
	}

	/// <summary>
	/// Returns all the nodes within a certain movement range of the given tile.
	/// </summary>
	/// <returns>A HashSet<int> of moveable nodes.</returns>
	/// <param name="startNode">The start node index.</param>
	/// <param name="range">The range that the given unit can move.</param>
	public HashSet<int> getMovementRange(int startNode, int range) {
		var moveableTiles = new HashSet<int>();

		var searchQueue = new Queue<int>();
		var ranges = new Dictionary<int, int>();

		searchQueue.Enqueue(startNode);
		ranges[startNode] = 0;

		while(searchQueue.Count > 0) {
			int currentNode = searchQueue.Dequeue();

			if(ranges[currentNode] <= range) {
				moveableTiles.Add(currentNode);

				foreach(int neighbor in getNodeNeigbors(currentNode)){
					if(!ranges.ContainsKey(neighbor)) {
						//TODO: Also confirm that this is a movable square

						ranges[neighbor] = ranges[currentNode] + 1;
						searchQueue.Enqueue(neighbor);
					}
				}
			}
		}

		moveableTiles.Remove(startNode);
		return moveableTiles;
	}

	// Use this for initialization
	void Start () {
	}

	/// <summary>
	/// A static function that can construct a Planet's vertex-edge graph
	/// without actually creating the mesh. The purpose of this is so that in the
	/// Map Generation phase, we can generate terrain features (this requires the vertex-edge graph)
	/// without actually constructing the mesh.
	/// </summary>
	/// <returns>A vertex-edge graph that represents the surface of the planet.</returns>
	public static Dictionary<int, HashSet<int>> createOnlyPlanetGraph(int subdivisions) {
		List<Triangle> triangles = generatePlanetTriangles(subdivisions);
	
		Dictionary<Vector3, int> vertex_to_node;
		Dictionary<int, Vector3> node_to_vertex;

		//Generate lookup tables
		generateLookups(triangles, out vertex_to_node, out node_to_vertex);
		
		//Generate vertex-edge graph
		return generatePlanetGraph(vertex_to_node, triangles);
	}

	private static List<Triangle> generatePlanetTriangles(int subdivisions) {
		Debug.Log("Generating triangles for planet...");

		List<Triangle> triangles = HexPlanetUtils.generateIcosohedron();
		
		for(int i = 0; i < subdivisions; ++i) {
			Debug.Log("Subdividing...");
			//Subdivision phase
			List<Triangle> newTriangles = new List<Triangle>();
			foreach(Triangle tri in triangles) {
				Vector3 a = (tri.i/2 + tri.j/2).normalized;
				Vector3 b = (tri.j/2 + tri.k/2).normalized;
				Vector3 c = (tri.i/2 + tri.k/2).normalized;
				
				newTriangles.Add(new Triangle(tri.i, a, c));
				newTriangles.Add(new Triangle(tri.j, b, a));
				newTriangles.Add(new Triangle(tri.k, c, b));
				newTriangles.Add(new Triangle(a, b, c));
			}
			triangles = newTriangles;
		}

		return triangles;
	}

	private static Dictionary<int, HashSet<int>> generatePlanetGraph(Dictionary<Vector3, int> vertex_to_node, List<Triangle> triangles) {
		Debug.Log("Creating node-edge graph from mesh.");

		Dictionary<int, HashSet<int>> graph = new Dictionary<int, HashSet<int>>();

		//Initialize empty edge lists
		foreach(int node in vertex_to_node.Values) {
			graph[node] = new HashSet<int>();
		}
		
		//Populate edge lists
		foreach(Triangle triangle in triangles) {
			//Add all possible edges to edge graph
			graph[vertex_to_node[triangle.i]].Add(vertex_to_node[triangle.j]);
			graph[vertex_to_node[triangle.i]].Add(vertex_to_node[triangle.k]);
			
			graph[vertex_to_node[triangle.j]].Add(vertex_to_node[triangle.i]);
			graph[vertex_to_node[triangle.j]].Add(vertex_to_node[triangle.k]);
			
			graph[vertex_to_node[triangle.k]].Add(vertex_to_node[triangle.i]);
			graph[vertex_to_node[triangle.k]].Add(vertex_to_node[triangle.j]);
		}

		return graph;
	}

	private static void generateLookups(List<Triangle> triangles,
	                                   out Dictionary<Vector3, int> vertex_to_node,
	                                   out Dictionary<int, Vector3> node_to_vertex) {

		vertex_to_node = new Dictionary<Vector3, int>();
		node_to_vertex = new Dictionary<int, Vector3>();

		//Generate node-edge graph
		int id = 0;
		foreach(Vector3 vertex in Triangle.ToVertices(triangles)) {
			if(!vertex_to_node.ContainsKey(vertex)) {
				//Enable hashing in both directions
				vertex_to_node[vertex] = id;
				node_to_vertex[id] = vertex;
				++id;
			}
		}
	}

	class helper_struct {
		public int terrain_type;
		public Vector2 uv;
		public int order;

		public helper_struct(int terrain_type, int order) {
			this.terrain_type = terrain_type;
			this.uv = Vector2.zero;
			this.order = order;
		}

		public override string ToString () {
			return string.Format ("{0}, {2}: {1}", terrain_type, uv, order);
		}
	}

	private static List<Vector2> computeCoordinates(int x, int y, int z) {
		int a = 0;
		int j = 21;
		for(int i = 0; i < x; ++i) {
			a += j;
			j -= (6 - i);
		}

		int b = 0;
		for(int i = 0; i < y; ++i) {
			if(i < x) continue;
			b += (6 - i);
		}

		int c = z - y;

		int TILE_SIZE = 85;
		int tileX = ((a + b + c) % 7) * TILE_SIZE;
		int tileY = ((a + b + c) / 7) * TILE_SIZE;

		List<Vector2> points = new List<Vector2>();
		float t = 0.0f;
		while(t < Mathf.PI * 2.0) {
			Vector2 p = new Vector2(Mathf.Sin(t)*0.44f + 0.5f, Mathf.Cos(t)*0.44f + 0.5f );
			p.x *= TILE_SIZE;
			p.y *= TILE_SIZE;
			points.Add(p);

			t += Mathf.PI * (2.0f / 3.0f);
		}

		List<Vector2> returnPoints = new List<Vector2>();
		returnPoints.Add(points[0] + new Vector2(tileX, tileY));
		returnPoints.Add(points[2] + new Vector2(tileX, tileY));
		returnPoints.Add(points[1] + new Vector2(tileX, tileY));

		return returnPoints;
	}

	private static void populateUVCoordinates(List<Triangle> triangles, Dictionary<Vector3, int> vertex_to_node, int[] terrain) {
		//Generate UV Coordinates
		foreach(Triangle triangle in triangles) {
			int i = terrain[vertex_to_node[triangle.i]];
			int j = terrain[vertex_to_node[triangle.j]];
			int k = terrain[vertex_to_node[triangle.k]];

			List<helper_struct> triples = new List<helper_struct>();
			triples.Add(new helper_struct(i, 0));
			triples.Add(new helper_struct(j, 1));
			triples.Add(new helper_struct(k, 2));

			List<helper_struct> sd = triples.OrderBy(x => x.terrain_type).ToList();
			Debug.Log(sd[0] + ", " + sd[1] + ", " + sd[2]);

			List<Vector2> coords = computeCoordinates(sd[0].terrain_type, sd[1].terrain_type, sd[2].terrain_type);
			sd[0].uv = coords[0];
			sd[1].uv = coords[1];
			sd[2].uv = coords[2];

			List<helper_struct> resorted = sd.OrderBy(x => x.order).ToList();
			Debug.Log(resorted[0] + ", " + resorted[1] + ", " + resorted[2]);

			float IMAGE_WIDTH = 595.0f;
			float IMAGE_HEIGHT = 680.0f;


			//Debug.Log(resorted[0].terrain_type + " : " + resorted[0].uv
			//          + ", " + resorted[1].terrain_type + " : " + resorted[1].uv
			 //         + ", " + resorted[2].terrain_type + " : " + resorted[2].uv);

			foreach(helper_struct s in resorted) {

				s.uv.x /= IMAGE_WIDTH;
				s.uv.y = 1 - s.uv.y/IMAGE_HEIGHT;
			}

			triangle.i_uv = resorted[0].uv;
			triangle.j_uv = resorted[1].uv;
			triangle.k_uv = resorted[2].uv;

			/*triangle.i_uv = triples[0].uv;
			triangle.j_uv = triples[1].uv;
			triangle.k_uv = triples[2].uv;*/
			/*triangle.i_uv = new Vector2(42.5f / IMAGE_WIDTH, 1 - (80.75f / IMAGE_HEIGHT));
			triangle.j_uv = new Vector2(9.37f / IMAGE_WIDTH, 1 - (23.375f / IMAGE_HEIGHT));
			triangle.k_uv = new Vector2(75.6254f / IMAGE_WIDTH, 1 - (23.375f / IMAGE_HEIGHT));*/
		}
	}

	/// <summary>
	/// When called, this function actually constructs the planet mesh, subdivides, and calculates the vertex-edge graph.
	/// To generate the vertex-edge graph without actually making a planet, see createOnlyPlanetGraph.
	/// </summary>
	public void CreatePlanet() {
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		
		List<Triangle> triangles = generatePlanetTriangles(GetComponent<PlanetBehavior>().planet.subdivisions);
		
		mesh.vertices = Triangle.ToVertices(triangles);
		mesh.triangles = Triangle.ToIndices(triangles);


		
		//Calculate smoothed normals
		List<Vector3> normals = new List<Vector3>(mesh.vertices.Length);
		foreach(Vector3 vertex in mesh.vertices) normals.Add(vertex);
		mesh.normals = normals.ToArray();

		//Generate lookup tables
		generateLookups(triangles, out vertex_to_node, out node_to_vertex);

		//Calculate texture coordinates
		populateUVCoordinates(triangles, vertex_to_node, GetComponent<PlanetBehavior>().planet.terrain);
		mesh.uv = Triangle.ToUVS(triangles);

		//Generate vertex-edge graph
		graph = generatePlanetGraph(vertex_to_node, triangles);
		
		mesh.RecalculateBounds();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
