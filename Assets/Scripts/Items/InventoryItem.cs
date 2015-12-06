using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	// variables
	public 		string 				itemName;
	public 		Color 				itemColor;
	public 		Sprite 				itemIcon;
	public 		string 				itemValue;

	// cache script references
	private 	PlayerInventory 	playerInventory;
	private		Animator 			anim;
	private		HUD_Inventory 		inv_HUD;
	private		GameObject 			pickupButton;


	void Start () 
	{
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		inv_HUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
		anim = GetComponent<Animator> ();

		// set the 3D model colour to the colour set in the inspector
		Renderer[] childMeshes = GetComponentsInChildren<Renderer> ();
		foreach(Renderer rend in childMeshes)
		{
			rend.material.color = itemColor;
		}
	}
	

	void Update () 
	{
		if (!inv_HUD) {
			print ("Delayed assignment of HUD_Inv");
			inv_HUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
		}
	}

	// function to pick up the item: move it to a new location, set it as a child of the player, deactivate it
	// called in the last frame of the pick up animation
	public void pickUpItem() 
	{
		GameObject player = GameObject.FindWithTag ("Player");
		transform.position = new Vector3 (0,0,0);
		transform.SetParent (player.transform);
		gameObject.SetActive (false);
	}


	// function to destroy the item (Invoke-able)
	public void destroyItem() 
	{
		Destroy (gameObject);
	}


	// public function to start the 'pick up item' animation
	public void pickupAnim()
	{
		if(playerInventory.nextAvailableSlot() != -1){
			StartCoroutine (pickupONCE ("PickedUp"));
		}
		inv_HUD.pickupSound ();
	}

	// public function to start the 'drop item' animation
	public void dropAnim()
	{
		StartCoroutine (dropONCE("Dropped"));
		inv_HUD.dropSound ();
	}


	// coroutine to play the pick up animation only once
	public IEnumerator pickupONCE (string paramName)
	{
		anim.SetBool (paramName, true);
		yield return null;
		anim.SetBool (paramName, false);
	}


	// coroutine to play the drop animation only once
	public IEnumerator dropONCE (string paramName)
	{
		anim.SetBool (paramName, true);
		yield return null;
		anim.SetBool (paramName, false);
	}

	// public function to add the item to the player inventory and HUD
	public void addItemToInventory()
	{
		// add the item to the inventory	
		playerInventory.addItem (gameObject);

		// switch off the pick up button
		pickupButton.GetComponent<ItemInfoButton>().deactivateButton();
	}


	// function to set up a reference to the pick up button when it is spawned
	public void identifyButton(GameObject button)
	{
		pickupButton = button;
	}
	
}
