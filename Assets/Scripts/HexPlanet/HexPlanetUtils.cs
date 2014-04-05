using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HexPlanetUtils
{
	/// <summary>
	/// Generates a unit Icosohedron.
	/// </summary>
	/// <returns>The icosohedron as a List<Triangle>.</returns>
	public static List<Triangle> generateIcosohedron() {
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

		List<Triangle> triangles = new List<Triangle>();

		Triangle.vertices = vertices;

		//TODO: Convert to use the Vector3 constructor, and not the int constructor
		triangles.Add(new Triangle(vertices[0], vertices[11], vertices[5]));
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

		return triangles;
	}
}

