using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextRoomInfo : MonoBehaviour {

	RawImage backgroundImage;
	RectTransform backgroundTransform;
	float sizeDifference = 1000;
	float backgroundHeight;
	bool showInfo = true, runOnce = false;

	public bool playerBesideDoor = false;

	RawImage[] colorIcons;

	float frameTimer;

	Text levelText, roomText;

	Vector3 hiddenSize, visibleSize;

	Color hiddenColor, visibleColor, disabledColor;

	Level lvl;
	Color newColor;
	// Use this for initialization
	void Start () {
		lvl = GameObject.Find("Level").GetComponent<Level>();;

		backgroundImage 	= transform.Find("Background").GetComponent<RawImage>();
		backgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
		backgroundHeight 	= backgroundTransform.sizeDelta.y;

		hiddenSize = new Vector3(2000, 0, 0);
		visibleSize = backgroundTransform.sizeDelta;

		backgroundTransform.sizeDelta = hiddenSize;

		roomText = transform.Find("Background").Find("CurrentRoomNameText").GetComponent<Text>();
		levelText = transform.Find("Background").Find("CurrentLevelNameText").GetComponent<Text>();
		levelText.text = Application.loadedLevelName.ToUpper();

		visibleColor = new Color(1,1,1,1);
		hiddenColor = new Color(1,1,1,0);



		roomText.color = hiddenColor;
		levelText.color = hiddenColor;

		colorIcons = new RawImage[5];

		colorIcons[0] = transform.Find("Colors").Find("1").GetComponent<RawImage>();
		colorIcons[1] = transform.Find("Colors").Find("2").GetComponent<RawImage>();
		colorIcons[2] = transform.Find("Colors").Find("3").GetComponent<RawImage>();
		colorIcons[3] = transform.Find("Colors").Find("4").GetComponent<RawImage>();
		colorIcons[4] = transform.Find("Colors").Find("5").GetComponent<RawImage>();

		disabledColor = new Color(0,0,0,0);

		for(int i = 0; i < colorIcons.Length; i++){
			colorIcons[i].color = new Color(colorIcons[i].color.r, colorIcons[i].color.g, colorIcons[i].color.b, 0);
		}

		DisplayNewRoomInfo(GameObject.Find("[0,0]").GetComponent<Room>().roomName, Application.loadedLevelName);

	}

	void RefreshCurrentRoomColors(){
		print(lvl.transform.Find("Rooms").Find("["+lvl.currentX+","+lvl.currentZ+"]"));
		for(int i = 0; i < colorIcons.Length; i++){
			newColor = lvl.transform.Find("Rooms").Find("["+lvl.currentX+","+lvl.currentZ+"]").GetComponent<Room>().roomColors[i];
			colorIcons[i].color = new Color(newColor.r, newColor.g, newColor.b, 0);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//RefreshCurrentRoomColors();
		print(showInfo);
		if(showInfo){
			sizeDifference = Vector3.Distance(backgroundTransform.sizeDelta, visibleSize);

			backgroundTransform.sizeDelta = Vector3.Lerp(backgroundTransform.sizeDelta, visibleSize, 0.05f);
			if(sizeDifference < 10f){
				roomText.color = Color.Lerp(roomText.color, visibleColor, 0.05f);
				levelText.color = Color.Lerp(levelText.color, visibleColor, 0.05f);

				for(int i = 0; i < colorIcons.Length; i++){
					newColor = lvl.transform.Find("Rooms").Find("["+lvl.currentX+","+lvl.currentZ+"]").GetComponent<Room>().roomColors[i];

					if(newColor != disabledColor)
						colorIcons[i].color = Color.Lerp(colorIcons[i].color, new Color(newColor.r, newColor.g, newColor.b, 1), 0.05f);
				}
			}

			if(sizeDifference < 0.1f){
				if(playerBesideDoor){
					frameTimer = 0;
				}
				else{
					frameTimer += Time.deltaTime;
					if(frameTimer > 3){
						HideRoomInfo();
					}
				}
			}
		}
		else{
			roomText.color = Color.Lerp(roomText.color, hiddenColor, 0.2f);
			levelText.color = Color.Lerp(levelText.color, hiddenColor, 0.2f);

			sizeDifference = Vector3.Distance(backgroundTransform.sizeDelta, hiddenSize);
			backgroundTransform.sizeDelta = Vector3.Lerp(backgroundTransform.sizeDelta, hiddenSize, 0.1f);
			//print(lvl.transform.Find("Rooms").Find("["+lvl.currentX+","+lvl.currentZ+"]").GetComponent<Room>().roomColors[0]);
			for(int i = 0; i < colorIcons.Length; i++){
				newColor = lvl.transform.Find("Rooms").Find("["+lvl.currentX+","+lvl.currentZ+"]").GetComponent<Room>().roomColors[i];
				if(newColor != disabledColor)
					colorIcons[i].color = Color.Lerp(colorIcons[i].color, new Color(newColor.r, newColor.g, newColor.b, 0), 0.4f);
			}
		}
	}

	public void DisplayNewRoomInfo(string roomName, string levelName){
		print("DISPLAY INFO");
		print("DISPLAY INFO");
		print("DISPLAY INFO");
		print("DISPLAY INFO");
		roomText.text = roomName;
		levelText.text = levelName;
		showInfo = true;
	}

	public void ShowRoomInfo(){
		showInfo = true;
	}

	public void SetRoomName(string roomName){
		roomText.text = roomName;
	}

	public void HideRoomInfo(){
		//print("HideRoomInfo");
		showInfo = false;
		frameTimer = 0;
	}
}
