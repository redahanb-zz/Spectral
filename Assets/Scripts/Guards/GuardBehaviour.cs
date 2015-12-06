/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// Guard behaviour: actions and abilities, handles animations & IK
/// </summary>

using UnityEngine;
using System.Collections;

public class GuardBehaviour : MonoBehaviour {

	// public variables
	public 		GameObject 			sentryPoint;
	public 		GameObject 			waypoint1;
	public 		GameObject 			waypoint2;
	public 		GameObject 			waypoint3;
	public 		GameObject 			alertWaypoint1;
	public 		GameObject 			alertWaypoint2;
	public 		GameObject 			alertWaypoint3;
	public 		float 				waitTime;
	public 		float 				waitCount;

	// private variables
	private 	bool 				walking;
	private 	Vector3[] 			patrolRoute;
	private 	int 				patrolIndex = 0;
	private 	Vector3[] 			alertRoute;
	private 	int 				alertIndex = 0;

	private 	float 				listenCount; // timer for Investigate behaviour
	private 	Quaternion 			lookRotation;
	private 	Vector3 			lastPlayerSighting;
	private 	Color				transpColor;

	public enum GuardState 
	{
		Idle,						// when player is dead
		Sentry,						// guard a single point
		Patrol,						// patrol a set of three points, pausing at each
		AlertPatrol,				// patrol a set of 3 different point, covering more area, moving faster 
		Attack,						// when player is in sight and not hidden
		Search,						// look for player when alerted but player is not in sight
		Investigate					// search the origin of a noise made by the player
	}
	public 		GuardState 			guardState;

	// script references
	private 	GameObject 			player;
	private 	NavMeshAgent 		navMeshAgent;
	private 	GuardSensing 		guardSensing;
	private 	GuardAI				guardAI;
	private 	Shooting 			guardShooting;
	private 	Animator			anim;
	private 	HealthManager		pHealth;
	private 	GuardSelfDestruct 	guardBodyParts;
	private 	GameObject 			visionCone;
	private 	GameObject 			hearingRing;


	void Start () 
	{
		player 			= 		GameObject.FindWithTag ("Player");
		navMeshAgent 	= 		GetComponent<NavMeshAgent> ();
		guardSensing 	= 		GetComponent<GuardSensing> ();
		guardAI 		= 		GetComponent<GuardAI> ();
		guardShooting 	= 		GetComponent<Shooting> ();
		anim 			= 		GetComponent<Animator> ();
		pHealth 		= 		GameObject.Find("Health Manager").GetComponent<HealthManager> ();
		guardBodyParts 	= 		GetComponent<GuardSelfDestruct> ();
		visionCone 		= 		transform.Find ("VisionCone").gameObject;
		hearingRing 	= 		transform.Find ("HearingRing").gameObject;

		// compile patrol and alert routes
		patrolRoute = new Vector3[4]
		{
			waypoint1.transform.position, 
			waypoint2.transform.position, 
			waypoint3.transform.position, 
			waypoint2.transform.position
		};
		alertRoute = new Vector3[4]{
			alertWaypoint1.transform.position,
			alertWaypoint2.transform.position,
			alertWaypoint3.transform.position,
			alertWaypoint2.transform.position
		};

	} // end Start

	void Update () 
	{
		// set walking boolean for OnAnimatorMove function
		if (navMeshAgent.hasPath) {
			walking = true;
		} else {
			walking = false;
		}

		// State behaviours
		switch(guardState)
		{
			case GuardState.Idle:
				Idle ();
				break;
			case GuardState.Sentry:
				Sentry();
				break;
			case GuardState.Patrol:
				Patrol ();
				break;
			case GuardState.AlertPatrol:
				AlertPatrol();
				break;
			case GuardState.Search:
				Search ();
				break;
			case GuardState.Investigate:
				Investigate();
				break;
			case GuardState.Attack:
				Attack();
				break;
		}

	} // end Update

	void OnAnimatorMove()
	// Function to sync the walking animation root motion with the NavMesh Agent
	{
		if (walking)
		{
			// Set NavMesh velocity to the root motion of the current frame of the current animation clip
			GetComponent<NavMeshAgent>().velocity = anim.deltaPosition / Time.deltaTime;

			// turn to face the intended direction based on the turn rate of the NavMeshAgent
			if(navMeshAgent.desiredVelocity != Vector3.zero) 
			{
				lookRotation = Quaternion.LookRotation (navMeshAgent.desiredVelocity);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation, navMeshAgent.angularSpeed * Time.deltaTime);
			}
		}
	}

	void Idle()
	// function to deactivate the guard, for example when the player is dead
	{
		if (Vector3.Distance (transform.position, sentryPoint.transform.position) > 0.5f) {
			// if the guard is away from its sentry point, walk there
			navMeshAgent.Resume();
			navMeshAgent.SetDestination (sentryPoint.transform.position);
			anim.SetFloat("Speed", 1.0f);
		} 
		else 
		{
			// if the guard is at the sentry point, stop moving
			navMeshAgent.Stop ();
			anim.SetFloat("Speed", 0.0f);
			guardSensing.enabled = false;
		}
		// set colour to blur to indicate idle mode
		updateColour (Color.blue);
	} // end Idle


	void Sentry()
	// function to set the guard stationary at a specific point, watching for the player
	{
		if (Vector3.Distance (transform.position, sentryPoint.transform.position) > 0.5f) {
			// if the guard is away from its sentry position, return to the sentry position
			navMeshAgent.Resume();
			navMeshAgent.SetDestination (sentryPoint.transform.position);
			anim.SetFloat("Speed", 1.0f);
		} else {
			// stop moving when at the sentry point
			navMeshAgent.Stop ();
			anim.SetFloat("Speed", 0.0f);
		}
		// set colour to grey to indicate neutral patrol
		updateColour (Color.gray);
	} // end Sentry


	void Patrol()
	// patrol routine for the guard, cycles through waypoints pause briefly at each one
	{
		navMeshAgent.Resume();

		if (Vector3.Distance (transform.position, patrolRoute [patrolIndex]) >= 0.5f) {
			// if guard is not at next waypoint, set destination to the waypoint
			navMeshAgent.SetDestination (patrolRoute [patrolIndex]);
			// set animation to walking speed
			anim.SetFloat ("Speed", 1.0f);
		}
		else {
			// stop at waypoint for the specified time, then set the next patrol point
			if(waitCount <= waitTime){
				anim.SetFloat("Speed", 0.0f);
				waitCount += Time.deltaTime;

			} else{
				nextPatrolPoint();
				waitCount = 0.0f;
			}
		}
		// set colour to grey to indicate neutral patrol
		updateColour (Color.gray);
	} // end Patrol


	void nextPatrolPoint()
	// support function for the Patrol routine, updates the guard's target waypoint
	{
		navMeshAgent.Resume();

		if (patrolIndex == patrolRoute.Length-1) {
			patrolIndex = 0;
		} else {
			patrolIndex++;
		}
		// set destination to next patrol point, at walking speed animation
		navMeshAgent.SetDestination (patrolRoute [patrolIndex]);
		anim.SetFloat("Speed", 1.0f);

	} // end nextPatrolPoint


	void AlertPatrol()
	// secondary patrol routine for the guard, patrols at a faster pace to a different array of waypoints
	{
		// lower weapon, start moving
		anim.SetBool ("InSight", false);
		navMeshAgent.Resume();
		
		if (Vector3.Distance (transform.position, alertRoute [alertIndex]) >= 0.5f) {
			// if out of range or waypoint, set destination there at 150% walk speed
			navMeshAgent.SetDestination(alertRoute[alertIndex]);
			anim.SetFloat("Speed", 1.5f);
		} else {
			// if in range of waypoint, stop moving and start a timer
			if(waitCount <= waitTime){
				anim.SetFloat("Speed", 0.0f);
				waitCount += Time.deltaTime;
			} else{
				// when timer expires, move on
				nextAlertPoint();
				waitCount = 0.0f;
			}
		}
		// set colour to red to indicate alert patrol
		updateColour (Color.red);
	} // end AlertPatrol


	void nextAlertPoint()
	// support function for the AlertPatrol routine, updates the guard's target waypoint
	{
		navMeshAgent.Resume();
		
		if (alertIndex == alertRoute.Length-1) {
			alertIndex = 0;
		} else {
			alertIndex++;
		}
		// set destination to next alert point, at fast walking speed animation
		navMeshAgent.SetDestination (alertRoute [alertIndex]);
		anim.SetFloat("Speed", 1.5f);
		
	} // end nextPatrolPoint


	void Search()
	// function for when the guards loses sight of the player, sends the guard to the target location after the player
	{
		// set search location
		lastPlayerSighting = guardSensing.lastPlayerSighting;
		if (Vector3.Distance (transform.position, lastPlayerSighting) > 2.0f) {
			// if out of range of search location, run towards location
			navMeshAgent.Resume ();
			navMeshAgent.SetDestination (lastPlayerSighting);
			anim.SetBool ("InSight", false);
			anim.SetFloat ("Speed", 2.0f);
		} else {
			// if at the location of the search point, stop and start a timer 
			anim.SetFloat("Speed", 0.0f);
			if(waitCount == 0.0f){
				waitCount = Time.time;
			}
			if(Time.time - waitCount >= 3.0f){
				// if time reaches +3 seconds, de-aggro guard, reset timer
				guardAI.aggro = false;
				waitCount = 0.0f;
			}
		}
		// update colour to red to indicate hostility
		updateColour (Color.red);
	} // end Search


	void Investigate()
	// function for when the guard hears a noise, sends the guard to investigate the sources of the noise
	{
		lastPlayerSighting = guardSensing.investigationLocation;
		// pause the guard's motion, and start a quick timer
		if (listenCount == 0.0f) {
			listenCount = Time.time;
			anim.SetFloat("Speed", 0.0f);
		}
		// once a 1 second pause has elapsed, start the guard investigating the noise
		if(Time.time - listenCount >= 1.0f){
			if (Vector3.Distance (transform.position, lastPlayerSighting) > 1.0f) {
				// if guard is more than 1m from source of noise, head towards the noise
				navMeshAgent.Resume ();
				navMeshAgent.SetDestination (lastPlayerSighting);
				anim.SetFloat ("Speed", 1.0f);
			} else {
				// pause at location of noise when the guard reaches it, start a timer
				anim.SetFloat("Speed", 0.0f);
				if(waitCount == 0.0f){
					waitCount = Time.time;
				}
				if(Time.time - waitCount >= 4.0f){
					// once the wait timer has elasped, reset the timers and set the curious parameter to false
					// the guardAI script will handle the behaviour change based on the false 'curious' parameter
					guardAI.curious = false;
					waitCount = 0.0f;
					listenCount = 0.0f;
				}
			}
		}
		updateColour (Color.yellow);
	} // end Investigate


	void Attack()
	// function to make the guard stop and draw its weapon on the player, and fire at intervals
	{
		// turn the guard to face the player
		lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation, navMeshAgent.angularSpeed * Time.deltaTime);
		// play the 'weapon up' animation
		anim.SetBool("InSight", true);

		if (Vector3.Distance (transform.position, player.transform.position) > 5.0f) {
			// if player is out of range, lower weapon and run closer
			navMeshAgent.Resume();
			navMeshAgent.SetDestination(player.transform.position);
			anim.SetBool("InSight", false);
			anim.SetFloat("Speed", 2.0f);
		} else {
			// if in range, stop running and fire at player
			navMeshAgent.Stop();
			anim.SetFloat("Speed", 0.0f);
			walking = false;
			if(!pHealth.playerDead){
				guardShooting.Shoot ();
			} else {
				anim.SetBool("InSight", false);
			}
		}
		// update colour to red to indicate hostile status
		updateColour (Color.red);
	} // end Attack


	// function to change the guard body parts and sense visualisation to indicate its alert status
	void updateColour(Color targetColor){
		// set target colour, set target colour alpha
		transpColor = targetColor;
		transpColor.a = visionCone.GetComponent<Renderer> ().material.color.a;
		// update senses visualisation colour
		visionCone.GetComponent<Renderer>().material.color = Color.Lerp(visionCone.GetComponent<Renderer>().material.color,transpColor, Time.deltaTime * 5.0f);
		hearingRing.GetComponent<Renderer>().material.color = Color.Lerp(hearingRing.GetComponent<Renderer>().material.color,transpColor, Time.deltaTime * 5.0f);
		// loop through all coloured bodyparts and update their colour
		foreach(GameObject bodypart in guardBodyParts.colorParts){
			bodypart.GetComponent<Renderer>().materials[0].color = Color.Lerp(bodypart.GetComponent<Renderer>().materials[0].color,targetColor, Time.deltaTime * 5.0f);
			if(bodypart.GetComponent<Renderer>().materials.Length > 1){
				bodypart.GetComponent<Renderer>().materials[1].color = Color.Lerp(bodypart.GetComponent<Renderer>().materials[1].color,targetColor, Time.deltaTime * 5.0f);
			}
		}
	} // end UpdateColour
}
