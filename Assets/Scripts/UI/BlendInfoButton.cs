using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlendInfoButton : MonoBehaviour {

	public GameObject 		target;
	GameObject 				canvasObject;
	GameObject 				player;
	PlayerController 		playerController;
	Image 					icon;
	RectTransform 			canvasTransform;
	RectTransform 			iconTransform;
	HealthManager 			pHealth;
	PauseManager 			pManager;

	void Start () 
	{
		canvasObject = GameObject.Find ("Canvas");
		canvasTransform = canvasObject.GetComponent<RectTransform> ();
		iconTransform = GetComponent<RectTransform> ();
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController> ();
		pHealth = GameObject.Find("Health Manager").GetComponent<HealthManager>();
		icon = transform.GetChild(0).gameObject.GetComponent<Image> ();
		GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ().ResetParent ();
		pManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
	}
	
	public void setTarget(GameObject t){

		target = t;
		//setFunction ();
	}

	void Update () 
	{
		if (target != null) {
			
			Vector2 ViewportPosition = Camera.main.WorldToViewportPoint (target.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2 (
				((ViewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
				((ViewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));
			
			iconTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, 60);
			
		}

		Color playerColor = playerController.targetcolor;
		icon.color = Color.Lerp (icon.color, playerColor, 10*Time.deltaTime);

		// Hide button when player is out of range
		if(target.activeSelf == false || Vector3.Distance(target.transform.position, player.transform.position) > 3.0f)
		{
			gameObject.SetActive(false);
		}

		// Hide button when player is hidden
		if(!playerController.isVisible){
			gameObject.SetActive(false);
		}

		// Hide button when player dies
		if(pHealth.playerDead)
		{
			gameObject.SetActive(false);
		}

		// Hide button when game is paused
		if (pManager.gamePaused) 
		{
			gameObject.SetActive (false);
		} 
//		else 
//		{
//			gameObject.SetActive(true);
//		}

	} // end Update
	
	public void deactivateButton()
	{
		gameObject.SetActive(false);
	}
	
	public void activateButton()
	{
		gameObject.SetActive(true);
	}
}