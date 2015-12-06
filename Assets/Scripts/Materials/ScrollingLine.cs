//Name:			ScrollingLine.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	A script used in the alpha, which created a scrolling dotted line used to indicate door thresholds.


using UnityEngine;
using System.Collections;

public class ScrollingLine : MonoBehaviour {

	private Renderer render;	//The renderer component

	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		render.material.mainTextureOffset = render.material.mainTextureOffset + new Vector2(0.001f,0);
	}
}
