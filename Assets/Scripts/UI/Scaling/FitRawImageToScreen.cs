//Name:			FitRawImageToScreen.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script scales a rect transform (although it is primarily used for raw images) and scales them
//				to match the screen size.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FitRawImageToScreen : MonoBehaviour {

	private RectTransform rT;	//the rect transform component of the gameobject

	// Use this for initialization
	void Start () {
		rT = GetComponent<RectTransform>();
		rT.sizeDelta = new Vector2(Screen.width, Screen.height);
	}	
}
