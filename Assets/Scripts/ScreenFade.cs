using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public bool fadeToColor = true;
	GameObject fadeObject;
	RawImage rImg;
	RectTransform rTrans;

	public float lastInterval, timeNow, myTime;

	// Use this for initialization
	void Start () {


		fadeObject = transform.Find("Fade").gameObject;
		fadeObject.SetActive(true);
		rImg = transform.Find("Fade").GetComponent<RawImage>();
		//rImg.color = new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1);
		rTrans = transform.Find("Fade").GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		fadeToColor = false;
	}

	public void ToggleCanFade(){
		fadeToColor = !fadeToColor;
	}
	
	// Update is called once per frame
	void Update () {
		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow;

		if(fadeToColor){
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 1), 0.04f);
		}
		else{
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), 0.04f);
		}

		lastInterval = timeNow;
	}
}
