using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlendInfoButton : MonoBehaviour {

	public 		GameObject 			target;

	private		GameObject 			canvasObject;
	private		GameObject 			player;
	private		PlayerController 	playerController;
	private		Image 				icon;
	private		RectTransform 		canvasTransform;
	private		RectTransform 		iconTransform;
	private		HealthManager 		pHealth;
	private		PauseManager 		pManager;

	void Start () 
	{
		canvasObject 		= 		GameObject.Find ("Canvas");
		canvasTransform 	= 		canvasObject.GetComponent<RectTransform> ();
		iconTransform		= 		GetComponent<RectTransform> ();
		player 				= 		GameObject.FindWithTag ("Player");
		playerController 	= 		player.GetComponent<PlayerController> ();
		pHealth 			= 		GameObject.Find("Health Manager").GetComponent<HealthManager>();
		icon 				= 		transform.GetChild(0).gameObject.GetComponent<Image> ();
		pManager 			= 		GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();

		// reset the canvas hierarchy to set the ScreenFade object at the top (to mask the rest of the canvas on transition)
		GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ().ResetParent ();
	}


	// public function to set up the target gameobject reference
	public void setTarget(GameObject t)
	{
		target = t;
	}


	void Update () 
	{
		// if there is a target, track the target's position on the screen and update the button's position to match
		if (target != null) {
			// get the screen position of the target object
			Vector2 ViewportPosition = Camera.main.WorldToViewportPoint (target.transform.position);

			// translate viewport position into screen pixel coordinates, scaling by canvas size
			Vector2 WorldObject_ScreenPosition = new Vector2 (
				((ViewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
				((ViewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f))
				);

			// set the icon's canvas position to the screen position of the target, offset by 60px vertically
			iconTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, 60);
		}

		// interpolate the icon's colour to the player's current colour
		Color playerColor = playerController.targetcolor;
		icon.color = Color.Lerp (icon.color, playerColor, 10*Time.deltaTime);

		// hide button when player is out of range
		if(target.activeSelf == false || Vector3.Distance(target.transform.position, player.transform.position) > 3.0f)
		{
			gameObject.SetActive(false);
		}

		// hide button when player is hidden
		if(!playerController.isVisible){
			gameObject.SetActive(false);
		}

		// hide button when player dies
		if(pHealth.playerDead)
		{
			gameObject.SetActive(false);
		}

		// hide button when game is paused
		if (pManager.gamePaused) 
		{
			gameObject.SetActive (false);
		} 

	} // end Update


	// public functions to hide/show the button
	public void deactivateButton()
	{
		gameObject.SetActive(false);
	}
	
	public void activateButton()
	{
		gameObject.SetActive(true);
	}
}