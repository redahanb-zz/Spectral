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
					destinationDoorObject,
					currentRoomObject;

	RectTransform 	rTransform;

	Vector2 		viewportPoint;

	Vector3 openScale, closedScale, targetScale;

	int roomX, roomZ;

	ScreenFade sFade;

	float currentTime, targetTime, timer = 0, timedAmount = 4;
	
	public float lastInterval, timeNow, myTime;

	bool startTimer = false;

	Room currentRoom;

	RoomInformation roomInfo;

	Transform player;

	float playerDistance = 1000;

	Level level;

	int nextX = 0, nextZ = 0;

	DoorButton dButton;

	NextRoomInfo nextRoomInformation;

	// Use this for initialization
	void Start () {
		roomX = 0;
		roomZ = 0;

		dButton = GameObject.Find("Door Button").GetComponent<DoorButton>();

		playerObject = GameObject.FindGameObjectWithTag("Player");
		level = GameObject.Find("Level").GetComponent<Level>();

		nextRoomInformation = GameObject.Find("NextRoomInfo").GetComponent<NextRoomInfo>();

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
		//print(targetTime + " : " +currentTime + " : " +timedAmount);

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

			dButton.SetCurrentDoor(gameObject);

//			if(!roomTextObject) roomTextObject = Instantiate( Resources.Load("Room Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
//			roomInfo = roomTextObject.GetComponent<RoomInformation>();
//			roomInfo.displayInfo = true;
//			roomInfo.SetDoor(this);
//			roomInfo.SetTarget(transform);
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
			//roomInfo.displayInfo = true;

		}
	}
	
	void OnTriggerStay(Collider c){
		if(c.tag == "Player"){
//			doorOpen = true;
//			roomInfo = roomTextObject.GetComponent<RoomInformation>();
			//roomInfo.displayInfo = true;
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
		//print(level.transform);

		foreach(Transform t in level.transform.Find("Rooms"))
		       print(t.transform.name);
		switch (doorName) {
		case "Door North":	
			foreach(Transform t in level.transform.Find("Rooms")){
				if(t.name == "["+(roomX)+","+(roomZ + 1)+"]"){
					destinationRoomObject = t.gameObject;
					//print("North " +destinationRoomObject);
					nextX = destinationRoomObject.GetComponent<Room>().xIndex;
					nextZ = destinationRoomObject.GetComponent<Room>().zIndex;

					level.currentX = nextX;
					level.currentZ = nextZ;
					

					if(destinationRoomObject.transform.Find("Doors").Find("Door South"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door South").Find("Teleport Destination").gameObject;
					else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
					//Debug.Log("[Door Trigger] Teleported North"); 
				}
				//else Debug.Log("[Door Trigger] Cannot find the next room.");
			}
			break;
			
		case "Door South":	
			foreach(Transform t in level.transform.Find("Rooms"))if(t.name == "["+(roomX)+","+(roomZ - 1)+"]"){
				print("South");
				destinationRoomObject = t.gameObject;

				nextX = destinationRoomObject.GetComponent<Room>().xIndex;
				nextZ = destinationRoomObject.GetComponent<Room>().zIndex;

				level.currentX = nextX;
				level.currentZ = nextZ;

				if(destinationRoomObject.transform.Find("Doors").Find("Door North"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door North").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported South"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door East" :	
			foreach(Transform t in level.transform.Find("Rooms"))if(t.name == "["+(roomX + 1)+","+(roomZ)+"]"){
				print("East");
				destinationRoomObject = t.gameObject;

				nextX = destinationRoomObject.GetComponent<Room>().xIndex;
				nextZ = destinationRoomObject.GetComponent<Room>().zIndex;

				level.currentX = nextX;
				level.currentZ = nextZ;

				if(destinationRoomObject.transform.Find("Doors").Find("Door West"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door West").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported East"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door West" :	
			foreach(Transform t in level.transform.Find("Rooms"))if(t.name == "["+(roomX - 1)+","+(roomZ)+"]"){
				print("West");
				destinationRoomObject = t.gameObject;

				nextX = destinationRoomObject.GetComponent<Room>().xIndex;
				nextZ = destinationRoomObject.GetComponent<Room>().zIndex;

				level.currentX = nextX;
				level.currentZ = nextZ;

				if(destinationRoomObject.transform.Find("Doors").Find("Door East"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door East").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported West"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		default:
			foreach(Transform t in level.transform.Find("Rooms"))if(t.name == "["+(roomX)+","+(roomZ)+"]"){
				print("Default");
				destinationRoomObject = t.gameObject;

				nextX = destinationRoomObject.GetComponent<Room>().xIndex;
				nextZ = destinationRoomObject.GetComponent<Room>().zIndex;

				level.currentX = nextX;
				level.currentZ = nextZ;

				if(destinationRoomObject.transform.Find("Doors").Find("Door South"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door South").Find("Teleport Destination").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported North"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
		}
		nextRoomInformation.DisplayNewRoomInfo(destinationRoomObject.GetComponent<Room>().roomName, Application.loadedLevelName);

		//print("Teleporting to " +destinationDoorObject.transform.parent);
	}

	void EnableGameObject(GameObject g){
		g.SetActive(true);
	}
	
	void Teleport(){
		destinationRoomObject.SetActive(true);

		PlayerController pControl = playerObject.GetComponent<PlayerController> ();
		pControl.StopMoving ();
		
		playerObject.SetActive(false);
		playerObject.transform.position = new Vector3(destinationDoorObject.transform.position.x,destinationDoorObject.transform.position.y + 0.5f,destinationDoorObject.transform.position.z); 
		Camera.main.transform.position = playerObject.transform.position + new Vector3(0,30,0);
		playerObject.SetActive(true);
		sFade.fadeToColor = false;
		currentRoom.gameObject.SetActive(false);
		level.DisableAllOtherRooms();
		Camera.main.transform.position = playerObject.transform.position + new Vector3(0,30,0);

	}

	public void StartNewTeleport(){
		level.EnableCurrentRoom();
		currentTime = timeNow;
		targetTime = currentTime + timedAmount;
		GetAssociatedDoorway();
		sFade.fadeToColor = true;
		startTimer = true;
	}



}
