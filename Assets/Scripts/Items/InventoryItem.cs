using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	PlayerInventory playerInventory;
	Animator anim;

	public string itemName;
	public Color itemColor;
	public Sprite itemIcon;
	public int itemValue;

	GameObject pickupButton;
	
	// Use this for initialization
	void Start () {
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
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
	}
	
	public void dropAnim()
	{
		StartCoroutine (dropONCE("Dropped"));
	}


	public IEnumerator pickupONCE (string paramname)
	{
		anim.SetBool (paramname, true);
		yield return null;
		anim.SetBool (paramname, false);
	}


	public IEnumerator dropONCE (string paramname)
	{
		anim.SetBool (paramname, true);
		yield return null;
		anim.SetBool (paramname, false);
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
