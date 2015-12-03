using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour {
	public float length = 5;
	public float minWidth = 1;
	public float maxWidth = 5;
	private MeshFilter mf;
	
	void Start () {
		mf = GetComponent<MeshFilter>();
	}
	
	void Update () {
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[] {
			new Vector3(-minWidth/2, 0, 0),
			new Vector3(-maxWidth/2, 0, length),
			new Vector3(maxWidth/2, 0, length),
			new Vector3(minWidth/2, 0, 0)
		};
		
		mesh.triangles = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.uv = new Vector2[] {
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0)
		};
		
		mesh.normals = new Vector3[] {
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		};
		
		mf.mesh = mesh;
	}
}
