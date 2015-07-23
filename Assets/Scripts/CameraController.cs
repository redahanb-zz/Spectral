using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform targetCamera, target;
	Vector3 targetCameraPosition, targetCameraRotation;
	float moveSpeed = 2.0f, damping = 6.0f;

	public bool lookAtTarget = false;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {


		if(targetCamera){


			targetCameraPosition = targetCamera.position;
			targetCameraRotation = targetCamera.eulerAngles;

			if(transform.position != targetCameraPosition){
				transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Time.deltaTime * moveSpeed);
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, targetCamera.rotation, Time.deltaTime * moveSpeed);

//			if(target && lookAtTarget){
//				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
//				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
//			}	
		
		}
	}
}
