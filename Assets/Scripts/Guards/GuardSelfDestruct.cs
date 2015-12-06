/// <summary>
/// Guard self destruct - script for the destruction of a guard that can alert other guards (unused in final build, apart from colour parts)
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardSelfDestruct : MonoBehaviour {

	// two arrays of guard body parts, one complete, the other a smaller array of coloured body parts
	public 		GameObject[] 	bodyparts 	= 	new		GameObject[29];
	public 		GameObject[] 	colorParts 	= 	new 	GameObject[8];

	void Start () 
	{

	}

	void Update () 
	{

	}

	// death function for guard, get all body parts and send them flying around, destroy the guard
	public void selfDestruct()
	{
		// get all bodypart, unparent them, add rigidbody and impulse force, set them to destroy themselves after a random delay
		foreach(GameObject part in bodyparts){
			part.transform.parent = null;
			part.AddComponent<Rigidbody>();
			Vector3 impulse = new Vector3(Random.Range(-0.05f,0.05f),Random.Range(0.1f,0.3f),Random.Range(-0.05f,0.05f));
			part.GetComponent<Rigidbody>().AddForce(impulse);
			part.AddComponent<CapsuleCollider>();
			Destroy (part, Random.Range(0.5f,1.5f));
		}

		// locate all other guards, if they are within 10m, call them to investigate the location
		GameObject[] otherGuards = GameObject.FindGameObjectsWithTag ("Guard"); 
		foreach (GameObject guard in otherGuards) {
			if(Vector3.Distance(transform.position, guard.transform.position) < 10.0f){
				guard.GetComponent<NavMeshPatrolv2>().Investigate(transform.position);
			}
		}

		// destroy the guard and the parent object (containing waypoints)
		Destroy (this.gameObject);
		Destroy (this.transform.parent.gameObject);
	}
}
