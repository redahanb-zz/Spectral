//Name:			LevelEnd.cs
//Author(s)		Conor Hughes
//Description:	This script is used to display the level end button when the player is near the end door.
//				It scales the button to show and hide it. The end button calls the end level function in this script
//				to save data and return to the title screen. The button is dynamically positioned so it always appears
//				over the end doorway.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEnd : MonoBehaviour {
	
	private bool 			displayEndButton;						//determines whether the end button is displayed
	private	Transform 		player;									//transform of the player
	private	float 			playerDistance = 100;					//distance between door and player
	
	private RectTransform 	endButtonTransform, 					//RectTransform component of end button
	canvasTransform;						//RectTransform component of canvas
	
	private Vector2 		ViewportPosition, 						//the screen position of the button
	WorldObject_ScreenPosition;				//the world position the button needs to match
	
	private ScreenFade 		sFade;									//instance of the screen fader
	
	private GameState		gameState;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
		endButtonTransform = GameObject.Find("Level End Button").GetComponent<RectTransform>();
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
	}
	
	// Update is called once per frame
	void Update () {
		playerDistance = Vector3.Distance(transform.position, player.transform.position);
		if(playerDistance < 4)displayEndButton = true;
		else displayEndButton = false;
		ScaleButton();
		PositionButton();
	}
	//Function that scales the end level button until its visible or invisible
	void ScaleButton(){
		if(displayEndButton){
			endButtonTransform.localScale = Vector3.Lerp(endButtonTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);
			
		}
		else{
			endButtonTransform.localScale = Vector3.Lerp(endButtonTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);
			
		}
	}
	//Function that positions the end level button so it is placed in front of the end door
	void PositionButton(){
		if(displayEndButton){
			ViewportPosition=Camera.main.WorldToViewportPoint(transform.position);
			WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			endButtonTransform.anchoredPosition=WorldObject_ScreenPosition;
			
		}
	}
	
	//Function that ends the level, saves the game, fades screen to black and creates a GameObject that hides the title screen
	public void EndLevel(){
		GameObject loadObject = Instantiate(Resources.Load("Basic Level Loader")) as GameObject;
		
		//Save the game
		if (!gameState)gameState = GameObject.Find("GameState").GetComponent<GameState>();
		gameState.SaveGame ();
		
		if(Application.loadedLevelName != "Restore Point"){
			print("Returning to Restore Point");
			//Create object to hide title screen if none exists
			if(!GameObject.Find("HideTitleObject")){
				GameObject hideTitleObject = new GameObject("HideTitleObject");
				DontDestroyOnLoad(hideTitleObject);
				
			}
			
			//Load the Restore Point
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Restore Point");
			Destroy(endButtonTransform.gameObject);
		}
	}
	
	
	
}
