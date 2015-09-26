using UnityEngine;
using System.Collections;

public class MotionSensor : MonoBehaviour {

	Transform pathObject, pathStartObject, pathEndObject, nextPointObject;
	PlayerController pController;
	AlertManager alertSystem;
	bool playerInsideSensor = false;
	int pathIndex;

	float remainingDistance = 1000, moveSpeed = 1f;

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
	
	// Update is called once per frame
	void Update () {
		if(playerInsideSensor){
			if(pController.currentMoveState != PlayerController.MoveState.Idle){
				alertSystem.TriggerAlert();
			}
		}
		if(!alertSystem.alertActive)Move();
	}

	void Move(){
		remainingDistance = Vector3.Distance(transform.position, nextPointObject.position);

		if(remainingDistance < 0.1f){
			if(nextPointObject == pathStartObject)		nextPointObject = pathEndObject;
			else if(nextPointObject == pathEndObject)	nextPointObject = pathStartObject;
		}
		transform.position = Vector3.MoveTowards(transform.position, nextPointObject.position, Time.deltaTime * moveSpeed);

	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			playerInsideSensor = true;

		}
	}

	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			playerInsideSensor = false;
		}
	}
}
