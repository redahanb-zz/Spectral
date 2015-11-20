﻿//Name:			PlayerController.cs
//Author(s)		Conor Hughes, Benjamin Redahan
//Description:
//
//
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
	
	EventSystem 			eventSystem;
	HealthManager			pHealth;
	TimeScaler 				timeScale;
	Animator 				playerAnimator;
	AlertManager 			alert;
	
	private NavMeshAgent 	agent;
	private NavMeshPath		path;
	private Ray 			ray;
	private RaycastHit 		rayHit;
	private Renderer		playerRenderer;
	private GameObject[]	bodyParts;
	private GameObject 		pathObject, 
							destinationObject,
							buttonBlendObject, 				// gameObject for when a blend order is given using the button
							mouseCursorObject;
	
	public 	Color 			targetcolor = Color.grey;
	private Transform 		pickupItemTransform, currentBlendSurface;
	
	public 	enum 			MoveState{ Idle, Sneak, Run, Blend_Stand, Blend_Prone};
	public 	MoveState 		currentMoveState = MoveState.Idle;
	
	private int 			currentPathIndex 	= 1;
	
	private float 			distance 			= 0f,
	currentSpeed 		= 0f,
	turnDistance 		= 100, 
	targetSpeed 		= 0,
	runSpeed 			= 3f,
	walkSpeed 			= 1.1f,
	sneakSpeed 			= 0.5f,
	stopSpeed 			= 0.1f,
	timeSinceLastClick 	= 0,		//calculates time from last click, used to check for double click.
	verticalDistance 	= 100,		//used to caluclate vertical distance between player and click position.
	lastInterval, 
	timeNow, 
	customDeltaTime,
	normalLookAtRate 	= 8.5f,
	slowTimeLookAtRate 	= 40.0f;
	
	
	private Vector3 		targetPosition;
	
	
	private bool 			
	hasPath 		 	= false,
	doubleTap 		 	= false,
	performAction 	 	= false,
	buttonBlendOrder 	= false, 	//bool for when a blend order is given using the button
	leftClick   	 	= true;		//used to indicate that a left click event occured.
	
	public 	bool 		isVisible 			= true,		//indicates if the player is visible to guards and hazards.
	isBlending 			= false,	//indicates if the player is attempting to hide.
	canMove 		 	= false;
	
	//public 
	Sprite 				defaultCursor,
						blendCursor,
						pickupCursor,
						useCursor;

	Color wallColor, newColor;

	RectTransform mouseTransform;
	Image mouseImage;
	
	
	// Use this for initialization
	void Start () {
		alert 			 = GameObject.Find ("Alert System").GetComponent<AlertManager>();
		eventSystem		 = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		pHealth 		 = GameObject.Find ("Health Manager").GetComponent<HealthManager>();
		timeScale 		 = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		
		playerAnimator 	 = GetComponent<Animator>();
		bodyParts 		 = GetComponent<PlayerBodyparts> ().bodyparts;
		agent 			 = GetComponent<NavMeshAgent>();
		
		customDeltaTime  = Time.deltaTime;

		SetupMouseCursor();


		foreach (GameObject part in bodyParts)part.GetComponent<Renderer>().material.color = Color.green;
		agent.SetDestination(transform.position);
		playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
		Invoke("ToggleCanMove", 0.1f);
	}

	void SetupMouseCursor(){
		mouseCursorObject = Instantiate(Resources.Load("Mouse Cursor"), Vector3.zero, Quaternion.identity) as GameObject;
		mouseCursorObject.transform.parent = GameObject.Find("Canvas").transform;
		mouseTransform = mouseCursorObject.GetComponent<RectTransform>();
		mouseImage = mouseCursorObject.GetComponent<Image>();
		
		defaultCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Main");
		blendCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Blend");
		pickupCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Pickup");
		useCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Use");
		
		Cursor.visible = false;
		
		mouseImage.sprite = defaultCursor;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		timeNow 		= Time.realtimeSinceStartup;
		customDeltaTime = timeNow - lastInterval;
		lastInterval 	= timeNow;

		hasPath = agent.hasPath;

		ContextualMouseCursor();
		if(canMove){
			GetInput();
			MoveStateManager();
		}
		else StopMoving();
		
		SetBodyColour();
		lastInterval = timeNow;
	} // end update
	
	public void ToggleCanMove(){
		canMove = !canMove;
	}

	private void SetBodyColour(){
		if(!pHealth.playerDead){
			// change colour of each bodypart in the array
			foreach (GameObject part in bodyParts) {
				part.GetComponent<Renderer>().material.color = Color.Lerp(part.GetComponent<Renderer>().material.color, targetcolor, 10*Time.deltaTime);
			}
		}
	}
	
	//Assigns action per state and sets the animator move state
	void MoveStateManager(){
		playerAnimator.SetInteger("moveState", (int)currentMoveState);
		switch(currentMoveState){
		case MoveState.Idle: 			Idle(); 				isBlending = false; isVisible = true;	break;
		case MoveState.Sneak: 			Sneak(); 				isBlending = false; isVisible = true;	break;
		case MoveState.Run: 			Run(); 					isBlending = false; isVisible = true;	break;
		case MoveState.Blend_Stand: 	BlendWhileStanding(); 	isBlending = true; 						break;
		}
	}

	void ContextualMouseCursor(){
		mouseTransform.position = Input.mousePosition + new Vector3(mouseTransform.sizeDelta.x/2 , -mouseTransform.sizeDelta.y/2, 0);

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out rayHit, 100f)){
			switch(rayHit.transform.tag){
				case "Blend Surface" :		//if(rayHit.transform.GetComponent<Renderer>().material.color == newColor){ 
					mouseImage.sprite = blendCursor;	mouseTransform.sizeDelta = new Vector3(64,64,0);
											//}
											//else{
					//mouseImage.sprite = defaultCursor;	mouseTransform.sizeDelta = new Vector3(32,32,0);
											//}
					break;
			case "Pickup" :				mouseImage.sprite = pickupCursor;	mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Door" :				mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Load Door" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			default:					mouseImage.sprite = defaultCursor;	mouseTransform.sizeDelta = new Vector3(32,32,0); break;
			}
		}
	}


	//Get input from the Player
	void GetInput(){
		//timeSinceLastClick = timeSinceLastClick + customDeltaTime;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// use ray to determine the object, change the cursor accordingly
//				if(Physics.Raycast(ray, out rayHit, 100.0f)){
//					switch(rayHit.transform.tag){
//						case "Blend Surface" :
//						Cursor.SetCursor(blendCursor, Vector2.zero, CursorMode.Auto);
//							break;
//						case "Pickup" :
//						Cursor.SetCursor(pickupCursor, Vector2.zero, CursorMode.Auto);
//							break;
//						case "Door" :
//						Cursor.SetCursor(useCursor, Vector2.zero, CursorMode.Auto);
//							break;
//						case "Load Door" :
//						Cursor.SetCursor(useCursor, Vector2.zero, CursorMode.Auto);
//							break;
//						default:
//							Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
//							break;
//					}
//				}
		
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
			if(Input.GetMouseButtonDown(0))leftClick = true;
			if(Input.GetMouseButtonDown(1))leftClick = false;
			//			buttonBlendOrder = false;
			//			//Check for DOuble Tap
			//			if(timeSinceLastClick < 1)
			//				doubleTap = true;
			//			else 
			//				doubleTap = false;
			//			timeSinceLastClick = 0;
			if(!eventSystem.IsPointerOverGameObject())
			if (Physics.Raycast(ray, out rayHit, 100f)){
				isBlending = false;
				buttonBlendOrder = false;

				print(rayHit.transform);
				
				switch(rayHit.transform.tag){
				case "Tile" : 
					currentBlendSurface = null;
					verticalDistance = Vector3.Distance(new Vector3(0,transform.position.y,0), new Vector3(0,rayHit.point.y,0));
					if(verticalDistance > 2){				//Go to idle state if clicking on another floor level.
						currentMoveState = MoveState.Idle;
						break;
					}
					SetMovement(rayHit.transform, rayHit.point);
					performAction = false;
					break;
					
				case "Blend Surface" :
					if(rayHit.transform != currentBlendSurface){
						SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.2f));
						Color newColor = bodyParts[0].GetComponent<Renderer>().material.color;
						Color wallColor = rayHit.transform.GetComponent<Renderer>().material.color;
						float colorDistance = Vector3.Distance(new Vector3(newColor.r, newColor.g, newColor.b), new Vector3(wallColor.r, wallColor.g, wallColor.b));
						if(colorDistance < 0.1f){
							performAction = true;
							currentBlendSurface = rayHit.transform;
						}
					}
					break;
					
				case "Pickup" :
					currentBlendSurface = null;
					pickupItemTransform = rayHit.transform;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
					
				case "Load Door" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
				}
				
				switch(rayHit.transform.name){
				case "Threshold" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
					
				case "Teleporter 1" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
					
				case "Teleporter 2" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;	
					
				case "Base" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
				}
			}
		}
	}
	
	
	void SetMovement(Transform t, Vector3 v){
		if(Vector3.Distance(transform.position, new Vector3(v.x, transform.position.y, v.z)) > 0){
			ClearPath();
			targetPosition 		= new Vector3(v.x, transform.position.y, v.z);
			if(!leftClick) 
				currentMoveState 	= MoveState.Sneak;
			else 
				currentMoveState 	= MoveState.Run;
		}
		else currentMoveState = MoveState.Idle;
	}
	
	void Idle(){
		agent.destination = agent.transform.position;
		distance = 1000f;
	}
	
	void BlendWhileStanding(){
		if(isBlending){
			newColor = bodyParts[0].GetComponent<Renderer>().material.color;
			if(rayHit.transform.tag == "Blend Surface")wallColor = rayHit.transform.GetComponent<Renderer>().material.color;
			if(buttonBlendOrder)wallColor = buttonBlendObject.GetComponent<Renderer>().material.color;
			float colorDistance = Vector3.Distance(new Vector3(newColor.r, newColor.g, newColor.b), new Vector3(wallColor.r, wallColor.g, wallColor.b));
			if(colorDistance < 0.1f) isVisible = false;	//Player cannot be seen.;
			else 					 isVisible = true;	//Player is visible to enemies.
		}
	}
	
	
	void Sneak(){
		agent.SetDestination(targetPosition);
		distance = Vector3.Distance(transform.position, targetPosition);
		GetPath();
		SetSneakSpeed();
		PathIndicator();
		
		if(distance < 1f){
			if(performAction)
				PerformAnAction();
			else currentMoveState = MoveState.Idle;
			destinationObject.transform.Find("Quad").GetComponent<DestinationMarker>().RemoveMarker();
			destinationObject = null;
		}
	}
	
	//Used to set running movement for the player.
	void Run(){
		agent.SetDestination(targetPosition);
		distance = Vector3.Distance(transform.position, targetPosition);
		GetPath();
		SetRunSpeed();
		PathIndicator();
		
		if(distance < 0.1f){
			if(performAction)
				PerformAnAction();
			else currentMoveState = MoveState.Idle;
			destinationObject.transform.Find("Quad").GetComponent<DestinationMarker>().RemoveMarker();
			destinationObject = null;
		}
	}
	
	//Sets the players running speed based on timescale and turn distance.
	void SetRunSpeed(){
		//Heading, distance and direction of next corner
		Vector3 	head;
		if(path.corners.Length > 2) head = path.corners[currentPathIndex] - transform.position; 
		else head = targetPosition - transform.position; 
		float		dist = head.magnitude;
		Vector3 	dir  = head/dist;
		
		//Sets the distance to the next turn along the path.
		if(path.corners.Length > 2)turnDistance = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
		else turnDistance = Vector3.Distance(transform.position, targetPosition);
		
		//If near a turn, slow agent. Otherwise the agent runs normally.
		if(turnDistance > 1.2f)targetSpeed = runSpeed;
		else{ targetSpeed = walkSpeed;   
			//agent.velocity = new Vector3(1,0,1); 
		}
		
		//Check the distance between the players direction and the path direction, used to slow player when turning.
		Quaternion q = Quaternion.LookRotation(head);
		Vector3 v3Euler = q.eulerAngles;
		float rotDistance = Vector3.Distance(transform.eulerAngles, v3Euler);
		if(rotDistance > 5){ 
			targetSpeed = walkSpeed;  //agent.speed = 1;
		}
		
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, customDeltaTime * 5);
		playerAnimator.SetFloat("movementSpeed" , currentSpeed);
		playerAnimator.speed = 1;
		
		
		if(timeScale.timeSlowed){
			//agent.speed = Mathf.Lerp(agent.speed, currentSpeed * 5, 0.02f);
			//agent.speed = 100;
			//agent.acceleration = Mathf.Lerp(agent.acceleration, 100, 0.02f);
			//agent.acceleration = 100;
			//playerAnimator.speed = Mathf.Lerp(playerAnimator.speed, 1, 0.02f);
		}
		else{
			//agent.speed  = Mathf.Lerp(agent.speed, currentSpeed, 0.02f);
			//agent.acceleration = Mathf.Lerp(agent.acceleration, 50, 0.02f);
			//agent.acceleration = 50;
			//playerAnimator.speed = Mathf.Lerp(playerAnimator.speed, 1, 0.02f);
		}
		
		//		if(timeScale.timeSlowed){
		//			playerAnimator.SetFloat("movementSpeed" , currentSpeed);
		//			//agent.speed = Mathf.Lerp(agent.speed, currentSpeed * 10, 0.02f);
		//			agent.speed = currentSpeed * 10;
		//			//playerAnimator.speed = Mathf.Lerp(playerAnimator.speed, 3, 0.02f);
		//		}
		//		else{
		//			playerAnimator.SetFloat("movementSpeed" , currentSpeed);
		//			//agent.speed  = Mathf.Lerp(agent.speed, currentSpeed, 0.02f);
		//			agent.speed = currentSpeed;
		//			//playerAnimator.speed = Mathf.Lerp(playerAnimator.speed, 1, 0.02f);
		//			//playerAnimator.speed = 1;
		//		}
		
		//Rotate the player to face the next corner on the path, otherwise look at the path end.
		if(path.corners.Length > 2)SmoothLookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
		else SmoothLookAt(targetPosition);
	}
	
	void SetSneakSpeed(){
		//Heading, distance and direction of next corner
		Vector3 	head;
		if(path.corners.Length > 2)	head = path.corners[currentPathIndex] - transform.position; 
		else head = targetPosition - transform.position; 
		float		dist = head.magnitude;
		Vector3 	dir  = head/dist;
		
		//Get distance of next corner
		if(path.corners.Length > 2)turnDistance = Vector3.Distance(transform.position, path.corners[currentPathIndex]);
		else turnDistance = Vector3.Distance(transform.position, targetPosition);
		
		//If near corner, walk.	Otherwise, run.
		if(turnDistance > 0.5f){targetSpeed = sneakSpeed;   agent.speed = 2;}
		else{ targetSpeed = stopSpeed;   agent.speed = 1;}
		
		//If not facing direction of next corner, walk
		if(transform.forward != dir){ targetSpeed = walkSpeed;  agent.speed = 1;}
		
		//Update current speed
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5);
		playerAnimator.SetFloat("movementSpeed" , currentSpeed);
		
		//Look at next corner
		if(path.corners.Length > 2)SmoothLookAt(new Vector3(path.corners[currentPathIndex].x, transform.position.y, path.corners[currentPathIndex].z));
		else SmoothLookAt(targetPosition);
	}
	
	void SmoothLookAt(Vector3 targetPosition){
		
		float lookAtRate = normalLookAtRate;
		if(timeScale.timeSlowed)lookAtRate = slowTimeLookAtRate;
		else lookAtRate = normalLookAtRate;
		
		Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, customDeltaTime * lookAtRate);
	}
	
	public void StopMoving(){
		currentMoveState = MoveState.Idle;
	}
	
	void PathIndicator(){
		if(!destinationObject)
			destinationObject = Instantiate(Resources.Load("DestinationMarker"), targetPosition, Quaternion.identity) as GameObject;
		
		if(!pathObject && distance > 1f){
			pathObject = Instantiate(Resources.Load("PathIndicator"), transform.position + new Vector3(0,0.01f,0) + (transform.forward * 1), Quaternion.identity) as GameObject;
			agent.SetDestination(targetPosition);
			
			
			
			//agent.destination = targetPosition;
			pathObject.transform.eulerAngles = transform.eulerAngles;
			pathObject.GetComponent<PathIndicator>().SetPath(agent.path);
		}
	}
	
	void OnAnimatorMove(){
		GetComponent<NavMeshAgent> ().velocity = playerAnimator.deltaPosition / Time.deltaTime;
	}
	
	//Chooses an action to perform based on what was clicked by the player.
	void PerformAnAction(){
		if(rayHit.transform.tag == "Blend Surface"){
			transform.forward = -rayHit.transform.forward;
			isBlending = true;
			currentMoveState = MoveState.Blend_Stand;
		}
		else if(rayHit.transform.tag == "Pickup"){
			transform.forward = -rayHit.transform.forward;
			pickupItemTransform.GetComponent<InventoryItem>().pickupAnim();
			currentMoveState = MoveState.Idle;
		}
		else if(rayHit.transform.tag == "Load Door"){
			transform.forward = -rayHit.transform.forward;
			rayHit.transform.Find("Door Trigger").GetComponent<Door>().StartNewTeleport();
			currentMoveState = MoveState.Idle;
		}
		else if(rayHit.transform.name == "Threshold"){
			transform.forward = -rayHit.transform.forward;
			rayHit.transform.parent.GetComponent<Door>().StartNewTeleport();
			currentMoveState = MoveState.Idle;
		}
		else if(rayHit.transform.name == "Door North" || rayHit.transform.name == "Door South" || rayHit.transform.name == "Door East" || rayHit.transform.name == "Door West"){
			transform.forward = -rayHit.transform.forward;
			if(!alert.alertActive)rayHit.transform.Find("Door Trigger").GetComponent<Door>().StartNewTeleport();
			currentMoveState = MoveState.Idle;
		}
		else if(rayHit.transform.name == "Teleporter 1" || rayHit.transform.name == "Teleporter 2"){
			transform.forward = -rayHit.transform.forward;
			rayHit.transform.GetComponent<Teleporter>().Teleport();
			currentMoveState = MoveState.Idle;
		}
		else if(rayHit.transform.name == "Base"){
			transform.forward = -rayHit.transform.forward;
			rayHit.transform.parent.GetComponent<Teleporter>().Teleport();
			currentMoveState = MoveState.Idle;
		}
		
		if(buttonBlendOrder){
			transform.forward = -buttonBlendObject.transform.forward;
			isBlending = true;
			currentMoveState = MoveState.Blend_Stand;
		}
	}
	
	//Blend button behaviour that triggers blend state
	public void blendButton(GameObject blendObject, Transform t, Vector3 v){
		SetMovement(t, t.position + (-t.forward * 0.2f));
		performAction = true;
		buttonBlendOrder = true;
		buttonBlendObject = blendObject;
	}
	
	//Sets a new path
	void GetPath(){
		currentPathIndex = 1;
		path = agent.path;
	}
	
	//Clears the current path
	void ClearPath(){
		Destroy(pathObject);
		Destroy(destinationObject);
	}
}
