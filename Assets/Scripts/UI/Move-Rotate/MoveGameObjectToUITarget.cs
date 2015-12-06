//Name:			MoveGameObjectToUITarget.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This positions a UI element over a target gameobject.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveGameObjectToUITarget : MonoBehaviour {

	public Vector3 			uiElementPosition;	//position of ui element

	public GameObject 		targetUIElement;	//the target ui element

	private RectTransform 	rtransform;			//the recttransform component of target element

	// Use this for initialization
	void Start () {
		rtransform = targetUIElement.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		uiElementPosition = rtransform.position;
		transform.position = Camera.main.ScreenToWorldPoint(uiElementPosition);
	}
}
