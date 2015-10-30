﻿using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {

	CameraController cController;
	public bool lookAtPlayer = true;
	// Use this for initialization
	void Start () {
		cController = Camera.main.GetComponent<CameraController>();
		GetComponent<Renderer>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player"){
			//print("[CameraTrigger] Detected Player");
			if(transform.Find("CameraPosRot(Clone)")) cController.targetCamera = transform.Find("CameraPosRot(Clone)").transform;
			else cController.targetCamera = transform.Find("CameraPosRot").transform;

			cController.target = c.transform;
		}
	}
}
