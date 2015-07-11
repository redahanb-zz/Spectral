using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionListing : MonoBehaviour {
	public bool isSelected = false;

	Text difficultyText;

	Color selectedColor = Color.white, normalColor = new Color32(155,155,155, 255), difficultyColor = Color.black;

	Image buttonImg;
	Button missionListingButton;


	// Use this for initialization
	void Start () {
		difficultyText = transform.Find("Difficulty").GetComponent<Text>();
		buttonImg = GetComponent<Image>();
		missionListingButton = GetComponent<Button>();
		missionListingButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {



		if(isSelected){
			missionListingButton.interactable = true;
			buttonImg.color = Color.Lerp(buttonImg.color, selectedColor, Time.deltaTime*2);
			difficultyText.color = Color.Lerp(difficultyText.color, Color.black, Time.deltaTime*2);
		}
		else{
			missionListingButton.interactable = false;
			buttonImg.color = Color.Lerp(buttonImg.color, normalColor, Time.deltaTime*2);
			difficultyText.color = Color.Lerp(difficultyText.color, difficultyColor, Time.deltaTime*2);
		}
	}

	public void SetDifficultyColor(string difficulty){
		switch(difficulty){
			case "Very Easy" : 	difficultyColor = new Color(0,      1,  0,  0.42f); break;
			case "Easy" : 		difficultyColor = new Color(0.5f,   1,  0,  0.42f); break;
			case "Normal" : 	difficultyColor = new Color(1,      1,  0,  0.42f); break;
			case "Hard" : 		difficultyColor = new Color(1,   0.5f,  0,  0.42f); break;
			case "Very Hard" : 	difficultyColor = new Color(1,      0,  0,  0.42f); break;
		}

		normalColor = difficultyColor * 0.5f;
	}
}
