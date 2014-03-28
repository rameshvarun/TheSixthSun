using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Utility Triangle class, used for helping generate the Hex-tiled planets
/// </summary>
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
