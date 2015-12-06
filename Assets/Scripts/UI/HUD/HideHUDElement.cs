/// <summary>
/// Hide HUD element - script to hide elements of the HUD on request
/// </summary>

using UnityEngine;
using System.Collections;

public class HideHUDElement : MonoBehaviour {
	
	public 			Vector3 		showLocation;
	public 			Vector3 		hideLocation;

	private 		float 			hideSpeed = 10.0f;

	private 		RectTransform 	rectTran;
	private 		bool 			hideHUDPiece = false;


	void Start () 
	{
		rectTran = GetComponent<RectTransform> ();
	}
	

	void Update () 
	{
		// hide/show the HUD element when the boolean is toggled
		if(hideHUDPiece)
		{
			hideHUD();
		} 
		else
		{
			returnHUD();
		}
	}

	// lerping function for the HUD piece's position
	void hideHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, hideLocation, Time.deltaTime*hideSpeed/2);
	}
	
	void returnHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, showLocation, Time.deltaTime*hideSpeed);
	}


	/// <summary>
	/// Public functions for other scripts to call the HUD elements to hide
	/// </summary>

	// Use this function if it gets called only once
	public void toggleHide(){
		hideHUDPiece = !hideHUDPiece;
	}

	// Use the functions below if they will get called repeatedly in Update function
	public void hide(){
		hideHUDPiece = true;
	}

	public void show(){
		hideHUDPiece = false;
	}

}
