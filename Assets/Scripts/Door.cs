using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour {

	bool 			doorOpen = false;

	Transform 		leftDoor, 
					rightDoor;

	Vector3			leftDoorClosedPosition,
					leftDoorOpenPosition,
					targetLeftDoorPosition,

					rightDoorClosedPosition,
					rightDoorOpenPosition,
					targetRightDoorPosition;

	GameObject		roomTextObject, 
					canvasObject;

	RectTransform 	rTransform;

	Vector2 		viewportPoint;

	Vector3 openScale, closedScale, targetScale;


	// Use this for initialization
	void Start () {
		leftDoor 	= transform.Find("LeftDoor");
		rightDoor 	= transform.Find("RightDoor");

		closedScale = leftDoor.localScale;
		openScale = new Vector3(leftDoor.localScale.x * 0.1f, leftDoor.localScale.y, leftDoor.localScale.z);;
		targetScale = closedScale;


		leftDoorClosedPosition 	= leftDoor.transform.position;
		leftDoorOpenPosition 	= leftDoor.transform.position + (-transform.right * 0.3f);
		targetLeftDoorPosition 	= leftDoorClosedPosition;

		rightDoorClosedPosition 	= rightDoor.transform.position;
		rightDoorOpenPosition 		= rightDoor.transform.position + (transform.right * 0.3f);
		targetRightDoorPosition		= rightDoorClosedPosition;

		canvasObject = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
		if(doorOpen){
			targetLeftDoorPosition		= leftDoorOpenPosition;
			targetRightDoorPosition		= rightDoorOpenPosition;
			targetScale = openScale;
		}
		else{
			targetLeftDoorPosition		= leftDoorClosedPosition;
			targetRightDoorPosition		= rightDoorClosedPosition;
			targetScale = closedScale;
		}

		leftDoor.position 	= Vector3.Lerp(leftDoor.position, 	targetLeftDoorPosition, 	Time.deltaTime * 3);
		rightDoor.position 	= Vector3.Lerp(rightDoor.position, 	targetRightDoorPosition, 	Time.deltaTime * 3);

		leftDoor.transform.localScale = Vector3.Lerp(leftDoor.transform.localScale, 	targetScale, 	Time.deltaTime * 3); 
		rightDoor.transform.localScale = Vector3.Lerp(leftDoor.transform.localScale, 	targetScale, 	Time.deltaTime * 3); 
//		if(roomTextObject){
//			rTransform.anchorMin = viewportPoint;  
//			rTransform.anchorMax = viewportPoint; 
//		}
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			doorOpen = true;
			roomTextObject = Instantiate( Resources.Load("Room Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
			roomTextObject.GetComponent<RoomInformation>().SetTarget(transform);
//			roomTextObject.transform.parent = canvasObject.transform;
//			rTransform = roomTextObject.GetComponent<RectTransform>();
//			Vector2 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
//			rTransform.anchorMin = viewportPoint;  
//			rTransform.anchorMax = viewportPoint; 

			//rTransform.set = new Rect(viewportPoint.x, viewportPoint.y, rTransform.sizeDelta.x, rTransform.sizeDelta.y);
			//roomTextObject.GetComponent<RectTransform>().position = GetComponent<Camera>().WorldToScreenPoint(transform.position);
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
			Destroy(roomTextObject);
		}
	}
}
