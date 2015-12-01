/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// Guard senses: vision and hearing
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardSensing : MonoBehaviour {

	// public variables
	public float 		sightRange;
	public float		hearingRange;
	public float		fieldOfView;
	public Vector3		lastPlayerSighting;
	public Vector3 		investigationLocation;
	public bool 		playerInSight;
	public bool 		playerDetected;
	public bool 		playerHeard;
	public bool 		soundProofed;
	public AudioClip 	guardAlertRoar;

	// private variables
	private float 		distanceToPlayer;
	private float 		angleToPlayer;
	private Vector3 	directionToPlayer;
	private RaycastHit 	rayHit;
	private float 		timeInSight;
	private bool 		canRoar = true;
	private bool		footstepInRange;
	
	// script references
	GameObject 			player;
	PlayerController	playerController;
	GuardAI 			guardAI;
	GuardBehaviour		gBehaviour;
	TimeScaler 			tScaler;

	public GameObject[] footsteps;
	Footfall 			tempFootstep;
	//List<GameObject> 	footsteps;
	
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		guardAI = GetComponent<GuardAI> ();
		gBehaviour = GetComponent<GuardBehaviour> ();
		tScaler = GameObject.Find ("Time Manager").GetComponent<TimeScaler> ();
		if (tScaler.GetNoiseDampening ()) 
		{
			hearingRange = 2.5f;
		}

	} // end Start
	
	
	void Update () 
	{
		CheckSight ();
		//CheckHearing ();
		CheckHearing2 ();

		if (playerInSight) {
			if (timeInSight <= 0.3f) {
				timeInSight += Time.deltaTime;
			} else {
				playerDetected = true;
			}
		} else {
			timeInSight = 0.0f;
		}

	} // end Update

	void CheckSight()
	{
		// update data on player
		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);
		directionToPlayer = player.transform.position - transform.position;
		angleToPlayer = Vector3.Angle (directionToPlayer, transform.forward);
        playerInSight = false;

		if(distanceToPlayer < sightRange && (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.5))
		{
			if(angleToPlayer < fieldOfView)
			{
				if(Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out rayHit, sightRange))
				{
					if(rayHit.transform.tag == "Player"){
						if(playerController.isVisible)
						{
							playerInSight = true;
							lastPlayerSighting = player.transform.position;
							if(canRoar)
							{
								AudioSource.PlayClipAtPoint(guardAlertRoar, Camera.main.transform.position);
								canRoar = false;
								Invoke("ResetCanRoar", 5.0f);
							}
						}
					}
				}
			}
		}
	} // end CheckSight

//	void CheckHearing()
//	{
//		// update data on player
//		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);
//
//		if (distanceToPlayer <= hearingRange) {
//			//print ("Player in hearing range!");
//			player.GetComponent<FootstepFX> ().enemyInRange = true;
//		} else {
//			//print ("Player outside hearing range!");
//			player.GetComponent<FootstepFX> ().enemyInRange = false;
//		}
//
//		if(!soundProofed){
//			if(!playerInSight)
//			{
//				if(distanceToPlayer < hearingRange && (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.05) && playerController.currentMoveState == PlayerController.MoveState.Run && !guardAI.curious)
//				{
//                    investigationLocation = player.transform.position;
//					//GameObject.Find("Lastheardlocation").transform.position = investigationLocation;
//					playerHeard = true;
//					gBehaviour.waitCount = 0.0f;
//
//				} 
//				else if(distanceToPlayer < hearingRange*0.25 && (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.05))
//				{
//					investigationLocation = player.transform.position;
//					playerHeard = true;
//					gBehaviour.waitCount = 0.0f;
//				}
//			}
//		}
//
//	} // end CheckHearing

	void CheckHearing2()
	{
		// Update distance data on player
		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);

		// Play footstep visualisation if player is in range
		if (distanceToPlayer <= hearingRange) 
		{
			player.GetComponent<FootstepFX> ().enemyInRange = true;
			print ("Player in hearing range!");
		} 
		else 
		{
			player.GetComponent<FootstepFX> ().enemyInRange = false;
			print ("Player outside hearing range!");
		}

		// Record all footsteps in range, use position of most recent one as position to investigate
		footsteps = GameObject.FindGameObjectsWithTag ("Footstep");
		float spawnOrder = 0.0f;
		footstepInRange = false;

		// Loop through all footsteps in the scene 
		foreach (GameObject echo in footsteps) {
			tempFootstep = echo.GetComponent<Footfall>();

			if(Vector3.Distance(echo.transform.position, transform.position) <= hearingRange && (Mathf.Abs(transform.position.y - tempFootstep.transform.position.y) < 0.5f) )
			{	
				// Find most recently instantiated footstep noise, set that as the location to investigate
				if(spawnOrder == 0.0f)
				{
					spawnOrder = tempFootstep.startTime;
					footstepInRange = true;
				}
				else if(tempFootstep.startTime > spawnOrder)
				{
					spawnOrder = tempFootstep.startTime;
					investigationLocation = echo.transform.position;
				}
			}
		}

		// Record the player as 'heard' if in range and not in sight, and guard is not in a soundproofed room
		if(!soundProofed)
		{
			if(!playerInSight)
			{
				if(footstepInRange && (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.05f))
				{
					print ("Playerheard!!");
					playerHeard = true;
					gBehaviour.waitCount = 0.0f;
				} 
			}
		}
	}

//	void OnTriggerStay()
//	{
//		footsteps = GameObject.FindGameObjectsWithTag ("Footstep");
//		float spawnOrder = 0.0f;
//		foreach (GameObject echo in footsteps) {
//			tempFootstep = echo.GetComponent<Footfall>();
//			if(spawnOrder == 0.0f){
//				spawnOrder = tempFootstep.startTime;
//			}
//			else if(tempFootstep.startTime > spawnOrder){
//				spawnOrder = tempFootstep.startTime;
//				investigationLocation = echo.transform.position;
//			}
//		}
//
//	}

	void ResetCanRoar()
	{
		canRoar = true;
	}
}
