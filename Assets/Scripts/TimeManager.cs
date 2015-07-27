using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

	Text t;

	bool timeSlowed = true;
	float myDeltaTime, slowScale = 0.2f, normalScale = 1f, currentScale = 0.6f;
	float changeSpeed = 0.8f;

	Animator playerAnimator;
	
	// Use this for initialization
	void Start () {
		playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		t = GameObject.Find("Button").transform.Find("Text").GetComponent<Text>();
		myDeltaTime = Time.deltaTime;
	}

	public void SlowTime(){
		timeSlowed = true;
	}

	public void ResumeTime(){
		timeSlowed = false;
	}
	
	// Update is called once per frame
	void Update () {
		t.text = "Time: " +Time.fixedDeltaTime +" : " +currentScale;
		
		print(timeSlowed + " : Time: " +Time.fixedDeltaTime +" currentScale: " +currentScale);
		
		if(timeSlowed)
			currentScale = Mathf.MoveTowards(currentScale, slowScale,  Time.deltaTime);
		
		else
			currentScale = Mathf.MoveTowards(currentScale, normalScale,  Time.deltaTime);
		
		//Time.timeScale = currentScale;
		
		playerAnimator.SetFloat ("animationSpeed", currentScale);
		Time.fixedDeltaTime =  Time.fixedDeltaTime * slowScale;

	}

	void FixedUpdate(){

	}
}
