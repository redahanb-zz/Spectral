using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScaler : MonoBehaviour {
	
	private Text 	timeText;
	
	public 	bool 	timeSlowed  = false,
					timeStopped = false;

	public 	float 	myDeltaTime, 
					slowScale 		= 0.1f,
					stopScale		= 0.01f,
					normalScale 	= 1f, 
					currentScale 	= 0.1f;

	float 			changeSpeed 	= 0.8f;
	
	Animator 		playerAnimator;
	
	public 	float 	lastInterval, 
					timeNow, 
					myTime;

	RectTransform fillTransform;

	bool canFill = true;

	//PauseManager pauseManager;

	float currentStoredTime = 0, maxStoredTime = 12, hourglassYscale;

	GameObject buttonObject;

	UIRotateOverTime rotateComponent;

	bool hasHourglass = false;

	// Use this for initialization
	void Start () {

		if(Application.loadedLevelName == "Upgrades Screen"){
			gameObject.SetActive(false);
		}
		else{
			myTime = 0;
			timeNow = 0;
			lastInterval = 0;

			buttonObject = GameObject.Find("Time Button");
			if(buttonObject.transform.Find("Text"))	timeText = buttonObject.transform.Find("Text").GetComponent<Text>();

			if(buttonObject.transform.Find("Hourglass"))hasHourglass = true;
			if(hasHourglass){
				rotateComponent = buttonObject.transform.Find("Hourglass").GetComponent<UIRotateOverTime>();
				fillTransform = GameObject.Find("Hourglass").transform.Find("Fill").GetComponent<RectTransform>();
			}
			playerAnimator 	= GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
			myDeltaTime 	= Time.deltaTime;
			lastInterval 	= Time.realtimeSinceStartup;
		}
	}

	public void ToggleTime(){
		timeSlowed = !timeSlowed;
	}

	public void StopTime(){
		timeStopped = true;
	}
	
	public void SlowTime(){
		if(currentStoredTime > 0.01f){
			timeSlowed 		= true;
			timeStopped = false;
		}
		else{
			ResumeTime();
		}
	}
	
	public void ResumeTime(){
		timeSlowed 		= false;
		timeStopped = false;
		Invoke("StartFill", 3);

	}

	void KeyInput(){
		if(Input.GetKey(KeyCode.T))SlowTime();
		else if(Input.GetKeyUp(KeyCode.T))ResumeTime();
	}
	
	// Update is called once per frame
	void Update () {
		KeyInput();
		if(hasHourglass)Hourglass();

		timeNow 		= Time.realtimeSinceStartup;
		if(timeText) timeText.text 	= ""+(int)currentStoredTime;
		
		if(timeSlowed)			currentScale = Mathf.MoveTowards(currentScale, slowScale,   0.02f);
		else if(timeStopped)	currentScale = Mathf.MoveTowards(currentScale, stopScale,   0.02f);
		else 					currentScale = Mathf.MoveTowards(currentScale, normalScale, 0.02f);

		if(playerAnimator) playerAnimator.SetFloat ("animationSpeed", currentScale);

		Time.timeScale 		= currentScale;
		Time.fixedDeltaTime = currentScale * 0.02f;
		
		lastInterval 		= timeNow;
		myTime 				= lastInterval/timeNow;

//		print (pauseManager.gamePaused == true);
//		if(pauseManager.gamePaused){
//			Time.timeScale = 0.0f;
//			print ("TIME STOPPED");
//		}
	}

	void Hourglass(){
		if(currentStoredTime == maxStoredTime){
			rotateComponent.enabled = true;
		}
		else{
			rotateComponent.enabled = false;
		}

		if(timeSlowed){
			currentStoredTime = currentStoredTime - ((Time.deltaTime/maxStoredTime) * 100);
			canFill = false;
			if(currentStoredTime <= 0){
				currentStoredTime = 0;
				ResumeTime();
			}
		}
		else{
			if(canFill)currentStoredTime = currentStoredTime + ((Time.deltaTime/maxStoredTime) * 50);
			if(currentStoredTime > maxStoredTime)currentStoredTime = maxStoredTime;

		}
		hourglassYscale = currentStoredTime * 4;
		hourglassYscale = currentStoredTime/maxStoredTime;
		hourglassYscale = hourglassYscale * (40/1);
		fillTransform.sizeDelta = new Vector2(fillTransform.sizeDelta.x, hourglassYscale);
	}

	void StartFill(){
		canFill = true;
	}

}
