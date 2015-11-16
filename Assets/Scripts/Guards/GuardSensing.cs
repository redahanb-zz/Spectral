/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// Guard senses: vision and hearing
/// </summary>

using UnityEngine;
using System.Collections;

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

	// private variables
	private float 		distanceToPlayer;
	private float 		angleToPlayer;
	private Vector3 	directionToPlayer;
	private RaycastHit 	rayHit;
	private float 		timeInSight;
	
	// script references
	GameObject 			player;
	PlayerController	playerController;
	GuardAI 			guardAI;
	GuardBehaviour		gBehaviour;
	
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		guardAI = GetComponent<GuardAI> ();
		gBehaviour = GetComponent<GuardBehaviour> ();

	} // end Start
	
	
	void Update () 
	{
		CheckSight ();
		CheckHearing ();

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
        //print("V Sight " + (player.transform.position.y - transform.position.y));
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
							print ("Player is VISIBLE!!!");
							playerInSight = true;
							lastPlayerSighting = player.transform.position;
						}
					}
				}
			}
		}

	} // end CheckSight

	void CheckHearing()
	{
		// update data on player
		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);

		if (distanceToPlayer <= hearingRange) {
			//print ("Player in hearing range!");
			player.GetComponent<FootstepFX> ().enemyInRange = true;
		} else {
			//print ("Player outside hearing range!");
			player.GetComponent<FootstepFX> ().enemyInRange = false;
		}

		if(!soundProofed){
			if(!playerInSight)
			{
				if(distanceToPlayer < hearingRange && (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.05) && playerController.currentMoveState == PlayerController.MoveState.Run && !guardAI.curious)
				{
                    //print("V Hearing " + (player.transform.position.y - transform.position.y));
                    investigationLocation = player.transform.position;
					//GameObject.Find("Lastheardlocation").transform.position = investigationLocation;
					playerHeard = true;
					gBehaviour.waitCount = 0.0f;

				} 
//				else if(distanceToPlayer < hearingRange*0.25 && (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.05) && playerController.currentMoveState == PlayerController.MoveState.Sneak)
//				{
//					investigationLocation = player.transform.position;
//					playerHeard = true;
//					gBehaviour.waitCount = 0.0f;
//				}
			}
		}

	} // end CheckHearing
}
