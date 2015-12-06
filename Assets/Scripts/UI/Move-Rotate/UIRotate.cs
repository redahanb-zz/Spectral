//Name:			UIRotateOverTime.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Simple script that rotates a UI element over time based on a vector3.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRotate : MonoBehaviour {

	RectTransform 	rTransform;				//the recttransform component
	public Vector3 	rotateDirection;		//the rotate direction

	// Use this for initialization
	void Start () {
		rTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
			rTransform.localEulerAngles += rotateDirection;
	}
}
