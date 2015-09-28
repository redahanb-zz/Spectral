using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportButton : MonoBehaviour {

	GameObject currentTeleporter, playerObject;
	bool buttonActive = false;

	bool textVisible = false;
	//Text actionText;

	Button button;

	RectTransform rTransform, canvasTransform;

	float playerDistance = 1000;
	float scaleRate = 10f;
	// Use this for initialization
	void Start () {
		//actionText = transform.Find("Text").GetComponent<Text>();
		//actionText.color = new Color (1,1,1,0);
		button = GetComponent<Button>();
		canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
		playerObject = GameObject.FindGameObjectWithTag("Player");
		rTransform = GetComponent<RectTransform>();
		InvokeRepeating("ToggleText", 1, 3);
	}
	
	// Update is called once per frame
	void Update () {
		if(currentTeleporter){ 
			playerDistance = Vector3.Distance(currentTeleporter.transform.position, playerObject.transform.position);
			if(playerDistance > 2){buttonActive = false; currentTeleporter = null;}
		}

		if(Input.GetKeyDown(KeyCode.G))buttonActive = !buttonActive;
		//print("TELEPORTER:" +currentTeleporter +"     ACTIVE: " +buttonActive);

		if(currentTeleporter)buttonActive = true;
		else buttonActive = false;

		//button.enabled = buttonActive;
		ScaleButton();
		PositionButton();
		FadeText();

	}


	void ScaleButton(){
		if(buttonActive) rTransform.localScale = Vector3.Lerp(rTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);// rTransform.sizeDelta = Vector2.Lerp(new Vector2(0,0), new Vector2(70,70), Time.deltaTime);
		else 			 rTransform.localScale = Vector3.Lerp(rTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);//rTransform.sizeDelta = Vector2.Lerp(new Vector2(70,70), new Vector2(0,0), Time.deltaTime);

	}

	void FadeText(){
		//if(textVisible) actionText.color = Color.Lerp(new Color(1,1,1,0), new Color(1,1,1,1), Time.deltaTime);
		//else 			actionText.color = Color.Lerp(new Color(1,1,1,1), new Color(1,1,1,0), Time.deltaTime);

	}

	void PositionButton(){
		if(buttonActive){

			Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(currentTeleporter.transform.position);
			Vector2 WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			rTransform.anchoredPosition=WorldObject_ScreenPosition;
		}
	}

	void ToggleText(){
		textVisible = !textVisible;
	}

	public void SetCurrentTeleporter(GameObject g){
		currentTeleporter = g;
		textVisible = false;
	}

	public void StartTeleport(){
		playerObject.GetComponent<PlayerController>().currentMoveState = PlayerController.MoveState.Idle;
		currentTeleporter.GetComponent<Teleporter>().Teleport();
	}
}
