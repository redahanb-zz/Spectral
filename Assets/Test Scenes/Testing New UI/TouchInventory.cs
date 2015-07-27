using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class TouchInventory : MonoBehaviour {
	
	EventSystem eventSystem;
	
	float touchBeganTime;
	float touchHoldTime = 1.0f;
	bool dragging = false;

	//Vector2 startLocation;
	//Vector2 endLocation;
	GameObject floatImage;

	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();

	}
	
	// Update is called once per frame
	void Update () {

		foreach(Touch touch in Input.touches){

			if(touch.phase == TouchPhase.Began && eventSystem.IsPointerOverGameObject() ){
				touchBeganTime = Time.time;
				//floatImage = Instantiate(eventSystem.currentSelectedGameObject.transform.GetChild(0).gameObject, touch.position, Quaternion.identity) as GameObject;
				//floatImage.transform.SetParent(GameObject.Find("Canvas").transform);

			} else if(touch.phase == TouchPhase.Stationary && (Time.time - touchBeganTime) >= touchHoldTime && !dragging){
				dragging = true;
				floatImage = Instantiate(eventSystem.currentSelectedGameObject.transform.GetChild(0).gameObject, touch.position, Quaternion.identity) as GameObject;
				floatImage.transform.SetParent(GameObject.Find("Canvas").transform);
				floatImage.transform.localScale = new Vector3 (2.0f, 2.0f, 2.0f);
			} 

			else if (touch.phase == TouchPhase.Moved && dragging){
				floatImage.transform.position = touch.position;
			} else if(touch.phase == TouchPhase.Ended && dragging){
				floatImage.SetActive(false);
				dragging = false;
			}
		}
	}
}
