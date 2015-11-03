using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEnd : MonoBehaviour {

	bool displayEndButton;
	Transform player;
	float playerDistance = 100;

	RectTransform endButtonTransform, canvasTransform;

	Vector2 ViewportPosition, WorldObject_ScreenPosition;

	ScreenFade sFade;

	GameState gameState;

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

		if(playerDistance < 3)displayEndButton = true;
		else displayEndButton = false;
	
		ScaleButton();
		PositionButton();
	}

	void ScaleButton(){
		if(displayEndButton){
			endButtonTransform.localScale = Vector3.Lerp(endButtonTransform.localScale, new Vector3(0.8f, 0.8f, 0.8f), Time.deltaTime * 22);

		}
		else{
			endButtonTransform.localScale = Vector3.Lerp(endButtonTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 22);

		}
	}

	void PositionButton(){
		if(displayEndButton){
			ViewportPosition=Camera.main.WorldToViewportPoint(transform.position);
			WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));
			
			endButtonTransform.anchoredPosition=WorldObject_ScreenPosition;

		}
	}

	public void EndLevel(){
		//sFade.FadeOut();

		GameObject loadObject = Instantiate(Resources.Load("Basic Level Loader")) as GameObject;

		if (!gameState) {
			gameState = GameObject.Find("GameState").GetComponent<GameState>();
		}
		gameState.SaveGame ();

		if(Application.loadedLevelName == "Restore Point"){
			print("Loading Next Level");
			//loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Colour Sensor Level 1 - Copy");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Colour Sensor Level 1 - Copy");

		}
		else{
			print("Greenlight Screen");
			loadObject.GetComponent<BasicLevelLoader>().SetNextLevel("Greenlight Screen");

			Destroy(endButtonTransform.gameObject);
			Destroy(gameObject);

		}
	}


	
}
