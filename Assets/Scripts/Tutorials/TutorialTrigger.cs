using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {
	public string tutorialText = "Tutorial text goes here!";
	GameObject tutorialObject;
	Text textObj;
	
	
	TimeScaler tScaler;
	PlayerController pController;
	// Use this for initialization
	void Start () {
		tScaler = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			//tScaler.StopTime();
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
