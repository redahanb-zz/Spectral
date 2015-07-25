using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class TouchInventory : MonoBehaviour {
	
	EventSystem eventSystem;

	Text text;

	//Vector2 startLocation;
	//Vector2 endLocation;
	GameObject floatImage;

	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();

		text = GameObject.Find ("ReportText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {

		print (eventSystem.currentSelectedGameObject);

		foreach(Touch touch in Input.touches){

			if(touch.phase == TouchPhase.Began){
				text.text = "Finger ID: " + touch.fingerId.ToString();
				floatImage = Instantiate(eventSystem.currentSelectedGameObject.transform.GetChild(0).gameObject, touch.position, Quaternion.identity) as GameObject;
				floatImage.transform.SetParent(GameObject.Find("Canvas").transform);
				//startLocation = touch.position;

			} else if (touch.phase == TouchPhase.Moved){
				floatImage.transform.position = touch.position;
			} else if(touch.phase == TouchPhase.Ended){
				//endLocation = touch.position;
				floatImage.SetActive(false);
				Destroy(floatImage);
			}
		}
	}
}
