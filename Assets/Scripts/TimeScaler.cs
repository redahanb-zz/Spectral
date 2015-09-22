using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScaler : MonoBehaviour {
	
	private Text 	timeText;
	
	public 	bool 	timeSlowed  = false,
					timeStopped = false;

	public 	float 	myDeltaTime, 
					slowScale 		= 0.1f, 
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

	float currentStoredTime = 0, maxStoredTime = 8, hourglassYscale;

	GameObject buttonObject;

	UIRotateOverTime rotateComponent;

	bool hasHourglass = false;

	// Use this for initialization
	void Start () {
		//print(timeSlowed);


		buttonObject = GameObject.Find("Time Button");
		if(buttonObject.transform.Find("Hourglass"))hasHourglass = true;
		if(hasHourglass){
			rotateComponent = buttonObject.transform.Find("Hourglass").GetComponent<UIRotateOverTime>();
			fillTransform = GameObject.Find("Hourglass").transform.Find("Fill").GetComponent<RectTransform>();
		}
		playerAnimator 	= GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		//timeText 		= GameObject.Find("Time Button").transform.Find("Text").GetComponent<Text>();
		myDeltaTime 	= Time.deltaTime;
		lastInterval 	= Time.realtimeSinceStartup;
		//pauseManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
	}

	public void ToggleTime(){
		timeSlowed = !timeSlowed;
	}

	public void StopTime(){
		timeStopped = false;
	}
	
	public void SlowTime(){
		//print ("Slow time function called");
		if(currentStoredTime > 0.01f){
			timeSlowed 		= true;
			timeStopped = false;
		}
		else{
			ResumeTime();
		}
	}
	
	public void ResumeTime(){
		//print ("Resume time function called");
		timeSlowed 		= false;
		timeStopped = false;
		Invoke("StartFill", 3);

	}


	
	// Update is called once per frame
	void Update () {
		if(hasHourglass)Hourglass();

		timeNow 		= Time.realtimeSinceStartup;
		//timeText.text 	= ""+currentScale;
		
		if(timeSlowed)			currentScale = Mathf.MoveTowards(currentScale, slowScale,   0.02f);
		else if(timeStopped)	currentScale = Mathf.MoveTowards(currentScale, slowScale,   0.00f);
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
		//print(currentStoredTime);


		if(currentStoredTime == maxStoredTime){
			rotateComponent.enabled = true;
		}
		else{
			rotateComponent.enabled = false;
		}

		if(timeSlowed){
			currentStoredTime = currentStoredTime - 0.03f;
			canFill = false;


			if(currentStoredTime <= 0){
				currentStoredTime = 0;
				ResumeTime();
				//buttonObject.SetActive(false);

			}


		}
		else{
			if(canFill)currentStoredTime = currentStoredTime + 0.02f;
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

	void FixedUpdate(){
		
	}
}
