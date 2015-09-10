using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class AlertManager : MonoBehaviour {

	public bool alertActive = false;

	Light mainLight;

	Color normalLightColor, alertLightColor, normalGradientColor, alertGradientColor;

	float lightChangeRate = 10;

	public AudioMixer mixer;
	float alertVolume = -80.0f, normalVolume = 0;

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
		mixer.SetFloat("NormalMusicVolume", normalVolume);
	}
	
	// Update is called once per frame
	void Update () {
		//print(alertActive + " : " +alertVolume);
		mixer.SetFloat("AlertMusicVolume", alertVolume);
		mixer.SetFloat("NormalMusicVolume", normalVolume);
		if(Input.GetKeyDown(KeyCode.B))alertActive = !alertActive;

		if(alertActive){
			if(alertVolume < -10f)alertVolume = alertVolume + 1.0f;
			if(normalVolume > -80f)normalVolume = normalVolume - 0.5f;
			mainLight.color = Color.Lerp(mainLight.color, alertLightColor, Time.deltaTime * lightChangeRate);
			gradientImage.color  = Color.Lerp(gradientImage.color, alertGradientColor, Time.deltaTime * lightChangeRate);
		}
		else{
			if(alertVolume > -80f)alertVolume = alertVolume - 0.5f;
			if(normalVolume < 0f)normalVolume = normalVolume + 1.0f;
			mainLight.color = Color.Lerp(mainLight.color, normalLightColor, Time.deltaTime * lightChangeRate);
			gradientImage.color  = Color.Lerp(gradientImage.color, normalGradientColor, Time.deltaTime * lightChangeRate);

		}
	}
	
}
