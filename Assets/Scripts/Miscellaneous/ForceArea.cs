//Name:			ForceArea.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Applies a physics force to a target rigidbody.

using UnityEngine;
using System.Collections;

public class ForceArea : MonoBehaviour {

	public float thrust;		//the amount of force
	public Rigidbody rb;		//the target rigidbody
	public Vector3 dir;			//the force direction

	//On entering trigger
	void OnTriggerEnter(Collider c){
		if(c.tag == "Physics Object"){
			print("TRIGGER!!!   " +c.name);
			c.GetComponent<Rigidbody>().velocity =  dir;
		}
	}

	//On Staying in trigger
	void OnTriggerStay(Collider c){
		if(c.tag == "Physics Object"){
			print("TRIGGER!!!   " +c.name);
			c.GetComponent<Rigidbody>().velocity =  dir;
		}
	}
}
