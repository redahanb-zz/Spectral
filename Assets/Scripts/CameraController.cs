using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform targetCamera, target;
	Vector3 targetCameraPosition, targetCameraRotation;
	float moveSpeed = 2.0f, damping = 3.0f, rotateSpeed = 0.6f, speed;
	float distance = 1000;
	public bool lookAtTarget = false;
	bool cameraShake = false;
	Vector3 originalPos, nextPos;
	float randomCount;

	float shakeAmount = 0.1f;
	public float lastInterval, timeNow, myTime;

	bool intro = false;

	Transform introObject;
	Vector3   introStartPos, introEndPos;
	Quaternion introStartRot, introEndRot;

	// Use this for initialization
	void Start () {

		//introObject = GameObject.Find("Intro Camera").transform;

//		introStartPos = introObject.Find("IntroCameraStart").position;
//		introEndPos = introObject.Find("IntroCameraEnd").position;
//
//		introStartRot = introObject.Find("IntroCameraStart").rotation;
//		introEndRot = introObject.Find("IntroCameraEnd").rotation;
//
//		transform.position = introStartPos;
//		transform.rotation = introStartRot;

		lookAtTarget = true;
		distance = 1000;
	}

	void SetShakePosition(){
		nextPos = originalPos + Random.insideUnitSphere * shakeAmount;
	}

	void ToggleLookAt(){
		lookAtTarget = !lookAtTarget;
	}

	// Update is called once per frame
	void Update () {
		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow;

		print(intro + " : " +distance);

		if(intro){
			distance = Vector3.Distance(transform.position, introEndPos);
			if(distance < 1)intro = false;
			else{
				transform.position = Vector3.Lerp(transform.position, introEndPos, myTime * moveSpeed * 1.5f);
				transform.rotation = Quaternion.Lerp(transform.rotation, introEndRot, myTime * rotateSpeed);
			}
		}
		else{
			if(targetCamera){
				
				distance = Vector3.Distance(transform.position, targetCamera.position);
				speed = distance * 3;
				
				targetCameraPosition = targetCamera.position;
				targetCameraRotation = targetCamera.eulerAngles;
				
				if(transform.position != targetCameraPosition){
					transform.position = Vector3.Lerp(transform.position, targetCameraPosition, myTime * moveSpeed);
				}
				
				//			if(cameraShake){
				//				originalPos = targetCameraPosition;
				//				transform.localPosition = Vector3.Lerp(transform.localPosition, nextPos, Time.deltaTime * distance * .01f);
				//			}
				
				
				if(target && lookAtTarget){
					Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.003f * damping);
				}
				else{
					transform.rotation = Quaternion.Lerp(transform.rotation, targetCamera.rotation, myTime * rotateSpeed);
				}
				
			}
		}
		lastInterval = timeNow;
	}



}
















