//Name:			Level.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Stores information about the level. Indicates what room the player is currently occupying and disables all other rooms.


using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public int 				currentX, 						//x index of current room
							currentZ;						//z index of current room
	private Room 			currentRoom;					//room component of current room
	private Transform 		player, 						//player transform
							playersRoom;					//the players occupied room
	private NextRoomInfo 	roomInfo;						//instance of roominfo
	private bool 			playerInCurrentRoom = false;	//indicates that player is in current room

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		roomInfo = GameObject.Find("NextRoomInfo").GetComponent<NextRoomInfo>();
		currentX = 0;
		currentZ = 0;
		DisableAllOtherRooms();
	}

	//Disables all other rooms in level.
	public void DisableAllOtherRooms(){
		foreach(Transform roomTransform in transform.Find("Rooms")){
			currentRoom = roomTransform.GetComponent<Room>();
			if((currentRoom.xIndex == currentX) && (currentRoom.zIndex == currentZ)){
				//Player is in this room
			}
			else{
				//Player is not in this room, so room is disabled
				roomTransform.gameObject.SetActive(false);
			}
		}
	}

	//Enables the current room
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
