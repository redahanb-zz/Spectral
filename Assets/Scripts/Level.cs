using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public int currentX, currentZ;
	Room currentRoom;
	Transform player, playersRoom;
	NextRoomInfo roomInfo;
	bool playerInCurrentRoom = false;
	float distance = 0;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		roomInfo = GameObject.Find("NextRoomInfo").GetComponent<NextRoomInfo>();
		currentX = 0;
		currentZ = 0;
		DisableAllOtherRooms();
		//InvokeRepeating("SetCurrentRoom", 0.1f, 0.1f);
		//print("Current " + currentX + " : " + currentZ);
	}

	public void DisableAllOtherRooms(){
		foreach(Transform roomTransform in transform.Find("Rooms")){
			currentRoom = roomTransform.GetComponent<Room>();
			if((currentRoom.xIndex == currentX) && (currentRoom.zIndex == currentZ)){
			
			}
			else{
				roomTransform.gameObject.SetActive(false);

			}
		}
	}

	public void EnableCurrentRoom(){
		foreach(Transform roomTransform in transform.Find("Rooms")){
			currentRoom = roomTransform.GetComponent<Room>();
			if((currentRoom.xIndex == currentX) && (currentRoom.zIndex == currentZ)){
				playersRoom = roomTransform;
				roomTransform.gameObject.SetActive(true);
				roomInfo.SetRoomName(roomTransform.GetComponent<Room>().roomName);
			}
		}
	}
}
