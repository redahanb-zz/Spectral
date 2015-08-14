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
					canvasObject,
					playerObject,
					destinationRoomObject,
					destinationDoorObject;

	RectTransform 	rTransform;

	Vector2 		viewportPoint;

	Vector3 openScale, closedScale, targetScale;

	int roomX, roomZ;

	ScreenFade sFade;

	float currentTime, targetTime, timer = 0, timedAmount = 2;
	
	public float lastInterval, timeNow, myTime;

	bool startTimer = false;

	Room currentRoom;

	RoomInformation roomInfo;

	Transform player;

	float playerDistance = 1000;

	// Use this for initialization
	void Start () {
		playerObject = GameObject.FindGameObjectWithTag("Player");


		currentRoom = transform.parent.parent.parent.GetComponent<Room>();
		if(currentRoom){
			roomX = currentRoom.xIndex;
			roomZ = currentRoom.zIndex;
		}

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
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckPlayerDistance();


		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		Timer();
		if(doorOpen)OpenDoor();
		else CloseDoor();
		MoveAndScaleDoorDoor();
		lastInterval = timeNow;
	}

	void CheckPlayerDistance(){
		//print(doorOpen + " : " +playerDistance);
		playerDistance = Vector3.Distance(transform.position, playerObject.transform.position);
		if(playerDistance < 2.25f){
			doorOpen = true;
			//playerObject = c.gameObject;
			if(!roomTextObject) roomTextObject = Instantiate( Resources.Load("Room Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
			roomInfo = roomTextObject.GetComponent<RoomInformation>();
			roomInfo.displayInfo = true;
			roomInfo.SetDoor(this);
			roomInfo.SetTarget(transform);
		}
		else{
			doorOpen = false;
			if(roomInfo) roomInfo.displayInfo = false;
		}
	}

	void Timer(){
		if(startTimer){
			//print(timeNow - currentTime);
			if(timer >= (targetTime)){
				Teleport();
				timer = 0;
				startTimer = false;
			}
			else{
				timer = timeNow;
			}
		}
	}

	void OpenDoor(){
		targetLeftDoorPosition		= leftDoorOpenPosition;
		targetRightDoorPosition		= rightDoorOpenPosition;
		targetScale = openScale;
	}

	void CloseDoor(){
		targetLeftDoorPosition		= leftDoorClosedPosition;
		targetRightDoorPosition		= rightDoorClosedPosition;
		targetScale = closedScale;
	}

	void MoveAndScaleDoorDoor(){
		leftDoor.position 	= Vector3.Lerp(leftDoor.position, 	targetLeftDoorPosition, 	Time.deltaTime * 3);
		rightDoor.position 	= Vector3.Lerp(rightDoor.position, 	targetRightDoorPosition, 	Time.deltaTime * 3);
		leftDoor.transform.localScale = Vector3.Lerp(leftDoor.transform.localScale, 	targetScale, 	Time.deltaTime * 3); 
		rightDoor.transform.localScale = Vector3.Lerp(leftDoor.transform.localScale, 	targetScale, 	Time.deltaTime * 3); 
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){

		}
	}
	
	void OnTriggerStay(Collider c){
		if(c.tag == "Player"){
//			doorOpen = true;
//			roomInfo = roomTextObject.GetComponent<RoomInformation>();
//			roomInfo.displayInfo = true;
//			roomInfo.SetDoor(this);
//			roomInfo.SetTarget(transform);
		}
	}
	
	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){

		}
	}

	void GetAssociatedDoorway(){
		string doorName = transform.parent.name;
		
		switch (doorName) {
		case "Door North":	
			if(GameObject.Find("["+(roomX)+","+(roomZ + 1)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX)+","+(roomZ + 1)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door South"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door South").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported North"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door South":	
			if(GameObject.Find("["+(roomX)+","+(roomZ - 1)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX)+","+(roomZ - 1)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door North"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door North").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported South"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door East" :	
			if(GameObject.Find("["+(roomX + 1)+","+(roomZ)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX + 1)+","+(roomZ)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door West"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door West").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported East"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door West" :	
			if(GameObject.Find("["+(roomX - 1)+","+(roomZ)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX - 1)+","+(roomZ)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door East"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door East").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported West"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		default:	
			if(GameObject.Find("["+(roomX)+","+(roomZ)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX)+","+(roomZ + 1)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door South"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door South").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported North"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
		}
		
		
		print("Teleporting to " +destinationDoorObject.transform.parent);
	}
	
	void Teleport(){
		PlayerController pControl = playerObject.GetComponent<PlayerController> ();
		pControl.StopMoving ();
		
		playerObject.SetActive(false);
		playerObject.transform.position = new Vector3(destinationDoorObject.transform.position.x,transform.position.y,destinationDoorObject.transform.position.z); 
		Camera.main.transform.position = playerObject.transform.position + new Vector3(-10,10,-10);
		playerObject.SetActive(true);
		sFade.fadeToColor = false;
	}

	public void StartNewTeleport(){
		currentTime = timeNow;
		targetTime = currentTime + timedAmount;
		GetAssociatedDoorway();
		sFade.fadeToColor = true;
		startTimer = true;
	}



}
