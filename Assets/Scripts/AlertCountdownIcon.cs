using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlertCountdownIcon : MonoBehaviour {

	Image countDownIcon;
	Image backgroundRing;
	AlertManager alertManager;
	Text timerText;
	string countdown;
	Color transpWhite;
	//public float threshold = 0.5f;

	// Use this for initialization
	void Start () {
		countDownIcon = GameObject.Find ("AlertCountdownIcon").GetComponent<Image> ();
		alertManager = GameObject.Find ("Alert System").GetComponent<AlertManager> ();
		countDownIcon.enabled = false;
		backgroundRing = transform.GetChild (0).GetComponent<Image> ();
		timerText = transform.GetChild (1).GetComponent<Text> ();
		countdown = "00.0";
		timerText.text = countdown;
		transpWhite = Color.white;
		transpWhite.a = 0.5f;
		//backgroundRing.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (alertManager.alertActive) {
			countDownIcon.enabled = true;
			backgroundRing.enabled = true;
			countDownIcon.fillAmount = alertManager.alertCountDown / 20.0f;
			countDownIcon.color = Color.Lerp(Color.red, Color.white, (1.0f - alertManager.alertCountDown / 20.0f));
			timerText.text = string.Format("{0:0.0}", alertManager.alertCountDown);
			timerText.color = Color.white;
			//backgroundRing.color = Color.Lerp(Color.red, Color.white, (1.0f - alertManager.alertCountDown / 20.0f));
		} else {
			countDownIcon.enabled = false;
			//backgroundRing.enabled = false;
			timerText.text =countdown;
			timerText.color = transpWhite;
		}
	}
}
