//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	In reality a colour sensor, triggers an alarm if the player is not hiding with the correct colour when they enter its trigger.

using UnityEngine;
using System.Collections;

public class MotionSensor : MonoBehaviour {

	private Transform 		pathObject, 				//the path game object
							pathStartObject, 			//starting point of path
							pathEndObject, 				//end point of path
							nextPointObject;			//next point to move towards
	PlayerController 		pController;				//instance of player controller
	private AlertManager 	alertSystem;				//instance of alert system
	private bool 			playerInsideSensor = false;	//indicates if player inside sensor
	private float 			remainingDistance = 1000, 	//distance to next point
							moveSpeed = 1f;				//movement speed

	// Use this for initialization
	void Start () {
		pathObject 		= transform.Find("Sensor Path");
		pathStartObject = pathObject.Find("Path Start");
		pathEndObject 	= pathObject.Find("Path End");

		nextPointObject = pathEndObject;
		pathObject.transform.parent = null;

		alertSystem = GameObject.Find("Alert System").GetComponent<AlertManager>();
		pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame, triggers an alert if player inside sensor and not hiding
	void Update () {
		if(playerInsideSensor){
			if(pController.isVisible){
				alertSystem.TriggerAlert();
			}
		}
		if(!alertSystem.alertActive)Move();
	}

	//Moves the sensor
	void Move(){
		remainingDistance = Vector3.Distance(transform.position, nextPointObject.position);
		if(remainingDistance < 0.1f){
			if(nextPointObject == pathStartObject)		nextPointObject = pathEndObject;
			else if(nextPointObject == pathEndObject)	nextPointObject = pathStartObject;
		}
		transform.position = Vector3.MoveTowards(transform.position, nextPointObject.position, Time.deltaTime * moveSpeed);

	}

	//If player enters sensor
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			playerInsideSensor = true;

		}
	}

	//If player exits sensor
	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			playerInsideSensor = false;
		}
	}
}
