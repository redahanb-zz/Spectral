using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenOverlayPattern : MonoBehaviour {

	RectTransform rTrans;
	RawImage	rImg;
	bool runOnce = false;
	// Use this for initialization
	void Start () {

		rImg = GetComponent<RawImage>();
		rTrans = GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
		rImg.color = Color.Lerp(rImg.color, new Color(1,1,1,0.15f), 0.04f);


	}
	
	// Update is called once per frame
	void Update () {

	}
}
