//Name:			SimpleTutorial.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Displays a tutorial box for five seconds before hiding again.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleTutorial : MonoBehaviour {
	
	
	public RectTransform 	nwCorner, 				//rect transform components to all tutorial ui elements
							neCorner, 
							swCorner, 
							seCorner, 
							backgroundTransform, 
							tutorialTransform,
							textTransform,
							topLineTransform, 
							bottomLineTransform, 
							leftLineTransform, 
							rightLineTransform;
	
	private RawImage 		backgroundImage;		//the background image
	
	public bool 			showTutorial = true, 	//determines if tutorial can show
							cornersVisible = false;	//indicates if corners are visible
	
	private Vector3 		boxSize, 				//dimensions of tutorial box
							cornerSize;				//size of corner image
	
	private float 			startTime, 				//the starting time
							deltaTime, 				//custom delta time
							previousFrameTime = 0,	//time of last frame
							forcedTutorialDelay = 5,//how long tutorial remains on screen
							hideRate = 0,			//how quickly tutorial hides
							scaleSpeed = 0.01f;		//how quickly tutorial scales

	public string 			tutorialMessage;		//message displayed in tutorial
	private bool 			interrupted = false;	//indicates tutorial was interrupted by newer tutorial

	ScreenFade 				fader;					//instance of screen fade
	
	// Use this for initialization
	void Start () {
		fader = GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ();
		fader.ResetParent ();

		ResetBox ();
		
		startTime = Time.realtimeSinceStartup;
		boxSize 	= new Vector3 (400, 140, 0);
		cornerSize 	= new Vector3 (15, 15, 0);
		tutorialTransform = GetComponent<RectTransform> ();
		
		Invoke ("InterruptTutorial", 5);
	}
	
	//Changes the tutorial message
	public void SetTutorialMessage(string s){
		tutorialMessage = s;
	}

	//Hides all elements
	private void ResetBox(){
		nwCorner.sizeDelta = new Vector3 (0, 0, 0);
		neCorner.sizeDelta = new Vector3 (0, 0, 0);
		swCorner.sizeDelta = new Vector3 (0, 0, 0);
		seCorner.sizeDelta = new Vector3 (0, 0, 0);
		backgroundTransform.sizeDelta = new Vector3 (0, 0, 0);
	}

	//Interrupts the tutorial, forcing it to hide early
	public void InterruptTutorial(){
		showTutorial = false;
		interrupted = true;
	}
	
	// Update is called once per frame
	void Update () {
		scaleSpeed = 0.03f * deltaTime;
		if(interrupted)hideRate = 35;
		else hideRate = 15;
		deltaTime = Time.realtimeSinceStartup - previousFrameTime;
		ScaleCorners();
		if (showTutorial) {
			if(cornersVisible)ScaleBackground();
		}
		else{
			HideTutorial();
		}
		previousFrameTime = Time.time;
	}

	//Hides every UI element for tutorial
	void HideTutorial(){
		Invoke("SelfDestruct", 3);
		backgroundTransform.sizeDelta = Vector3.Lerp(backgroundTransform.sizeDelta, new Vector3(0,0,0), scaleSpeed);
		nwCorner.position = Vector3.Lerp (nwCorner.position, tutorialTransform.position, scaleSpeed);
		neCorner.position = Vector3.Lerp (neCorner.position, tutorialTransform.position, scaleSpeed);
		swCorner.position = Vector3.Lerp (swCorner.position, tutorialTransform.position, scaleSpeed);
		seCorner.position = Vector3.Lerp (seCorner.position, tutorialTransform.position, scaleSpeed);
		
		nwCorner.sizeDelta = Vector3.Lerp (nwCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed);
		neCorner.sizeDelta = Vector3.Lerp (neCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed);
		swCorner.sizeDelta = Vector3.Lerp (swCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed);
		seCorner.sizeDelta = Vector3.Lerp (seCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed);
		
		
	}

	//Show or hide the corners
	private void ScaleCorners(){
		if(showTutorial){
			nwCorner.sizeDelta = Vector3.Lerp(nwCorner.sizeDelta, cornerSize, scaleSpeed);
			neCorner.sizeDelta = Vector3.Lerp(neCorner.sizeDelta, cornerSize, scaleSpeed);
			swCorner.sizeDelta = Vector3.Lerp(swCorner.sizeDelta, cornerSize, scaleSpeed);
			seCorner.sizeDelta = Vector3.Lerp(seCorner.sizeDelta, cornerSize, scaleSpeed);
		}
		else{
			nwCorner.sizeDelta = Vector3.Lerp(nwCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed/15);
			neCorner.sizeDelta = Vector3.Lerp(neCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed/15);
			swCorner.sizeDelta = Vector3.Lerp(swCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed/15);
			seCorner.sizeDelta = Vector3.Lerp(seCorner.sizeDelta, new Vector3(0,0,0), scaleSpeed/hideRate);
		}
		
		if (seCorner.sizeDelta.x > 13)cornersVisible = true;
	}

	//Show the background panel
	private void ScaleBackground(){
		backgroundTransform.sizeDelta = Vector3.Lerp(backgroundTransform.sizeDelta, boxSize, scaleSpeed);
		nwCorner.position = Vector3.Lerp (nwCorner.position, tutorialTransform.position + new Vector3 ((-boxSize.x)/2 , ( boxSize.y/2), 0), scaleSpeed);
		neCorner.position = Vector3.Lerp (neCorner.position, tutorialTransform.position + new Vector3 (( boxSize.x)/2 , ( boxSize.y/2), 0), scaleSpeed);
		swCorner.position = Vector3.Lerp (swCorner.position, tutorialTransform.position + new Vector3 ((-boxSize.x)/2,  (-boxSize.y/2), 0), scaleSpeed);
		seCorner.position = Vector3.Lerp (seCorner.position, tutorialTransform.position + new Vector3 (( boxSize.x)/2,  (-boxSize.y/2), 0), scaleSpeed);
	}

	//Destroy this gameobject
	void SelfDestruct(){
		Destroy(gameObject);
	}
}
