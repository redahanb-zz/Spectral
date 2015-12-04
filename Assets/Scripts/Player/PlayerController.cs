//Name:			PlayerController.cs
//Author(s)		Conor Hughes, Benjamin Redahan
//Description:	This script is used to handle the players movement and behaviours.
//				It performs a raycast on mouse click and assigns an action to the player
//				which is performed at the end of their movement.
//				It also calculates the path for the player and tells them when to stop.
//				The dynamic mouse cursor functionality is also handled here as it depends upon 
//				the raycasting performed in this script.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
	
	EventSystem 			eventSystem;					// event system
	HealthManager			pHealth;						// health manager instance
	TimeScaler 				timeScale;						// time scaler instance
	Animator 				playerAnimator;					// player animator component
	AlertManager 			alert;							// alert manager instance
	PauseManager 			pManager;						// pause manager instance
	Renderer 				tempChangeRenderer;				// renderer for individual body parts (used in loop)
	
	private NavMeshAgent 	agent;							// nav mesh agent of the player
	private NavMeshPath		path;							// path assigned to the nav mesh agent
	private Ray 			ray;							// ray used for each raycast
	private RaycastHit 		rayHit, cursorRayhit;			// rayhits for mouse cursor and player input
	private Renderer		playerRenderer;					// renderer component of the player
	private GameObject[]	bodyParts;						// array of player bodyparts
	private GameObject 		pathObject, 					// gameobject used to indicate the players path
							destinationObject,				// gameobject of the destination indicator
							buttonBlendObject, 				// gameObject for when a blend order is given using the button
							mouseCursorObject;				// gameobject of the mouse cursor
							
	public 	Color 			targetcolor = Color.grey;		// the target colour of the player (for colour changing)
	
	private Transform 		pickupItemTransform, 			// the transform of the item to be picked up
							currentBlendSurface;			// used to store the current surface used for hiding when the correct colour
	
	public 	enum 			MoveState{ 						//The multiple action states of the player. Each has the player perform a unique behaviour
								Idle, 						// -The player remains stationary and cancels all actions
								Sneak, 						// -The player sneaks to their destination(slow, but makes no noise)
								Run, 						// -The player runs to their destination(fast but makes noise which draws guards attention)
								Blend_Stand, 				// -The player hides against clicked blend surface if their colour matches it
	};
	
	public 	MoveState 		currentMoveState = MoveState.Idle;//the current move state
	
	private int 			currentPathIndex 	= 1;		//current path index, used to indicate the next corner along the path.
	
	private float 			distance 			= 0f,		//the distance from the destination
							currentSpeed 		= 0f,		//the current movement speed of the player
							turnDistance 		= 100, 		//distance to next turn, changes speed based on distance
							targetSpeed 		= 0,		//stores the current target speed
							runSpeed 			= 3f,		//target speed for running
							walkSpeed 			= 1.1f,		//target speed of walking
							sneakSpeed 			= 0.5f,		//target speed for sneaking
							stopSpeed 			= 0.1f,		//target speed for stopping
							timeSinceLastClick 	= 0,		//calculates time from last click, used to check for double click
							verticalDistance 	= 100,		//used to caluclate vertical distance between player and click position
							lastInterval, 					//the time of the last frame
							timeNow, 						//calculates time since last frame
							customDeltaTime,				//used to create deltatime independent of timescale
							normalLookAtRate 	= 8.5f,
							slowTimeLookAtRate 	= 40.0f,
							colorDistance;
	
	private Vector3 		targetPosition;					//the destination position for moving
	
	private bool 			buttonBlendOrder 	= false, 	//bool for when a blend order is given using the button
							leftClick   	 	= true;		//used to indicate that a left click event occured.
	
	public 	bool 			isVisible 			= true,		//indicates if the player is visible to guards and hazards.
							isBlending 			= false,	//indicates if the player is attempting to hide.
							canMove 		 	= false,	//determines if the player is able to move (or be controlled at all)
							performAction 	 	= false;	//determines whether the player can perform an action at the end of their movement.
	
	private Sprite 			defaultCursor,					//The default cursor used for movement.
							blendCursor,					//Cursor for hiding againts blend surface.
							pickupCursor,					//Cursor for picking up objects
							useCursor;						//Cursor for using teleporters.
	
	private Color 			wallColor, 						//The colour of the wall the player is attempting to hide against. 
							newColor;						//The target colour of the player
	
	RectTransform 			mouseTransform;
	Image 					mouseImage;
	
	
	// Use this for initialization
	void Start () {
		alert 			 = GameObject.Find ("Alert System").GetComponent<AlertManager>();
		eventSystem		 = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		pHealth 		 = GameObject.Find ("Health Manager").GetComponent<HealthManager>();
		timeScale 		 = GameObject.Find("Time Manager").GetComponent<TimeScaler>();
		pManager		 = GameObject.Find ("Pause Manager").GetComponent<PauseManager> ();
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
	
	//loads the cursor object and additional cursor textures from Resources
	void SetupMouseCursor(){
		mouseCursorObject = Instantiate(Resources.Load("Mouse Cursor"), Vector3.zero, Quaternion.identity) as GameObject;
		mouseCursorObject.transform.parent = GameObject.Find("Canvas").transform;
		mouseCursorObject.name = "Mouse Cursor";
		mouseTransform = mouseCursorObject.GetComponent<RectTransform>();
		mouseImage = mouseCursorObject.GetComponent<Image>();
		
		defaultCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Main");
		blendCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Blend");
		pickupCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Pickup");
		useCursor = Resources.Load<Sprite>("UI/Cursors/Cursor_Use");
		
		Cursor.visible = false;
		
		mouseImage.sprite = defaultCursor;
	}
	
	//Used to quickly restart the game (for demonstration purposes)
	void ResetGame(){
		Application.LoadLevel(0);
	}
	
	
	
	// Update is called once per frame
	void Update () {
		//Used to set a custom delta time independent of timescale(used for movement and lerping when changing main timescale).
		timeNow 		= Time.realtimeSinceStartup;
		customDeltaTime = timeNow - lastInterval;
		lastInterval 	= timeNow;
		
		ContextualMouseCursor();
		if(canMove){
			GetInput();
			MoveStateManager();
		}
		else StopMoving();
		
		SetBodyColour();
		lastInterval = timeNow;
	} // end update
	
	//Enables/Disables player movement
	public void ToggleCanMove(){
		canMove = !canMove;
	}
	
	//Set colour of player to selected object colour.
	private void SetBodyColour(){
		if(!pHealth.playerDead){
			// change colour of each bodypart in the array
			foreach (GameObject part in bodyParts) {
				tempChangeRenderer = part.GetComponent<Renderer>();
				tempChangeRenderer.material.color = Color.Lerp(tempChangeRenderer.material.color, targetcolor, 10*Time.deltaTime);
				if(!isVisible) 	tempChangeRenderer.material.SetColor("_OutlineColor", Color.Lerp(tempChangeRenderer.material.color, targetcolor, 10*Time.deltaTime));
				else 			tempChangeRenderer.material.SetColor("_OutlineColor", Color.Lerp(targetcolor, Color.black, 100*Time.deltaTime));
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
	
	//Function for setting the position and cursor of the in-game mouse.
	void ContextualMouseCursor(){
		//Set position of mouse.
		mouseTransform.position = Input.mousePosition + new Vector3(mouseTransform.sizeDelta.x/2 , -mouseTransform.sizeDelta.y/2, 0);
		
		//Cancel function if game paused or over
		if(pManager.gamePaused || pHealth.playerDead){
			return;
		}
		
		//Cast mouse ray.
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out cursorRayhit, 100f)){
			//Perform action based on name of raycast hit transform
			switch(cursorRayhit.transform.name){
			case "Teleporter 1" :		mouseImage.sprite = useCursor;	mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Teleporter 2" :		mouseImage.sprite = useCursor;	mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Base" :				mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Threshold" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Door North" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Door South" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Door East" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Door West" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			}
			
			//Perform action based on tag of raycast hit transform (overwirtes name switch)
			switch(cursorRayhit.transform.tag){
			case "Tile" :				mouseImage.sprite = defaultCursor;	mouseTransform.sizeDelta = new Vector3(32,32,0);break;
			case "Blend Surface" :		mouseImage.sprite = blendCursor;	mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Pickup" :				mouseImage.sprite = pickupCursor;	mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Door" :				mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			case "Load Door" :			mouseImage.sprite = useCursor;		mouseTransform.sizeDelta = new Vector3(64,64,0);break;
			}
		}
	}
	
	
	//Get input from the Player
	void GetInput(){
		if(Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha0))
			ResetGame();
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
			//Set left or right mouse click
			if(Input.GetMouseButtonDown(0))leftClick = true;
			if(Input.GetMouseButtonDown(1))leftClick = false;
			
			//If mouse is over gameobject, raycast
			if(!eventSystem.IsPointerOverGameObject())
			if (Physics.Raycast(ray, out rayHit, 100f)){
				isBlending = false;
				buttonBlendOrder = false;
				distance = 1000;
				performAction = false;
				
				switch(rayHit.transform.tag){
					
					//Move to click location if at floor level
				case "Tile" : 
					Vector3 targetDestination;
					currentBlendSurface = null;
					verticalDistance = Vector3.Distance(new Vector3(0,transform.position.y,0), new Vector3(0,rayHit.point.y,0));
					if(verticalDistance > 0.2f){				//Go to idle state if clicking above floor level.
						currentMoveState = MoveState.Idle;
						break;
					}
					else targetDestination = rayHit.point;
					SetMovement(rayHit.transform, targetDestination);
					performAction = false;
					break;
					
					//Hide against blend surface if correct colour
				case "Blend Surface" :
					if(rayHit.transform != currentBlendSurface){
						SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.3f));
						Color newColor = bodyParts[0].GetComponent<Renderer>().material.color;
						Color wallColor = rayHit.transform.GetComponent<Renderer>().material.color;
						colorDistance = Vector3.Distance(new Vector3(newColor.r, newColor.g, newColor.b), new Vector3(wallColor.r, wallColor.g, wallColor.b));
						if(colorDistance < 0.1f){
							performAction = true;
							currentBlendSurface = rayHit.transform;
							isBlending = true;
							transform.forward = -rayHit.transform.forward;
							currentMoveState = MoveState.Run;
						}
						else{
							performAction = false;
							isBlending = false;
							currentMoveState = MoveState.Run;
						}
					}
					break;
					
					//Pick up object
				case "Pickup" :
					currentBlendSurface = null;
					pickupItemTransform = rayHit.transform;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
					
					//Enter doorway
				case "Load Door" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
				}
				
				switch(rayHit.transform.name){
					//Enter doorway
				case "Threshold" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
					
					//Perform Teleport
				case "Teleporter 1" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;
					
					//Perform Teleport
				case "Teleporter 2" :
					currentBlendSurface = null;
					SetMovement(rayHit.transform, rayHit.transform.position + (-rayHit.transform.forward * 0.5f));
					performAction = true;
					break;	
					
					//Perform Teleport
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
		if(Vector3.Distance(transform.position, new Vector3(v.x, transform.position.y, v.z)) > 0.5f){
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
			transform.position = Vector3.MoveTowards(transform.position, currentBlendSurface.position, Time.deltaTime);
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
		
		if(distance < 0.4f){
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
		
		if(distance < 0.4f){
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
		else targetSpeed = walkSpeed;   
		
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
		
		if (agent.velocity != Vector3.zero) {
			
			float lookAtRate = normalLookAtRate;
			if (timeScale.timeSlowed)
				lookAtRate = slowTimeLookAtRate;
			else
				lookAtRate = normalLookAtRate;
			
			Quaternion rotation = Quaternion.LookRotation (targetPosition - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, customDeltaTime * lookAtRate);
		}
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
			if(colorDistance < 0.1f){
				transform.forward = -rayHit.transform.forward;
				isBlending = true;
				currentMoveState = MoveState.Blend_Stand;
			}
			else{
				currentMoveState = MoveState.Idle;
				
			}
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
			rayHit.transform.parent.Find("Door Trigger").GetComponent<Door>().StartNewTeleport();
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
		performAction = false;
		
		
		if(buttonBlendOrder){
			transform.forward = -buttonBlendObject.transform.forward;
			isBlending = true;
			currentMoveState = MoveState.Blend_Stand;
		}
	}
	
	//Blend button behaviour that triggers blend state
	public void blendButton(GameObject blendObject, Transform t, Vector3 v){
		SetMovement(t, t.position + (-t.forward * 0.3f));
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
