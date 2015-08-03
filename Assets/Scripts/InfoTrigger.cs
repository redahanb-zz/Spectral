using UnityEngine;
using System.Collections;

public class InfoTrigger : MonoBehaviour {

	RoomInfo rInfo;
	ScreenDarken sDark;

	// Use this for initialization
	void Start () {
	//	rInfo = GameObject.Find("RoomInfo").GetComponent<RoomInfo>();
	//	sDark = GameObject.Find("Screen Darken").GetComponent<ScreenDarken>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			//print ("PLAYER ENTERED TRIGGER");
		//	rInfo.textVisible = true;
		//	sDark.canDarken = true;
		}
	}

	void OnTriggerStay(Collider c){
		if(c.tag == "Player"){
		//	rInfo.textVisible = true;
		//	sDark.canDarken = true;
		}
	}

	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
		//	rInfo.textVisible = false;
		//	sDark.canDarken = false;
		}
	}
}
