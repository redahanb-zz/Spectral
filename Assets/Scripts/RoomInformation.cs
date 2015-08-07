using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomInformation : MonoBehaviour {

	public 	Transform 	target;

	private Transform 	touchObject, 
						textObject;

	Text				roomNameText;

	Image 			touchImage;

	RectTransform 		roomNameTransform, 
						touchIconTransform,
						canvasTransform;

	Vector2 			touchPosition,
						namePosition,
	targetScreenPosition;

	bool 				runOnce = false, 
						isVisible = false;

	Door doorway;

	Button 				touchButton;


	public bool 		displayInfo = true;


	Color inactiveColor;


	// Use this for initialization
	private void Start () {
		touchObject = transform.Find("Touch Icon");
		textObject	= transform.Find("Room Name");

		touchImage 			= touchObject.GetComponent<Image>();
		touchIconTransform 	= touchObject.GetComponent<RectTransform>();
		touchButton			= touchObject.GetComponent<Button>();

		roomNameText 		= textObject.GetComponent<Text>();
		roomNameTransform 	= textObject.GetComponent<RectTransform>();

		transform.parent = GameObject.Find("Canvas").transform;

		touchButton.onClick.RemoveAllListeners();
		print("ADDING LISTENER");
		touchButton.onClick.AddListener(TeleportPlayer);
		print("ADDED");

		inactiveColor = new Color(0.3f, 0.3f, 0.3f, 0.6f);
	}



	public void SetTarget(Transform t){
		target = t;
		roomNameText.text = target.parent.parent.parent.name;
	}

	public void SetDoor(Door d){
		doorway = d;
	}
	
	// Update is called once per frame
	private void Update () {


		//print(target);
		if(target != null){
			canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();

			Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(target.position);
			Vector2 WorldObject_ScreenPosition =new Vector2(
				((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
				((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));

			touchIconTransform.anchoredPosition=WorldObject_ScreenPosition;
			roomNameTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(50,100);
			isVisible = true;
		}


		if(displayInfo){
			touchImage.color = Color.Lerp(touchImage.color, Color.white, 0.05f);
			touchIconTransform.sizeDelta = Vector2.Lerp(touchIconTransform.sizeDelta, new Vector2(120,120), 0.05f);
			if(touchIconTransform.sizeDelta.x >= 100)
			foreach(Transform t in touchObject.transform){
				//t.gameObject.SetActive(true);

				if(t.GetComponent<Text>()){
					t.GetComponent<Text>().color = Color.Lerp(t.GetComponent<Text>().color, Color.white,0.05f);
				}
				if(t.GetComponent<RawImage>()){
					Color c = t.GetComponent<RawImage>().color;
					c = Color.Lerp(c, new Color(c.r,c.g,c.b,1),0.05f);
					t.GetComponent<RawImage>().color = c;
				}
			}
			touchButton.interactable = true;
		}
		else{
			touchImage.color = Color.Lerp(touchImage.color, inactiveColor, 0.05f);
			touchIconTransform.sizeDelta = Vector2.Lerp(touchIconTransform.sizeDelta, new Vector2(40,40), 0.05f);
			foreach(Transform t in touchObject.transform){
				//t.gameObject.SetActive(false);

				if(t.GetComponent<Text>()){
					t.GetComponent<Text>().color = Color.Lerp(t.GetComponent<Text>().color, new Color(0,0,0,0),0.05f);
				}

				if(t.GetComponent<RawImage>()){
					Color c = t.GetComponent<RawImage>().color;
					c = Color.Lerp(c, new Color(c.r,c.g,c.b,0),0.05f);
					t.GetComponent<RawImage>().color = c;
				}
			}
			touchButton.interactable = false;
		}

			
	}

	void TeleportPlayer(){
		print("Teleporting");
		doorway.StartNewTeleport();
	}

	void Fade(){
		if(isVisible){ 
			roomNameText.color = Color.Lerp(roomNameText.color, new Color(roomNameText.color.a, roomNameText.color.g, roomNameText.color.b, 1), 0.06f);
			touchImage.color = Color.Lerp(touchImage.color, new Color(touchImage.color.a, touchImage.color.g, roomNameText.color.b, 1), 0.06f);
		}
	}
}
