/// <summary>
/// Blend info - script for a blend wall to spawn a blend button when the player is in range
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlendInfo : MonoBehaviour {

	GameObject 			button;
	GameObject 			canvasObject;
	GameObject 			player;
	PlayerController 	pController;
	Renderer 			rend;
	HealthManager 		pHealth;
	PauseManager 		pManager;

	Color 				playerColor;
	Color 				wallColor;
	

	void Start () {
		player 			= 		GameObject.FindWithTag ("Player");
		pController 	= 		player.GetComponent<PlayerController> ();
		canvasObject 	= 		GameObject.Find ("Canvas");
		rend 			= 		GetComponent<Renderer> ();
		pHealth 		= 		GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		pManager 		= 		GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
	}
	

	void Update () {
		// calculate the difference in colour between the player and the wall itself
		wallColor = rend.material.color;
		playerColor = pController.targetcolor;
		float colorDistance = Vector3.Distance( new Vector3(wallColor.r, wallColor.b, wallColor.g), new Vector3(playerColor.r,playerColor.b,playerColor.g));

		// if the player is in range of the wall
		if(Vector3.Distance(transform.position, player.transform.position) < 3.0f)
		{
			// if the colour distance is close enough
			if(colorDistance < 0.1f)
			{
				// check angle to player to stop the button appearing through walls
				Vector3 direction = player.transform.position - transform.position;
				float angle = Vector3.Angle(direction, -transform.forward); 
				if(angle <= 90.0f){
					// if there is no button yet, create one
					if(!button && !pHealth.playerDead){
						button = Instantiate(Resources.Load("UI/Worldspace Buttons/Blend Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
						button.transform.SetParent(canvasObject.transform);
						button.GetComponent<BlendInfoButton>().setTarget(gameObject);
						button.GetComponent<Button>().onClick.AddListener( GetComponent<BlendInfo>().callPlayertoBlend );
					} 
					// otherwise, set it visible and interactable
					else if(!pHealth.playerDead && pController.isVisible && !pManager.gamePaused) 
					{
						button.SetActive(true);
						if(button.GetComponent<Button>().interactable == false){
							button.GetComponent<Button>().interactable = true;
						}
					}
				} // end check player angle
			} // end check colour distance
		} // end check player in range


		// if the player is in range and the player is the correct colour, show the button, otherwise hide it
		if (Vector3.Distance (transform.position, player.transform.position) > 10.0f || colorDistance > 0.1f) {
			if(button){
				button.SetActive(false);
			}
		} else if (Vector3.Distance (transform.position, player.transform.position) > 4.0f || colorDistance > 0.1f){
			if(button){
				button.GetComponent<BlendInfoButton>().deactivateButton();
			}
		}

	} // end Update


	// function to tell the player to blend with this wall
	void callPlayertoBlend(){
		player.GetComponent<PlayerController> ().blendButton (this.gameObject, this.transform, transform.position);
	}


	// hide the button when the player leaves the trigger (legacy from older implementation)
	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			button.GetComponent<Button>().interactable = false;
		}
	}
}
