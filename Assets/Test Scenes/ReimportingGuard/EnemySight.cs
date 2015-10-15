using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	public float fieldOfView = 90f;
	public bool playerInSight;

	public bool patrolling;
	public bool alerted;

	public Vector3 lastPlayerSighting;
	
	private SphereCollider coll;
	private GameObject player;
	private PlayerController playerController;
	private AudioSource guardCry;

	private bool canCry = true;

	private Vector3 direction;
	private float angle;
	private RaycastHit hit;

	public int visionCount;

	// Guard vision and hearing
//	void OnTriggerStay (Collider other)
//	{
//		// filter out objects not tagged as 'player'
//		if (other.gameObject.tag == "Player") 
//		{
//			//set playr in sight to false, so it will only stay as treu while the player is actually in sight
//			playerInSight = false;
//			// calculate angle between player position and guard position, to calculate field of view.
//			Vector3 direction = other.transform.position - transform.position;
//			float angle = Vector3.Angle (direction, transform.forward);
//
//			// check that the player is within +/- 45 degrees of the enemy's forward vector
//			if (angle < 0.5f * fieldOfView) 
//			{
//				RaycastHit hit;
//				// IF the player is in range, and IF the player is in front of the enemy, cast a ray to check line of sight
//				if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, coll.radius)) 
//				{
//					// if the raycast hits the player, not an obstruction
//					print("Raycast hit: " + hit.collider.gameObject.name);
//					if (hit.collider.gameObject.tag == "Player") 
//					{
//						if(pcc.isBlending){
//							if (hit.transform.GetComponent<ColourComparison>().camoIndex < 80){
//								playerInSight = true;
//								lastPlayerSighting = GameObject.FindWithTag("Player").transform.position;
//							}
//							else{
//
//							}
//						}
//						else{
//							playerInSight = true;
//							lastPlayerSighting = GameObject.FindWithTag("Player").transform.position;
//						}
//
//					}
//						// if the player is camoflaged sufficiently (better than 80%)
//						//print (hit.transform.GetComponent<ColourComparison>().camoIndex);
//						//if (hit.transform.GetComponent<ColourComparison>().camoIndex < 80){
//					//
//					//			playerInSight = true;
// 					//			lastPlayerSighting = GameObject.FindWithTag("Player").transform.position;
//					//		}
//					//	}
//					//}
//				}
//			}
//
//			// If player is running withing the guard's sphere of hearing
//			if(other.gameObject.GetComponent<PlayerMoveWithAnimation>().isRunning){
//				// Investigate the noise at the location the noise came from
//				lastPlayerSighting = other.transform.position;
//				GetComponent<NavMeshPatrolv2>().Investigate(lastPlayerSighting);
//			} // If the player is moving too close to the guard
////			else if (Vector3.Distance(transform.position, other.transform.position) < 3.0f && !other.transform.GetComponent<PlayerMoveWithAnimation>().stationary){
////				lastPlayerSighting = other.transform.position;
////				GetComponent<NavMeshPatrolv2>().Investigate(lastPlayerSighting);
////			}
//
//
//		} 
//	}

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
		//playerController = player.GetComponent<PlayerController> ();
		patrolling = true;
		alerted = false;

		visionCount = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (!player && GameObject.FindWithTag("Player")) {
			player = GameObject.FindWithTag ("Player");
			//pcc = player.GetComponent<PlayerColorChanger> ();
		} else {
			//print( pcc.isBlending);
		}

		if(!playerController && GameObject.FindWithTag("Player")){
			playerController = player.GetComponent<PlayerController> ();
		}

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

			if (playerInSight == false && huntRange <= 2.0f ){
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
		if (Vector3.Distance (transform.position, player.transform.position) < 7.0f) 
		{
			//print ("player in range");
			playerInSight = false;

			// calculate angle between guard forward vector and player
			direction = player.transform.position - transform.position;
			angle = Vector3.Angle (direction, transform.forward); // v3.angle returns acute angle always
			
			if (angle <= 50.0f) {
				// if player is within +-50 degrees of forward vector, cast a ray to player
				//print("player in field of view");
//				RaycastHit hit;
				if (Physics.Raycast (transform.position + Vector3.up, direction, out hit, coll.radius)) {
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
		// If player is running withing the guard's sphere of hearing
//		if((Vector3.Distance(transform.position, player.transform.position) < 3.0f) && player.gameObject.GetComponent<PlayerMoveWithAnimation>().isRunning){
//			// Investigate the noise at the location the noise came from
//			lastPlayerSighting = player.transform.position;
//			GetComponent<NavMeshPatrolv2>().Investigate(lastPlayerSighting);
//		} // If the player is moving too close to the guard
//		else if (Vector3.Distance(transform.position, player.transform.position) < 1.0f && !player.transform.GetComponent<PlayerMoveWithAnimation>().stationary){
//			lastPlayerSighting = player.transform.position;
//			GetComponent<NavMeshPatrolv2>().Investigate(lastPlayerSighting);
//		}
	
	}	// end of checkSight
}
