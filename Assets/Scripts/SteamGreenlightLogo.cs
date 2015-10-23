using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SteamGreenlightLogo : MonoBehaviour {
	bool showLogo = true;

	RawImage faderImage;

	// Use this for initialization
	void Start () {
		Invoke("ToggleLogo", 4);
		Invoke("ResetDemo", 6);
		Time.timeScale = 1;
		faderImage = transform.Find("Fader").GetComponent<RawImage>();
		faderImage.color = new Color(0,0,0,1);
	}

	void ToggleLogo(){
		showLogo = !showLogo;
	}
	
	// Update is called once per frame
	void Update () {
		if(showLogo){
			faderImage.color = Color.Lerp(faderImage.color, new Color(0,0,0,0), Time.deltaTime * 2);
		}
		else{
			faderImage.color = Color.Lerp(faderImage.color, new Color(0,0,0,1), Time.deltaTime * 2);
		}
	}

	void ResetDemo(){
		Application.LoadLevel("Motor Tree Intro");
	}
}
