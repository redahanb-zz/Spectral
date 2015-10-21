/// <summary>
///  Ben Redahan, redahanb@gmail.com, 2015
/// </summary>

using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	public float fieldOfView = 90f;
	public bool playerInSight;
	public float visionRange = 12.0f;

	public bool patrolling;
	public bool alerted;

	public Vector3 lastPlayerSighting;
	
	private SphereCollider coll;
	private GameObject player;
	private PlayerController playerController;
	private AudioSource guardCry;

	private bool canCry = true;
	private float timeInSight;
	private Vector3 direction;
	private float distanceToPlayer;
	private float angle;
	private RaycastHit hit;

	public int visionCount;
	
	// Public function used by alarm systems and noisemaker items to alert the guard from a distance
	public void globalAlert(Vector3 alertLocation) {
		patrolling = false;
		alerted = true;
		lastPlayerSighting = alertLocation;
	}

	public void returnPatrol() {
		alerted = false;
		patrolling = true;
	}

	// Use this for initialization
	void Start () {
		coll = GetComponent<SphereCollider> ();
		guardCry = GetComponent<AudioSource> ();
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController> ();
		patrolling = true;
		alerted = false;

		visionCount = 0;
		timeInSight = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
//		if (!player && GameObject.FindWithTag("Player")) {
//			player = GameObject.FindWithTag ("Player");
//			//pcc = player.GetComponent<PlayerColorChanger> ();
//		} else {
//			//print( pcc.isBlending);
//		}
//
//		if(!playerController && GameObject.FindWithTag("Player")){
//			playerController = player.GetComponent<PlayerController> ();
//		}

		/// TESTING ALTERNATE IMPLEMENTATION: No Trigger Collider ///
		if (player) {
			checkSight ();
		}

		if (patrolling) {	
			if (playerInSight) {
				if (visionCount >= 60) {
					patrolling = false;
					alerted = true;
					visionCount = 120;
				} else {
					visionCount += 3;
				}
			} else {
				if (visionCount > 0) {
					visionCount--;
				}
			}
		} 
		else {
			// check distance from last known sighting of player, if player is not seen in area, return to patrol
			float huntRange = Vector3.Distance(transform.position, lastPlayerSighting);

			if (playerInSight == false && huntRange <= 1.0f ){
				if (visionCount > 0) {

					visionCount--;
				} 
				else {
					GuardAI guardAI = GetComponent<GuardAI>();
					guardAI.alerted = false;
					returnPatrol();

					NavMeshPatrolv3 patrol = GetComponent<NavMeshPatrolv3>();
					patrol.nextAlertPoint();
				}
			}
		}
	}

	void resetCanCry(){
		canCry = true;
	}

	void checkSight() {
		// Performed every frame to report whether or not the player can be seen by the guard
		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);
		if (distanceToPlayer < visionRange) 
		{
			//print ("player in range");
			playerInSight = false;

			// calculate angle between guard forward vector and player
			direction = player.transform.position - transform.position;
			angle = Vector3.Angle (direction, transform.forward); // v3.angle returns acute angle always
			
			if (angle <= 50.0f) {
				// if player is within +-50 degrees of forward vector, cast a ray to player
				//print("player in field of view");
				if (Physics.Raycast (transform.position + Vector3.up, direction, out hit, visionRange)) {
					if (hit.collider.gameObject.tag == "Player") 
					{
						// if ray hits player, check player camoflage
						if(playerController.isVisible)
						{
							playerInSight = true;

							// record last sighting coordinates for chasing
							lastPlayerSighting = player.transform.position;

							// play guard cry sound
							if(canCry)
							{
								AudioSource.PlayClipAtPoint(guardCry.clip, transform.position);
								// put canCry on cooldown
								canCry = false;
								Invoke("resetCanCry", 5);
							}
						}
					}
				}
			}
		} 
		else 
		{
			playerInSight = false;
		}

		// HEARING //
		// If player is running withing the guard's sphere of hearing, the guard will hear them
//		if((distanceToPlayer < 4.0f) && playerController.currentMoveState == PlayerController.MoveState.Run){
//			// Investigate the noise at the location the noise came from
//			print ("Heard player running");
//			patrolling = false;
//			alerted = true;
//			lastPlayerSighting = player.transform.position;
//			GetComponent<NavMeshPatrolv3>().Investigate(lastPlayerSighting);
//		} // If the player is moving at any speed too close to the guard the guard will detect them
//		else if (distanceToPlayer < 2.0f && playerController.currentMoveState != PlayerController.MoveState.Blend_Stand){
//			print ("Heard player in vicinity");
//			patrolling = false;
//			alerted = true;
//			lastPlayerSighting = player.transform.position;
//			GetComponent<NavMeshPatrolv3>().Investigate(lastPlayerSighting);
//		}
	
	}	// end of checkSight
}
