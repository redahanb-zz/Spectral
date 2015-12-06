using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
		
	// variables
	public 		int 			inventorySize;
	public 		int 			highlightedSlot = 0;

	// scripts references
	public 		GameObject[] 	playerInventory;
	private 	GameObject 		player;
	private 	HUD_Inventory 	inventoryUI;

	void Awake () 
	{
		if(GameObject.Find("HUD_Inventory")) inventoryUI = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
		player = GameObject.FindWithTag ("Player");

	}

	void Start()
	{

	}

	void Update () 
	{

	}

	// function to add an item to the next available slot in the inventory
	public void addItem(GameObject pickup) 
	{			
		// check if there is an available slot, if so, add the item
		if (nextAvailableSlot () != -1) {
			int emptySlot = nextAvailableSlot();

			// move the object to the player's position and hide it
			pickup.GetComponent<InventoryItem> ().pickUpItem (); 

			// put item in next available slot, update icon in inventory HUD
			playerInventory [emptySlot] = pickup;
			inventoryUI.updateIcon(emptySlot);
		}	
	}


	//function to remove an item from the inventory (legacy from touch control build)
	public void dropItem()
	{
		Vector3 dropLocation = player.transform.position + (player.transform.forward*-0.5f);
		playerInventory[highlightedSlot].transform.position = dropLocation;
		playerInventory[highlightedSlot].gameObject.SetActive (true);
	}

	// overloaded function to remove an item from the inventory by index
	public void dropItem(int index)
	{
		// check that there is an item in the slot
		if(playerInventory[index])
		{
			// set the drop location just behind the player
			Vector3 dropLocation = player.transform.position + (player.transform.forward*-0.5f) + (Vector3.up * 0.25f);

			// place the item at the drop location, set its parent to null and activate it, play the drop animation
			playerInventory[index].transform.position = dropLocation;
			playerInventory[index].transform.SetParent(null);
			playerInventory[index].gameObject.SetActive (true);
			playerInventory[index].gameObject.GetComponent<InventoryItem>().dropAnim();

			// empty that slot in the inventory, update the icon
			playerInventory [index] = null;
			inventoryUI.updateIcon(index);
		}
	}

	// searches the inventory for the first empty slot, because the first slot could be emptied after the second is filled
	public int nextAvailableSlot() 
	{
		for(int i = 0; i < playerInventory.Length; i++)
		{
			if(playerInventory[i] == null){
				return i;
			} 
		}
		// if the inventory is full, return -1
		return -1;
	}

	// upgrade function to increase inventory size
	public void UpgradeInventory()
	{
		inventorySize++;
	}

}
