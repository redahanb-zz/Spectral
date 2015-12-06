/// <summary>
/// HUD Inv Button - script for the button component of each element of the HUD inventory
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUD_Inv_Button : MonoBehaviour {

	public 		float 				timeToDrag;

	public 		bool 				blockRaycast = false;

	public 		bool 				draggable = false;
	public 		bool 				dragging = false;

	private 	bool 				countUp = false;

	private 	GameObject 			dragImage;
	private 	Vector2 			dragPosition;

	private 	int 				fingerID;

	private 	PlayerInventory 	playerInventory;
	private 	EventSystem 		eventSystem;
	private 	int 				slotIndex;
	private 	float 				timePressed = 0;


	void Start () 
	{
		playerInventory = GameObject.Find("Inventory Manager").GetComponent<PlayerInventory>();
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		slotIndex = GetComponent<SimpleIndex> ().index;

		// create a draggable copy of the item icon, set it's scale to 170%, and hide it for use later
		dragImage = Instantiate (transform.GetChild(0).gameObject, transform.position, Quaternion.identity ) as GameObject;
		dragImage.transform.SetParent(GameObject.Find("Canvas").transform);
		dragImage.transform.localScale = new Vector3 (1.7f, 1.7f, 1.7f);
		dragImage.SetActive (false);
	}
	

	void Update () 
	{
		//findTouch ();

		// if the button is held down, start the counter, otherwise reset the timer
		if (countUp) {
			timePressed += 3*Time.deltaTime;
		} else {
			timePressed = 0.0f;
		}

		// if the inventory button is held down for long enough, enable dragging and show the dragging icon
		if (timePressed >= timeToDrag) {
			draggable = true;
			dragging = true;
			spawnIcon();
		} else {
			draggable = false;
			dragging = false; // this might be redundant...
		}

		// block raycasts of the cursor if it is over the HUD element, to prevent player move orders
		if (eventSystem.IsPointerOverGameObject ()) {
			blockRaycast = true;
		} else {
			blockRaycast = false;
		}

		// update the drag position to the cursor position if the left mouse button is held down
		if(Input.GetKey(KeyCode.Mouse0)){
			dragPosition = Input.mousePosition;
		}

		// update drag position from touch data
		findTouchbyID (fingerID);

	} // end Update

	// functions to track how long an inventory button is held down
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
			//spawnIcon();
			//dragImage.SetActive(true);
		}
	}


	// drag the icon from the inventory slot if the player holds down on the slot
	public void dragIcon(){
		if (draggable) {
			findTouchbyID(fingerID);
			dragImage.transform.position = dragPosition;	
		} else {
			//print ("Can't drag!!");
		}

	}

	// drop the dragged item to the ground
	public void dropIcon(){
		if (dragging) {
			// reset dragging variables
			dragging = false;
			draggable = false;
			countUp = false;

			// hide dragging icon
			dragImage.SetActive(false);
			dragImage.transform.position = transform.position;

			// drop relative inventory item from inventory
			playerInventory.dropItem(slotIndex);
		}
	}
	
	// public function to set player target colour to the colour of the the object in the inventory slot
	public void changeColour(){
		if (playerInventory.playerInventory [slotIndex] != null) {
			GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().targetcolor = playerInventory.playerInventory [slotIndex].GetComponent<InventoryItem> ().itemColor;
		}
	}

	// display the draggable icon when a drag is initiated
	void spawnIcon(){
		// get touch that initiated the drag
		findTouchbyID(fingerID);

		// track the dragging touch, update icon position to drag position
		dragImage.transform.position = dragPosition;
		dragImage.GetComponent<Image> ().sprite = playerInventory.playerInventory [slotIndex].GetComponent<InventoryItem> ().itemIcon;

		// chang eicon colour
		Color iconColor = playerInventory.playerInventory [slotIndex].GetComponent<InventoryItem> ().itemColor;
		iconColor.a = 1;
		dragImage.GetComponent<Image> ().color = iconColor;

		// display icon
		dragImage.SetActive(true);
	}


	/// <summary>
	/// Old functions from touch control build
	/// </summary>
	 
	// find the most recent touch ID
	void findTouch(){
		foreach(Touch touch in Input.touches){
			if(touch.phase == TouchPhase.Began){
				fingerID = touch.fingerId;
				break;
			}
		}
	}

	// lookup the touch by the designated touch ID, and track its position for drag'n'drop inventory function
	void findTouchbyID(int ID){
		foreach(Touch touch in Input.touches){
			if (touch.fingerId == ID){
				dragPosition = touch.position;
			}
		}
	}

}
