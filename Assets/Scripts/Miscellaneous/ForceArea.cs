using UnityEngine;
using System.Collections;

public class ForceArea : MonoBehaviour {

	public float thrust;
	public Rigidbody rb;

	public Vector3 dir;

	void Start() {
		//rb = GetComponent<Rigidbody>();
	}
	void FixedUpdate() {
		//rb.AddForce(dir * thrust);
	}

	void OnTriggerEnter(Collider c){

		if(c.tag == "Physics Object"){
			print("TRIGGER!!!   " +c.name);
			c.GetComponent<Rigidbody>().velocity =  dir;
		}
	}

	void OnTriggerStay(Collider c){
		if(c.tag == "Physics Object"){
			print("TRIGGER!!!   " +c.name);
			c.GetComponent<Rigidbody>().velocity =  dir;
		}
	}
}
