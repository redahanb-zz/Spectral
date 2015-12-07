/// <summary>
/// Footstep FX - script to spawn a footstep effect while the player is running
/// </summary>

using UnityEngine;
using System.Collections;

public class FootstepFX : MonoBehaviour {

	private 	Vector3 			drawLoc;
	public 		bool 				enemyInRange;
	private		PlayerController 	pController;

	// Use this for initialization
	void Start () {
		pController = GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// function called by animation, plays footstep visualisation when the player is near a guard
	public void playEcho() {
		drawLoc = transform.position;
		drawLoc.y += 0.23f;
		if(enemyInRange || !enemyInRange){
			if(!pController.isBlending){
				Instantiate (Resources.Load("FootfallFX"), drawLoc, Quaternion.Euler(90,0,0));
			}
		}
	}
}
