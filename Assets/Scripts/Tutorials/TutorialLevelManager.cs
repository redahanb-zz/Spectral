//Name:			TutorialLevelManager.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script controls the tutorial doors in the Restore Point. It assigns the active level and triggers
//				a level transition when the player clicks on a button when near the door.


using UnityEngine;
using System.Collections;

public class TutorialLevelManager : MonoBehaviour {

	private bool 			displayTutorialButton;		//determines if button is displayed
	private Transform 		player;						//transform component of player

	private RectTransform 	tutorialButtonTransform, 	//transform of button
							canvasTransform;			//transform of canvas
	
	private Vector2 		ViewportPosition, 			//screen position of active foor
							WorldObject_ScreenPosition;	//canvas position of object
	
	private ScreenFade 		sFade;						//instance of ScreenFade
	
	private GameState 		gameState;					//instance of GameState
	
	public bool 			moveTutorial = false, 		//determines if movement tutorial is active door
							sensorTutorial = false, 	//determines if colour sensor tutorial is active door
							teleportTutorial = false;	//determines if teleporter tutorial is active door

	private Transform 		currentTutorialDoor;		//transform of the active door

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
		tutorialButtonTransform = GameObject.Find("Tutorial Button").GetComponent<RectTransform>();
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
	}
	
	// Update is called once per frame
	void Update () {

		if(moveTutorial || sensorTutorial || teleportTutorial)displayTutorialButton = true;
		else displayTutorialButton = false;
		
		ScaleButton();
		PositionButton();
	}

	//assigns the current door
	public void SetCurrentDoor(Transform t){
		currentTutorialDoor = t;
	}

	//shows/hides the tutorial button
	void ScaleButton(){
		if(displayTutorialButton){
			tutorialButtonTransform.localScale = Vector3.Lerp(tutorialButtonTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);
		}
		else{
			tutorialButtonTransform.localScale = Vector3.Lerp(tutorialButtonTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);
		}
	}

	//positions the tutorial button over the active door
	void PositionButton(){
		if(displayTutorialButton){
			ViewportPosition=Camera.main.WorldToViewportPoint(currentTutorialDoor.position);
			WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			tutorialButtonTransform.anchoredPosition=WorldObject_ScreenPosition;
		}
	}

	//ends the current level and proceeds to the chosen tutorial
	public void EndLevel(){
		GameObject loadObject = Instantiate(Resources.Load("Basic Level Loader")) as GameObject;
		
		if (!gameState) {
			gameState = GameObject.Find("GameState").GetComponent<GameState>();
		}
		gameState.SaveGame ();
		
		if(moveTutorial){
			print("Basic Training");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Basic Training");
		}
		else if(teleportTutorial){
			print("Teleporter Training");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Teleporter Training");
		}
		else if(sensorTutorial){
			print("Sensor Training");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Sensor Training");
		}
	}
}
