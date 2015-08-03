using UnityEngine;
using System.Collections;

public class SpinningObject : MonoBehaviour {

	public float spinSpeed = 1f;

	public bool spinRight, spinUp, spinForward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.eulerAngles += spinAmount;
		if(spinRight)	transform.Rotate(transform.right 	* Time.deltaTime * spinSpeed);
		if(spinUp) 		transform.Rotate(transform.up 		* Time.deltaTime * spinSpeed);
		if(spinForward) transform.Rotate(transform.forward 	* Time.deltaTime * spinSpeed);
	}
}
