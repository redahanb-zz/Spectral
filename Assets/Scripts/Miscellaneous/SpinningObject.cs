//Name:			SpinningObject.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Rotates an object. Each axis can be toggled on/off.


using UnityEngine;
using System.Collections;

public class SpinningObject : MonoBehaviour {

	public float spinSpeed = 1f;	//the spin speed
	public bool spinRight, 			//determines if the object can spin right
	spinUp, 						//determines if the object can spin up
	spinForward;					//determines if the object can spin forward
	
	// Update is called once per frame
	void Update () {
		if(spinRight)	transform.Rotate(transform.right 	* Time.deltaTime * spinSpeed);
		if(spinUp) 		transform.Rotate(transform.up 		* Time.deltaTime * spinSpeed);
		if(spinForward) transform.Rotate(transform.forward 	* Time.deltaTime * spinSpeed);
	}
}
