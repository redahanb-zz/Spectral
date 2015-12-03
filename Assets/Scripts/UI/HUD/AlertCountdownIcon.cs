using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlertCountdownIcon : MonoBehaviour {

	Image 			countDownIcon;
	Image 			backgroundRing;
	AlertManager 	alertManager;
	Text 			timerText;
	string 			countdown;
	Color 			transpWhite;
	//public float threshold = 0.5f;

	// Use this for initialization
	void Start () {

		backgroundRing = GameObject.Find ("AlertCountdownIcon").GetComponent<Image> ();
		alertManager = GameObject.Find ("Alert System").GetComponent<AlertManager> ();

		countDownIcon = transform.GetChild (0).GetComponent<Image> ();
		countDownIcon.enabled = false;
		timerText = transform.GetChild (1).GetComponent<Text> ();
		countdown = "00.0";
		timerText.text = countdown;
		transpWhite = Color.white;
		transpWhite.a = 0.5f;
		backgroundRing.color = transpWhite;
	}
	
	// Update is called once per frame
	void Update () {
		if (alertManager.alertActive) {
			countDownIcon.enabled = true;
			backgroundRing.enabled = true;
			backgroundRing.color = Color.white;
			countDownIcon.fillAmount = alertManager.alertCountDown / 20.0f;
			countDownIcon.color = Color.Lerp(Color.red, Color.white, (1.0f - alertManager.alertCountDown / 20.0f));
			timerText.text = string.Format("{0:0.0}", alertManager.alertCountDown);
			timerText.color = Color.white;
			transpWhite = Color.white;
			transpWhite.a = 0.5f;
		} else {
			countDownIcon.enabled = false;
			timerText.text = countdown;

			transpWhite.a = Mathf.Lerp(transpWhite.a, 0.0f, Time.deltaTime * 2.5f);

			timerText.color = transpWhite;
			backgroundRing.color = transpWhite;
		}
	}
}
