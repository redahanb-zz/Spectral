using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD_Inventory : MonoBehaviour {

	public 			GameObject[] 		inventoryIcons;
	public 			int 				panelSize;
	public 			Sprite 				defaultIcon;

	private			int 				inventorySize;
	private			PlayerInventory 	playerInventory;
	private 		float 				hideSpeed = 10.0f;
	private			bool 				hideInvHUD = false;
	private			RectTransform 		rectTran;

	// sound effects for pickup/drop
	public 			AudioClip 			pickUpFX;
	public 			AudioClip 			dropFX;

	// optimizing script: global variables to reuse
	private			Image 				itemIcon;
	private			Color 				iconColor;
	private			Text 				valueText;


	void Start () 
	{
		rectTran = GetComponent<RectTransform> ();
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();

		if (Application.loadedLevelName == "Upgrade Screen") {
			hideInvHUD = true;
		}
	}
	

	void Update () 
	{
		// hide/show HUD when boolean is toggled		
		if (hideInvHUD) {
			hideHUD();
		} else {
			returnHUD();
		}
	}


	// function to build the inventory at the start of the scene
	public void buildInventoryUI(int size) 
	{
		// Take size from player inventory, instantiate UI box for each slot, plus bookend graphics
		inventoryIcons = new GameObject[size];
		Vector3 panelPosition;

		// Draw shoulder graphics
		GameObject L_bookend = Instantiate (Resources.Load("UI/HUD/Inventory/HUD_Bookend_Inv_L"), new Vector3(-1*((size*panelSize)/2) - panelSize/2, 20, 0), Quaternion.identity) as GameObject;
		L_bookend.transform.SetParent(gameObject.transform, false);
		L_bookend.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1*((size*panelSize)/2) - panelSize/2, 20) ;
		
		GameObject R_bookend = Instantiate (Resources.Load("UI/HUD/Inventory/HUD_Bookend_Inv_R"), new Vector3(1*((size*panelSize)/2) + panelSize/2, 20, 0), Quaternion.identity) as GameObject;
		R_bookend.transform.SetParent(gameObject.transform, false);
		R_bookend.GetComponent<RectTransform>().anchoredPosition = new Vector2(1*((size*panelSize)/2) + panelSize/2, 20) ;

		// build chain of inventory panels, depending upon the size of the inventory
		for(int i = 0; i < size ; i++)
		{
			// position their rect-transforms according to their index and the size of the array
			panelPosition = new Vector3( -1*((size*panelSize)/2) + panelSize/2 + i*panelSize, 20, 0);
			inventoryIcons[i] = Instantiate(Resources.Load("UI/HUD/Inventory/HUD_Button_Inv"), panelPosition, Quaternion.identity) as GameObject;
			
			// set the parent and keep the position relative to the parent
			inventoryIcons[i].transform.SetParent(gameObject.transform, false);
			inventoryIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-1*((size*panelSize)/2) + panelSize/2 + i*panelSize, 20);
			inventoryIcons[i].GetComponent<SimpleIndex>().index = i;
			
			// name them as an index for the UI array
			inventoryIcons[i].gameObject.name = "Cell " + i.ToString();
		}	
	} // end buildInventoryUI

	// public function to update the icon of a particular panel
	public void updateIcon(int i){
		// if there is an object in the inventory slot
		if(playerInventory.playerInventory[i] != null){
			// set icon
			itemIcon = inventoryIcons[i].transform.GetChild(1).transform.GetComponent<Image>();
			itemIcon.sprite = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemIcon;
			// set color
			iconColor = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemColor;
			iconColor.a = 1;
			itemIcon.color = iconColor;
			// set hex colour value
			valueText = inventoryIcons[i].transform.GetComponentInChildren<Text>();
			//valueText.text = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemValue.ToString();
			valueText.text = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemValue;
		} else {
			// if no item, set icon and other parameters to default
			itemIcon = inventoryIcons[i].transform.GetChild(1).transform.GetComponent<Image>();
			itemIcon.sprite = defaultIcon;
			itemIcon.color = Color.white;
			valueText = inventoryIcons[i].transform.GetComponentInChildren<Text>();
			valueText.text = "-";
		}
	} // end updateIcon


	// public function to update all the icons in the inventory HUD
	public void UpdateAllIcons(){
		if (!playerInventory) {
			playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		}

		for(int x = 0; x < inventoryIcons.Length; x++){
			updateIcon(x);
		}
	}

	// private functions to hide/show the inventory 
	void hideHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, new Vector3(0, -75, 0), Time.deltaTime*hideSpeed/2);
	}
	
	void returnHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, new Vector3(0, 25, 0), Time.deltaTime*hideSpeed);
	}

	// public functions to hide/show the inventory
	public void toggleHide(){
		hideInvHUD = !hideInvHUD;
	}

	public void hide(){
		hideInvHUD = true;
	}

	public void show(){
		hideInvHUD = false;
	}


	// public functions to play the pickup/drop sounds
	public void pickupSound()
	{
		AudioSource.PlayClipAtPoint (pickUpFX, Camera.main.transform.position);
	}

	public void dropSound()
	{
		AudioSource.PlayClipAtPoint (dropFX, Camera.main.transform.position);
	}
}
