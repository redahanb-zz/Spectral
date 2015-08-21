using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public bool gamePaused = false;
	float pausedTimedScale;

	GameObject mainCanvas;
	public GameObject pauseScreen;

	// Use this for initialization
	void Start () {
		mainCanvas = GameObject.Find ("Canvas");
		//pauseScreen = GameObject.Find ("Pause Screen");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PauseGame(){
		// call timeScaler to stop time
		gamePaused = true;
		mainCanvas.SetActive (false);
		pauseScreen.SetActive (true);
	}

	public void ResumeGame(){
		// call timeScaler to resume time
		gamePaused = false;
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
