using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeInventorySize : MonoBehaviour {
	
	public 	bool 	inventoryVisible = false;
	public 	int 	currentInventory = 5;
	
	private int 	currentIndex = 0, 
	nextIndex = 0;
	
	private bool 	fadeIn = true;
	
	public bool		displayPanel = false;
	
	private Color 	activeColor, 
	inactiveColor, 
	nextUpgradeColorVisible, 
	nextUpgradeColorTransparent;
	
	private Vector3 hiddenPos, 
	visiblePos;
	
	private 		RawImage[] inventoryImages;
	private 		RectTransform inventoryInfoTransform;
	private 		PlayerInventory pInventory;
	
	
	Image			inventoryButtonImage;
	
	Color		buttonTargetColor = Color.white;
	// Use this for initialization
	void Start () {
		
		inventoryButtonImage = GameObject.Find("Inventory Button").GetComponent<Image>();
		buttonTargetColor = Color.white;
		inventoryInfoTransform = transform.GetComponent<RectTransform>();
		pInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		
		hiddenPos = inventoryInfoTransform.position;
		visiblePos = inventoryInfoTransform.position + new Vector3(340,0,0);
		inventoryInfoTransform.position = hiddenPos;
		
		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);
		
		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);
		
		inventoryImages = new RawImage[9];
		inventoryImages[0] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 1").GetComponent<RawImage>();
		inventoryImages[1] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 2").GetComponent<RawImage>();
		inventoryImages[2] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 3").GetComponent<RawImage>();
		inventoryImages[3] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 4").GetComponent<RawImage>();
		inventoryImages[4] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 5").GetComponent<RawImage>();
		inventoryImages[5] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 6").GetComponent<RawImage>();
		inventoryImages[6] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 7").GetComponent<RawImage>();
		inventoryImages[7] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 8").GetComponent<RawImage>();
		inventoryImages[8] = transform.Find("Current Inventory").Find("Inventory Bar").Find("Inventory 9").GetComponent<RawImage>();
		
		ClearInventory();
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		MovePanel();
		if(Vector3.Distance(inventoryInfoTransform.position,visiblePos) < 5){
			FillInventory();
			inventoryVisible = true;
		}
		if(Vector3.Distance(inventoryInfoTransform.position,visiblePos) > 330){
			ClearInventory();
			inventoryVisible = false;
		}
		
		//Debug Input
		//if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
		//if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;
		
	}
	
	void MovePanel(){
		if(displayPanel){
			inventoryInfoTransform.position = Vector3.Lerp(inventoryInfoTransform.position, visiblePos, 0.1f);
			buttonTargetColor = Color.green;
		}
		else{
			inventoryInfoTransform.position = Vector3.Lerp(inventoryInfoTransform.position, hiddenPos, 0.1f);
			buttonTargetColor = Color.white;
		}
		inventoryButtonImage.color = Color.Lerp(inventoryButtonImage.color,buttonTargetColor, Time.deltaTime * 5);
	}
	
	void FillInventory(){
		if(inventoryImages[currentIndex].color.a < 0.95f){
			inventoryImages[currentIndex].color = Color.Lerp(inventoryImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (currentInventory))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(inventoryImages[nextIndex].color.a < 0.9f) inventoryImages[nextIndex].color = Color.Lerp(inventoryImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
					else fadeIn = false;
				}
				else{
					if(inventoryImages[nextIndex].color.a > 0.1f) inventoryImages[nextIndex].color = Color.Lerp(inventoryImages[nextIndex].color, nextUpgradeColorTransparent, 0.07f);
					else fadeIn = true;
				}
			}
		}
	}
	
	void ClearInventory(){
		currentIndex = 0;
		nextIndex = 0;
		
		for(int i = 0; i < 9; i++)
			inventoryImages[i].color = inactiveColor;
	}
	
	public void IncreaseInventory(){
		pInventory.UpgradeInventory ();
		currentInventory = currentInventory + 1;
	}
}
