//Name:			CameraController.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com, Benjamin Redahan - redahanb@gmail.com
//Description:	This script manages all of the main camera behaviours. It assigns a new camera position each time the player enters a camera trigger.


using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform 	targetCamera, 			//the current target camera
						target, 				//the current look at target
						lookAtOtherTarget;		//target that overrides normal target (used in title screen)
	private Vector3 	targetCameraPosition, 	//position of target camera
						targetCameraRotation;	//rotation of target camera
	private float 		moveSpeed = 2.0f, 		//camera movement speed
						damping = 9.0f, 		//camera look at damping
						rotateSpeed = 0.9f, 	//camera rotation speed
						distance = 1000, 		//distance to target camera position
						shakeAmount = 0.1f;		//the amount the screen shakes by
	public bool 		lookAtTarget = false;	//determines if camera can look at current target
	private bool 		cameraShake = false; 	//determines if the camera can shake

	private Vector3 	originalPos, 			//used to set center of skaje
						nextPos;				//random pos around original pos

	public float 		lastInterval, 			//time of last frame
						timeNow, 				//current time
						myTime;					//custom delta time

	private Vector3   	zoomedOutPos;			//the map view position
	private Quaternion 	zoomedOutRot;			//the map view rotation

	public bool 		mapCamera = false, 		//determines if map view is active
						mapButtonDown = false;	//indicates if map button is pressed

	private GameObject 	mapCamObject;			//the map camera object for the current room
	
	HUD_Inventory 		invHUD;					//instance of inventory
	HUD_Healthbar		healthHUD;				//instance of healthbar
	HideHUDElement		hidePause, hideTime;	//instances of hide hud element

	TitleScreen tScreen;


	// Use this for initialization
	void Start () {
		zoomedOutPos = transform.position;
		zoomedOutRot = transform.rotation;
		lookAtTarget = true;
		distance = 1000;

		invHUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
		healthHUD = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
		hidePause = GameObject.Find("PauseButton").GetComponent<HideHUDElement>();
		hideTime = GameObject.Find("Time Button").GetComponent<HideHUDElement>();

		if(Application.loadedLevelName == "Restore Point"){
			tScreen = GameObject.Find("Title Screen Canvas").GetComponent<TitleScreen>();
			if(tScreen.showTitleScreen){
				invHUD.hide();
				healthHUD.hide();
				hidePause.hide();
				hideTime.hide();
			}
		}
	}

	//Map button is pressed
	public void MapButtonDown(){
		mapButtonDown = true;
	}

	//Map button is released
	public void MapButtonUp(){
		mapButtonDown = false;
	}

	//Sets next shake position
	void SetShakePosition(){
		nextPos = originalPos + Random.insideUnitSphere * shakeAmount;
	}

	//Toggles look at target mode
	void ToggleLookAt(){
		lookAtTarget = !lookAtTarget;
	}

	//Zooms camera out to view entire room
	public void MapView(){
		transform.position = Vector3.Lerp(transform.position, zoomedOutPos, myTime * moveSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation, zoomedOutRot, myTime * rotateSpeed);
	}

	// Update is called once per frame
	void Update () {
		timeNow = Time.realtimeSinceStartup;
		myTime = timeNow - lastInterval;
		lastInterval = timeNow;


		if(lookAtOtherTarget){
			transform.LookAt(lookAtOtherTarget.position);
		}
		else{

			if(targetCamera){
				if(targetCamera.parent.parent.parent.Find("Map Camera Position"))mapCamera = true;
				else mapCamera = false;
			}
			

			if(mapCamera){
				mapCamObject = targetCamera.parent.parent.parent.Find("Map Camera Position").gameObject;
				zoomedOutPos = mapCamObject.transform.position;
				zoomedOutRot = mapCamObject.transform.rotation;

			}


			if(Input.GetKey(KeyCode.C)){
				mapButtonDown = true;

				if(!invHUD){
					invHUD = GameObject.Find("HUD_Inventory").GetComponent<HUD_Inventory>();
					healthHUD = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
					hidePause = GameObject.Find("PauseButton").GetComponent<HideHUDElement>();
					hideTime = GameObject.Find("Time Button").GetComponent<HideHUDElement>();
				}
				else{
					invHUD.hide();
					healthHUD.hide();
					hidePause.hide();
					hideTime.hide();
				}
			}
			else if(Input.GetKeyUp(KeyCode.C)){
				if(Application.loadedLevelName == "Restore Point"){
					if(tScreen.showTitleScreen){
						mapButtonDown = false;
						invHUD.hide();
						healthHUD.hide ();
						hidePause.hide();
						hideTime.hide();
					}
					else{
						mapButtonDown = false;
						invHUD.show();
						healthHUD.show ();
						hidePause.show();
						hideTime.show();
					}
				}
				else{
					if(invHUD){
						mapButtonDown = false;
						invHUD.show();
						healthHUD.show ();
						hidePause.show();
						hideTime.show();
					}
				}
			}

			if(mapButtonDown){
				if(mapCamera){
					MapView();
				}
			}

			else{
				if(targetCamera){
					
					distance = Vector3.Distance(transform.position, targetCamera.position);

					targetCameraPosition = targetCamera.position;
					targetCameraRotation = targetCamera.eulerAngles;
					
					if(transform.position != targetCameraPosition){
						transform.position = Vector3.Lerp(transform.position, targetCameraPosition, myTime * moveSpeed);
					}
					
					if(target && lookAtTarget){
						Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
						transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.003f * damping);
					}
					else{
						transform.rotation = Quaternion.Lerp(transform.rotation, targetCamera.rotation, myTime * rotateSpeed);
					}
					
				}
			}

		}
		lastInterval = timeNow;
	}



}
















