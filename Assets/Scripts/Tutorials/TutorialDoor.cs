//Name:			SimpleTutorial.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Displays a tutorial box for five seconds before hiding again.


using UnityEngine;
using System.Collections;

public class TutorialDoor : MonoBehaviour {

	private bool 			displayTutorialButton,		//determines if tutorial button is visible
							moveTutorial = false, 		//
							sensorTutorial = false, 	//
							teleportTutorial = false;	//
	private Transform 		player;						//transform of player
	private float 			playerDistance = 100;		//distance between door and player
	
	private RectTransform 	tutorialButtonTransform,	//the tutorial button transform
							canvasTransform;			//the canvas transform
	
	private Vector2 		ViewportPosition, 			//world pos of object
							WorldObject_ScreenPosition;	//target pos of ui
	
	private ScreenFade 		sFade;						//instance of screen fade
	
	private GameState 		gameState;					//instance of game state

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
		tutorialButtonTransform = GameObject.Find("Tutorial Button").GetComponent<RectTransform>();
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
	}
	
	// Update is called once per frame
	void Update () {
		playerDistance = Vector3.Distance(transform.position, player.transform.position);
		
		if(playerDistance < 3)displayTutorialButton = true;
		else displayTutorialButton = false;
		
		ScaleButton();
		PositionButton();
	}

	//Shows/Hides the button
	void ScaleButton(){
		if(displayTutorialButton){
			tutorialButtonTransform.localScale = Vector3.Lerp(tutorialButtonTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);
		}
		else{
			tutorialButtonTransform.localScale = Vector3.Lerp(tutorialButtonTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);
		}
	}

	//Move the button over the door
	void PositionButton(){
		if(displayTutorialButton){
			ViewportPosition=Camera.main.WorldToViewportPoint(transform.position);
			WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			tutorialButtonTransform.anchoredPosition=WorldObject_ScreenPosition;
		}
	}

	//Start the tutorial level based on next door
	public void StartTutorialLevel(){
		GameObject loadObject = Instantiate(Resources.Load("Basic Level Loader")) as GameObject;
		
		if (!gameState) {
			gameState = GameObject.Find("GameState").GetComponent<GameState>();
		}
		gameState.SaveGame ();
		
		if(moveTutorial){
			print("Loading Movement Tutorial");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("TUTORIAL - Movement");
		}
		else if(moveTutorial){
			print("Loading Teleport Tutorial");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Move Tutorial");
		}
		else{
			print("Loading Sensor Tutorial");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Greenlight Screen");
		}
	}
}
