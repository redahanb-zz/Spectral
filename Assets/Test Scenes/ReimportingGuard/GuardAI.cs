/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// Guard AI: decision making / state machine
/// </summary>

using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {

	// public variables
	public 		bool 			patrolling;
	public 		bool 			alerted;
	public 		bool			aggro;
	public 		bool 			curious;
	public 		Vector3 		lastSighting;

	// cache references
	private 	GuardSensing 	sensing;
	private 	GuardBehaviour 	behaviour;
	private 	HealthManager	pHealth;
	public 		AlertManager 	alertSystem;
	
	void Start () 
	{
		sensing 		= 		GetComponent<GuardSensing> ();
		behaviour 		= 		GetComponent<GuardBehaviour> ();
		alerted 		= 		false;
		alertSystem 	= 		GameObject.Find ("Alert System").GetComponent<AlertManager>();
		pHealth 		= 		GameObject.Find ("Health Manager").GetComponent<HealthManager> ();

		// Set default state: patrol or sentry (stationary)
		if (patrolling) 
		{
			behaviour.guardState = GuardBehaviour.GuardState.Patrol;
		} else {
			behaviour.guardState = GuardBehaviour.GuardState.Sentry;
		}

	} // end Start
	

	void Update () 
	{
		// if the guard sees the player, trigger the alert and aggro the guard
		if(sensing.playerDetected)
		{
			alerted = true;
			aggro = true;
			alertSystem.TriggerAlert();
			sensing.playerDetected = false;
			curious = false;
			sensing.playerHeard = false;
		}

		// if the guard is not alerted and hears the player, set curious state
		if (sensing.playerHeard && !alerted) {
			curious = true;
			sensing.playerHeard = false;
		}

		// if the alert system is active, go into alert patrol
		if (alertSystem.alertActive) {
			alerted = true;
			curious = false;
		} else {
			alerted = false;
			sensing.playerDetected = false;
			aggro = false;
		}

		/// State machine
		if (alerted) {
			if (sensing.playerInSight && sensing.freeShot) {
				behaviour.guardState = GuardBehaviour.GuardState.Attack;
			} else {
				if (aggro) {
					behaviour.guardState = GuardBehaviour.GuardState.Search;
				} else {
					behaviour.guardState = GuardBehaviour.GuardState.AlertPatrol;
				}
			}
		} else {
			if(patrolling)
			{
				behaviour.guardState = GuardBehaviour.GuardState.Patrol;
			} else {
				behaviour.guardState = GuardBehaviour.GuardState.Sentry;
			}
		}

		// if there is not alert but the guard hears a noise, investigate the location of the noise
		if(curious && !alerted && !aggro){
			behaviour.guardState = GuardBehaviour.GuardState.Investigate;
		}

		// switch to idle state if the player is killed
		if(pHealth.playerDead){
			behaviour.guardState = GuardBehaviour.GuardState.Idle;
		}

	} // end Update
	
}
