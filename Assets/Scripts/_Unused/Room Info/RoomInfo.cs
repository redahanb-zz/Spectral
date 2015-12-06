using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomInfo : MonoBehaviour {
	
	public bool textVisible = false;
	bool coloursVisible = false;

	GameObject textObject;
	Text roomText;
	RawImage[] circleImage = new RawImage[5];


	RectTransform rTrans;
	float fadeSpeed = 2f;

	// Use this for initialization
	void Start () {
		textObject = transform.Find("Text").gameObject;
		textObject.SetActive(true);
		roomText = textObject.GetComponent<Text>();

		//Invoke("ToggleText", 0.2f);

		SetupColors();
	}

	void SetupColors(){
		circleImage[0] = transform.Find("Color 1").GetComponent<RawImage>();
		circleImage[1] = transform.Find("Color 2").GetComponent<RawImage>();
		circleImage[2] = transform.Find("Color 3").GetComponent<RawImage>();
		circleImage[3] = transform.Find("Color 4").GetComponent<RawImage>();
		circleImage[4] = transform.Find("Color 5").GetComponent<RawImage>();
	}
	
	public void ToggleText(){
		textVisible = !textVisible;
	}
	
	// Update is called once per frame
	void Update () {
		if(textVisible){
			roomText.color = Color.Lerp(roomText.color, new Color(roomText.color.r, roomText.color.g, roomText.color.g, 1), Time.deltaTime * fadeSpeed);
			foreach(Transform t in transform){
				if(t.GetComponent<RawImage>()){
					RawImage rImg = t.GetComponent<RawImage>();
					rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1), Time.deltaTime * fadeSpeed);
				}
			}
		}
		else{
			roomText.color = Color.Lerp(roomText.color, new Color(roomText.color.r, roomText.color.g, roomText.color.g, 0), Time.deltaTime * fadeSpeed);
			foreach(Transform t in transform){
				if(t.GetComponent<RawImage>()){
					RawImage rImg = t.GetComponent<RawImage>();
					rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), Time.deltaTime * fadeSpeed);
				}
			}
		}
	}
}
