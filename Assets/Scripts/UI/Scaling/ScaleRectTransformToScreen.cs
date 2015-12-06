//Name:			ScaleRectTransformToScreen.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script is used to scale UI Rect Transform to match the current screen size.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScaleRectTransformToScreen : MonoBehaviour {
	
	private 	RectTransform 	rTransform;					//the Rect Transform componenet of the object
	public 		bool 			scaleEveryFrame = false;	//if enabled, changes size in FixedUpdate
	
	// Use this for initialization
	void Start () {
		rTransform = GetComponent<RectTransform>();
		rTransform.sizeDelta = new Vector3(Screen.width, Screen.height,0);
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		if(scaleEveryFrame)rTransform.sizeDelta = new Vector3(Screen.width, Screen.height,0);
	}
}