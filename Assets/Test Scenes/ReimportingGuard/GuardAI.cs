using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {

	public bool alerted;

	public Vector3 lastSighting;

	private EnemySight vision;
	//private NavMeshPatrol patrolScript;
	private AlertManager alertSystem;

	// Use this for initialization
	void Start () {
		vision = gameObject.GetComponent<EnemySight>();
		alerted = false;
		alertSystem = GameObject.Find ("Game Manager").GetComponent<AlertManager> ();
	}
	
	// Update is called once per frame
	void Update () {
//		vision = gameObject.GetComponent<EnemySight>();
		if (vision.alerted) {

			alertSystem.alarmOn = true;

//			// take last player location form vision
//			lastSighting = vision.lastPlayerSighting;
//			// Alert all other guards, send them coordinates
//			GameObject[] guards;
//			guards = GameObject.FindGameObjectsWithTag("Guard");
//			foreach (GameObject guard in guards) {
//				guard.GetComponent<EnemySight>().globalAlert();
//				guard.GetComponent<EnemySight>().lastPlayerSighting = lastSighting;
//			}
		} else {
			alertSystem.alarmOn = false;
		}
	}
	
}
