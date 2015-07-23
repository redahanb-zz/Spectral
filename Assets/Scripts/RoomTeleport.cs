using UnityEngine;
using System.Collections;

public class RoomTeleport : MonoBehaviour {

	ScreenFade sFade;
	RoomInfo rInfo;
	// Use this for initialization
	void Start () {
		rInfo = GameObject.Find("RoomInfo").GetComponent<RoomInfo>();
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			rInfo.textVisible = true;
			sFade.fadeToColor = true;
		}
	}
	
	void OnTriggerStay(Collider c){
		if(c.tag == "Player"){
			rInfo.textVisible = true;
			sFade.fadeToColor = true;
		}
	}
	
	void OnTriggerExit(Collider c){
		if(c.tag == "Player"){
			rInfo.textVisible = false;
			sFade.fadeToColor = false;
		}
	}
}
