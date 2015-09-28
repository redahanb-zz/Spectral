using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public bool fadeToColor = true;
	GameObject fadeObject;
	RawImage rImg;
	RectTransform rTrans;

	public float lastInterval, timeNow, myTime;
	//
	float fadeRate = 0.03f;

	Transform healthBarObject, inventoryObject, timeButtonObject;

	// Use this for initialization
	void Start () {
		if(transform.parent.Find("HUD_Healthbar"))
		healthBarObject = transform.parent.Find("HUD_Healthbar");
		if(transform.parent.Find("HUD_Inventory"))
		inventoryObject = transform.parent.Find("HUD_Inventory");
		if(transform.parent.Find("Time Button"))
		timeButtonObject = transform.parent.Find("Time Button");

		fadeObject = transform.Find("Fade").gameObject;
		fadeObject.SetActive(true);
		rImg = transform.Find("Fade").GetComponent<RawImage>();
		//rImg.color = new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1);
		rTrans = transform.Find("Fade").GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		fadeToColor = false;
	}

	public void ToggleCanFade(){
		fadeToColor = !fadeToColor;
	}
	
	// Update is called once per frame
	void Update () {
		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow;

		if(fadeToColor){
			//if(rImg.color.a > 0.6f){
			if(healthBarObject) healthBarObject.gameObject.SetActive(false);
			if(inventoryObject)	inventoryObject.gameObject.SetActive(false);
			if(timeButtonObject)	timeButtonObject.gameObject.SetActive(false);
			//}
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1), fadeRate);
		}
		else{
			if(rImg.color.a > 0.6f){
				if(healthBarObject)healthBarObject.gameObject.SetActive(true);
				if(inventoryObject)inventoryObject.gameObject.SetActive(true);
				if(timeButtonObject)timeButtonObject.gameObject.SetActive(true);
			}
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), fadeRate);
		}

		lastInterval = timeNow;
	}

	public void FadeOut(){

		fadeToColor = false;
	}

	public void FadeIn(){

		fadeToColor = true;
	}
}
