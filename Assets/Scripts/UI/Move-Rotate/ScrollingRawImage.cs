//Name:			ScrollingRawImage.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Simple script that scrolls a repeating raw image texture.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingRawImage : MonoBehaviour {

	private RawImage 	rImage;				//rawimage component of transform
	public 	Vector2 	scrollDirection;	//the scroll direction

	// Use this for initialization
	void Start () {
		rImage = GetComponent<RawImage>();	
	}
	
	// Update is called once per frame
	void Update () {
		rImage.uvRect = new Rect(rImage.uvRect.x + scrollDirection.x,rImage.uvRect.y + scrollDirection.y,rImage.uvRect.width,rImage.uvRect.height);
	}
}
