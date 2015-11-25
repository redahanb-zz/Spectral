using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public bool 	fadeToColor = true;
	GameObject 		fadeObject;
	RawImage 		rImg;
	RectTransform 	rTrans;
	GameObject 		canvas;
	GameObject 		pausePanel;
	GameObject 		mouseCursor;

	public float lastInterval, timeNow, myTime;
	//
	float fadeRate = 0.01f;

	Transform healthBarObject, inventoryObject, timeButtonObject;

	// Use this for initialization
	void Start () {
		if(transform.parent.Find("HUD_Healthbar"))
		healthBarObject = transform.parent.Find("HUD_Healthbar");
		if(transform.parent.Find("HUD_Inventory"))
		inventoryObject = transform.parent.Find("HUD_Inventory");
		if(transform.parent.Find("Time Button"))
		timeButtonObject = transform.parent.Find("Time Button");
		if (GameObject.Find ("Mouse Cursor"))
		mouseCursor = GameObject.Find ("Mouse Cursor");

		fadeObject = transform.Find("Fade").gameObject;
		fadeObject.SetActive(true);
		rImg = transform.Find("Fade").GetComponent<RawImage>();
		//rImg.color = new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1);
		rTrans = transform.Find("Fade").GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		fadeToColor = false;
		canvas = gameObject.transform.parent.gameObject;
		pausePanel = GameObject.Find ("Pause Manager").GetComponent<PauseManager>().pauseScreen;
	}

	public void ToggleCanFade(){
		fadeToColor = !fadeToColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (!mouseCursor)
			mouseCursor = GameObject.Find ("Mouse Cursor");

		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow;

		if(fadeToColor){
			//if(rImg.color.a > 0.6f){
			//if(healthBarObject) healthBarObject.gameObject.SetActive(false);
			//if(inventoryObject)	inventoryObject.gameObject.SetActive(false);
			if(timeButtonObject)	timeButtonObject.gameObject.SetActive(false);
			//}
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1), Time.deltaTime * 3);
		}
		else{
			if(rImg.color.a > 0.6f){
				//if(healthBarObject)healthBarObject.gameObject.SetActive(true);
				//if(inventoryObject)inventoryObject.gameObject.SetActive(true);
				if(timeButtonObject)timeButtonObject.gameObject.SetActive(true);
			}
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), Time.deltaTime * 3);
		}

		lastInterval = timeNow;
	}

	public void FadeOut(){

		fadeToColor = false;
	}

	public void FadeIn(){

		fadeToColor = true;
		Time.timeScale = 1;
	}

	public void ResetParent(){
		print ("Restting fader in hierarchy!");
		transform.SetParent (null);
		pausePanel.GetComponent<RectTransform>().SetParent (null);
		pausePanel.GetComponent<RectTransform>().SetParent (canvas.transform);
		mouseCursor.transform.SetParent (null);
		mouseCursor.transform.SetParent (canvas.transform);
		transform.SetParent (canvas.transform);
	}
}
