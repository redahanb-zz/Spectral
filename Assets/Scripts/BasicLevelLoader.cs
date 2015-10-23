using UnityEngine;

using System.Collections;

public class BasicLevelLoader : MonoBehaviour {
	public string nextLevelName = "Restore Point";
	bool runOnce = false;
	AsyncOperation async;
	bool startLoad = false;
	bool loading = false;
	bool loadDone = false;
	// Use this for initialization
	void Start () {
		Invoke("StartLoad", 2);}
	
	void StartLoad(){
		startLoad = true;
	}

	public void SetNextLevel(string s){
		nextLevelName = s;
		//Invoke("OpenLoadingScreen", 1f);
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
	

	void OpenLoadingScreen(){
		Application.LoadLevel("Loading Screen");
	}
	
}
