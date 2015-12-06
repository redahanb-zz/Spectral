//Name:			CameraTrigger.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	A trigger which overwrites the cameracontrollers current target position once it is entered.

using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {

	private CameraController 	cController;			//instance of camera controller
	public 	bool 				lookAtPlayer = true;	//determines if camera controller can look at player

	// Use this for initialization
	void Start () {
		cController = Camera.main.GetComponent<CameraController>();
		GetComponent<Renderer>().enabled = false;
	}

	//When trigger is entered, overwrite current camera target
	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			if(transform.Find("CameraPosRot(Clone)")) cController.targetCamera = transform.Find("CameraPosRot(Clone)").transform;
			else cController.targetCamera = transform.Find("CameraPosRot").transform;

			cController.target = c.transform;
		}
	}
}
