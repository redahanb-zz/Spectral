using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomInformation : MonoBehaviour {

	public 	Transform 	target;

	private Transform 	touchObject, 
						textObject;

	Text				roomNameText;

	RawImage 			touchImage;

	RectTransform 		roomNameTransform, 
						touchIconTransform,
						canvasTransform;

	Vector2 			touchPosition,
						namePosition,
	targetScreenPosition;

	bool 				runOnce = false, 
						isVisible = false;

	// Use this for initialization
	private void Start () {
		touchObject = transform.Find("Touch Icon");
		textObject	= transform.Find("Room Name");

		touchImage 			= touchObject.GetComponent<RawImage>();
		touchIconTransform 	= touchObject.GetComponent<RectTransform>();

		roomNameText 		= textObject.GetComponent<Text>();
		roomNameTransform 	= textObject.GetComponent<RectTransform>();

		transform.parent = GameObject.Find("Canvas").transform;
	}

	public void SetTarget(Transform t){
		target = t;
	}
	
	// Update is called once per frame
	private void Update () {


		print(target);
		if(target != null){
			//if(runOnce == false){
				canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();

				Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(target.position);
				Vector2 WorldObject_ScreenPosition =new Vector2(
					((ViewportPosition.x*canvasTransform.sizeDelta.x)-(canvasTransform.sizeDelta.x*0.5f)),
					((ViewportPosition.y*canvasTransform.sizeDelta.y)-(canvasTransform.sizeDelta.y*0.5f)));


				//Ray ray = Screen.ra
				//RaycastHit hit;

				//now you can set the position of the ui element
				touchIconTransform.anchoredPosition=WorldObject_ScreenPosition;
				roomNameTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(50,100);
				isVisible = true;
				//runOnce = true;




			//}
		}
	}

	void Fade(){
		if(isVisible){ 
			roomNameText.color = Color.Lerp(roomNameText.color, new Color(roomNameText.color.a, roomNameText.color.g, roomNameText.color.b, 1), 0.06f);
			touchImage.color = Color.Lerp(touchImage.color, new Color(touchImage.color.a, touchImage.color.g, roomNameText.color.b, 1), 0.06f);
		}
	}
}
