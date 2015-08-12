using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	bool gamePaused;
	float pausedTimedScale;

	GameObject mainCanvas;
	GameObject pauseScreen;
	GameObject timeManager;
	TimeScaler timeScaler;

	// Use this for initialization
	void Start () {
		mainCanvas = GameObject.Find ("Canvas");
		pauseScreen = GameObject.Find ("Pause Screen").transform.GetChild (0).gameObject;
		timeManager = GameObject.Find ("Time Manager");
		timeScaler = timeManager.GetComponent<TimeScaler> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PauseGame(){
		// call timeScaler to stop time
		mainCanvas.SetActive (false);
		pauseScreen.SetActive (true);
	}

	public void ResumeGame(){
		// call timeScaler to resume time
		pauseScreen.SetActive (false);
		mainCanvas.SetActive (true);
	}

	public void RestartLevel(){
		// reload the current level
		Application.LoadLevel(Application.loadedLevel);
	}

	public void QuitGame(){
		// quit the application
		Application.Quit();
	}


}
