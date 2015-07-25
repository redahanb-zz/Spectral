using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenDarken : MonoBehaviour {
	
	public bool canDarken = false;
	GameObject darkenObject;
	RawImage rImg;
	RectTransform rTrans;
	float darkenSpeed = 2.0f, darkenAlpha = 0.6f;

	TimeTest tm;

	public float lastInterval, timeNow, myTime;
	// Use this for initialization
	void Start () {
		tm = GameObject.Find("Time Manager").GetComponent<TimeTest>();
		lastInterval = Time.realtimeSinceStartup;
		darkenObject = transform.Find("Darken").gameObject;
		darkenObject.SetActive(true);
		rImg = darkenObject.GetComponent<RawImage>();
		rTrans = darkenObject.GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
	}
	
	public void ToggleCanDarken(){
		canDarken = !canDarken;
	}
	
	// Update is called once per frame
	void Update () {
//		timeAtCurrentFrame = Time.realtimeSinceStartup;
//		deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
//		_timeAtLastFrame = _timeAtCurrentFrame; 

		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow; 

//		timeNow = Time.realtimeSinceStartup;
//		lastInterval = timeNow;
//		myTime = lastInterval/timeNow;



		if(canDarken){
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, darkenAlpha),  0.02f * myTime);
		}
		else{
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0),  0.02f * myTime);
		}



		lastInterval = timeNow;
	}
}
