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
		transform.position = GameObject.FindWithTag ("Player").transform.position;
		gameObject.SetActive (false);
	}

	public void destroyItem() 
	{
		Destroy (gameObject);
	}

	void soundOnImpact() 
	{
		//print ("FLASHBANG");
		playEcho ();
		GameObject[] guards = GameObject.FindGameObjectsWithTag ("Guard");
		int x = 0;
		foreach(GameObject guard in guards)
		{
			if(Vector3.Distance(transform.position, guard.transform.position) < 10.0f) {
				guard.GetComponent<NavMeshPatrolv2>().Investigate(transform.position);
				x++;
			}
		}
	}


	void playEcho() 
	{
		Vector3 drawLoc = transform.position;
		drawLoc.y += 0.01f;
		GameObject echo = Instantiate (Resources.Load("FootfallFX"), drawLoc, Quaternion.Euler(90,0,0)) as GameObject;
		echo.GetComponent<SpriteRenderer> ().color = Color.red;
		//AudioSource.PlayClipAtPoint (impactSound.clip, transform.position);
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
