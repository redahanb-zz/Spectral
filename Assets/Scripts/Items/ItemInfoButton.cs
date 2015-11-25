using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfoButton : MonoBehaviour {

	public GameObject 	target;

	GameObject 			canvasObject;
	RectTransform 		canvasTransform;
	RectTransform 		iconTransform;
	ScreenFade 			sFader;
	PauseManager 		pManager;
	
	// Use this for initialization
	void Start () {
		canvasObject = GameObject.Find ("Canvas");
		canvasTransform = canvasObject.GetComponent<RectTransform> ();
		iconTransform = GetComponent<RectTransform> ();
		sFader = GameObject.Find ("Screen Fade").GetComponent<ScreenFade> ();
		sFader.ResetParent ();
		pManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
	}

	public void setTarget(GameObject t){
		target = t;
	}
		
	// Update is called once per frame
	void Update () {
		if (target != null) {

			Vector2 ViewportPosition = Camera.main.WorldToViewportPoint (target.transform.position);
			Vector2 WorldObject_ScreenPosition = new Vector2 (
			((ViewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
			((ViewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));

			iconTransform.anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, 30);

		}

		if (pManager.gamePaused) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive(true);
		}
	}

	public void deactivateButton(){
		gameObject.SetActive(false);
	}

	public void activateButton(){
		gameObject.SetActive(true);
	}
}
