//Name:			FixedRotation.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Fixes the rotation and position of a GameObject. independet of its parent object.

using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour {

	private Quaternion 	rotation;	//starting rotation
	private Vector3 	postion;	//startomg position

	void Awake(){
		rotation = transform.rotation;
		postion  = transform.position;
	}

	void LateUpdate(){
		transform.rotation = rotation;
		transform.position = postion;
	}
}
