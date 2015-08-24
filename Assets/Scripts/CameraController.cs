using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform targetCamera, target;
	Vector3 targetCameraPosition, targetCameraRotation;
	float moveSpeed = 0.0085f, damping = 3.0f, rotateSpeed = 0.01f, speed;
	float distance;
	public bool lookAtTarget = false;
	bool cameraShake = false;
	Vector3 originalPos, nextPos;
	float randomCount;

	float shakeAmount = 0.1f;

	// Use this for initialization
	void Start () {

		if(cameraShake){
			originalPos = targetCamera.position;
			randomCount = Random.Range(0.6f, 1f);
			StartCoroutine(CalculateDistance());
			SetShakePosition();
		}
		lookAtTarget = true;
	}

	IEnumerator CalculateDistance(){
		while(true){
			distance = Vector3.Distance(transform.localPosition, nextPos);
			if(distance < (shakeAmount)){
				//distance = 100;
				SetShakePosition();
			}
			randomCount = Random.Range(0.6f, 1.2f);
			yield return new WaitForSeconds(randomCount);
		}
	}

	void SetShakePosition(){
		nextPos = originalPos + Random.insideUnitSphere * shakeAmount;
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

			if(cameraShake){
				originalPos = targetCameraPosition;
				transform.localPosition = Vector3.Lerp(transform.localPosition, nextPos, Time.deltaTime * distance);
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, targetCamera.rotation, distance * rotateSpeed);

			if(target && lookAtTarget){
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.003f * damping);
			}	
		
		}
	}



}
















