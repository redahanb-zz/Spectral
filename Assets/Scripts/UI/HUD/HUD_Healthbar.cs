/// <summary>
/// HUD healthbar - script for the building and updating of the healthbar HUD
/// </summary>

using UnityEngine;
using System.Collections;

public class HUD_Healthbar : MonoBehaviour {

	private 		GameObject[] 		healthbarIcons;
	private 		GameObject 			healthManager;
	private 		HealthManager 		playerHealth;
	private 		RectTransform 		rectTran;

	public 			int 				healthBarSize;
	public 			int 				panelSize;

	private			int 				currentHealth;
	private 		float 				hideSpeed = 10.0f;
	private			bool 				hideHealthHUD = false;
	

	void Start () 
	{
		rectTran 			= 		GetComponent<RectTransform> ();
		healthManager		= 		GameObject.Find ("Health Manager");
		playerHealth 		= 		healthManager.GetComponent<HealthManager> ();

		// hide the healthbar in the upgrade screen
		if (Application.loadedLevelName == "Upgrade Screen") {
			hideHealthHUD = true;
		} 

		// hide health_HUD on title screen
		if(Application.loadedLevelName == "Restore Point")
		{
			if(!GameObject.Find("HideTitleObject"))
			{
				hideHealthHUD = true;
				rectTran.anchoredPosition = new Vector3(0, 75, 0);
			}
		}

	} // end Start

	void Update () 
	{
		// refresh the healthbar to show if the player has taken damage
		updateIcons ();

		// hide the healthbar of
		if (hideHealthHUD) {
			hideHUD();
		} else {
			returnHUD();
		}
	} // end Update


	// function for the building of the healthbar at the start of the scene
	public void buildHealthbarUI(int size) 
	{
		// Take size from player inventory, instantiate UI box for each slot, plus bookend graphics
		healthbarIcons = new GameObject[size];
		Vector3 panelPosition;
		
		// draw shoulder buttons
		GameObject L_bookend = Instantiate (Resources.Load("UI/HUD/Health/HUD_Bookend_Health_L"), new Vector3(-1*((size*panelSize)/2) - panelSize/2, -10, 0), Quaternion.identity) as GameObject;
		L_bookend.transform.SetParent(gameObject.transform, false);
		L_bookend.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1*((size*panelSize)/2) - panelSize/2, -10) ;
		
		GameObject R_bookend = Instantiate (Resources.Load("UI/HUD/Health/HUD_Bookend_Health_R"), new Vector3(1*((size*panelSize)/2) + panelSize/2, -10, 0), Quaternion.identity) as GameObject;
		R_bookend.transform.SetParent(gameObject.transform, false);
		R_bookend.GetComponent<RectTransform>().anchoredPosition = new Vector2(1*((size*panelSize)/2) + panelSize/2, -10) ;

		// draw the healthbar panels in order, depending upon the size of the healthbar
		for(int i = 0; i < size ; i++){
			// position their rect-transforms according to their index and the size of the array
			panelPosition = new Vector3( -1*((size*panelSize)/2) + panelSize/2 + i*panelSize, -10, 0);
			healthbarIcons[i] = Instantiate(Resources.Load("UI/HUD/Health/HUD_Panel_Health"), panelPosition, Quaternion.identity) as GameObject;
			
			// set the parent and keep the position relative to the parent
			healthbarIcons[i].transform.SetParent(gameObject.transform, false);
			healthbarIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-1*((size*panelSize)/2) + panelSize/2 + i*panelSize, -10);
			
			// name them as an index for the UI array
			healthbarIcons[i].gameObject.name = "Cell " + i.ToString();
		}
		
	}


	// function to update all the icons in the healthbar, to show changes in player health
	void updateIcons()
	{
		currentHealth = playerHealth.playerHealth;
		for (int i = 0; i < healthBarSize; i++) {
			if(i >= currentHealth){
				healthbarIcons[i].transform.GetChild(1).gameObject.SetActive(false);
			}
		}
	}


	// private functions to show/hide the healthbar
	void hideHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, new Vector3(0, 75, 0), Time.deltaTime*hideSpeed/2);
	}

	void returnHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, new Vector3(0, -25, 0), Time.deltaTime*hideSpeed);
	}


	// public functions to hide the healthbar: one for toggling, and two for use in the update function
	public void toggleHide(){
		hideHealthHUD = !hideHealthHUD;
	}

	public void hide()
	{
		hideHealthHUD = true;
	}

	public void show()
	{
		hideHealthHUD = false;
	}
}
