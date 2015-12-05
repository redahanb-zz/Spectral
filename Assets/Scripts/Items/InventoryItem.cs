using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	PlayerInventory 	playerInventory;
	Animator 			anim;
	HUD_Inventory 		inv_HUD;

	public string 	itemName;
	public Color 	itemColor;
	public Sprite 	itemIcon;
	public string 	itemValue;

	GameObject 		pickupButton;
	
	// Use this for initialization
	void Start () {
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		inv_HUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
		anim = GetComponent<Animator> ();

		Renderer[] childMeshes = GetComponentsInChildren<Renderer> ();
		foreach(Renderer rend in childMeshes)
		{
			rend.material.color = itemColor;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!inv_HUD) {
			print ("Delayed assignment of HUD_Inv");
			inv_HUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>(); // dubLUDO bug
		}
	}

	public void pickUpItem() 
	{
		GameObject player = GameObject.FindWithTag ("Player");
		transform.position = new Vector3 (0,0,0);
		transform.SetParent (player.transform);
		gameObject.SetActive (false);
	}

	public void destroyItem() 
	{
		Destroy (gameObject);
	}
	
	public void pickupAnim()
	{
		if(playerInventory.nextAvailableSlot() != -1){
			StartCoroutine (pickupONCE ("PickedUp"));
		}
		inv_HUD.pickupSound ();
	}
	
	public void dropAnim()
	{
		StartCoroutine (dropONCE("Dropped"));
		inv_HUD.dropSound ();
	}


	public IEnumerator pickupONCE (string paramName)
	{
		anim.SetBool (paramName, true);
		yield return null;
		anim.SetBool (paramName, false);
	}


	public IEnumerator dropONCE (string paramName)
	{
		anim.SetBool (paramName, true);
		yield return null;
		anim.SetBool (paramName, false);
	}


	public void addItemToInventory()
	{
			playerInventory.addItem (gameObject);
			pickupButton.GetComponent<ItemInfoButton>().deactivateButton();
	}


	public void identifyButton(GameObject button)
	{
		pickupButton = button;
	}
	
}
