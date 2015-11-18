using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {

	GameObject button;
	GameObject canvasObject;
	GameObject player;

	HealthManager healthManager;

	InventoryItem inventoryItem;

	// Use this for initialization
	void Start () 
	{
		canvasObject = GameObject.Find ("Canvas");
		inventoryItem = GetComponent<InventoryItem> ();
		player = GameObject.FindWithTag ("Player");
		healthManager = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Vector3.Distance(transform.position, player.transform.position) < 2.0f)
		{
			if(!button)
			{
				button = Instantiate(Resources.Load("Pickup Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
				button.transform.SetParent(canvasObject.transform);
				button.GetComponent<ItemInfoButton>().setTarget(gameObject);
				button.GetComponent<Button>().onClick.AddListener( button.GetComponent<ItemInfoButton>().target.GetComponent<InventoryItem>().pickupAnim );
				inventoryItem.identifyButton(button);
			} 
			else /*if(button.GetComponent<Button>().interactable == false)*/
			{
				button.SetActive(true);
				//button.GetComponent<Button>().interactable = true;
			}

		}

		if (Vector3.Distance (transform.position, player.transform.position) > 10.0f)
		{
			if(button)
			{
				button.SetActive(false);
			}
		} 
		else if (Vector3.Distance (transform.position, player.transform.position) > 4.0f)
		{
			if(button)
			{
				button.GetComponent<ItemInfoButton>().deactivateButton();
			}
		}

		if (healthManager.playerDead) {
			if(button){
				button.SetActive(false);
			}
		}

	} // end Update
}
