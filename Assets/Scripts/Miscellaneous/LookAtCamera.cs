//Name:			LookAtCamera.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Forces GameObject to look at main camera.


using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	private GameObject mainCamera; //the main camera gameobject

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find("Main Camera");
		transform.LookAt(mainCamera.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(mainCamera.transform.position);
	}
}
