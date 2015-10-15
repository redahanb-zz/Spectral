using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class AlertManager : MonoBehaviour {

	public bool alertActive = false;
	public float alertCountDown;

	Light mainLight;

	Color normalLightColor, alertLightColor, normalGradientColor, alertGradientColor, slowColor, slowAlertColor;

	float lightChangeRate = 4;

	public AudioMixer mixer;
	float alertVolume = -80.0f, normalVolume = 0;

	bool alerted = false;

	RawImage gradientTopImage, gradientBottomImage;

	Material cloudMaterial;

	TimeScaler tScaler;

	// Use this for initialization
	void Start (){
		tScaler = GameObject.Find("Time Manager").GetComponent<TimeScaler>();

		cloudMaterial = Resources.Load("Cloud Mat") as Material;
		cloudMaterial.color = new Color(1,1,1,0.05f);

		mainLight = GameObject.Find("Directional Light").GetComponent<Light>();
		gradientBottomImage = GameObject.Find("Gradient Bottom Color").GetComponent<RawImage>();

		normalGradientColor = gradientBottomImage.color;
		alertGradientColor = new Color(0,0,0,1);

		SetColors();

		mixer.SetFloat("AlertMusicVolume", alertVolume);
		mixer.SetFloat("NormalMusicVolume", normalVolume);
	}

	void SetColors(){
		normalLightColor = mainLight.color;
		alertLightColor = Color.red;
		slowColor = new Color(0,0,0.8f);
		slowAlertColor = new Color(0.6f,0,0.6f);
	}
	
	// Update is called once per frame
	void Update () {
		//timeNow 		= Time.realtimeSinceStartup;



		//print(alertActive + " : " +alertVolume);
		mixer.SetFloat("AlertMusicVolume", alertVolume);
		mixer.SetFloat("NormalMusicVolume", normalVolume);
		if(Input.GetKeyDown(KeyCode.B))alertActive = !alertActive;

		if(alertActive){
			if(alertVolume < -10f)alertVolume = alertVolume + 1.0f;
			if(normalVolume > -60f)normalVolume = normalVolume - 0.5f;

			if(tScaler.timeSlowed){
				mainLight.color = Color.Lerp(mainLight.color, slowAlertColor, Time.deltaTime * lightChangeRate);
				gradientBottomImage.color  = Color.Lerp(gradientBottomImage.color, slowAlertColor, Time.deltaTime * lightChangeRate);

			}
			else{
				mainLight.color = Color.Lerp(mainLight.color, alertLightColor, Time.deltaTime * lightChangeRate);
				gradientBottomImage.color  = Color.Lerp(gradientBottomImage.color, alertGradientColor, Time.deltaTime * lightChangeRate);

			}
			cloudMaterial.color = Color.Lerp(cloudMaterial.color, new Color(0,0,0,0.02f), Time.deltaTime * lightChangeRate);

		}
		else{
			if(alertVolume > -80f)alertVolume = alertVolume - 0.5f;
			if(normalVolume < 0f)normalVolume = normalVolume + 1.0f;

			if(tScaler.timeSlowed){
				mainLight.color = Color.Lerp(mainLight.color, slowColor, Time.deltaTime * lightChangeRate);
				gradientBottomImage.color  = Color.Lerp(gradientBottomImage.color, slowColor, Time.deltaTime * lightChangeRate);

			}
			else{ 
				mainLight.color = Color.Lerp(mainLight.color, normalLightColor, Time.deltaTime * lightChangeRate);
				gradientBottomImage.color  = Color.Lerp(gradientBottomImage.color, normalGradientColor, Time.deltaTime * lightChangeRate);
			}
			cloudMaterial.color = Color.Lerp(cloudMaterial.color, new Color(1,1,1,0.05f), Time.deltaTime * lightChangeRate);

		}

		if (alertCountDown > 0) {
			alertCountDown -= Time.deltaTime;
		}
		if(alertCountDown <= 0){
			alertActive = false;
			alertCountDown = 0;
		}
	}

	public void TriggerAlert(){
		// Trigger a global alert, putting all guards on alert behaviour
		alertActive = true;
		alertCountDown = 20.0f;
	}



}
