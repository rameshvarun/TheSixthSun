using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle {
	public int i, j, k;
	public Triangle(int i, int j, int k) {
		this.i = i;
		this.j = j;
		this.k = k;
	}

	public static int[] TriangleListToArray(List<Triangle> triangles) {
		int[] indices = new int[triangles.Count*3];
		for(int i = 0; i < triangles.Count; ++i) {
			indices[3*i] = triangles[i].i;
			indices[3*i + 1] = triangles[i].j;
			indices[3*i + 2] = triangles[i].k;
		}
		return indices;
	}
	public static Vector3 trilinearToPoint(Vector3 tri, Vector3 A, Vector3 B, Vector3 C) {
		Vector3 CB = B - C;
		Vector3 CA = A - C;

		float a = Vector3.Distance(B, C);
		float b = Vector3.Distance(A, C);
		float c = Vector3.Distance(A, B);

		float alpha = (b*tri.y)/(a*tri.x + b*tri.y + c*tri.z);
		float beta = (a*tri.x)/(a*tri.x + b*tri.y + c*tri.z);

		return C + CB*alpha + CA*beta;
	}
	public Vector3 inCenter(List<Vector3> vertices) {
		return trilinearToPoint(new Vector3(1, 1, 1), vertices[i], vertices[j], vertices[k]);
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

		for(int i = 0; i < 5; ++i) {
			//Subdivision phase
			List<Triangle> newTriangles = new List<Triangle>();
			foreach(Triangle tri in triangles) {

				int a = vertices.Count;
				vertices.Add((vertices[tri.i]/2 + vertices[tri.j]/2).normalized);

				int b = vertices.Count;
				vertices.Add((vertices[tri.j]/2 + vertices[tri.k]/2).normalized);

				int c = vertices.Count;
				vertices.Add((vertices[tri.i]/2 + vertices[tri.k]/2).normalized);

				newTriangles.Add(new Triangle(tri.i, a, c));
				newTriangles.Add(new Triangle(tri.j, b, a));
				newTriangles.Add(new Triangle(tri.k, c, b));
				newTriangles.Add(new Triangle(a, b, c));
			}
			triangles = newTriangles;
		}


		mesh.vertices = vertices.ToArray();
		mesh.triangles = Triangle.TriangleListToArray(triangles);

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

	}
	
	// Update is called once per frame
	void Update () {

	}
}
