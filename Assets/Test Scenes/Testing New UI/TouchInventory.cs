using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class TouchInventory : MonoBehaviour {
	
	EventSystem eventSystem;
	PlayerInventory playerInventory;
	float touchBeganTime;
	float touchHoldTime = 1.0f;
	bool dragging = false;
	
	GameObject floatImage;

	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
	}
	
	// Update is called once per frame
	void Update () {

		foreach(Touch touch in Input.touches){

			if(touch.phase == TouchPhase.Began && eventSystem.IsPointerOverGameObject() ){
				print ("Touching UI...");
				touchBeganTime = Time.time;
			} else if(touch.phase == TouchPhase.Stationary && (Time.time - touchBeganTime) >= touchHoldTime && !dragging){
				print ("Touch held down...");
				if(playerInventory.playerInventory[eventSystem.currentSelectedGameObject.GetComponent<SimpleIndex>().index]){
					print ("Spawn copy icon...");
					dragging = true;
					floatImage = Instantiate(eventSystem.currentSelectedGameObject.transform.GetChild(0).gameObject, touch.position, Quaternion.identity) as GameObject;
					SimpleIndex newIndex = floatImage.gameObject.AddComponent<SimpleIndex>() as SimpleIndex;
					newIndex.index = eventSystem.currentSelectedGameObject.GetComponent<SimpleIndex>().index;
					floatImage.transform.SetParent(GameObject.Find("Canvas").transform);
					floatImage.transform.localScale = new Vector3 (1.7f, 1.7f, 1.7f);
				}
			} else if (touch.phase == TouchPhase.Moved && dragging){
				print ("Touch dragging...");
				floatImage.transform.position = touch.position;
			} else if(touch.phase == TouchPhase.Ended && dragging){
				print ("Touch dropping...");
				playerInventory.dropItem(floatImage.GetComponent<SimpleIndex>().index);

				floatImage.SetActive(false);
				dragging = false;

			} else if(touch.phase == TouchPhase.Ended && !dragging){
				//call colour change function for correct item in inventory
				print ("Change colour...");
				if(eventSystem.currentSelectedGameObject && eventSystem.currentSelectedGameObject.tag == "HUD_Inv"){	
//					string objectName = eventSystem.currentSelectedGameObject.name; // take name of object as a string
//					string lastLetter = objectName[objectName.Length-1].ToString(); // take last letter and convert to int
//					int invIndex = int.Parse(lastLetter ); // use int to call the right colour from playerInventory
//					print (invIndex);
					int itemIndex = eventSystem.currentSelectedGameObject.GetComponent<SimpleIndex>().index;
					//print("Set player colour to: " + playerInventory.playerInventory[itemIndex].GetComponent<InventoryItem>().itemColor);
				}
			}
		}
	}
	
}
