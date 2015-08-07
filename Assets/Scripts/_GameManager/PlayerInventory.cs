using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
		
	public GameObject[] playerInventory;
	public int inventorySize = 0;
	public int highlightedSlot = 0;
	
	HUD_Inventory inventoryUI;
	
	// Use this for initialization
	void Awake () {
		inventorySize = 6; // This value should be loaded from the player data file, as its size can be upgraded in between missions
		playerInventory = new GameObject[inventorySize]; // array of inventory items
		inventoryUI = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>(); // assign refernece to invetory HUD
		//inventoryUI.buildInventoryUI (inventorySize); // Draw the HUD of the inventory to the correct size
	}
	
	// Update is called once per frame
	void Update () {
		// Cycle through inventory slots
		///DIABLED FOR NEW PROJECT BUILD: NO CONTORLLER INPUT YET///
//		if(Input.GetKeyDown(KeyCode.Equals) || Input.GetButtonDown("RB")) {
//			if(highlightedSlot < inventorySize - 1) {
//				highlightedSlot++;
//			} else {
//				highlightedSlot = 0;
//			}
//		}
//		
//		if(Input.GetKeyDown(KeyCode.Minus) || Input.GetButtonDown("LB")){
//			if(highlightedSlot > 0){
//				highlightedSlot--;
//			} else {
//				highlightedSlot = inventorySize - 1;
//			}
//		}
//		
//		if (Input.GetKeyDown (KeyCode.J) || Input.GetButton("X_Button")) {
//			if(playerInventory[highlightedSlot]){
//				dropItem();
//				//playerInventory[highlightedSlot].GetComponent<InventoryItem>().dropItem();
//				playerInventory[highlightedSlot] = null;
//			}
//		}
//		
//		if(Input.GetKeyDown(KeyCode.I) || Input.GetButton("Y_Button")){
//			if(playerInventory[highlightedSlot]){
//				throwItem();
//				playerInventory[highlightedSlot] = null;
//			}
//		}
//		
//		if(Input.GetKeyDown(KeyCode.L) || Input.GetButton("B_Button")){
//			if(playerInventory[highlightedSlot]){
//				//GetComponent<PlayerColorChanger> ().setTargetColor();
//				print ("Change colour!!");
//			}
//		}

	}
	
	public void addItem(GameObject pickup) {
		//		if(playerInventory[highlightedSlot] == null){
		//			pickup.GetComponent<InventoryItem> ().pickUpItem (); // move the object to the player's position and hide it
		//			playerInventory [highlightedSlot] = pickup; // put item in selected slot
		//			inventoryUI.updateIcon (pickup);
		//		}
		
		if (nextAvailableSlot () != -1) {
			int emptySlot = nextAvailableSlot();
			pickup.GetComponent<InventoryItem> ().pickUpItem (); // move the object to the player's position and hide it
			playerInventory [emptySlot] = pickup; // put item in next available slot
			//inventoryUI.updateIcon (emptySlot);
		}
		
	}
		
	public void dropItem(){
		Vector3 dropLocation = GameObject.FindWithTag ("Player").transform.position + (GameObject.FindWithTag ("Player").transform.forward*0.3f);
		playerInventory[highlightedSlot].transform.position = dropLocation;
		playerInventory[highlightedSlot].gameObject.SetActive (true);
		//inventoryUI.inventoryIcons [highlightedSlot].transform.GetChild(0).transform.GetComponent<Image> ().sprite = inventoryUI.defaultSprite;
		//inventoryUI.inventoryIcons [highlightedSlot].transform.GetChild(1).transform.GetComponent<Text> ().text = "-";
	}

	public void dropItem(int index){
		Vector3 dropLocation = GameObject.FindWithTag ("Player").transform.position + (GameObject.FindWithTag ("Player").transform.forward*0.3f);
		playerInventory[index].transform.position = dropLocation;
		playerInventory[index].gameObject.SetActive (true);
		playerInventory [index] = null;
		//inventoryUI.inventoryIcons [index].transform.GetChild(0).transform.GetComponent<Image> ().sprite = inventoryUI.defaultIcon;
		//inventoryUI.inventoryIcons [highlightedSlot].transform.GetChild(1).transform.GetComponent<Text> ().text = "-";
	}
	
	public void throwItem(){
		Vector3 dropLocation = transform.position + transform.forward * 0.3f + Vector3.up;
		playerInventory [highlightedSlot].transform.position = dropLocation;
		playerInventory [highlightedSlot].gameObject.SetActive (true);
		//inventoryUI.inventoryIcons [highlightedSlot].transform.GetChild(0).transform.GetComponent<Image> ().sprite = inventoryUI.defaultSprite;
		//inventoryUI.inventoryIcons [highlightedSlot].transform.GetChild(1).transform.GetComponent<Text> ().text = "-";
		playerInventory [highlightedSlot].GetComponent<Rigidbody> ().AddForce (transform.forward * 2.5f, ForceMode.Impulse);
	}
	
	public int nextAvailableSlot() {
		for(int i = 0; i < playerInventory.Length; i++){
			if(playerInventory[i] == null){
				return i;
			} 
		}
		return -1;
	}
}
