using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialBox : MonoBehaviour {


	bool showBox = true, boxVisible = false, bordersVisible = false, showContinue = false;


	RawImage backgroundImage, boxImage, gradientImage, topImage, bottomImage, iconImage;
	RectTransform backgroundTransform, boxTransform, gradientTransform, topBorder, bottomBorder;
	Text tutorialText, continueText;

	float scaleSpeed = 0.1f, colorSpeed = 0.05f;

	int i = 0;

	TimeScaler tScaler;

	// Use this for initialization
	void Start () {
		Invoke("ShowContinueText", 5);

		tScaler = GameObject.Find("Time Manager").GetComponent<TimeScaler>();

		boxTransform 			= GetComponent<RectTransform>();
		backgroundTransform 	= transform.parent.GetComponent<RectTransform>();
		gradientTransform 		= transform.Find("Background Gradient").GetComponent<RectTransform>();
		topBorder			 	= transform.Find("Top Border").GetComponent<RectTransform>();
		bottomBorder 			= transform.Find("Bottom Border").GetComponent<RectTransform>();

		boxImage 				= GetComponent<RawImage>();
		backgroundImage 		= transform.parent.GetComponent<RawImage>();
		gradientImage 			= transform.Find("Background Gradient").GetComponent<RawImage>();
		topImage			 	= transform.Find("Top Border").GetComponent<RawImage>();
		bottomImage 			= transform.Find("Bottom Border").GetComponent<RawImage>();
		iconImage				= transform.Find("Info Icon").GetComponent<RawImage>();

		tutorialText			= transform.Find("Tutorial Text").GetComponent<Text>();
		continueText			= transform.Find("Continue Text").GetComponent<Text>();

		SetStartingColors();

	}
	
	// Update is called once per frame
	void Update () {



		if(showBox){
			tScaler.StopTime();
			backgroundImage.color = Color.Lerp(backgroundImage.color, new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0.6f), colorSpeed);

			topImage.color = Color.Lerp(topImage.color, new Color(topImage.color.r, topImage.color.g, topImage.color.b, 0.6f), colorSpeed);
			bottomImage.color = Color.Lerp(bottomImage.color, new Color(bottomImage.color.r, bottomImage.color.g, bottomImage.color.b, 0.6f), colorSpeed);

			if(topImage.color.a > 0.4f)bordersVisible = true;

			if(bordersVisible){
				topBorder.position 		= Vector3.Lerp(topBorder.position, boxTransform.position + new Vector3(0,100,0), scaleSpeed);
				bottomBorder.position 	= Vector3.Lerp(bottomBorder.position, boxTransform.position + new Vector3(0,-100,0), scaleSpeed);
				boxTransform.sizeDelta  = Vector3.Lerp(boxTransform.sizeDelta, new Vector3(1500,200,0), scaleSpeed);
				gradientTransform.sizeDelta  = Vector3.Lerp(boxTransform.sizeDelta, new Vector3(1500,200,0), scaleSpeed);
				if(boxTransform.sizeDelta.y > 120)boxVisible = true;
			}
			if(boxVisible){
				tutorialText.color 	= Color.Lerp(tutorialText.color, new Color(tutorialText.color.r, tutorialText.color.g, tutorialText.color.b, 1), colorSpeed);
				iconImage.color 	= Color.Lerp(iconImage.color, new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 0.5f), colorSpeed);
			}

			i = i + 1;
			if(i > 350)showContinue = true;
		}
		else{
			tScaler.ResumeTime();
			backgroundImage.color 	= Color.Lerp(backgroundImage.color, new Color(0,0,0,0), colorSpeed);
			gradientImage.color 	= Color.Lerp(gradientImage.color, 	new Color(0,0,0,0), colorSpeed);
			boxImage.color			= Color.Lerp(boxImage.color, 		new Color(0,0,0,0), colorSpeed);
			topImage.color 			= Color.Lerp(topImage.color, 		new Color(0,0,0,0), colorSpeed);
			bottomImage.color 		= Color.Lerp(bottomImage.color, 	new Color(0,0,0,0), colorSpeed);
			iconImage.color 		= Color.Lerp(iconImage.color, 		new Color(0,0,0,0), colorSpeed);

			tutorialText.color 		= Color.Lerp(tutorialText.color, 	new Color(0,0,0,0), colorSpeed);
			continueText.color 		= Color.Lerp(continueText.color, 	new Color(0,0,0,0), colorSpeed);
			//print(backgroundImage.color.a);
			if(backgroundImage.color.a < 0.01f)Destroy(transform.parent.gameObject);

		}


		if(showContinue == true){
			continueText.color 		= Color.Lerp(continueText.color, new Color(continueText.color.r, continueText.color.g, continueText.color.b, 1), 0.01f);
		}
	}

	void ShowContinueText(){
		showContinue = true;
	}

	void SetStartingColors(){
		backgroundImage.color 		= new Color(backgroundImage.color.r,	 backgroundImage.color.g, 	backgroundImage.color.b, 	0);
		topImage.color 				= new Color(topImage.color.r, 			topImage.color.g, 			topImage.color.b, 			0);
		bottomImage.color 			= new Color(bottomImage.color.r, 		bottomImage.color.g, 		bottomImage.color.b, 		0);
		iconImage.color 			= new Color(iconImage.color.r, 			iconImage.color.g, 			iconImage.color.b, 			0);

		tutorialText.color 			= new Color(tutorialText.color.r, 		tutorialText.color.g, 		tutorialText.color.b, 		0); 
		continueText.color 			= new Color(continueText.color.r, 		continueText.color.g, 		continueText.color.b, 		0); 

		topBorder.position 			= boxTransform.position;
		bottomBorder.position 		= boxTransform.position;
		boxTransform.sizeDelta  	= new Vector3(1500,0,0);
		gradientTransform.sizeDelta = new Vector3(1500,0,0);
	}

	public void CloseTutorial(){
		//Destroy(transform.parent.gameObject);
		tScaler.ResumeTime();
		showBox = false;
	}
}
