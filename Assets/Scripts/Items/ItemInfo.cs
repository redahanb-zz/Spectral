/// <summary>
/// Item info - Script to spawn a button for the player to pick up the item
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {
	
	GameObject 		button;
	GameObject 		canvasObject;
	GameObject 		player;
	
	HealthManager 	healthManager;
	InventoryItem 	inventoryItem;
	PauseManager  	pauseManager;
	
	TitleScreen 	tScreen;
	
	
	void Start () 
	{
		if(Application.loadedLevelName == "Restore Point") tScreen = GameObject.Find ("Title Screen Canvas").GetComponent<TitleScreen> ();
		canvasObject = GameObject.Find ("Canvas");
		inventoryItem = GetComponent<InventoryItem> ();
		player = GameObject.FindWithTag ("Player");
		healthManager = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		pauseManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
	}
	
	
	void Update () 
	{
		if(Application.loadedLevelName == "Restore Point")
			if (!player && !tScreen.showTitleScreen) 
				player = GameObject.FindWithTag ("Player");
		
		
		if (player) {
			// track player position, when in range, spawn a pickup button
			if (Vector3.Distance (transform.position, player.transform.position) < 2.0f) {
				// only spawn a button if one does not already exist
				if (!button) {
					button = Instantiate (Resources.Load ("Pickup Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
					button.transform.SetParent (canvasObject.transform);
					button.GetComponent<ItemInfoButton> ().setTarget (gameObject);
					button.GetComponent<Button> ().onClick.AddListener (button.GetComponent<ItemInfoButton> ().target.GetComponent<InventoryItem> ().pickupAnim);
					inventoryItem.identifyButton (button);
				} else if (!pauseManager.gamePaused) {
					button.SetActive (true);
				}
				
			}
			
			// if the player goes out of range, switch of the button
			if (Vector3.Distance (transform.position, player.transform.position) > 10.0f) {
				if (button) {
					button.SetActive (false);
				}
			} else if (Vector3.Distance (transform.position, player.transform.position) > 4.0f) {
				if (button) {
					button.GetComponent<ItemInfoButton> ().deactivateButton ();
				}
			}
			
			// if the player is dead, switch off the button
			if (healthManager.playerDead) {
				if (button) {
					button.SetActive (false);
				}
			}
		}
		
	} // end Update
}
