using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public bool fadeToColor = true;
	GameObject fadeObject;
	RawImage rImg;
	RectTransform rTrans;

	// Use this for initialization
	void Start () {
		fadeObject = transform.Find("Fade").gameObject;
		fadeObject.SetActive(true);
		rImg = transform.Find("Fade").GetComponent<RawImage>();
		rTrans = transform.Find("Fade").GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		Invoke("ToggleCanFade", 2);
	}

	public void ToggleCanFade(){
		fadeToColor = !fadeToColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(fadeToColor){
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1), Time.deltaTime);
		}
		else{
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), Time.deltaTime);
		}
	}
}
