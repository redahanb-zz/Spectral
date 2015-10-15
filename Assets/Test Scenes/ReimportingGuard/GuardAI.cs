using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {

	// public variables
	public bool alerted;
	public Vector3 lastSighting;

	// cache references
	private EnemySight vision;
	public GameObject alertSystem;
	
	void Start () 
	{
		vision = gameObject.GetComponent<EnemySight>();
		alerted = false;
		alertSystem = GameObject.Find ("Alert System");
	} // end Start
	

	void Update () 
	{
		// check every frame to see if a guard can see the player
		if (vision.alerted) {
			// if a guard sees the player, trigger the Alert System
			alertSystem.GetComponent<AlertManager>().TriggerAlert();

//			// update last player location from vision to Alert System
//			lastSighting = vision.lastPlayerSighting;

		}
	} // end Update
	
}
