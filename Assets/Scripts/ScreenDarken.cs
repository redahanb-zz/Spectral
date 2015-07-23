using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenDarken : MonoBehaviour {
	
	public bool canDarken = false;
	GameObject darkenObject;
	RawImage rImg;
	RectTransform rTrans;
	float darkenSpeed = 2.0f, darkenAlpha = 0.6f;


	// Use this for initialization
	void Start () {
		darkenObject = transform.Find("Darken").gameObject;
		darkenObject.SetActive(true);
		rImg = darkenObject.GetComponent<RawImage>();
		rTrans = darkenObject.GetComponent<RectTransform>();
		rTrans.sizeDelta = new Vector2(Screen.width, Screen.height);
	}
	
	public void ToggleCanDarken(){
		canDarken = !canDarken;
	}
	
	// Update is called once per frame
	void Update () {
		if(canDarken){
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, darkenAlpha), Time.deltaTime * darkenSpeed);
		}
		else{
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.g, 0), Time.deltaTime * darkenSpeed);
		}
	}
}
