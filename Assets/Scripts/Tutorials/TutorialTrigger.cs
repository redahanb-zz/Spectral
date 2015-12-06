//Name:			TutorialTrigger.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Triggers a tutorial message when entered by the player.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

	public 	string 				tutorialText = "Tutorial text goes here!";	//Tutorial message to be displayed
	private GameObject 			tutorialObject;								//Tutorial box object
	private Text 				textObj;									//Tutorial text component
	private TimeScaler 			tScaler;									//Timescaler instance
	private PlayerController 	pController;								//Player Controller instance

	// Use this for initialization
	void Start () {
		tScaler = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		GetComponent<Renderer>().enabled = false;
	}

	//When player enters trigger, open a tutorial
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			pController = c.GetComponent<PlayerController>();
			pController.StopMoving();
			
			if(GameObject.Find("Tutorial"))GameObject.Find("Tutorial").GetComponent<SimpleTutorial>().InterruptTutorial();

			tutorialObject = Instantiate(Resources.Load("Tutorial_Box"), Vector3.zero, Quaternion.identity) as GameObject;
			tutorialObject.name = "Tutorial";
			tutorialObject.transform.parent = GameObject.Find("Canvas").transform;
			tutorialObject.transform.position = tutorialObject.transform.parent.position + new Vector3(0,(-Screen.height/2) + 200,0);

			textObj = tutorialObject.transform.Find("Background").Find("Text").GetComponent<Text>();
			textObj.text = tutorialText;
			Destroy(gameObject);
		}
	}
}
