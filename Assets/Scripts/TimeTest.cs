using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeTest : MonoBehaviour {

	Text t;
	
	bool timeSlowed = true;
	float myDeltaTime, slowScale = 0.1f, normalScale = 1f;
	float changeSpeed = 0.8f;
	public float currentScale = 0.2f;
	Animator playerAnimator;

	public float lastInterval, timeNow, myTime;

	// Use this for initialization
	void Start () {
		playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		t = GameObject.Find("Button").transform.Find("Text").GetComponent<Text>();
		myDeltaTime = Time.deltaTime;
		lastInterval = Time.realtimeSinceStartup;
	}
	
	public void SlowTime(){
		timeSlowed = true;
	}
	
	public void ResumeTime(){
		timeSlowed = false;
	}
	
	// Update is called once per frame
	void Update () {

		timeNow = Time.realtimeSinceStartup;
		t.text = ""+currentScale;
		
		//print(timeSlowed + " : Time: " +Time.fixedDeltaTime +" currentScale: " +currentScale);
		//print(Time.timeScale + " : " + myTime);
		//print(currentScale);
		if(timeSlowed)
			currentScale = Mathf.MoveTowards(currentScale, slowScale,  0.02f);
		else
			currentScale = Mathf.MoveTowards(currentScale, normalScale,  0.02f);
		Time.timeScale = currentScale;
		
		playerAnimator.SetFloat ("animationSpeed", currentScale);
		//Time.timeScale =  Time.timeScale / currentScale;


		Time.timeScale = currentScale;

		Time.fixedDeltaTime = currentScale * 0.02f;

		lastInterval = timeNow;
		myTime = lastInterval/timeNow;

		
	}
	
	void FixedUpdate(){
		
	}
}
