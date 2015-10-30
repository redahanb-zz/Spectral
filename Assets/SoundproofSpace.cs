using UnityEngine;
using System.Collections;

public class SoundproofSpace : MonoBehaviour {



	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll){
		if (coll.tag == "Guard") {
			coll.transform.GetComponent<GuardSensing>().soundProofed = true;
		}
	}

	void OnTriggerStay(Collider coll){
		if (coll.tag == "Guard") {
			//print (coll.name);
			coll.transform.GetComponent<GuardSensing>().soundProofed = true;
		}
	}

	void OnTriggerExit(Collider coll){
		if (coll.tag == "Guard") {
			coll.transform.GetComponent<GuardSensing>().soundProofed = false;
		}
	}
}
