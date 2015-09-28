using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavMeshPatrolv3 : MonoBehaviour {

	public GameObject point1;
	public GameObject point2;
	public GameObject point3;
	public GameObject A_point1;
	public GameObject A_point2;
	public GameObject A_point3;

	//public GameObject speechbubble;

	public float pauseTime = 2;
	
	private Vector3[] patrolRoute;
	private Vector3[] alertRoute;
	private int nextIndex;
	private int alertIndex;
	private float curTime = 0;
	private bool walking = false;
	private bool paused = false;

	private NavMeshAgent navMesh;
	private EnemySight vision;
	private GuardAI alertAI;
	private Animator anim;

	// Use this for initialization
	void Start () {
		vision = gameObject.GetComponent<EnemySight> ();
		anim = GetComponent<Animator> ();
		alertAI = GetComponent<GuardAI> ();

		nextIndex = 0;
		alertIndex = 0;

		// Compile array of waypoints for guard to patrol
		patrolRoute = new Vector3[4]
		{
			point1.transform.position, 
			point2.transform.position, 
			point3.transform.position, 
			point2.transform.position
		};

		// Ditto, for the extra points to patrols when alerted
		alertRoute = new Vector3[6]{
			point1.transform.position,
			point2.transform.position,
			point3.transform.position,
			A_point1.transform.position,
			A_point2.transform.position,
			A_point3.transform.position
		};

		navMesh = gameObject.GetComponent<NavMeshAgent>();

		//Patrol ();
		//nextPatrolPoint ();
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
		if (vision.alerted == false){ // when guard cannot see the player, either patrol the standard route or the alerted route
			if(alertAI.alertSystem.GetComponent<AlertManager>().alertActive == true){ // patrol extended route if the system is alerted
				//print ("Alert patrolling");
				AlertPatrol();
				navMesh.SetDestination (alertRoute [alertIndex]);
				if(!paused){
					anim.SetFloat ("Speed", 1.5f);
				}
			}
			else{
				//print ("Standard patrol");
				Patrol();
				navMesh.SetDestination(patrolRoute[nextIndex]);
				if(!paused){
					anim.SetFloat("Speed", 1.0f);
				}
			}
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
		if(Vector3.Distance(transform.position, patrolRoute[nextIndex]) <= 0.5f ){
			if (curTime == 0){
				curTime = Time.time;
				anim.SetFloat ("Speed", 0.0f);
				paused = true;
			}
			if((Time.time - curTime) >= pauseTime){
				paused = false;
				nextPatrolPoint();
				curTime = 0;
			}

			// testing alternate implementation using Invoke
//			anim.SetFloat("Speed", 0.0f);
//			walking = false;
//			Invoke("nextPatrolPoint", 1.5f);
		}
	}

	void AlertPatrol() {
		if(Vector3.Distance(transform.position, alertRoute[alertIndex]) <= 0.5f ){

			if (curTime == 0){
				curTime = Time.time;
				anim.SetFloat ("Speed", 0.0f);
				paused = true;
			}
			if((Time.time - curTime) >= pauseTime*0.75f){ // shorter pause time when on alert
				paused = false;
				nextAlertPoint();
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

	public void nextAlertPoint() {
		// function to set the guard on alert patrol, at faster walking speed
		navMesh.Resume ();
		if (alertIndex == 5) {
			alertIndex = 0;
		} 
		else {
			alertIndex += 1;
		}
		navMesh.SetDestination (alertRoute [alertIndex]);
		anim.SetFloat ("Speed", 1.5f);
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
