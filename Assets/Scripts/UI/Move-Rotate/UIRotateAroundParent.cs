//Name:			UIRotateAroundParent.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Simple script that rotates a UI element around its parent object.


using UnityEngine;
using System.Collections;

public class UIRotateAroundParent : MonoBehaviour {

	private float 	rotateSpeed = 120;	//the speed of the rotation
	public 	bool 	rotateRight = true;	//if true, rotate right. Otherwise rotate left.

	// Update is called once per frame
	void Update () {
		if(rotateRight) transform.RotateAround(transform.parent.position, Vector3.forward, rotateSpeed * 0.01f);
		else transform.RotateAround(transform.parent.position, -Vector3.forward, rotateSpeed * 0.01f);
	}
}
