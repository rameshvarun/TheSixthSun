using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The main HexPlanet script.
/// </summary>
public class HexPlanet : MonoBehaviour {

	public Vector2[] uv;
	public List<Triangle> triangles;

	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;


		float t = (1.0f + Mathf.Sqrt(5.0f))/2.0f;

		List<Vector3> vertices = new List<Vector3>(12);

		vertices.Add(new Vector3(-1,  t,  0));
		vertices.Add(new Vector3(1,  t,  0));
		vertices.Add(new Vector3(-1,  -t,  0));
		vertices.Add(new Vector3(1,  -t,  0));

		vertices.Add(new Vector3(0, -1, t));
		vertices.Add(new Vector3(0, 1, t));
		vertices.Add(new Vector3(0, -1, -t));
		vertices.Add(new Vector3(0, 1, -t));

		vertices.Add(new Vector3(t, 0, -1));
		vertices.Add(new Vector3(t, 0, 1));
		vertices.Add(new Vector3(-t, 0, -1));
		vertices.Add(new Vector3(-t, 0, 1));

		for(int i = 0; i < vertices.Count; ++i) {
			vertices[i] = vertices[i].normalized;
		}

		triangles = new List<Triangle>();

		Triangle.vertices = vertices;
		triangles.Add(new Triangle(0, 11, 5));
		triangles.Add(new Triangle(0, 5, 1));
		triangles.Add(new Triangle(0, 1, 7));
		triangles.Add(new Triangle(0, 7, 10));
		triangles.Add(new Triangle(0, 10, 11));

		triangles.Add(new Triangle(1, 5, 9));
		triangles.Add(new Triangle(5, 11, 4));
		triangles.Add(new Triangle(11, 10, 2));
		triangles.Add(new Triangle(10, 7, 6));
		triangles.Add(new Triangle(7, 1, 8));

		triangles.Add(new Triangle(3, 9, 4));
		triangles.Add(new Triangle(3, 4, 2));
		triangles.Add(new Triangle(3, 2, 6));
		triangles.Add(new Triangle(3, 6, 8));
		triangles.Add(new Triangle(3, 8, 9));

		triangles.Add(new Triangle(4, 9, 5));
		triangles.Add(new Triangle(2, 4, 11));
		triangles.Add(new Triangle(6, 2, 10));
		triangles.Add(new Triangle(8, 6, 7));
		triangles.Add(new Triangle(9, 8, 1));

		for(int i = 0; i < 3; ++i) {
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


		mesh.vertices = Triangle.ToVertices(triangles);
		mesh.triangles = Triangle.ToIndices(triangles);

		//Generate UV Coordinates
		foreach(Triangle triangle in triangles) {
			triangle.i_uv = new Vector2(0.5f, -0.94999f);
			triangle.j_uv = new Vector2(0.8897f, -0.275f);
			triangle.k_uv = new Vector2(0.1103f, -0.275f);
		}
		mesh.uv = Triangle.ToUVS(triangles);

		//Calculate smoothed normals
		List<Vector3> normals = new List<Vector3>(mesh.vertices.Length);
		foreach(Vector3 vertex in mesh.vertices) normals.Add(vertex);
		mesh.normals = normals.ToArray();

		Debug.Log("Creating node-edge graph from mesh.");

		//Generate node-edge graph
		Dictionary<Vector3, int> vertex_to_node = new Dictionary<Vector3, int>();
		int id = 0;
		foreach(Vector3 vertex in mesh.vertices) {
			if(!vertex_to_node.ContainsKey(vertex)) {
				vertex_to_node[vertex] = id;
				++id;
			}
		}

		Dictionary<int, HashSet<int>> graph = new Dictionary<int, HashSet<int>>();
		foreach(int node in vertex_to_node.Values) {
			graph[node] = new HashSet<int>();
		}
		foreach(Triangle triangle in triangles) {
			//Add all possible edges to edge graph
			graph[vertex_to_node[triangle.i]].Add(vertex_to_node[triangle.j]);
			graph[vertex_to_node[triangle.i]].Add(vertex_to_node[triangle.k]);

			graph[vertex_to_node[triangle.j]].Add(vertex_to_node[triangle.i]);
			graph[vertex_to_node[triangle.j]].Add(vertex_to_node[triangle.k]);

			graph[vertex_to_node[triangle.k]].Add(vertex_to_node[triangle.i]);
			graph[vertex_to_node[triangle.k]].Add(vertex_to_node[triangle.j]);
		}

		mesh.RecalculateBounds();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
