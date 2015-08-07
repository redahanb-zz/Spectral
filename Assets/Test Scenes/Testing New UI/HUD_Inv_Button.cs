using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUD_Inv_Button : MonoBehaviour {

	public float timeToDrag;

	public bool blockRaycast = false;

	public bool draggable = false;
	public bool dragging = false;
	bool countUp = false;

	GameObject dragImage;
	Vector2 dragPosition;

	int fingerID;

	PlayerInventory playerInventory;
	EventSystem eventSystem;
	int slotIndex;
	float timePressed = 0;

	// Use this for initialization
	void Start () {
		playerInventory = GameObject.Find("Inventory Manager").GetComponent<PlayerInventory>();
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		slotIndex = GetComponent<SimpleIndex> ().index;

		dragImage = Instantiate (transform.GetChild(0).gameObject, transform.position, Quaternion.identity ) as GameObject;
		dragImage.transform.SetParent(GameObject.Find("Canvas").transform);
		dragImage.transform.localScale = new Vector3 (1.7f, 1.7f, 1.7f);
		dragImage.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//findTouch ();

		if (countUp) {
			timePressed += 7*Time.deltaTime;
			//print ("Timer: " + timePressed);
		} else {
			timePressed = 0.0f;
		}

		if (timePressed >= timeToDrag) {
			draggable = true;
		} else {
			draggable = false;
			dragging = false; // this might be redundant
		}

		if (eventSystem.IsPointerOverGameObject ()) {
			blockRaycast = true;
		} else {
			blockRaycast = false;
		}

		findTouchbyID (fingerID);
		//print (dragPosition);
	}


	public void startCount(){
		//print ("Start Count");
		countUp = true;
		findTouch ();
	}

	public void endCount(){
		//print ("End count");
		countUp = false;

	}

	public void pointerExit(){
		if (!dragging) {
			
		} else {
			//print ("Copy Icon");
			spawnIcon();
			//dragImage.SetActive(true);
		}
	}



	public void dragIcon(){
		if (draggable) {
			dragging = true;
			findTouchbyID(fingerID);
			dragImage.transform.position = dragPosition;
			//print ("DRAGGING");	
		} else {
			//print ("Can't drag!!");
		}

	}

	public void dropIcon(){
		if (dragging) {
			//print ("DROPPED!!");
			dragging = false;
			draggable = false;
			countUp = false;
			dragImage.SetActive(false);
			dragImage.transform.position = transform.position;
			playerInventory.dropItem(slotIndex);
		}
	}
	

	public void changeColour(){
		print ("Change colour");
	}

	void spawnIcon(){
		findTouchbyID(fingerID);
		dragImage.transform.position = dragPosition;
		dragImage.GetComponent<Image> ().sprite = playerInventory.playerInventory [slotIndex].GetComponent<InventoryItem> ().itemIcon;
		Color iconColor = playerInventory.playerInventory [slotIndex].GetComponent<InventoryItem> ().itemColor;
		iconColor.a = 1;
		dragImage.GetComponent<Image> ().color = iconColor;
		dragImage.SetActive(true);
	}

	void findTouch(){
		foreach(Touch touch in Input.touches){
			if(touch.phase == TouchPhase.Began){
				//print ("Found Touch");
				fingerID = touch.fingerId;
				break;
			}
		}
	}

	void findTouchbyID(int ID){
		foreach(Touch touch in Input.touches){
			if (touch.fingerId == ID){
				dragPosition = touch.position;
			}
		}
	}

}
