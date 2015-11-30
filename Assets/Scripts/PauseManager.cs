using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseManager : MonoBehaviour {

	public bool 		gamePaused = false;
	float 				pausedTimedScale;
	bool 				gameOver;


	GameObject 			mainCanvas;
	public GameObject 	pauseScreen;
	public GameObject 	failScreen;
	TimeScaler 			timeScaler;
	ScreenFade 			screenFader;
	HealthManager 		healthManager;
	BlurOptimized 		blurFX;
	HUD_Inventory 		invHUD;
	HUD_Healthbar		healthHUD;
	GameObject 			timeButton;
	GameObject			pauseButton;
	GameObject 			alertCountdown;
	GameState 			gameState;

	bool runOnce = false;

	// Use this for initialization
	void Start () {
		mainCanvas = GameObject.Find ("Canvas");
		timeScaler = GameObject.Find ("Time Manager").GetComponent<TimeScaler>();
		healthManager = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		//pauseScreen = GameObject.Find ("Pause Screen");
		screenFader = GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ();
		pauseScreen.SetActive (false);
		failScreen.SetActive (false);
		//DOField = GameObject.FindWithTag ("MainCamera").GetComponent<DepthOfFieldDeprecated> ();
		blurFX = GameObject.FindWithTag ("MainCamera").GetComponent<BlurOptimized> ();
		blurFX.enabled = false;

		invHUD = GameObject.Find ("HUD_Inventory").GetComponent<HUD_Inventory> ();
		healthHUD = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
		timeButton = GameObject.Find("Time Button");
		pauseButton = GameObject.Find ("PauseButton");
		alertCountdown = GameObject.Find ("AlertCountdownIcon");
	}
	
	// Update is called once per frame
	void Update () {
		if (gamePaused) {
			blurFX.enabled = true;
			blurFX.blurSize = Mathf.Lerp (blurFX.blurSize, 10.0f, Time.deltaTime * 5.0f);
			timeScaler.StopTime();
			runOnce = false;
		} 
		else {
			blurFX.blurSize = Mathf.Lerp (blurFX.blurSize, 0.0f, Time.deltaTime * 50.0f);
			if(!runOnce){
				timeScaler.ResumeTime();
				runOnce = true;
			}
		}

		if(blurFX.blurSize <= 0.0f){
			blurFX.enabled = false;
		}

		if(healthManager.playerDead){
//			gamePaused = true;
//			failScreen.SetActive(true);

			if(!gameOver){
				invHUD.toggleHide();
				healthHUD.toggleHide();
				timeButton.GetComponent<HideHUDElement>().toggleHide();
				pauseButton.GetComponent<HideHUDElement>().toggleHide();
				alertCountdown.GetComponent<HideHUDElement>().toggleHide();
				gameOver = true;
			}

			Invoke ("DelayDeathScreen", 2.5f);
		}
	}

	public void PauseGame(){
		// call timeScaler to stop time
		//timeScaler.StopTime ();
		gamePaused = true;
		pauseScreen.SetActive (true);
	}

	public void ResumeGame(){
		// call timeScaler to resume time
		//timeScaler.ResumeTime ();
		gamePaused = false;
		pauseScreen.SetActive (false);
		print ("Resuming play...");
	}

	public void RestartLevel(){
		// reload the current level
		Application.LoadLevel(Application.loadedLevel);
	}

	public void HomeBase(){
		// Quit current level and return to homebase
		Application.LoadLevel (0);
	}

	public void QuitGame(){
		// quit the application completely
		Application.Quit();
	}

	public void ResetData(){
		if (!gameState) {
			gameState = GameObject.Find("GameState").GetComponent<GameState>();
		}
		gameState.ResetGame ();
	}

	void DelayDeathScreen()
	{
		gamePaused = true;
		failScreen.SetActive(true);
	}

}
