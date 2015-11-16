using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleTutorial : MonoBehaviour {


	public RectTransform 	nwCorner, 
							neCorner, 
							swCorner, 
							seCorner, 
							backgroundTransform, 
							tutorialTransform,
							textTransform,
	topLineTransform, bottomLineTransform, leftLineTransform, rightLineTransform;

	private RawImage backgroundImage;

	public bool showTutorial = true, cornersVisible = false;

	Vector3 boxSize, cornerSize;

	float scaleSpeed = 0.01f;
	float startTime, deltaTime, previousFrameTime = 0;
	float count = 0, forcedTutorialDelay = 5;

	public string tutorialMessage;

	bool interrupted = false;

	float hideRate = 0;

	// Use this for initialization
	void Start () {

		ResetBox ();

		startTime = Time.realtimeSinceStartup;

		//boxSize =  backgroundTransform.sizeDelta;
		boxSize 	= new Vector3 (400, 140, 0);
		cornerSize 	= new Vector3 (15, 15, 0);
		tutorialTransform = GetComponent<RectTransform> ();;
		//tutorialTransform.position = new Vector3(0,200,0);

	}

	public void SetTutorialMessage(string s){
		tutorialMessage = s;
	}

	private void ResetBox(){
		nwCorner.sizeDelta = new Vector3 (0, 0, 0);
		neCorner.sizeDelta = new Vector3 (0, 0, 0);
		swCorner.sizeDelta = new Vector3 (0, 0, 0);
		seCorner.sizeDelta = new Vector3 (0, 0, 0);
		backgroundTransform.sizeDelta = new Vector3 (0, 0, 0);
	}

	public void InterruptTutorial(){
		showTutorial = false;
		interrupted = true;
	}
	
	// Update is called once per frame
	void Update () {


		scaleSpeed = 0.03f * deltaTime;


		if(interrupted)hideRate = 35;
		else hideRate = 15;


		print(count + " : " + showTutorial + " : " +backgroundTransform.sizeDelta);

		deltaTime = Time.realtimeSinceStartup - previousFrameTime;
		count = deltaTime - startTime;
		ScaleCorners();
		if (showTutorial) {

			if(cornersVisible)ScaleBackground();

		}
		else{
			HideTutorial();
		}
		if(count > forcedTutorialDelay)showTutorial = false;

		previousFrameTime = Time.time;
	}

	void HideTutorial(){
		Invoke("SelfDestruct", 3);
		backgroundTransform.sizeDelta = Vector3.Lerp(backgroundTransform.sizeDelta, new Vector3(0,0,0), scaleSpeed);
		nwCorner.position = Vector3.Lerp (nwCorner.position, tutorialTransform.position, scaleSpeed);
		neCorner.position = Vector3.Lerp (neCorner.position, tutorialTransform.position, scaleSpeed);
		swCorner.position = Vector3.Lerp (swCorner.position, tutorialTransform.position, scaleSpeed);
		seCorner.position = Vector3.Lerp (seCorner.position, tutorialTransform.position, scaleSpeed);
	}

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

	private void ScaleBackground(){
		backgroundTransform.sizeDelta = Vector3.Lerp(backgroundTransform.sizeDelta, boxSize, scaleSpeed);
		nwCorner.position = Vector3.Lerp (nwCorner.position, tutorialTransform.position + new Vector3 ((-boxSize.x)/2 , ( boxSize.y/2), 0), scaleSpeed);
		neCorner.position = Vector3.Lerp (neCorner.position, tutorialTransform.position + new Vector3 (( boxSize.x)/2 , ( boxSize.y/2), 0), scaleSpeed);
		swCorner.position = Vector3.Lerp (swCorner.position, tutorialTransform.position + new Vector3 ((-boxSize.x)/2,  (-boxSize.y/2), 0), scaleSpeed);
		seCorner.position = Vector3.Lerp (seCorner.position, tutorialTransform.position + new Vector3 (( boxSize.x)/2,  (-boxSize.y/2), 0), scaleSpeed);
	}

	void SelfDestruct(){
		Destroy(gameObject);
	}
}
