//Name:			TutorialDoorTrigger.cs
//Author(s)		Conor Hughes
//Description:	This script is used to set the active tutorial doorway in the Restore Point, based on the trigger the player enters.

using UnityEngine;
using System.Collections;

public class TutorialDoorTrigger : MonoBehaviour {
	
	private TutorialLevelManager 	tutorialManager;				//instance of tutorial level manager
	
	public 	bool 					moveTutorial 		= false, 	//enables the move tutorial
	sensorTutorial 		= false, 	//enables the sensor tutorial
	teleportTutorial 	= false;	//enables the teleporter tutorial
	
	// Update is called once per frame
	void Update () {
		if(!tutorialManager)tutorialManager = GameObject.Find("Tutorial Level Manager").GetComponent<TutorialLevelManager>();
	}
	
	//If player enters the trigger, set the active tutorial
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			tutorialManager.SetCurrentDoor(transform);
			if(moveTutorial){
				tutorialManager.moveTutorial = true;
				tutorialManager.teleportTutorial = false;
				tutorialManager.sensorTutorial = false;
			}
			else if(sensorTutorial){
				tutorialManager.moveTutorial = false;
				tutorialManager.teleportTutorial = false;
				tutorialManager.sensorTutorial = true;
			}
			else if(teleportTutorial){
				tutorialManager.moveTutorial = false;
				tutorialManager.teleportTutorial = true;
				tutorialManager.sensorTutorial = false;
			}
		}
	}
	
	//If the player leaves trigger, disable all tutorials.
	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			tutorialManager.moveTutorial = false;
			tutorialManager.teleportTutorial = false;
			tutorialManager.sensorTutorial = false;
		}
	}
}
