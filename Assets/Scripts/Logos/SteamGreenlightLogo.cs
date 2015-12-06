//Name:			PlayerController.cs
//Author(s)		Conor Hughes
//Description:	This script is used to fade in and out the Steam Greenlight logo
//				It the loads the title screen after six seconds.


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SteamGreenlightLogo : MonoBehaviour {
	bool showLogo = true;
	
	RawImage faderImage;
	
	// Use this for initialization
	void Start () {
		Invoke("ToggleLogo", 4);
		Invoke("LoadTitle", 6);
		Time.timeScale = 1;
		faderImage = transform.Find("Fader").GetComponent<RawImage>();
		faderImage.color = new Color(0,0,0,1);
	}
	
	//Show/Hide Logo
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
	
	//Loads the Restore Point Level (which includes title screen)
	void LoadTitle(){
		Application.LoadLevel("Restore Point");
	}
}
