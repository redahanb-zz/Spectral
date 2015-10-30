using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UpgradeTree : MonoBehaviour {

	Vector3 mouseStartPosition, currentMousePosition, positionOffset, offsetFromCenter;
	RectTransform rTransform;

	bool panelVisible = false;

	UpgradeHealth healthInfo;

	UpgradeInventorySize invSizeInfo;

	UpgradeMoveSpeed speedInfo;

	UpgradeColourChange colourInfo;

	UpgradeMoveNoise noiseInfo;

	UpgradeReactionTime timescaleInfo;

	bool returnToCenter = true;

	string visiblePanelType;
	Vector3 lastPosition;

	float distance;

	Vector3 centerPoint, startPoint,  healthPoint, inventoryPoint, speedPoint, colourPoint, noisePoint, timescalePoint;

	Text selectUpgradeText;
		

	// Use this for initialization
	void Start () {
		lastPosition = Input.mousePosition;
		healthInfo = transform.parent.Find("Health Upgrade Info").GetComponent<UpgradeHealth>();
		invSizeInfo = transform.parent.Find("Inventory Upgrade Info").GetComponent<UpgradeInventorySize>();
		speedInfo = transform.parent.Find("MoveSpeed Upgrade Info").GetComponent<UpgradeMoveSpeed>();
		colourInfo = transform.parent.Find("Colour Upgrade Info").GetComponent<UpgradeColourChange>();
		noiseInfo = transform.parent.Find("Noise Upgrade Info").GetComponent<UpgradeMoveNoise>();
		timescaleInfo = transform.parent.Find("Timescale Upgrade Info").GetComponent<UpgradeReactionTime>();

		Time.timeScale = 1;
		rTransform = GetComponent<RectTransform>();

		selectUpgradeText = GameObject.Find("Select An Upgrade").GetComponent<Text>();

		startPoint = GetComponent<RectTransform>().localPosition;
		healthPoint = GameObject.Find("Health Button").GetComponent<RectTransform>().localPosition;
		inventoryPoint = GameObject.Find("Inventory Button").GetComponent<RectTransform>().localPosition;
		speedPoint = GameObject.Find("Move Speed Button").GetComponent<RectTransform>().localPosition;
//		colourPoint = GameObject.Find("Colour Speed Button").GetComponent<RectTransform>().localPosition;
//		noisePoint = GameObject.Find("Move Noise Button").GetComponent<RectTransform>().localPosition;
		timescalePoint = GameObject.Find("Timescale Button").GetComponent<RectTransform>().localPosition;

		centerPoint = startPoint;

	}
	
	// Update is called once per frame
	void Update () {
		if(returnToCenter){
			rTransform.localPosition = Vector3.Lerp(rTransform.localPosition, centerPoint, Time.deltaTime * 0.5f);
		}

		if(panelVisible){
			selectUpgradeText.color = Color.Lerp(selectUpgradeText.color, new Color(1,1,1,0), Time.deltaTime * 5);
		}
		else{
			selectUpgradeText.color = Color.Lerp(selectUpgradeText.color, new Color(1,1,1,1), Time.deltaTime * 5);
		}
		//print(rTransform.localPosition);
		//print(mouseStartPosition + "  :  " +currentMousePosition + "  :  " +positionOffset);
	}

	void ShowPanel(){
		panelVisible = true;
		if(visiblePanelType == "health")   	healthInfo.displayPanel  = true;
		if(visiblePanelType == "inventory")	invSizeInfo.displayPanel = true;
		if(visiblePanelType == "speed")		speedInfo.displayPanel = true;
		if(visiblePanelType == "colour")	colourInfo.displayPanel = true;
		if(visiblePanelType == "noise")		colourInfo.displayPanel = true;
		if(visiblePanelType == "timescale")	timescaleInfo.displayPanel = true;
	}

	void HidePanel(){
		if(visiblePanelType != "health")   	healthInfo.displayPanel  = false;
		if(visiblePanelType != "inventory")	invSizeInfo.displayPanel = false;
		if(visiblePanelType != "speed")	   	speedInfo.displayPanel = false;
		if(visiblePanelType != "colour")   	colourInfo.displayPanel = false;
		if(visiblePanelType != "noise")		colourInfo.displayPanel = false;
		if(visiblePanelType != "timescale")	timescaleInfo.displayPanel = false;
	}

	public void ClearDisplay(){
		panelVisible = false;
		centerPoint = startPoint;
		visiblePanelType = " ";
		HidePanel();
	}

	public void DisplayHealth(){
		centerPoint = startPoint - healthPoint; 
		visiblePanelType = "health";
		HidePanel();
		Invoke("ShowPanel", 0.6f);
	}

	public void DisplayInventory(){
		centerPoint = startPoint - inventoryPoint; 
		visiblePanelType = "inventory";
		HidePanel();
		Invoke("ShowPanel", 0.6f);
	}

	public void DisplaySpeed(){
		centerPoint = startPoint - speedPoint; 
		visiblePanelType = "speed";
		HidePanel();
		Invoke("ShowPanel", 0.6f);
	}

	public void DisplayColour(){
		centerPoint = startPoint - colourPoint; 
		visiblePanelType = "colour";
		HidePanel();
		Invoke("ShowPanel", 0.6f);
	}

	public void DisplayNoise(){
		centerPoint = startPoint - noisePoint; 
		visiblePanelType = "noise";
		HidePanel();
		Invoke("ShowPanel", 0.6f);
	}

	public void DisplayTimescale(){
		centerPoint = startPoint - timescalePoint; 
		visiblePanelType = "timescale";
		HidePanel();
		Invoke("ShowPanel", 0.6f);
	}

	public void SetOffsetPosition(){
		returnToCenter = false;
		//if (!EventSystem.current.IsPointerOverGameObject()){
		mouseStartPosition = Input.mousePosition;
		offsetFromCenter = rTransform.localPosition - Input.mousePosition;
		//}
	}



	public void OnDrag(){
		//if (EventSystem.current.IsPointerOverGameObject()){

		currentMousePosition = Input.mousePosition;
		//positionOffset = currentMousePosition - mouseStartPosition;

		//if(rTransform.localPosition.x > -550f && rTransform.localPosition.x < 550f){
			returnToCenter = false;
			rTransform.localPosition = Input.mousePosition + offsetFromCenter;
			lastPosition = rTransform.localPosition;
		//}
		//else{
		//	returnToCenter = true;
		//}
		//}
	}

	public void OnRelease(){
		Invoke("ReturnToCenter", 1);
	}

	void ReturnToCenter(){
		returnToCenter = true;
	}

	public void RemoveTree(){
		Destroy(transform.parent.Find("Return").gameObject);
		Destroy(gameObject);
	}

	public void CloseUpgrades(){
		print("1");
		StartCoroutine(RTB());
		print("2");
		//Invoke("ReturnToRestorePoint", 1);
	}

	public void ReturnToRestorePoint(){

		Application.LoadLevel("Restore Point");
	}

	IEnumerator RTB(){
		print("RTB1");

		yield return new WaitForSeconds(1.5f);
		print("RTB2");

		Application.LoadLevel("Restore Point");
		print("RTB3");


	}
}
