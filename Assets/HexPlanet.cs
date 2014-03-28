using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle {
	public Vector3 i, j, k;
	public Vector2 i_uv, j_uv, k_uv;

	public static List<Vector3> vertices;
	public Triangle(Vector3 i, Vector3 j, Vector3 k) {
		this.i = i;
		this.j = j;
		this.k = k;
	}
	public Triangle(int i, int j, int k) {
		this.i = vertices[i];
		this.j = vertices[j];
		this.k = vertices[k];
	}

	public static Vector3[] ToVertices(List<Triangle> triangles) {
		Vector3[] vertices = new Vector3[triangles.Count*3];
		for(int i = 0; i < triangles.Count; ++i) {
			vertices[3*i] = triangles[i].i;
			vertices[3*i + 1] = triangles[i].j;
			vertices[3*i + 2] = triangles[i].k;
		}
		return vertices;
	}

	public static int[] ToIndices(List<Triangle> triangles) {
		int[] indices = new int[triangles.Count*3];
		for(int i = 0; i < triangles.Count; ++i) {
			indices[3*i] = 3*i;
			indices[3*i + 1] = 3*i + 1;
			indices[3*i + 2] = 3*i + 2;
		}
		return indices;
	}

	public static Vector2[] ToUVS(List<Triangle> triangles) {
		Vector2[] uvs = new Vector2[triangles.Count*3];
		for(int i = 0; i < triangles.Count; ++i) {
			uvs[3*i] = triangles[i].i_uv;
			uvs[3*i + 1] = triangles[i].j_uv;
			uvs[3*i + 2] = triangles[i].k_uv;
		}
		return uvs;
	}
}

public class HexPlanet : MonoBehaviour {
	public List<Vector3> vertices;
	public Vector2[] uv;
	public List<Triangle> triangles;

	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;


		float t = (1.0f + Mathf.Sqrt(5.0f))/2.0f;

		vertices = new List<Vector3>(12);

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

		mesh.RecalculateBounds();

	}
	
	// Update is called once per frame
	void Update () {

	}
}
