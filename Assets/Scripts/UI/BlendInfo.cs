using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlendInfo : MonoBehaviour {

	GameObject button;
	GameObject canvasObject;
	GameObject player;
	PlayerController pController;
	Renderer rend;
	HealthManager pHealth;

	Color playerColor;
	Color wallColor;
	
	//InventoryItem inventoryItem;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		pController = player.GetComponent<PlayerController> ();
		canvasObject = GameObject.Find ("Canvas");
		//inventoryItem = GetComponent<InventoryItem> ();
		rend = GetComponent<Renderer> ();
		pHealth = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//print ("Wall: " + rend.material.color);
		wallColor = rend.material.color;
		//print ("Player: " + pController.targetcolor);
		playerColor = pController.targetcolor;
		float colorDistance = Vector3.Distance( new Vector3(wallColor.r, wallColor.b, wallColor.g), new Vector3(playerColor.r,playerColor.b,playerColor.g));
		if(Vector3.Distance(transform.position, player.transform.position) < 3.0f)
		{
			if(colorDistance < 0.1f)
			{
			// check angle to player to stop the button appearing through walls
				Vector3 direction = player.transform.position - transform.position;
				float angle = Vector3.Angle(direction, -transform.forward); 
				if(angle <= 90.0f){
					if(!button && !pHealth.playerDead){
						button = Instantiate(Resources.Load("UI/Worldspace Buttons/Blend Info"), canvasObject.transform.position, Quaternion.identity) as GameObject;
						button.transform.SetParent(canvasObject.transform);
						button.GetComponent<BlendInfoButton>().setTarget(gameObject);
						button.GetComponent<Button>().onClick.AddListener( GetComponent<BlendInfo>().callPlayertoBlend );
					} else if(!pHealth.playerDead && pController.isVisible) 
					{
						button.SetActive(true);
						if(button.GetComponent<Button>().interactable == false){
							button.GetComponent<Button>().interactable = true;
						}
					}
				} // end check player angle
			}
		
		} // end check player in range

		if (Vector3.Distance (transform.position, player.transform.position) > 10.0f || colorDistance > 0.1f) {
			if(button){
				button.SetActive(false);
			}
		} else if (Vector3.Distance (transform.position, player.transform.position) > 4.0f || colorDistance > 0.1f){
			if(button){
				button.GetComponent<BlendInfoButton>().deactivateButton();
			}
		}

	}
		
	void callPlayertoBlend(){
		player.GetComponent<PlayerController> ().blendButton (this.gameObject, this.transform, transform.position);
	}
	
	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			button.GetComponent<Button>().interactable = false;
		}
	}
}
