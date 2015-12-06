//Name:			DestinationMarker.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Places and shows/hides the path destination marker.

using UnityEngine;
using System.Collections;

public class DestinationMarker : MonoBehaviour {

	private Renderer 	r;						//the renderer component
	private Vector3 	visibleScale, 			//visible vector3 scale
						hiddenScale;			//hidden vector3 scale
	private bool		markerVisible = true;	//if true, marker is visible
	private float 		scaleSpeed = 2;			//the rate at which the maker scales

	// Use this for initialization
	void Start () {
		visibleScale = transform.localScale;
		hiddenScale = new Vector3(0,0,0);
		transform.localScale = hiddenScale;

		r = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(markerVisible){
			transform.localScale = Vector3.Lerp(transform.localScale, visibleScale, Time.deltaTime * scaleSpeed);
		}
		else{
			transform.localScale = Vector3.Lerp(transform.localScale, hiddenScale, Time.deltaTime * scaleSpeed * 1.6f);
			if(transform.localScale.x < 0.01f)			Destroy(gameObject);

		}
	}

	//forces the DestinationMarker to hide
	public void RemoveMarker(){
		markerVisible = false;
	}
}
