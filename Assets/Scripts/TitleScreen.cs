using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	public GameObject healthObject, inventoryObject, timeButtonObject, mapButtonObject, playerObject, gameManagerObject, playerCameraObject, alertSystemObject, roomInfoObject;

	Level lvl;
	public RawImage logoImage;
	public Text continueText;

	Light directionalLight;

	CameraController cController;
	DepthOfFieldDeprecated dof;
	BloomOptimized bloom;

	public Transform dofTarget, targetCamera, lookAtTarget;

	RawImage gradientImage;

	Color restorePointLightColor, titleScreenLightColor;

	bool showTitleScreen = true, canInput = false;

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

		bloom = playerCameraObject.GetComponent<BloomOptimized>();
		dof = playerCameraObject.GetComponent<DepthOfFieldDeprecated>();
		dof.objectFocus = lookAtTarget.transform;

		gradientImage = transform.Find("Rainbow Gradient").GetComponent<RawImage>();

		healthObject.GetComponent<HUD_Healthbar>().enabled = false;
		inventoryObject.GetComponent<HUD_Inventory>().enabled = false;
		//timeButtonObject.SetActive(false);
		//mapButtonObject.SetActive(false);

		//gameManagerObject.SetActive(false);
		playerObject.SetActive(false);
		//alertSystemObject.SetActive(false);

		lvl = GameObject.Find("Level").GetComponent<Level>();
		lvl.enabled = false;

		logoImage.color = new Color(0,0,0,0);
		continueText.color = new Color(0,0,0,0);


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



		//print(lookAtTarget + " : " +cController.lookAtOtherTarget);
		GetInput();
		ChangeColors();

		if(!showTitleScreen){
			playerCameraObject.transform.position = Vector3.Lerp(playerCameraObject.transform.position, targetCamera.position, Time.deltaTime * Vector3.Distance(playerCameraObject.transform.position, targetCamera.transform.position)/30);
			//playerCameraObject.transform.LookAt(playerObject.transform.position);

		}
		else{
			//playerCameraObject.transform.LookAt(dofTarget.position);

		}
	}

	void ChangeColors(){
		LightColor();
		TitleColor();
		ContinueColor();
		GradientColor();
	}

	void GradientColor(){
		if(!showTitleScreen) gradientImage.color = Color.Lerp(gradientImage.color, new Color(1,1,1,0), Time.deltaTime * 3);
	}

	void LightColor(){
		if(showTitleScreen) directionalLight.color = Color.Lerp(directionalLight.color, titleScreenLightColor, Time.deltaTime * 10f);
		else 				directionalLight.color = Color.Lerp(directionalLight.color, restorePointLightColor, Time.deltaTime);

	}

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

	void ContinueColor(){
		if(canInput)continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,1), Time.deltaTime * 3);
		else continueText.color = Color.Lerp(continueText.color, new Color(0,0,0,0), Time.deltaTime * 3);
	}

	void EnableInput(){
		canInput = true;
	}

	void GetInput(){
		if(Input.GetKeyDown(KeyCode.Space) && canInput){
			canInput = false;

			showTitleScreen = false;
			//normalCanvasObject.SetActive(true);
//			playerObject.SetActive(true);
//
//			dof.objectFocus = playerObject.transform;
			Invoke("EnableHudAndControls", 1);
		}
	}

	void CameraTargetPlayer(){
		//cController.lookAtOtherTarget = null;
	}

	void EnableHudAndControls(){
		healthObject.GetComponent<HUD_Healthbar>().enabled = true;
		inventoryObject.GetComponent<HUD_Inventory>().enabled = true;
		cController.lookAtOtherTarget = null;

		gameManagerObject.SetActive(true);
		alertSystemObject.SetActive(true);
		healthObject.SetActive(true);
		inventoryObject.SetActive(true);
		timeButtonObject.SetActive(true);
		lvl.enabled = true;

		roomInfoObject.SetActive(true);

		playerObject.SetActive(true);
		dof.objectFocus = playerObject.transform;


		Destroy(this.gameObject);

	}
}
