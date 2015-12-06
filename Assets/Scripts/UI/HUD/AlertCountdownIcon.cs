/// <summary>
/// Alert countdown icon - script to handle the alert countdown icon 
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlertCountdownIcon : MonoBehaviour {

	private			Image 			countDownIcon;
	private			Image 			backgroundRing;
	private			AlertManager 	alertManager;
	private			Text 			timerText;
	private			string 			countdown;
	private			Color 			transpWhite;


	void Start () 
	{
		backgroundRing 					= 		GameObject.Find ("AlertCountdownIcon").GetComponent<Image> ();
		alertManager 					= 		GameObject.Find ("Alert System").GetComponent<AlertManager> ();

		// get the foreground ring icon, from the first child object 
		countDownIcon 					= 		transform.GetChild (0).GetComponent<Image> ();
		countDownIcon.enabled 			= 		false;

		// get the countdown text, from the second child object
		timerText 						= 		transform.GetChild (1).GetComponent<Text> ();
		countdown 						= 		"00.0";
		timerText.text 					= 		countdown;

		// set the transparent white colour for the text and background ring
		transpWhite 					= 		Color.white;
		transpWhite.a 					= 		0.5f;
		backgroundRing.color 			= 		transpWhite;
	} // end Start
	

	void Update () 
	{
		// if the alert is active, display the countdown icon
		if (alertManager.alertActive) {
			countDownIcon.enabled 		= 		true;
			backgroundRing.enabled 		= 		true;
			backgroundRing.color 		= 		Color.white;

			// alter the fill amount of the countdown ring to a percentage of the time remaining on alert
			countDownIcon.fillAmount 	= 		alertManager.alertCountDown / 20.0f;
			// fade the colour from red to white
			countDownIcon.color 		= 		Color.Lerp(Color.red, Color.white, (1.0f - alertManager.alertCountDown / 20.0f));

			// format the text to 0:0.0 style clock display
			timerText.text 				= 		string.Format("{0:0.0}", alertManager.alertCountDown);
			timerText.color 			= 		Color.white;

			// reset transparent white colour values
			transpWhite 				= 		Color.white;
			transpWhite.a 				= 		0.5f;
		} else {
			countDownIcon.enabled 		= 		false;
			timerText.text 				= 		countdown;

			// fade the transparent white to complete transparency
			transpWhite.a 				= 		Mathf.Lerp(transpWhite.a, 0.0f, Time.deltaTime * 2.5f);
			// set the text and background ring to the transparent white
			timerText.color 			= 		transpWhite;
			backgroundRing.color 		= 		transpWhite;
		}
	} // end Update
}
