/// <summary>
/// Soundproof space - script for soundproof room to block guard hearing
/// </summary>

using UnityEngine;
using System.Collections;

public class SoundproofSpace : MonoBehaviour {

	void Start () 
	{
		GetComponent<MeshRenderer> ().enabled = false;
	}

	void Update () 
	{
	
	}

	// block guard hearing when they enter the trigger
	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Guard") {
			coll.transform.GetComponent<GuardSensing>().soundProofed = true;
		}
	}

	// block guard heearing while they are inside the trigger
	void OnTriggerStay(Collider coll)
	{
		if (coll.tag == "Guard") {
			coll.transform.GetComponent<GuardSensing>().soundProofed = true;
		}
	}

	// reactivate guard hearing as they leave the trigger
	void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Guard") {
			coll.transform.GetComponent<GuardSensing>().soundProofed = false;
		}
	}
}
