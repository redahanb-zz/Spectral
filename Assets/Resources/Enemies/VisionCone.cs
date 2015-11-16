using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public int 			raysToCast;
	public int 			sightRange;
	public float 		angleOfVision; // in radians

	public Vector3[] 	vertices;
	private Vector2[] 	uvs;
	public int[] 		triangles;

	public Mesh 		visionConeMesh;
	public MeshFilter	meshFilter;

	private float 		castAngle;
	private float 		sinX;
	private float		cosX;
	private Vector3		dir;
	private Vector3		temp;
	private RaycastHit 	hit;

	// Use this for initialization
	void Start () 
	{
		vertices = new Vector3[raysToCast + 1];
		uvs = new Vector2[vertices.Length];
		triangles = new int[(vertices.Length * 3) - 9];

		// Set up procedural mesh
		visionConeMesh = new Mesh();
		visionConeMesh.name = "VisionCone";
		meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = visionConeMesh;

	}
	
	// Update is called once per frame
	void Update () 
	{
		RaySweep ();


	}

	void RaySweep()
	{
		//castAngle = Mathf.Deg2Rad*angleOfVision;
		castAngle = -angleOfVision + Mathf.Deg2Rad*transform.eulerAngles.y;
		
		for(int i = 0; i < raysToCast; i++)
		{
			sinX = sightRange * Mathf.Sin(castAngle);
			cosX = sightRange * Mathf.Cos(castAngle);
			
			castAngle += 2*angleOfVision/raysToCast;
			
			dir = new Vector3(sinX,0,cosX);
			
			//Debug.DrawRay(transform.position, dir, Color.green);
			
			if(Physics.Raycast(transform.position, dir, out hit, sightRange))
			{
				temp = transform.InverseTransformPoint(hit.point);
				//temp = hit.point;
				vertices[i] = new Vector3(temp.x,0.005f,temp.z);
				//Debug.DrawLine (transform.position, hit.point, Color.red);
			} 
			else
			{
				temp = transform.InverseTransformPoint(transform.position + dir);
				//temp = transform.position + dir;
				vertices[i] = new Vector3(temp.x,0.005f,temp.z);
			}
			
		} // end loop

		// Building/Updating the vision cone mesh
		//vertices [raysToCast] = new Vector3 (transform.position.x, 0, transform.position.y);
		visionConeMesh.vertices = vertices;

		for(int i = 0; i < vertices.Length; i++)
		{
			uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
		} // end uvs loop

		int x = -1;
		for(int i = 0; i < triangles.Length; i+=3){
			x++;
			triangles[i] = x+1;
			triangles[i+1] = x+2;
			triangles[i+2] = vertices.Length-1; // all triangles end at the centre
		}

		visionConeMesh.triangles = triangles;
		visionConeMesh.uv = uvs;

		//visionConeMesh.RecalculateNormals ();

	} // end RaySweep
}
