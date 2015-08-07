using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenOverlayPattern : MonoBehaviour {

	RectTransform rTrans;
	RawImage	rImg;
	TimeScaler tScale;

	// Use this for initialization
	void Start () {

		tScale = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		rImg = GetComponent<RawImage>();
		rTrans = GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);

	}
	
	// Update is called once per frame
	void Update () {
		if(tScale.timeSlowed){
			rImg.color = Color.Lerp(rImg.color, new Color(1,1,1,0.15f), 0.04f);
		}
		else{
			rImg.color = Color.Lerp(rImg.color, new Color(1,1,1,0f), 0.04f);
		}
	}
}
