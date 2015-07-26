using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavMeshPatrolv2 : MonoBehaviour {

	public GameObject point1;
	public GameObject point2;
	public GameObject point3;

	//public GameObject speechbubble;

	public float pauseTime = 1;
	
	private Vector3[] patrolRoute;
	private int nextIndex;
	private float curTime = 0;
	private bool walking = false;

	private NavMeshAgent navMesh;
	private EnemySight vision;
	private Animator anim;

	// Use this for initialization
	void Start () {
		vision = gameObject.GetComponent<EnemySight> ();
		anim = GetComponent<Animator> ();

		nextIndex = 0;

		// Compile array of waypoints for guard to patrol
		patrolRoute = new Vector3[4]
		{
			point1.transform.position, 
			point2.transform.position, 
			point3.transform.position, 
			point2.transform.position
		};

		navMesh = gameObject.GetComponent<NavMeshAgent>();

		navMesh.SetDestination (patrolRoute[nextIndex]);

		// start at walking speed
		anim.SetFloat ("Speed", 1.0f);

	}
	
	// Update is called once per frame
	void Update () {
		// set walking boolean for OnAnimatorMove function
		if (navMesh.hasPath) {
			walking = true;
		} else {
			walking = false;
		}

		//Check sight component to refresh state of playerInSight and alert status
		vision = gameObject.GetComponent<EnemySight> ();

		// Movement Decision tree
		// if not alerted, patrol
		if (vision.alerted == false){
			Patrol();
			//speechbubble.SetActive(false);
		} 
		else {
			// if alerted and player is visibile, attack
			if(vision.playerInSight){ 
				//if player is in sight, attack
				//speechbubble.SetActive(true);
				anim.SetBool("InSight", true);
				Attack ();
			}
			// if alerted and player is not visible, search last known location
			else{ 
				// if player is not in sight (but guard is alarmed), chase and search
				anim.SetBool("InSight", false);
				Search ();
			}
		}
	}

	void OnAnimatorMove(){
		if (walking)
		{
			GetComponent<NavMeshAgent>().velocity = anim.deltaPosition / Time.deltaTime;

			if(navMesh.desiredVelocity != Vector3.zero) {
				Quaternion lookRotation = Quaternion.LookRotation (navMesh.desiredVelocity);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation, navMesh.angularSpeed * Time.deltaTime);
			}
		}
	}

	void Patrol() {
		if(Vector3.Distance(transform.position, patrolRoute[nextIndex]) <= 1 ){
			if (curTime == 0){
				curTime = Time.time;
				anim.SetFloat ("Speed", 0.0f);
			}
			if((Time.time - curTime) >= pauseTime){
				nextPatrolPoint();
				curTime = 0;
			}
		}
	}

	public void nextPatrolPoint() {
		// function to set the guard back on patrol, at walking speed
		navMesh.Resume ();
		if (nextIndex == 3) {
			nextIndex = 0;
		} 
		else {
			nextIndex += 1;
		}
		navMesh.SetDestination (patrolRoute [nextIndex]);
		anim.SetFloat ("Speed", 1.0f);
	}

	void Search() {
		// if last position of player is known and player is not in sight, run to last known position
		//print ("Searching...");
		if( Vector3.Distance(transform.position, GetComponent<EnemySight>().lastPlayerSighting) > 1f ){
			navMesh.Resume();
			navMesh.SetDestination(GetComponent<EnemySight>().lastPlayerSighting);
			anim.SetFloat ("Speed", 2.0f);
		}
		else {
			navMesh.Stop();
			anim.SetFloat("Speed", 0.0f);
			walking = false;
		}
	}


	void Attack() {
		// stop moving and moving animations
		navMesh.Stop();
		anim.SetFloat("Speed", 0.0f);
		walking = false;
		GetComponent<Shooting>().Shoot();
		// call shoot function from Shooting component
//		if((GameObject.FindWithTag("Player").GetComponent<Health>().hitPoints) > 0){
//			GetComponent<Shooting>().Shoot();
//		}
	}

	public void Investigate(Vector3 searchPosition){
		// function for when guard is alerted by noisemaker device (called from other room)
		GetComponent<EnemySight> ().globalAlert(searchPosition);
		GetComponent<EnemySight> ().visionCount = 120;
		//GetComponent<EnemySight> ().lastPlayerSighting = searchPosition;
		Search ();
		anim.SetFloat ("Speed", 1.0f);
	}
	
}
