using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseManager : MonoBehaviour {

	public bool gamePaused = false;
	float pausedTimedScale;


	GameObject mainCanvas;
	public GameObject pauseScreen;
	public GameObject failScreen;
	TimeManager timeManager;
	HealthManager healthManager;
	//DepthOfFieldDeprecated DOField;
	BlurOptimized blurFX;

	// Use this for initialization
	void Start () {
		mainCanvas = GameObject.Find ("Canvas");
		timeManager = GameObject.Find ("Time Manager").GetComponent<TimeManager>();
		healthManager = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		//pauseScreen = GameObject.Find ("Pause Screen");
		pauseScreen.SetActive (false);
		failScreen.SetActive (false);
		//DOField = GameObject.FindWithTag ("MainCamera").GetComponent<DepthOfFieldDeprecated> ();
		blurFX = GameObject.FindWithTag ("MainCamera").GetComponent<BlurOptimized> ();
		blurFX.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gamePaused) {
			blurFX.enabled = true;
			blurFX.blurSize = Mathf.Lerp (blurFX.blurSize, 10.0f, Time.deltaTime * 5.0f);
		} else {
			blurFX.blurSize = Mathf.Lerp (blurFX.blurSize, 0.0f, Time.deltaTime * 50.0f);
		}

		if(blurFX.blurSize <= 0.0f){
			blurFX.enabled = false;
		}

		if(healthManager.playerDead){
			gamePaused = true;
			failScreen.SetActive(true);
		}
	}

	public void PauseGame(){
		// call timeScaler to stop time

		gamePaused = true;
		//mainCanvas.SetActive (false);
		pauseScreen.SetActive (true);
	}

	public void ResumeGame(){
		// call timeScaler to resume time

		gamePaused = false;
		pauseScreen.SetActive (false);
		//mainCanvas.SetActive (true);
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


}
