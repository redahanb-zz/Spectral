using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {

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
			button = Instantiate(Resources.Load("Pickup Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
			button.transform.SetParent(canvasObject.transform);
			button.GetComponent<ItemInfoButton>().setTarget(gameObject);
			button.GetComponent<Button>().onClick.AddListener( button.GetComponent<ItemInfoButton>().target.GetComponent<InventoryItem>().addItemToInventory );
			inventoryItem.identifyButton(button);
		}
	}
}
