//Name:			GraphicIgnoreRaycast.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	A basic script that makes a UI element hidden to all raycasts.


using UnityEngine;

public class GraphicIgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter{

	//Returns false to raycast if it hits the current UI element
	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera){
		return false;
	}
}