using UnityEngine;
using System.Collections;

public class TutorialLevelManager : MonoBehaviour {

	bool displayTutorialButton;
	Transform player;

	RectTransform tutorialButtonTransform, canvasTransform;
	
	Vector2 ViewportPosition, WorldObject_ScreenPosition;
	
	ScreenFade sFade;
	
	GameState gameState;
	
	public bool moveTutorial = false, sensorTutorial = false, teleportTutorial = false;

	Transform currentTutorialDoor;

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

	public void SetCurrentDoor(Transform t){
		currentTutorialDoor = t;
	}
	
	void ScaleButton(){
		if(displayTutorialButton){
			tutorialButtonTransform.localScale = Vector3.Lerp(tutorialButtonTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);
		}
		else{
			tutorialButtonTransform.localScale = Vector3.Lerp(tutorialButtonTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);
		}
	}
	
	void PositionButton(){
		if(displayTutorialButton){
			ViewportPosition=Camera.main.WorldToViewportPoint(currentTutorialDoor.position);
			WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			tutorialButtonTransform.anchoredPosition=WorldObject_ScreenPosition;
		}
	}
	
	public void EndLevel(){
		//sFade.FadeOut();
		
		GameObject loadObject = Instantiate(Resources.Load("Basic Level Loader")) as GameObject;
		
		if (!gameState) {
			gameState = GameObject.Find("GameState").GetComponent<GameState>();
		}
		gameState.SaveGame ();
		
		if(moveTutorial){
			print("Loading Movement Tutorial");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("TUTORIAL - Movement");
		}
		else if(teleportTutorial){
			print("Loading Teleport Tutorial");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("TUTORIAL - Teleporter");
		}
		else if(sensorTutorial){
			print("Loading Sensor Tutorial");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Sensor TUT ");
		}
	}
}
