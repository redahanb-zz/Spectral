using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform targetCamera, target;
	Vector3 targetCameraPosition, targetCameraRotation;
	float moveSpeed = 2.0f, damping = 9.0f, rotateSpeed = 0.9f, speed;
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

	Vector3 zoomedOutPos;
	Quaternion zoomedOutRot;

	bool mapCamera = false;
	GameObject mapCamObject;

	bool mapButtonDown = false;

	// Use this for initialization
	void Start () {

		zoomedOutPos = transform.position;
		zoomedOutRot = transform.rotation;
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

	public void MapButtonDown(){
		mapButtonDown = true;
	}

	public void MapButtonUp(){
		mapButtonDown = false;
	}

	void SetShakePosition(){
		nextPos = originalPos + Random.insideUnitSphere * shakeAmount;
	}

	void ToggleLookAt(){
		lookAtTarget = !lookAtTarget;
	}

	public void MapView(){
		transform.position = Vector3.Lerp(transform.position, zoomedOutPos, myTime * moveSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation, zoomedOutRot, myTime * rotateSpeed);
	}

	// Update is called once per frame
	void Update () {
		//print(mapCamera);
		if(targetCamera.parent.parent.parent.Find("Map Camera Position"))mapCamera = true;
		else mapCamera = false;

		if(mapCamera){
			mapCamObject = targetCamera.parent.parent.parent.Find("Map Camera Position").gameObject;
			zoomedOutPos = mapCamObject.transform.position;
			zoomedOutRot = mapCamObject.transform.rotation;

		}

		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow;

		//print(intro + " : " +distance);

		if(Input.GetKey(KeyCode.C)){
			mapButtonDown = true;
		}
		else if(Input.GetKeyUp(KeyCode.C)){
			mapButtonDown = false;
		}

		if(mapButtonDown){
			if(mapCamera){
				MapView();
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
















