using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScaler : MonoBehaviour {
	
	private Text 	timeText;
	
	public 	bool 	timeSlowed = true;

	public 	float 	myDeltaTime, 
					slowScale 		= 0.1f, 
					normalScale 	= 1f, 
					currentScale 	= 0.1f;

	float 			changeSpeed 	= 0.8f;
	
	Animator 		playerAnimator;
	
	public 	float 	lastInterval, 
					timeNow, 
					myTime;
	
	// Use this for initialization
	void Start () {
		playerAnimator 	= GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		timeText 		= GameObject.Find("Button").transform.Find("Text").GetComponent<Text>();
		myDeltaTime 	= Time.deltaTime;
		lastInterval 	= Time.realtimeSinceStartup;
	}
	
	public void SlowTime(){
		timeSlowed 		= true;
	}
	
	public void ResumeTime(){
		timeSlowed 		= false;
	}
	
	// Update is called once per frame
	void Update () {
		timeNow 		= Time.realtimeSinceStartup;
		timeText.text 	= ""+currentScale;
		
		if(timeSlowed)	currentScale = Mathf.MoveTowards(currentScale, slowScale,  0.02f);
		else 			currentScale = Mathf.MoveTowards(currentScale, normalScale,  0.02f);

		playerAnimator.SetFloat ("animationSpeed", currentScale);

		Time.timeScale 		= currentScale;
		Time.fixedDeltaTime = currentScale * 0.02f;
		
		lastInterval 		= timeNow;
		myTime 				= lastInterval/timeNow;
	}
	
	void FixedUpdate(){
		
	}
}
