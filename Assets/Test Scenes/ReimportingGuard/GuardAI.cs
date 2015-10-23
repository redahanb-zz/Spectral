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
	public bool 			patrolling;
	public bool 			alerted;
	public bool				aggro;
	public bool 			curious;
	public Vector3 			lastSighting;

	// cache references
	//private EnemySight 		vision;  // from old version
	private GuardSensing 	sensing;
	private GuardBehaviour 	behaviour;
	private HealthManager	pHealth;
	public AlertManager 	alertSystem;
	
	void Start () 
	{
		//vision = gameObject.GetComponent<EnemySight>(); // from old version
		sensing = GetComponent<GuardSensing> ();
		behaviour = GetComponent<GuardBehaviour> ();
		alerted = false;
		alertSystem = GameObject.Find ("Alert System").GetComponent<AlertManager>();
		pHealth = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
	} // end Start
	

	void Update () 
	{
		// OLD VERSION //
		// check every frame to see if a guard can see the player
//		if (vision.alerted) {
//			// if a guard sees the player, trigger the Alert System
//			alertSystem.GetComponent<AlertManager>().TriggerAlert();
////			// update last player location from vision to Alert System
////			lastSighting = vision.lastPlayerSighting;
//		}

		if(sensing.playerDetected)
		{
			alerted = true;
			aggro = true;
			alertSystem.TriggerAlert();
			sensing.playerDetected = false;
		}

		if (sensing.playerHeard && !alerted) {
			curious = true;
			sensing.playerHeard = false;
		}

		if (alertSystem.alertActive) {
			alerted = true;
		} else {
			alerted = false;
			sensing.playerDetected = false;
			aggro = false;
		}

		/// State machine
		if (alerted) {
			if (sensing.playerInSight) {
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

		if(curious && !alerted && !aggro){
			behaviour.guardState = GuardBehaviour.GuardState.Investigate;
		}

		if(pHealth.playerDead){
			behaviour.guardState = GuardBehaviour.GuardState.Idle;
		}

//		// Alternate State Machine (clone of old version)
//		if (!sensing.playerDetected) {
//			if (alerted) {
//				behaviour.guardState = GuardBehaviour.GuardState.AlertPatrol;
//			} else {
//				if(patrolling){
//				behaviour.guardState = GuardBehaviour.GuardState.Patrol;
//				sensing.playerDetected = false;
//				} else{
//					behaviour.guardState = GuardBehaviour.GuardState.Sentry;
//				}
//			}
//		} else {
//			if(sensing.playerInSight){
//				behaviour.guardState = GuardBehaviour.GuardState.Attack;
//			} else{
//				behaviour.guardState = GuardBehaviour.GuardState.Search;
//			}
//		}
	} // end Update
	
}
