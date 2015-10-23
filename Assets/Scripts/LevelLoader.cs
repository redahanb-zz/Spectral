using UnityEngine;

using System.Collections;

public class LevelLoader : MonoBehaviour {
	string nextLevelName = "Restore Point";
	bool runOnce = false;
	AsyncOperation async;
	ScreenFade fade;
	LoadingCamera lCam;
	bool startLoad = false;
	bool loading = false;
	bool loadDone = false;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		Invoke("StartLoad", 1);
	}

	void StartLoad(){
		startLoad = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName == "Loading Screen" && startLoad){
			if(!runOnce){
				lCam = Camera.main.GetComponent<LoadingCamera>();
				StartCoroutine(AsyncLoadLevel());
				runOnce = true;
			}
			else{
				if(loading){
					print(async.progress);
					lCam.loadScreenVisible = false;
					Invoke("StartNextScene", 1.5f);
					Invoke("SelfDestruct", 4.5f);

				}
			}
		}
	}

	void StartNextScene(){
		print("Starting Next Scene");
		async.allowSceneActivation = true;
	}
	
	
	IEnumerator AsyncLoadLevel() {
		print("AsyncLoadLevel: " +nextLevelName);
		async = Application.LoadLevelAsync(nextLevelName);
		async.allowSceneActivation = false;
		print("Load 1");


		while(async.progress < 0.9f){
			print("Load 2");
			yield return null;
		}
		loading = true;
		print("Load 3");
		yield return async;
	}

	public void SetNextLevel(string s){
		nextLevelName = s;
		Invoke("OpenLoadingScreen", 1f);

	}

	void OpenLoadingScreen(){
		Application.LoadLevel("Loading Screen");
	}

	void SelfDestruct(){
		Destroy(this.gameObject);
	}
}
