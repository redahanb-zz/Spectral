using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour {

	bool paused;
	public Vector2 targetLocation;

	float moveSpeed = 13;
	RectTransform buttonTransform;
	Vector2 startlocation;
	Vector2 aimAtLocation;
	PauseManager pauseManager;

	float moveTime;

	// Use this for initialization
	void Start () {
		buttonTransform = GetComponent<RectTransform>();
		startlocation = buttonTransform.anchoredPosition;
		pauseManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(pauseManager.gamePaused != paused){
			moveTime = 0;
			paused = pauseManager.gamePaused;
		}


		if(paused){
			//buttonTransform.anchoredPosition = Vector2.Lerp (startlocation, targetLocation, moveTime * moveSpeed );
			aimAtLocation = targetLocation;
		}
		else{
			//buttonTransform.anchoredPosition = Vector2.Lerp (targetLocation, startlocation, moveTime * moveSpeed );
			aimAtLocation = startlocation;
		}

		buttonTransform.anchoredPosition = Vector2.Lerp (buttonTransform.anchoredPosition, aimAtLocation, moveSpeed*Time.deltaTime);
		//moveTime += Time.deltaTime;
	}
}
