//Name:			Teleporter.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	This script manages the behaviour between a teleporter pair, choosing to teleport the player based on which
//				Teleporter object the player is standing beside. It scales down and moves the player on a new Teleport.

using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	public bool 				canTeleport = true, 	//determines if the player can teleport
								scaleUp = false, 		//scales the player to normal size if true
								scaleDown = false;		//scales the player down until invisible if false
	private Transform 			pairedTeleporter, 		//transform of paired teleporter object
								player;					//transform of the player
	private float 				teleportDuration = 1,   //how long player is inactive during teleport
								scaleDuration = 0.2f,   //duration for scaling the player
								tick;					//rate used to scale the player
	private float 				playerDistance = 100;	//distance player is from teleporter
	private GameObject 			teleportButtonObject;	//teleport button gameobject

	private PauseManager 		pManager;				//instance of pause manager

	private PlayerController 	pController;			//instance of player controller

	private bool 				destroyOnce = false;	//used to destroy one instance of the teleport destination indicator

	private GameObject 			indicatorObject;		//gameobject used to indicate teleport destination

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		teleportButtonObject = GameObject.Find("Teleport Button");
		foreach(Transform t in transform.parent){
			if(t != transform)pairedTeleporter = t;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!pController)pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		playerDistance = Vector3.Distance(player.position, transform.position);
		//if(name == "Teleporter 1")print(playerDistance);

		if(playerDistance < 2){
			if(!GameObject.Find("Teleporter Destination Indicator")){
				destroyOnce = false;
				indicatorObject = Instantiate(Resources.Load("Teleport Destination Indicator"), pairedTeleporter.transform.position + new Vector3(0,0.2f,0), Quaternion.identity) as GameObject;
				indicatorObject.name = "Teleporter Destination Indicator";
			}
			teleportButtonObject.GetComponent<TeleportButton>().SetCurrentTeleporter(gameObject);
		}
		else{
			if(!destroyOnce){
				Destroy(indicatorObject);
				destroyOnce = true;
			}
		}

		tick = Time.deltaTime/scaleDuration;
		if(scaleUp)ScalePlayerUp();
		if(scaleDown)ScalePlayerDown();
	}

	//Function that scales the player up until visible
	private void ScalePlayerUp(){
		player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, Time.deltaTime);
		player.localScale = Vector3.Lerp(player.localScale, new Vector3(0.001f, 0.001f, 0.001f), tick);
	}

	//Repositions the player during teleport
	private void TeleportPlayer(){
		Destroy(indicatorObject);

		player.gameObject.SetActive(false);
		player.position = new Vector3(pairedTeleporter.position.x,pairedTeleporter.position.y + 1,pairedTeleporter.position.z);
		player.gameObject.SetActive(true);
	}

	//Disables player controls
	private void DisablePlayerMovement(){
		pController.canMove = false;
	}

	//Enables player controls
	private void EnablePlayerMovement(){
		pController.canMove = true;
	}

	//Function that scales the player down until invisible
	private void ScalePlayerDown(){
		player.localScale = Vector3.Lerp(player.localScale, new Vector3(1, 1, 1), tick);
	}

	//Function thatis a series of invokes that times the teleport behaviour
	public void Teleport(){
		if(!pController.isBlending){
			canTeleport = false;
			if(pController.canMove)DisablePlayerMovement();
			Invoke("EnablePlayerMovement", 1.5f);
			Invoke("ToggleScaleUp", 	0.2f);
			Invoke("ToggleScaleUp", 	1f);
			Invoke("TeleportPlayer", 	1f);
			Invoke("ToggleScaleDown", 	1.2f);
			Invoke("ToggleScaleDown", 	2f);
			canTeleport = true;
		}
	}

	//Toggles scale up
	public void ToggleScaleUp(){scaleUp 		= !scaleUp;}

	//Toggles scale down
	public void ToggleScaleDown(){scaleDown 	= !scaleDown;}

}
