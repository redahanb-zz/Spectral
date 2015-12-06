/// <summary>
/// Pause menu button - script for HUD pause button
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour {

	private 		bool 			paused;
	public 			Vector2 		targetLocation;

	private			float 			moveSpeed = 13;
	private			RectTransform 	buttonTransform;
	private			Vector2 		startlocation;
	private			Vector2 		aimAtLocation;
	private			PauseManager 	pauseManager;

	private			float 			moveTime;


	void Start () 
	{
		buttonTransform = GetComponent<RectTransform>();
		startlocation = buttonTransform.anchoredPosition;
		pauseManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager>();
	}
	

	void Update () 
	{

		if(pauseManager.gamePaused != paused){
			moveTime = 0;
			paused = pauseManager.gamePaused;
		}


		if(paused){
			//buttonTransform.anchoredPosition = Vector2.Lerp (startlocation, targetLocation, moveTime * moveSpeed );
			aimAtLocation = targetLocation;
		}
		else{
			//buttonTransform.anchoredPosition = Vector2.Lerp (targetLocation, startlocation, moveTime * moveSpeed );
			aimAtLocation = startlocation;
		}

		buttonTransform.anchoredPosition = Vector2.Lerp (buttonTransform.anchoredPosition, aimAtLocation, moveSpeed*Time.deltaTime);
	} // Update
}
