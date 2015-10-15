//Author(s):	Conor Hughes
//Date:			12/10/2015
//Description:	Used to animate a camera around a scene independent of the player camera.
//				Primarily used for recording gameplay from different perspectives. Movement 
//				and look at behaviours are assigned in the parameters.

using UnityEngine;
using System.Collections;

public class TrailerCamera : MonoBehaviour {
	public 	Vector3 	constantMoveDirection;			//The move direction of the camera
	public 	Transform 	cameraTarget;					//The look at target for the camera
	public 	bool 		lookAtPlayer 		= false,	//Used to override the look at target with player gameobject
						rotateAroundTarget  = false; 	//Enables behaviour allowing the camera to rotate around target
	private bool 		cameraActive 		= false;	//Used to toggle the Camera component on and off
	private Camera 		cameraComponent;				//Camera component of Camera gameobject
	private float 		defaultFov;						//Starting Field of View for the Camera

	// Used for initialization
	void Start () {
		cameraComponent = GetComponent<Camera>();
		cameraComponent.depth = 100;
		defaultFov = cameraComponent.fieldOfView;
		if(lookAtPlayer)cameraTarget = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Called once per frame
	void Update () {
		ToggleCamera();
		if(cameraActive){
			Move();
			Look();
		}
	}

	//Enables/Disables Camera component on keypress
	void ToggleCamera(){
		if(Input.GetKeyDown(KeyCode.P))cameraActive = !cameraActive; 	//If PRINT key is pressed
		cameraComponent.enabled = cameraActive;								//Toggle Camera component
	}

	//Moves the Camera gameobject
	void Move(){
		if(rotateAroundTarget){if(cameraTarget)transform.RotateAround(cameraTarget.position, Vector3.down, Time.deltaTime * 5);}
		else transform.position += constantMoveDirection;
	}

	//Look at target transform
	void Look(){
		if(cameraTarget)transform.LookAt(cameraTarget);

		if(Input.GetKey(KeyCode.Equals)){
			if(cameraComponent.fieldOfView > 20)cameraComponent.fieldOfView -= 0.2f;
		}
		else{
			if(cameraComponent.fieldOfView < defaultFov)cameraComponent.fieldOfView += 0.2f;

		}
	}
}
