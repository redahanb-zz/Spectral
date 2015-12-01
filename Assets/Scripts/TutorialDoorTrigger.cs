using UnityEngine;
using System.Collections;

public class TutorialDoorTrigger : MonoBehaviour {

	TutorialLevelManager tutorialManager;

	public bool moveTutorial = false, sensorTutorial = false, teleportTutorial = false;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!tutorialManager){
			tutorialManager = GameObject.Find("Tutorial Level Manager").GetComponent<TutorialLevelManager>();
		}
	}

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

	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			tutorialManager.moveTutorial = false;
			tutorialManager.teleportTutorial = false;
			tutorialManager.sensorTutorial = false;
		}
	}
}
