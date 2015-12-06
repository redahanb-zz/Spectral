//Name:			SimpleRotate.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	A basic script to rotate an object in a single direction.


using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	public float speed;	//the spped at which the object rotates.

	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up*speed*Time.deltaTime);
	}
}
