using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlendInfoButton : MonoBehaviour {

	public GameObject target;
	GameObject canvasObject;
	GameObject player;
	PlayerController playerController;
	Image icon;
	RectTransform canvasTransform;
	RectTransform iconTransform;
	
	// Use this for initialization
	void Start () {
		canvasObject = GameObject.Find ("Canvas");
		canvasTransform = canvasObject.GetComponent<RectTransform> ();
		iconTransform = GetComponent<RectTransform> ();
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController> ();
		icon = transform.GetChild(0).gameObject.GetComponent<Image> ();
	}
	
	public void setTarget(GameObject t){
		target = t;
		//setFunction ();
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			
			Vector2 ViewportPosition = Camera.main.WorldToViewportPoint (target.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2 (
				((ViewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
				((ViewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));
			
			iconTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, 60);
			
		}

		//print ("Icon color: " + icon.color + ", Player color: " + playerController.targetcolor);
		Color playerColor = playerController.targetcolor;
		//playerColor.a = 1;
		icon.color = Color.Lerp (icon.color, playerColor, 10*Time.deltaTime);
		//icon.color = playerColor;

		if(target.activeSelf == false || Vector3.Distance(target.transform.position, player.transform.position) > 3.0f){
			gameObject.SetActive(false);
		}

	}
	
	public void deactivateButton(){
		gameObject.SetActive(false);
	}
	
	public void activateButton(){
		gameObject.SetActive(true);
	}
}