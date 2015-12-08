//Name:			TimeScaler.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script manages the in-game timescale, which affects everything but the player.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScaler : MonoBehaviour {
	
	private Text 				timeText;					//text component of time button
	public 	bool 				timeSlowed  	= false,	//determines if time is slowed
								timeStopped 	= false;	//determines if time is stopped
	public 	float 				slowScale 		= 0.1f,		//slower timescale
								stopScale		= 0.01f,	//stopped timescale
								normalScale 	= 1f, 		//normal timescale
								currentScale 	= 0.1f,		//the current timescale
								customDeltaTime;			//custom deltatime
	private float 				changeSpeed 	= 0.8f;		//timescale change rate
	private Animator 			playerAnimator;				//player animator component
	public 	float 				lastInterval, 				//time of last frame
								timeNow; 					//time of current frame

	private 		 			RectTransform fillTransform;//Rect transform component of the hourglass fill
	
	private bool 				canFill = true;				//determines if hourglass can refill
	
	public bool 				noiseDampening;				// make this public for testing, private eventually for encapsulation
	
	private float 				currentStoredTime = 0, 		//current amount of stored time
								maxStoredTime = 5, 			//maximum amount of stored time
								hourglassYscale;			//scale of the hourglass determined by max stored time.
	
	private GameObject 			buttonObject;				//the time button object

	private UIRotateOverTime 	rotateComponent;			//component used to rotate hourglass
	
	private bool 				hasHourglass = false;		//checks if hourglass detected
	
	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "Upgrades Screen"){
			//gameObject.SetActive(false);
		}
		else{
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
			lastInterval 	= Time.realtimeSinceStartup;
			currentStoredTime = maxStoredTime;
		}
	}

	//Enable/Disable SlowTime
	public void ToggleTime(){
		timeSlowed = !timeSlowed;
	}

	//Enable/Disable StopTime
	public void StopTime(){
		timeStopped = true;
	}

	//SlowTime if stored time is greater than 0
	public void SlowTime(){
		if(currentStoredTime > 0.01f){
			timeSlowed 		= true;
			timeStopped = false;
		}
		else ResumeTime();
		
	}

	//Resume normal time
	public void ResumeTime(){
		timeSlowed 		= false;
		timeStopped = false;
	}

	//Input for slowing/resuming time
	void KeyInput(){
		if(Input.GetKey(KeyCode.Space))SlowTime();
		else if(Input.GetKeyUp(KeyCode.Space))ResumeTime();
	}
	
	// Update is called once per frame
	void Update () {
		timeNow 		= Time.realtimeSinceStartup;
		customDeltaTime = timeNow - lastInterval;
		lastInterval 	= timeNow;
		
		KeyInput();
		if(hasHourglass)Hourglass();
		
		timeNow 		= Time.realtimeSinceStartup;
		if(timeText) timeText.text 	= ""+(int)currentStoredTime;
		
		if (timeSlowed)currentScale = Mathf.MoveTowards(currentScale, slowScale,   0.02f);
		else if (timeStopped)currentScale = Mathf.MoveTowards(currentScale, stopScale,   0.02f);
		else currentScale = Mathf.MoveTowards(currentScale, normalScale, 0.02f);

		Time.timeScale 		= currentScale;
		Time.fixedDeltaTime = currentScale * 0.02f;
		lastInterval 	= timeNow;
		
	}

	//Drains hourglass icon if time is slowed. Rotates icon if it is full.
	void Hourglass(){
		if(currentStoredTime == maxStoredTime){
			rotateComponent.enabled = true;
		}
		else{
			rotateComponent.enabled = false;
		}
		
		if(timeSlowed){
			currentStoredTime = currentStoredTime - ((customDeltaTime/maxStoredTime) * 5);
			canFill = false;
			if(currentStoredTime <= 0){
				currentStoredTime = 0;
				ResumeTime();
			}
		}

		hourglassYscale = currentStoredTime * 4;
		hourglassYscale = currentStoredTime/maxStoredTime;
		hourglassYscale = hourglassYscale * (40/1);
		fillTransform.sizeDelta = new Vector2(fillTransform.sizeDelta.x, hourglassYscale);
	}

	//Start filling the hourglass
	void StartFill(){
		canFill = true;
	}

	//The following functions are relevant to the Upgrades Screen, used to increase slow time duration and noise dampening
	public void UpgradeMaxStoredTime(){
		maxStoredTime += 1.0f;
	}

	public void SetMaxStoredTime(float f){
		maxStoredTime = f;
	}

	public float GetMaxStoredTime(){
		return maxStoredTime;
	}

	public void UpgradeNoiseDampening(){
		noiseDampening = true;
	}

	public void SetNoiseDampening(bool b){
		noiseDampening = b;
	}

	public bool GetNoiseDampening(){
		bool temp = noiseDampening;
		return temp;
	}

	
}
