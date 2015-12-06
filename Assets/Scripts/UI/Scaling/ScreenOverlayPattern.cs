//Name:			ScreenOverlayPattern.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script creates a diagonal line texture overlay which covers the entire screen.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenOverlayPattern : MonoBehaviour {

	private RectTransform 	rTrans;	//rect transform component
	private RawImage		rImg;	//raw iamge component

	// Use this for initialization
	void Start () {
		rImg = GetComponent<RawImage>();
		rTrans = GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		rImg.color = Color.Lerp(rImg.color, new Color(1,1,1,0.15f), 0.04f);
	}
}
