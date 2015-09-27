using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD_Inventory : MonoBehaviour {

	public GameObject[] inventoryIcons;
	public int panelSize;
	int inventorySize;
	PlayerInventory playerInventory;
	public Sprite defaultIcon;

	// Use this for initialization
	void Start () {
		playerInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		inventorySize = playerInventory.inventorySize;
		//print (inventorySize);
		buildInventoryUI (inventorySize);
	}
	
	// Update is called once per frame
	void Update () {
		updateIcon ();
	}

	public void buildInventoryUI(int size) {
		// Take size from player inventory, instantiate UI box for each slot, plus bookend graphics
		//print ("Building Inventory: " + size);
		inventoryIcons = new GameObject[size];
		Vector3 panelPosition;

		// draw shoulder buttons
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
	}	

	public void updateIcon(){
		for (int i = 0; i < inventorySize; i++) {
			if(playerInventory.playerInventory[i] != null){
				// set icon
				Image itemIcon = inventoryIcons[i].transform.GetChild(1).transform.GetComponent<Image>();
				itemIcon.sprite = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemIcon;
				// set color
				Color iconColor = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemColor;
				iconColor.a = 1;
				itemIcon.color = iconColor;
				// set value
				Text valueText = inventoryIcons[i].transform.GetComponentInChildren<Text>();
				valueText.text = playerInventory.playerInventory[i].GetComponent<InventoryItem>().itemValue.ToString();
			} else {
				// icon is default
				Image itemIcon = inventoryIcons[i].transform.GetChild(1).transform.GetComponent<Image>();
				itemIcon.sprite = defaultIcon;
				itemIcon.color = Color.white;
				Text valueText = inventoryIcons[i].transform.GetComponentInChildren<Text>();
				valueText.text = "-";
			}
		}
	}
}
