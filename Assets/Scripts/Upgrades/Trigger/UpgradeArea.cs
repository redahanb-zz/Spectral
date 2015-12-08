using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeArea : MonoBehaviour {
	
	private RawImage 	tintImage;
	private Text 		instructionText, 
	activateText;
	private bool 		displayDropInstructions = false,  
	offerItem = false;
	public 	bool 		displayActivateInstructions = false;
	private ScreenFade 	fade;
	public 	Transform 	itemTransform;
	private Transform 	playerTransform;
	private BoxCollider boxCol;
	private HUD_Inventory hudInv;
	private CameraController cController;
	public 	GameObject[] objs;
	
	// Use this for initialization
	void Start () {
		boxCol = GetComponent<BoxCollider>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		
		hudInv = GameObject.Find ("HUD_Inventory").GetComponent<HUD_Inventory> ();
		cController = Camera.main.GetComponent<CameraController> ();
		
		Time.timeScale = 1;
		fade = GameObject.Find("Screen Fade").GetComponent<ScreenFade>();
		tintImage = GameObject.Find("Upgrade Fader").GetComponent<RawImage>();
		tintImage.color = new Color(1,1,1,0);
		
		instructionText 		= GameObject.Find("InstructionText").GetComponent<Text>();
		instructionText.color 	= new Color(0,0,0,0);
		activateText 			= GameObject.Find("ActivateText").GetComponent<Text>();
		activateText.color 		= new Color(0,0,0,0);
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//if(boxCol.bounds.Contains(playerTransform.position))print("Player inside trigger");
		//   else print("Player outside trigger");
		//print (displayDropInstructions + " : " + displayActivateInstructions);
		if (!itemTransform)
			displayActivateInstructions = false;
		
		if (displayActivateInstructions) {
			if(cController.mapButtonDown)hudInv.hide();
			else hudInv.hide ();
		} 
		else {
			if(cController.mapButtonDown)hudInv.hide();
			else hudInv.show ();
		}
		
		Instructions();
		TintScreen();
		GetInput();
		//RemoveItem();
	}
	
	void RemoveItem(){
		if(itemTransform && offerItem){
			itemTransform.localScale += new Vector3(-0.01f,-0.01f,-0.01f);
		}
	}
	
	void GetInput(){
		if(displayActivateInstructions){
			if(Input.GetKeyDown(KeyCode.Space)){
				offerItem = true;
				fade.FadeIn();
				GameState.gameState.SaveGame();
				Invoke("OpenUpgradesScreen", 2);
			}
		}
	}
	
	void OpenUpgradesScreen(){
		Application.LoadLevel("Upgrades Screen");
		
	}
	
	void TintScreen(){
		if(displayDropInstructions || displayActivateInstructions){
			tintImage.color = Color.Lerp(tintImage.color, new Color(1,1,1,0.45f), Time.deltaTime);
		}
		else{
			tintImage.color = Color.Lerp(tintImage.color, new Color(1,1,1,0), Time.deltaTime);
		}
	}
	
	void Instructions(){
		if(displayDropInstructions){
			instructionText.color = Color.Lerp(instructionText.color, new Color(0,0,0,1), Time.deltaTime);
		}
		else{
			instructionText.color = Color.Lerp(instructionText.color, new Color(0,0,0,0), Time.deltaTime);
		}
		
		if(displayActivateInstructions){
			activateText.color = Color.Lerp(activateText.color, new Color(0,0,0,1), Time.deltaTime);
		}
		else{
			activateText.color = Color.Lerp(activateText.color, new Color(0,0,0,0), Time.deltaTime);
		}
	}
	
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			displayDropInstructions = true;
			if(itemTransform)displayActivateInstructions = true;
		}
		if (displayDropInstructions) {
			
			if(c.tag == "Pickup"){
				itemTransform = c.transform;
				displayActivateInstructions = true;
			}
		}
	}
	
	void OnTriggerExit(Collider c){
		if (c.tag == "Player") {
			displayDropInstructions = false;
			displayActivateInstructions = false;
			//itemTransform = null;
		} 
		else if (c.tag == "Pickup") {
			itemTransform = null;
		}
		
	}
	
	void OnTriggerStay(Collider c){
		
	}
}
