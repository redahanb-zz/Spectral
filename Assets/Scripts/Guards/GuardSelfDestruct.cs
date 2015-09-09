using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardSelfDestruct : MonoBehaviour {

	public GameObject[] bodyparts = new GameObject[29];
	//List<GameObject> bodyparts = new List<GameObject>();

	// Use this for initialization
	void Start () {
		//Invoke ("selfDestruct", 0.2f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void selfDestruct(){
		foreach(GameObject part in bodyparts){
			part.transform.parent = null;
			part.AddComponent<Rigidbody>();
			Vector3 impulse = new Vector3(Random.Range(-0.05f,0.05f),Random.Range(0.1f,0.3f),Random.Range(-0.05f,0.05f));
			part.GetComponent<Rigidbody>().AddForce(impulse);
			part.AddComponent<CapsuleCollider>();
			Destroy (part, Random.Range(0.5f,1.5f));
		}

		GameObject[] otherGuards = GameObject.FindGameObjectsWithTag ("Guard"); 
		foreach (GameObject guard in otherGuards) {
			if(Vector3.Distance(transform.position, guard.transform.position) < 10.0f){
				guard.GetComponent<NavMeshPatrolv2>().Investigate(transform.position);
			}
		}

		Destroy (this.gameObject);
		Destroy (this.transform.parent.gameObject);
	}
}
