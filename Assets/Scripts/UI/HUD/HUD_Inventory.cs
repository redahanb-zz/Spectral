using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD_Inventory : MonoBehaviour {

	public GameObject[] inventoryIcons;
	public int panelSize;
	int inventorySize;
	PlayerInventory playerInventory;
	public Sprite defaultIcon;
	public float hideSpeed;
	bool hideInvHUD = false;
	RectTransform rectTran;

	public AudioClip pickUpFX;
	public AudioClip dropFX;

	// optimizing script: global variables to reuse
	Image itemIcon;
	Color iconColor;
	Text valueText;

	// Use this for initialization
	void Start () 
	{
		rectTran = GetComponent<RectTransform> ();
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.H)){
			toggleHide();
		}
		
		if (hideInvHUD) {
			hideHUD();
		} else {
			returnHUD();
		}
	}

	public void buildInventoryUI(int size) {
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
		
		for(int i = 0; i < size ; i++){
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

	public void updateIcon(int i){
		if(playerInventory.playerInventory[i] != null){
			// set icon
			itemIcon = inventoryIcons[i].transform.GetChild(1).transform.GetComponent<Image>();
			itemIcon.sprite = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemIcon;
			// set color
			iconColor = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemColor;
			iconColor.a = 1;
			itemIcon.color = iconColor;
			// set value
			valueText = inventoryIcons[i].transform.GetComponentInChildren<Text>();
			valueText.text = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemValue.ToString();
		} else {
			// icon is default
			itemIcon = inventoryIcons[i].transform.GetChild(1).transform.GetComponent<Image>();
			itemIcon.sprite = defaultIcon;
			itemIcon.color = Color.white;
			valueText = inventoryIcons[i].transform.GetComponentInChildren<Text>();
			valueText.text = "-";
		}
	} // end updateIcon

	public void UpdateAllIcons(){
		if (!playerInventory) {
			playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		}

		for(int x = 0; x < inventoryIcons.Length; x++){
			updateIcon(x);
		}
	}

	void hideHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, new Vector3(0, -75, 0), Time.deltaTime*hideSpeed/2);
	}
	
	void returnHUD()
	{
		rectTran.anchoredPosition = Vector3.Lerp (rectTran.anchoredPosition, new Vector3(0, 25, 0), Time.deltaTime*hideSpeed);
	}

	public void toggleHide(){
		hideInvHUD = !hideInvHUD;
	}

	public void pickupSound()
	{
		AudioSource.PlayClipAtPoint (pickUpFX, Camera.main.transform.position);
	}

	public void dropSound()
	{
		AudioSource.PlayClipAtPoint (dropFX, Camera.main.transform.position);
	}
}
