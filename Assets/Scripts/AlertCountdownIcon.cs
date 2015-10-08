using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlertCountdownIcon : MonoBehaviour {

	Image countDownIcon;
	Image backgroundRing;
	AlertManager alertManager;
	//public float threshold = 0.5f;

	// Use this for initialization
	void Start () {
		countDownIcon = GameObject.Find ("AlertCountdownIcon").GetComponent<Image> ();
		alertManager = GameObject.Find ("Alert System").GetComponent<AlertManager> ();
		countDownIcon.enabled = false;
		backgroundRing = transform.GetChild (0).GetComponent<Image> ();
		backgroundRing.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (alertManager.alertActive) {
			countDownIcon.enabled = true;
			backgroundRing.enabled = true;
			//print ((1.0f - alertManager.alertCountDown / 20.0f));
			//countDownIcon.eventAlphaThreshold = (1.0f - alertManager.alertCountDown / 20.0f);
			//countDownIcon.eventAlphaThreshold = threshold;
			countDownIcon.fillAmount = alertManager.alertCountDown / 20.0f;
		} else {
			countDownIcon.enabled = false;
			backgroundRing.enabled = false;
		}
	}
}
