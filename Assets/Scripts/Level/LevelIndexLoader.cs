using UnityEngine;
using System.Collections;

public class LevelIndexLoader : MonoBehaviour {

	public int nextLevelIndex;
	bool runOnce = false;
	AsyncOperation async;
	bool startLoad = false;
	bool loading = false;
	bool loadDone = false;
	ScreenFade sFade;
	// Use this for initialization
	void Start () {
		sFade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
		sFade.FadeIn();
		Invoke("StartLoad", 2);}
	
	void StartLoad(){
		Time.timeScale = 1;
		startLoad = true;
	}
	
	public void SetNextLevel(int i){
		nextLevelIndex = i;
		//Invoke("OpenLoadingScreen", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(startLoad){
			if(!runOnce){
				Application.LoadLevel(nextLevelIndex);
				runOnce = true;
			}
		}
	}
	
	
	void OpenLoadingScreen(){
		//Application.LoadLevel(nextLevelIndex);
	}
}
