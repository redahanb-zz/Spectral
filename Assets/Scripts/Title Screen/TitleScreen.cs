//Name:			TitleScreen.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script controls the display of the title screen, used in the Restore Point scene.


using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	public GameObject 				healthObject, 		//health bar gameobject
									inventoryObject, 	//inventory bar gameobject
									timeButtonObject, 	//time buttonprivate 
									mapButtonObject, 	//map button gameobject
									playerObject, 		//player gameobject
									gameManagerObject, 	//game manager gameobject
									playerCameraObject, //player camera gameobject
									alertSystemObject, 	//alert system gameobject
									roomInfoObject,		//next room info gameobject
									upgradeAreaTrigger;	//upgrade area

	private Level 					lvl;				//instance of level script
	public 	RawImage 				logoImage;			//the logo image
	public 	Text 					continueText;		//'Press space to contine' text component

	private Light 					directionalLight;	//the main directional light

	private CameraController 		cController;		//instance of CameraController
	private DepthOfFieldDeprecated 	dof;				//used to control cameras depth of field

	public 	Transform 				dofTarget, 			//the depth of field target object
									targetCamera, 		//the title screen target camera (used for position and rotation)
									lookAtTarget;		//the title screen look at target for the camera

	private RawImage 				gradientImage;		//the gradient overlay iamge

	private Color 					restorePointLightColor, //the normal level lighting color
									titleScreenLightColor;	//the title screen lighting color

	public bool 					showTitleScreen = true, //determines if title screen can be displayed
									canInput = false;		//determines if the player can input

	HUD_Inventory 					invHUD;					//instance of inventory
	HUD_Healthbar					healthHUD;				//instance of healthbar
	HideHUDElement					hidePause, hideTime;	//instances of hide hud element
	HideHUDElement					hideMapButton;			//instance of hide hud element

	// Use this for initialization
	void Start () {
		Invoke("EnableInput", 3);

		Time.timeScale = 1;

		roomInfoObject.SetActive(false);

		directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
		restorePointLightColor = directionalLight.color;
		titleScreenLightColor = Color.white;

		cController = playerCameraObject.GetComponent<CameraController>();
		cController.lookAtOtherTarget = lookAtTarget;

		dof = playerCameraObject.GetComponent<DepthOfFieldDeprecated>();
		dof.objectFocus = lookAtTarget.transform;

		gradientImage = transform.Find("Rainbow Gradient").GetComponent<RawImage>();
		upgradeAreaTrigger = GameObject.Find("Upgrade Area Trigger");
		upgradeAreaTrigger.GetComponent<UpgradeArea> ().enabled = false;

		//healthObject.GetComponent<HUD_Healthbar>().enabled = false;
		//inventoryObject.GetComponent<HUD_Inventory>().enabled = false;
		playerObject.SetActive(false);

		lvl = GameObject.Find("Level").GetComponent<Level>();
		lvl.enabled = false;

		logoImage.color = new Color(0,0,0,0);
		continueText.color = new Color(0,0,0,0);

		hideMapButton = GameObject.Find ("Map Button").GetComponent<HideHUDElement> ();

		if(!invHUD){
			invHUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
			healthHUD = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
			hidePause = GameObject.Find("PauseButton").GetComponent<HideHUDElement>();
			hideTime = GameObject.Find("Time Button").GetComponent<HideHUDElement>();
		}

		if(GameObject.Find("HideTitleObject")){
			showTitleScreen = false;
			playerObject.SetActive(true);
			dof.objectFocus = playerObject.transform;
			EnableHudAndControls();
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		GetInput();
		ChangeColors();
		if(!showTitleScreen)playerCameraObject.transform.position = Vector3.Lerp(playerCameraObject.transform.position, targetCamera.position, Time.deltaTime * Vector3.Distance(playerCameraObject.transform.position, targetCamera.transform.position)/30);
	}
	
	//Function used to set all colours
	void ChangeColors(){
		LightColor();
		TitleColor();
		ContinueColor();
		GradientColor();
	}

	//Sets gradient color
	void GradientColor(){
		if(!showTitleScreen) gradientImage.color = Color.Lerp(gradientImage.color, new Color(1,1,1,0), Time.deltaTime * 3);
	}

	//Sets light colour
	void LightColor(){
		if(showTitleScreen) directionalLight.color = Color.Lerp(directionalLight.color, titleScreenLightColor, Time.deltaTime * 10f);
		else 				directionalLight.color = Color.Lerp(directionalLight.color, restorePointLightColor, Time.deltaTime);
	}

	//Sets title logo colour
	void TitleColor(){
		if(showTitleScreen){
			logoImage.color = Color.Lerp(logoImage.color, new Color(1,1,1,1), Time.deltaTime * 3);
			if(canInput)continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,1), Time.deltaTime * 3);
			else continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,0), Time.deltaTime * 3);
		}
		else{
			logoImage.color = Color.Lerp(logoImage.color, new Color(0,0,0,0), Time.deltaTime * 3);
			continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,0), Time.deltaTime * 3);
		}
	}

	//Sets text colour
	void ContinueColor(){
		if(canInput)continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,1), Time.deltaTime * 3);
		else continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,0), Time.deltaTime * 3);
	}

	//Enables input
	void EnableInput(){
		canInput = true;
	}

	//Reads Spacebar input
	void GetInput(){
		if(Input.GetKeyDown(KeyCode.Space) && canInput){
			canInput = false;

			showTitleScreen = false;
			Invoke("EnableHudAndControls", 1);
		}
	}

	//Restores in-game hud and controls
	void EnableHudAndControls(){
		healthObject.GetComponent<HUD_Healthbar> ().toggleHide ();
		inventoryObject.GetComponent<HUD_Inventory> ().toggleHide ();
		cController.lookAtOtherTarget = null;

		gameManagerObject.SetActive(true);
		alertSystemObject.SetActive(true);
		//healthObject.SetActive(true);
		//inventoryObject.SetActive(true);
		timeButtonObject.SetActive(true);
		hideTime.toggleHide ();
		hidePause.toggleHide ();
		hideMapButton.toggleHide ();
		lvl.enabled = true;
		upgradeAreaTrigger.GetComponent<UpgradeArea> ().enabled = true;

		roomInfoObject.SetActive(true);

		playerObject.SetActive(true);
		dof.objectFocus = playerObject.transform;

		Destroy(this.gameObject);
	}
}
