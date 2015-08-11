using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlendInfo : MonoBehaviour {

	GameObject button;
	GameObject canvasObject;
	
	InventoryItem inventoryItem;
	
	// Use this for initialization
	void Start () {
		canvasObject = GameObject.Find ("Canvas");
		inventoryItem = GetComponent<InventoryItem> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			if(!button){
				button = Instantiate(Resources.Load("UI/Worldspace Buttons/Blend Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
				button.transform.SetParent(canvasObject.transform);
				button.GetComponent<BlendInfoButton>().setTarget(gameObject);
				//button.GetComponent<Button>().onClick.AddListener( button.GetComponent<ItemInfoButton>().target.GetComponent<InventoryItem>().addItemToInventory );
				//button.GetComponent<Button>().onClick.AddListener( button.GetComponent<ItemInfoButton>().target.GetComponent<InventoryItem>().pickupAnim );
				//inventoryItem.identifyButton(button);
			} else if(button.GetComponent<Button>().interactable == false){
				button.GetComponent<Button>().interactable = true;
			}
		}
		
		// reactivate in case the object has been put back onto the ground (button deactivates then)
		//button.SetActive (true);
	}
	
	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			button.GetComponent<Button>().interactable = false;
		}
	}
}
