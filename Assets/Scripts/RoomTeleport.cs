using UnityEngine;
using System.Collections;

public class RoomTeleport : MonoBehaviour {
	
	ScreenFade sFade;
	RoomInfo rInfo;
	
	GameObject destinationRoomObject, destinationDoorObject, playerObject;
	
	int roomX, roomZ;
	
	Room currentRoom;

	float currentTime, targetTime, timer = 0, timedAmount = 2;

	public float lastInterval, timeNow, myTime;


	bool startTimer = false;

	// Use this for initialization
	void Start () {



		//rInfo = GameObject.Find("RoomInfo").GetComponent<RoomInfo>();
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
		
		//destinationRoomObject = gameObject;
		//destinationDoorObject = gameObject;
		
		currentRoom = transform.parent.parent.parent.GetComponent<Room> ();
		roomX = currentRoom.xIndex;
		roomZ = currentRoom.zIndex;
	}
	
	// Update is called once per frame
	void Update () {
		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;

		if(startTimer){

			print(timeNow - currentTime);
			if(timer >= (targetTime)){
				Teleport();
				timer = 0;
				startTimer = false;
			}
			else{
				timer = timeNow;
			}

		}

		lastInterval = timeNow;
	}
	
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			//Debug.Log("[Door Trigger] Attempting to teleport Player.");
			playerObject = c.gameObject;
			currentTime = timeNow;
			targetTime = currentTime + timedAmount;
			GetAssociatedDoorway();
			sFade.fadeToColor = true;

			startTimer = true;


		}
	}

	//void ToggleFade(){
	//	sFade.ToggleCanFade();
	//}
	
	void OnTriggerStay(Collider c){
		if(c.tag == "Player"){
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
				if(destinationRoomObject.transform.Find("Doors").Find("Door South"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door South").Find("RoomInfoTrigger").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported North"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door South":	
			if(GameObject.Find("["+(roomX)+","+(roomZ - 1)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX)+","+(roomZ - 1)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door North"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door North").Find("RoomInfoTrigger").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported South"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door East" :	
			if(GameObject.Find("["+(roomX + 1)+","+(roomZ)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX + 1)+","+(roomZ)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door West"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door West").Find("RoomInfoTrigger").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported East"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
			
		case "Door West" :	
			if(GameObject.Find("["+(roomX - 1)+","+(roomZ)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX - 1)+","+(roomZ)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door East"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door East").Find("RoomInfoTrigger").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported West"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;

		default:	
			if(GameObject.Find("["+(roomX)+","+(roomZ + 1)+"]")){
				destinationRoomObject = GameObject.Find("["+(roomX)+","+(roomZ + 1)+"]");
				if(destinationRoomObject.transform.Find("Doors").Find("Door South"))destinationDoorObject =	destinationRoomObject.transform.Find("Doors").Find("Door South").Find("RoomInfoTrigger").gameObject;
				else Debug.Log("[Door Trigger] Cannot find the associated Door in the next room.");
				Debug.Log("[Door Trigger] Teleported North"); 
			}
			else Debug.Log("[Door Trigger] Cannot find the next room.");
			break;
		}


		print("Teleporting to " +destinationDoorObject.transform.parent);
	}
	
	void Teleport(){
		PlayerNavmeshTest nvTest = playerObject.GetComponent<PlayerNavmeshTest> ();

		nvTest.StopMoving ();

		playerObject.SetActive(false);
//
//
		playerObject.transform.position = new Vector3(destinationDoorObject.transform.position.x,
		                                              transform.position.y,
		                                              destinationDoorObject.transform.position.z); 
//
		playerObject.SetActive(true);
		sFade.fadeToColor = false;
		//destinationRoomObject.transform.Find("Doors").Find("Door East").Find("TeleportTrigger").gameObject.SetActive(false);
		
		//playerObject.gameObject.SetActive(false);
		//}
	}
}
