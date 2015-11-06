using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
		
	public GameObject[] playerInventory;
	public int inventorySize;
	public int highlightedSlot = 0;

	GameObject player;
	HUD_Inventory inventoryUI;

	void Awake () 
	{
		// This value should be loaded from the player data file, as its size can be upgraded in between missions
		//inventorySize = GameState.data.inventorySize;
		//playerInventory = new GameObject[inventorySize];
		if(GameObject.Find("HUD_Inventory")) inventoryUI = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
		player = GameObject.FindWithTag ("Player");

	}

	void Start()
	{
		//playerInventory = new GameObject[inventorySize];
		//inventoryUI.buildInventoryUI (inventorySize);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	public void addItem(GameObject pickup) 
	{			
		if (nextAvailableSlot () != -1) {
			int emptySlot = nextAvailableSlot();
			pickup.GetComponent<InventoryItem> ().pickUpItem (); // move the object to the player's position and hide it
			playerInventory [emptySlot] = pickup; // put item in next available slot
			inventoryUI.updateIcon(emptySlot);
		}	
	}
		
	public void dropItem()
	{
		Vector3 dropLocation = player.transform.position + (player.transform.forward*0.3f);
		playerInventory[highlightedSlot].transform.position = dropLocation;
		playerInventory[highlightedSlot].gameObject.SetActive (true);
	}

	public void dropItem(int index)
	{
		if(playerInventory[index])
		{
			Vector3 dropLocation = player.transform.position + (player.transform.forward*0.3f) + (Vector3.up * 0.25f);
			playerInventory[index].transform.position = dropLocation;
			playerInventory[index].gameObject.SetActive (true);
			playerInventory[index].gameObject.GetComponent<InventoryItem>().dropAnim();
			playerInventory [index] = null;
			inventoryUI.updateIcon(index);
		}
	}
	
//	public void throwItem()
//	{
//		Vector3 dropLocation = transform.position + transform.forward * 0.3f + Vector3.up;
//		playerInventory [highlightedSlot].transform.position = dropLocation;
//		playerInventory [highlightedSlot].gameObject.SetActive (true);
//		playerInventory [highlightedSlot].GetComponent<Rigidbody> ().AddForce (transform.forward * 2.5f, ForceMode.Impulse);
//	}
	
	public int nextAvailableSlot() 
	{
		// searches the inventory for the first available slot 
		// because the first slot could be emptied after the second is filled
		for(int i = 0; i < playerInventory.Length; i++)
		{
			if(playerInventory[i] == null){
				return i;
			} 
		}
		return -1;
	}

}
