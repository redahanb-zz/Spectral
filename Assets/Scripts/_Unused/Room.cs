using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public int xIndex, zIndex;

	public Color[] roomColors = new Color[5];

	Level levelManager;

	public string roomName = "Room Name Goes Here!";

	// Use this for initialization
	void Start () {
		//print("Room " + xIndex + " : " + zIndex);
		levelManager = transform.parent.parent.GetComponent<Level>();
//		if(roomColors[0] != Color.black){
//			//Set Color on Touch Icon
//		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
