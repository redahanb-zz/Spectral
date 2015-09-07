using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class AlertManager : MonoBehaviour {

	bool alertActive = false;

	Light mainLight;

	Color normalLightColor, alertLightColor, normalGradientColor, alertGradientColor;

	float lightChangeRate = 10;

	public AudioMixer mixer;
	float alertVolume = -80.0f;

	bool alerted = false;

	RawImage gradientImage;

	// Use this for initialization
	void Start () {
		mainLight = GameObject.Find("Directional light").GetComponent<Light>();
		gradientImage = GameObject.Find("Background Gradient").GetComponent<RawImage>();

		normalGradientColor = gradientImage.color;
		alertGradientColor = new Color(0,0,0,1);

		normalLightColor = mainLight.color;
		alertLightColor = Color.red;

		mixer.SetFloat("AlertMusicVolume", alertVolume);
		//mixer.SetFloat("NormalMusicVolume", -80.0f);
	}
	
	// Update is called once per frame
	void Update () {
		print(alertActive + " : " +alertVolume);
		mixer.SetFloat("AlertMusicVolume", alertVolume);
		//mixer.SetFloat("NormalMusicVolume", -80.0f);
		if(Input.GetKeyDown(KeyCode.B))alertActive = !alertActive;

		if(alertActive){
			if(alertVolume < -20f)alertVolume = alertVolume + 0.5f;

			//mainLight.color = alertColor;
			mainLight.color = Color.Lerp(mainLight.color, alertLightColor, Time.deltaTime * lightChangeRate);
			gradientImage.color  = Color.Lerp(gradientImage.color, alertGradientColor, Time.deltaTime * lightChangeRate);
		}
		else{
			if(alertVolume > -80f)alertVolume = alertVolume - 0.5f;

			//mainLight.color = normalColor;
			mainLight.color = Color.Lerp(mainLight.color, normalLightColor, Time.deltaTime * lightChangeRate);
			gradientImage.color  = Color.Lerp(gradientImage.color, normalGradientColor, Time.deltaTime * lightChangeRate);

		}
	}
	
}
