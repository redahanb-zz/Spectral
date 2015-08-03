using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	bool 		doorOpen = false;

	Transform 	leftDoor, 
				rightDoor;

	Vector3		leftDoorClosedPosition,
				leftDoorOpenPosition,
				targetLeftDoorPosition,

				rightDoorClosedPosition,
				rightDoorOpenPosition,
				targetRightDoorPosition;

	// Use this for initialization
	void Start () {
		leftDoor 	= transform.Find("LeftDoor");
		rightDoor 	= transform.Find("RightDoor");

		leftDoorClosedPosition 	= leftDoor.transform.position;
		leftDoorOpenPosition 	= leftDoor.transform.position + new Vector3(-1,0,0);
		targetLeftDoorPosition = leftDoorClosedPosition;

		rightDoorClosedPosition 	= rightDoor.transform.position;
		rightDoorOpenPosition 		= rightDoor.transform.position + new Vector3(1,0,0);
		targetRightDoorPosition		= rightDoorClosedPosition;

	}
	
	// Update is called once per frame
	void Update () {
		if(doorOpen){
			targetLeftDoorPosition		= leftDoorOpenPosition;
			targetRightDoorPosition		= rightDoorOpenPosition;
		}
		else{
			targetLeftDoorPosition		= leftDoorClosedPosition;
			targetRightDoorPosition		= rightDoorClosedPosition;
		}

		leftDoor.position 	= Vector3.Lerp(leftDoor.position, 	targetLeftDoorPosition, 	Time.deltaTime * 3);
		rightDoor.position 	= Vector3.Lerp(rightDoor.position, 	targetRightDoorPosition, 	Time.deltaTime * 3);


	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			doorOpen = true;
		}
	}
	
	void OnTriggerStay(Collider c){
		if(c.tag == "Player"){
			doorOpen = true;
		}
	}
	
	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			doorOpen = false;
		}
	}
}
