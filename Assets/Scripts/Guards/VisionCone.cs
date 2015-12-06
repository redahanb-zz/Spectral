/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// Vision Cone: procedural mesh 2D cone of vision
/// </summary>

using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public 		int 		raysToCast;
	public 		int 		sightRange;
	public 		float 		angleOfVision; // in radians

	public 		Vector3[] 	vertices;
	private		Vector2[] 	uvs;
	public 		int[] 		triangles;

	public 		Mesh 		visionConeMesh;
	public 		MeshFilter	meshFilter;

	private 	float 		castAngle;
	private 	float 		sinX;
	private 	float		cosX;
	private 	Vector3		dir;
	private 	Vector3		temp;
	private 	RaycastHit 	hit;


	void Start () 
	{
		vertices 			= 	new Vector3[raysToCast + 1];
		uvs 				= 	new Vector2[vertices.Length];
		triangles 			= 	new int[(vertices.Length * 3) - 9];

		// Set up procedural mesh
		visionConeMesh 		= 	new Mesh();
		visionConeMesh.name = 	"VisionCone";
		meshFilter 			= 	GetComponent<MeshFilter> ();
		meshFilter.mesh 	= 	visionConeMesh;

	}
	

	void Update () 
	{
		// every frame, update the mesh, so it shapes itself to the environment it is cast on
		RaySweep ();
	}


	// function that sweeps a set of rays across a 2 radian arc, +/- the guard's forward vector, uses the raycast hit data to construct a mesh
	void RaySweep()
	{
		// set the cast angle to the guard's forward vector minus half the vision range
		castAngle = -angleOfVision + Mathf.Deg2Rad*transform.eulerAngles.y;

		// cast a ray forward, increaseing the cast angle each time
		for(int i = 0; i < raysToCast; i++)
		{
			// use sine/cosine to get the direction vector
			sinX = sightRange * Mathf.Sin(castAngle);
			cosX = sightRange * Mathf.Cos(castAngle);

			//increment the cast angle by a constant factor (ratio of rays:sweep-angle
			castAngle += 2*angleOfVision/raysToCast;
			
			dir = new Vector3(sinX,0,cosX);

			// if the ray hits a collider, get the point hit and add it to the vertices array
			if(Physics.Raycast(transform.position, dir, out hit, sightRange))
			{
				temp = transform.InverseTransformPoint(hit.point);
				vertices[i] = new Vector3(temp.x,0.005f,temp.z);;
			} 
			else
			{
				// otherwise, add a point at the end of the ray to the vertices array
				temp = transform.InverseTransformPoint(transform.position + dir);
				vertices[i] = new Vector3(temp.x,0.005f,temp.z);
			}
			
		} // end loop

		// Building/Updating the vision cone mesh ///

		// assign mesh vertices
		visionConeMesh.vertices = vertices;

		// set up mesh uvs
		for(int i = 0; i < vertices.Length; i++)
		{
			uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
		} // end uvs loop

		// set up mesh triangles
		int x = -1;
		for(int i = 0; i < triangles.Length; i+=3){
			x++;
			triangles[i] = x+1;
			triangles[i+1] = x+2;
			triangles[i+2] = vertices.Length-1; // all triangles end at the centre
		}

		// assign mesh triangles and uvs
		visionConeMesh.triangles = triangles;
		visionConeMesh.uv = uvs;

		// for efficiency, possibly not necessary here
		//visionConeMesh.RecalculateNormals ();

	} // end RaySweep
}
