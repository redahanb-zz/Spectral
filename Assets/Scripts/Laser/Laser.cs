using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	Vector3 sourcePosition, hitPosition;
	RaycastHit hit;

	LineRenderer lRenderer;

	AlertManager alertSystem;

	float rotateSpeed = 0.8f;
	int rotateDistance = 100;
	public bool canRotate = false, rotateRight = false, changingDirection = false;

	bool alerted = false;
	// Use this for initialization
	void Start () {

		alertSystem = GameObject.Find("Alert System").GetComponent<AlertManager>();

		hitPosition = transform.position + new Vector3(0,0,20);

		lRenderer = GetComponent<LineRenderer>();
		lRenderer.SetPosition(0, transform.position);
		lRenderer.SetPosition(1, hitPosition);
	}

	void ToggleRotateDirection(){
		rotateRight = !rotateRight;
		changingDirection = false;
	}

	IEnumerator ChangeDirection(){
		changingDirection = true;
		rotateRight = !rotateRight;
		yield return new WaitForSeconds(0.1f);
		changingDirection = false;
		StopAllCoroutines();
	}

	// Update is called once per frame
	void Update () {
		MoveLaser();
		DrawLaser();
	}

	void MoveLaser(){
		if(canRotate && !alerted){
			if(rotateRight == true){
				if(transform.eulerAngles.y >= 44.9f && transform.eulerAngles.y < 100)
				if(changingDirection == false){
					StartCoroutine(ChangeDirection());
				}
				transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,45,0), Time.deltaTime * rotateSpeed);
			}
			else{
				if(transform.eulerAngles.y <= 315.1f && transform.eulerAngles.y > 100) 
				if(changingDirection == false){
					StartCoroutine(ChangeDirection());
				}
				transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,-45,0), Time.deltaTime * rotateSpeed);
			}
		}
	}
	
	void DrawLaser(){
		if(alerted)lRenderer.SetColors(Color.red, Color.red);
		else lRenderer.SetColors(Color.green, Color.green);

		if (Physics.Raycast(transform.position, transform.right, out hit, 1000.0F)){
			if(hit.transform != null){
				hitPosition = hit.point;
				if(hit.transform.tag == "Player"){
					alerted = true;
					alertSystem.alertActive = true;
				}
			}
			else
				hitPosition = transform.position + (transform.right * 6f);
			
			lRenderer.SetPosition(1, hitPosition);
		}
	}
}
