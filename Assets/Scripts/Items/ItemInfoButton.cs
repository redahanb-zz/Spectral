/// <summary>
/// Item info button - script attached to the button spawned by ItemInfo
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfoButton : MonoBehaviour {

	public 		GameObject 		target;

	private 	GameObject 		canvasObject;
	private 	RectTransform 	canvasTransform;
	private		RectTransform 	iconTransform;
	private		ScreenFade 		sFader;
	private		PauseManager 	pManager;
	

	void Start () {
		canvasObject = GameObject.Find ("Canvas");
		canvasTransform = canvasObject.GetComponent<RectTransform> ();
		iconTransform = GetComponent<RectTransform> ();

		// reset canvas hierarchy so ScreenFade is always on top (masks UI/HUD on fade)
		sFader = GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ();
		sFader.ResetParent ();

		pManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
	}

	// set up the target gameobject/transform to track on screen
	public void setTarget(GameObject t)
	{
		target = t;
	}
		

	void Update () 
	{
		// if the button has a target (the item that spawned it)...
		if (target != null) {
			//... track the item's position onscreen, and move the button to match it
			Vector2 ViewportPosition = Camera.main.WorldToViewportPoint (target.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2 (
			((ViewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
			((ViewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));

			iconTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, 30);
		}

		// hide the button when game is paused (looks ugly)
		if (pManager.gamePaused) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive(true);
		}
	}

	// public functions to hide/show the button
	public void deactivateButton(){
		gameObject.SetActive(false);
	}

	public void activateButton(){
		gameObject.SetActive(true);
	}
}
