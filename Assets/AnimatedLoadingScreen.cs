using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedLoadingScreen : MonoBehaviour {

	float loadingTime;
	RawImage rImg;
	bool iconVisible;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		Time.timeScale = 1;
		rImg = transform.Find("Fader").GetComponent<RawImage>();
		iconVisible = true;
		Invoke("HideIcon", 5);
	}
	
	// Update is called once per frame
	void Update () {
		print(iconVisible);
		//if(iconVisible){
		if(Application.isLoadingLevel){
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.b, 0), Time.deltaTime/1);
		}
		else{
			rImg.color = Color.Lerp(rImg.color, new Color(rImg.color.r, rImg.color.g, rImg.color.b, 1), Time.deltaTime/0.3f);
		}
	}

	void HideIcon(){
		iconVisible = false;
	}
}
