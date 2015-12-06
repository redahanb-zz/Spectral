//Name:			ScreenFade.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script creates a fade-to-black transition used by several other scripts.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {
	public 	bool 			fadeToColor = true;		//determines if iamge fades in/out
	public 	float 			lastInterval, 			//time of last frame
							timeNow, 				//time of current frame
							customDeltaTime;		//custom deltatime independent of timescale
	private GameObject 		fadeObject;				//the ui element that fades
	private RawImage 		rImg;					//the rawimage component of the fade object
	private RectTransform 	rTrans;					//the recttransform component of the fade object
	private GameObject 		canvas;					//the main canvas
	private GameObject 		pausePanel;				//the pause panel gameobject
	private GameObject 		mouseCursor;			//the mouse cursor gameobject
	private float 			fadeRate = 3f;			//the rate at which the image fades
	private Transform 		healthBarObject, 		//healthbar transform
							inventoryObject, 		//inventory transform
							timeButtonObject;		//time button transform
	
	// Use this for initialization
	void Start () {
		//find main hud elements
		if(transform.parent.Find("HUD_Healthbar"))healthBarObject = transform.parent.Find("HUD_Healthbar");
		if(transform.parent.Find("HUD_Inventory"))inventoryObject = transform.parent.Find("HUD_Inventory");
		if(transform.parent.Find("Time Button"))timeButtonObject = transform.parent.Find("Time Button");
		if (GameObject.Find ("Mouse Cursor"))mouseCursor = GameObject.Find ("Mouse Cursor");
		pausePanel = GameObject.Find ("Pause Manager").GetComponent<PauseManager>().pauseScreen;
		canvas = gameObject.transform.parent.gameObject;
		
		//find and enable fader
		fadeObject = transform.Find("Fade").gameObject;
		fadeObject.SetActive(true);
		
		//Get Components and size of fader
		rImg = transform.Find("Fade").GetComponent<RawImage>();
		rTrans = transform.Find("Fade").GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		
		//fade out 
		fadeToColor = false;
	}
	
	//Shows/hides the fader
	public void ToggleCanFade(){
		fadeToColor = !fadeToColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (!mouseCursor)mouseCursor = GameObject.Find ("Mouse Cursor");
		
		timeNow = Time.realtimeSinceStartup;
		customDeltaTime = timeNow - lastInterval;
		lastInterval = timeNow;
		
		if(fadeToColor){
			if(timeButtonObject)	timeButtonObject.gameObject.SetActive(false);
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1), Time.deltaTime * fadeRate);
		}
		else{
			if(rImg.color.a > 0.6f)if(timeButtonObject)timeButtonObject.gameObject.SetActive(true);
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), Time.deltaTime * fadeRate);
		}
		lastInterval = timeNow;
	}
	
	//has the fader disappear
	public void FadeOut(){
		fadeToColor = false;
	}
	
	//has the fader appear
	public void FadeIn(){
		fadeToColor = true;
		Time.timeScale = 1;
	}
	
	//Move the fader to the bottom of the hierarchy so no other UI elements appear in front of it.
	public void ResetParent(){
		transform.SetParent (null);
		pausePanel.GetComponent<RectTransform>().SetParent (null);
		pausePanel.GetComponent<RectTransform>().SetParent (canvas.transform);
		mouseCursor.transform.SetParent (null);
		mouseCursor.transform.SetParent (canvas.transform);
		transform.SetParent (canvas.transform);
	}
}
