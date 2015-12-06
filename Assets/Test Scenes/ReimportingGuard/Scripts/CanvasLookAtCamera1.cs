using UnityEngine;
using System.Collections;

public class CanvasLookAtCamera : MonoBehaviour {

	Camera mainCamera;

	// Use this for initialization
	void Start () {
		//mainCamera = Camera.main;
		//print (Camera.allCameras);
	}
	
	// Update is called once per frame
	void Update () {
		if(mainCamera == null){
			mainCamera = Camera.main;
			//print (mainCamera);
		}

		GetComponent<RectTransform>().rotation = mainCamera.gameObject.transform.rotation;
	}
}
