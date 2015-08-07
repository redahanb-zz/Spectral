using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform targetCamera, target;
	Vector3 targetCameraPosition, targetCameraRotation;
	float moveSpeed = 0.0085f, damping = 3.0f, rotateSpeed = 0.01f, speed;
	float distance;
	public bool lookAtTarget = false;

	// Use this for initialization
	void Start () {
		lookAtTarget = true;
		//Invoke("ToggleLookAt", 2);
	}


	void ToggleLookAt(){
		lookAtTarget = !lookAtTarget;
	}

	// Update is called once per frame
	void Update () {


		if(targetCamera){

			distance = Vector3.Distance(transform.position, targetCamera.position);
			speed = distance * 3;

			targetCameraPosition = targetCamera.position;
			targetCameraRotation = targetCamera.eulerAngles;

			if(transform.position != targetCameraPosition){
				transform.position = Vector3.Lerp(transform.position, targetCameraPosition, distance * moveSpeed);
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, targetCamera.rotation, distance * rotateSpeed);

			if(target && lookAtTarget){
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.003f * damping);
			}	
		
		}
	}
}
