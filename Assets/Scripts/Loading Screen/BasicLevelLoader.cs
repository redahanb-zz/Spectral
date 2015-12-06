//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Loads a level based on a string name.


using UnityEngine;
using System.Collections;

public class BasicLevelLoader : MonoBehaviour {
	public string 	nextLevelName = "Restore Point";	//next level name
	private bool 	runOnce = false,					//loads level once
					startLoad = false,					//starts loading if true
					loading = false,					//indicates load in progress
					loadDone = false;					//loading finished if true

	AsyncOperation 	async;								//operation for loading next scene

	ScreenFade sFade;

	// Use this for initialization
	void Start () {
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
		sFade.FadeIn();
		Invoke("StartLoad", 2);}

	//starts the level load
	void StartLoad(){
		Time.timeScale = 1;
		startLoad = true;
	}

	//Sets the level to be loaded
	public void SetNextLevel(string s){
		nextLevelName = s;
	}
	
	// Update is called once per frame
	void Update () {
		if(startLoad){
			if(!runOnce){
				Application.LoadLevel(nextLevelName);
				runOnce = true;
			}
		}
	}
	
	//Opens the next level
	void OpenLoadingScreen(){
		Application.LoadLevel(nextLevelName);
	}
	
}
