using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseManager : MonoBehaviour {

	// variables
	public 		bool 			gamePaused = false;

	private		float 			pausedTimedScale;
	private		bool 			gameOver;
	private 	bool 			runOnce = false;

	// scripts references
	public 		GameObject 		pauseScreen;
	public 		GameObject 		failScreen;

	private		GameObject 		mainCanvas;
	private		TimeScaler 		timeScaler;
	private		ScreenFade 		screenFader;
	private		HealthManager 	healthManager;
	private		BlurOptimized 	blurFX;
	private		HUD_Inventory 	invHUD;
	private		HUD_Healthbar	healthHUD;
	private		GameObject 		timeButton;
	private		GameObject		pauseButton;
	private 	GameObject 		mapButton;
	private		GameObject 		alertCountdown;
	private		GameState 		gameState;
	

	void Start () 
	{
		mainCanvas = GameObject.Find ("Canvas");
		timeScaler = GameObject.Find ("Time Manager").GetComponent<TimeScaler>();
		healthManager = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		screenFader = GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ();

		blurFX = GameObject.FindWithTag ("MainCamera").GetComponent<BlurOptimized> ();
		blurFX.enabled = false;
		pauseScreen.SetActive (false);
		failScreen.SetActive (false);

		invHUD = GameObject.Find ("HUD_Inventory").GetComponent<HUD_Inventory> ();
		healthHUD = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
		timeButton = GameObject.Find("Time Button");
		pauseButton = GameObject.Find ("PauseButton");
		mapButton = GameObject.Find ("Map Button");
		alertCountdown = GameObject.Find ("AlertCountdownIcon");
	}
	
	// Update is called once per frame
	void Update () {
		// keyboard control for pausing the game
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			TogglePause();
			pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
		}

		// when game is paused, blur camera and stop time, and vice versa
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

		// turn off blur effect when it it set to minimum blursize
		if(blurFX.blurSize <= 0.0f){
			blurFX.enabled = false;
		}

		// when player is dead, hide HUD elements
		if(healthManager.playerDead){
			// Check if gameOver is already set, so as to only call this function once
			if(!gameOver){
				invHUD.toggleHide();
				healthHUD.toggleHide();
				timeButton.GetComponent<HideHUDElement>().toggleHide();
				pauseButton.GetComponent<HideHUDElement>().toggleHide();
				alertCountdown.GetComponent<HideHUDElement>().toggleHide();
				mapButton.GetComponent<HideHUDElement>().toggleHide();
				gameOver = true;
			}
			// Show death screen menu after a delay
			Invoke ("DelayDeathScreen", 2.5f);
		}
	} // end Update

	void TogglePause()
	{
		gamePaused = !gamePaused;
	}

	// set the game as 'Paused', activate pause menu
	public void PauseGame()
	{
		gamePaused = true;
		pauseScreen.SetActive (true);
	}


	// return to gameplay from pause menu, diable pause menu
	public void ResumeGame()
	{
		gamePaused = false;
		pauseScreen.SetActive (false);
	}


	// restart the current level
	public void RestartLevel()
	{
		// reload the current level
		Application.LoadLevel(Application.loadedLevel);
	}


	// exit the level to the restore point (previously called HomeBase)
	public void HomeBase()
	{
		// Quit current level and return to homebase
		Application.LoadLevel (0);
	}


	// quit the application entirely
	public void QuitGame()
	{
		// quit the application completely
		Application.Quit();
	}


	// reset the games save data (only available from the Restore Point
	public void ResetData()
	{
		if (!gameState) {
			gameState = GameObject.Find("GameState").GetComponent<GameState>();
		}
		gameState.ResetGame ();
	}


	// set the death/'Mission Failed' screen active
	void DelayDeathScreen()
	{
		gamePaused = true;
		failScreen.SetActive(true);
	}

}
