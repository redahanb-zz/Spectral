using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour {

	RectTransform doorButtonTransform, canvasTransform;

	bool displayDoorButton = false, forceHide = false;

	Vector2 ViewportPosition, WorldObject_ScreenPosition;

	GameObject currentDoor, playerObject;

	Door useableDoor;

	float playerDistance = 100;

	// Use this for initialization
	void Start () {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		canvasTransform = transform.parent.GetComponent<RectTransform>();
		doorButtonTransform = GetComponent<RectTransform>();
		currentDoor = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentDoor){ 
			displayDoorButton = true;
			playerDistance = Vector3.Distance(currentDoor.transform.position, playerObject.transform.position);
			if(playerDistance > 2){displayDoorButton = false; currentDoor = null;}
		}
		else displayDoorButton = false;

		ScaleButton();
		PositionButton();
	}

	public void SetCurrentDoor(GameObject g){
		currentDoor = g;
	}

	void ScaleButton(){
		if(displayDoorButton && !forceHide){
			doorButtonTransform.localScale = Vector3.Lerp(doorButtonTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);
			
		}
		else{
			doorButtonTransform.localScale = Vector3.Lerp(doorButtonTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);
			
		}
	}
	
	void PositionButton(){
		if(displayDoorButton){
			ViewportPosition=Camera.main.WorldToViewportPoint(currentDoor.transform.position);
			WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			doorButtonTransform.anchoredPosition=WorldObject_ScreenPosition;
			
		}
	}

	public void UseDoor(){
		ToggleForceHide();
		Invoke("ToggleForceHide", 6);
		useableDoor = currentDoor.GetComponent<Door>();
		useableDoor.StartNewTeleport();

	}

	void ToggleForceHide(){
		forceHide = !forceHide;
	}
}
