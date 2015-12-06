//Name:			TeleportDestinationIndicator.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Indicates the paired teleporter when standing near an active teleporter.


using UnityEngine;
using System.Collections;

public class TeleportDestinationIndicator : MonoBehaviour {

  	private Renderer 	indicatorRenderer;	//renderer component of the indicator
	private Color	 	startingColor;		//starting colour of indicater
	private Vector3 	startingPos;		//starting position of indicator

	// Use this for initialization
	void Start () {
		indicatorRenderer = GetComponent<Renderer>();
		startingColor = indicatorRenderer.material.color;
		startingPos = transform.position;
		ResetIndicator();
		InvokeRepeating("ResetIndicator", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += new Vector3(0.003f,0.003f,0.003f);
		transform.position += new Vector3(0,0.004f,0);
		indicatorRenderer.material.color = Color.Lerp(indicatorRenderer.material.color, new Color(startingColor.r,startingColor.g,startingColor.b,0), Time.deltaTime * 3);
	}

	//Resets the colour, scale and position of the indicator object.
	void ResetIndicator(){
		transform.localScale = new Vector3(0,0,0);
		transform.position = startingPos;
		indicatorRenderer.material.color = startingColor;
	}
}
