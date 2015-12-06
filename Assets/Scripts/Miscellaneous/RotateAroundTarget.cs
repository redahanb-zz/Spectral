//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Rotates transform around target transform in single direction.


using UnityEngine;
using System.Collections;

public class RotateAroundTarget : MonoBehaviour {
	public Transform target;	//target transform to rotate around
	public float speed = 10;	//the rotate speed
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(target.position, Vector3.up, Time.deltaTime * speed);
	}
}
