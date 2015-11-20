using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	public bool canTeleport = true, scaleUp = false, scaleDown = false;
	Transform pairedTeleporter, player;
	float teleportDuration = 1, scaleDuration = 0.2f, tick;
	float playerDistance = 100;
	GameObject teleportButtonObject;

	PauseManager pManager;

	PlayerController pController;

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

			teleportButtonObject.GetComponent<TeleportButton>().SetCurrentTeleporter(gameObject);
		}

		tick = Time.deltaTime/scaleDuration;
		//if(Input.GetKeyDown(KeyCode.T))if(canTeleport)Teleport();
		if(scaleUp)ScalePlayerUp();
		if(scaleDown)ScalePlayerDown();
	}

	void ScalePlayerUp(){
		player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, Time.deltaTime);
		player.localScale = Vector3.Lerp(player.localScale, new Vector3(0.001f, 0.001f, 0.001f), tick);
	}

	void TeleportPlayer(){

		player.gameObject.SetActive(false);
		player.position = new Vector3(pairedTeleporter.position.x,pairedTeleporter.position.y + 1,pairedTeleporter.position.z);
		player.gameObject.SetActive(true);
	}

	void DisablePlayerMovement(){
		pController.canMove = false;
	}

	void EnablePlayerMovement(){
		pController.canMove = true;
	}

	void ScalePlayerDown(){
		player.localScale = Vector3.Lerp(player.localScale, new Vector3(1, 1, 1), tick);
	}

	public void Teleport(){
		if(!pController.isBlending){
			//print("Teleporting to " +pairedTeleporter);
			canTeleport = false;
			if(pController.canMove)DisablePlayerMovement();
			Invoke("EnablePlayerMovement", 1.5f);

			//teleportButtonObject.GetComponent<TeleportButton>().SetCurrentTeleporter(null);
			Invoke("ToggleScaleUp", 	0.2f);
			Invoke("ToggleScaleUp", 	1f);
			Invoke("TeleportPlayer", 	1f);
			Invoke("ToggleScaleDown", 	1.2f);
			Invoke("ToggleScaleDown", 	2f);
			canTeleport = true;
		}
	}

	void ToggleScaleUp(){scaleUp 		= !scaleUp;}
	void ToggleScaleDown(){scaleDown 	= !scaleDown;}

}
