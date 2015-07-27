using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class RoomEditor : EditorWindow {

	GameObject 	newLevelObject,newRoomObject, newWallPillarObject, newWallSectionObject, exteriorObject, boundaryObject,
				roomGroupObject, navMeshPlaneObject;


	private int roomX, roomZ,
				roomTemplateIndex = 0;

	//public string[] roomTemplateOptions = new string[] {"Normal", "Small", "Large", "Wide", "Long"};
	public enum RoomTemplateOptions { 
		Normal = 0, 
		Small = 1, 
		Large = 2,
		Wide = 3, 
		Long = 4
	}


	Room roomScript;

	public RoomTemplateOptions selectedRoomTemplate;




	[MenuItem ("Window/Level Editor/Room Editor")]
	static void Init () {
		RoomEditor window = (RoomEditor)EditorWindow.GetWindow (typeof (RoomEditor));
		window.minSize = new Vector2(300,300);
		window.maxSize = new Vector2(300,300);
		window.maximized = true;
		window.Show();
	}

	void OnGUI(){
		SetEditorColor();
		if(Selection.activeGameObject){
			if(Selection.activeGameObject.GetComponent<Room>())CreateNewRoomMenu();
			else if(Selection.activeGameObject.tag == "Wall Section")EditWallSectionMenu();
			else if(Selection.activeGameObject.tag == "Wall Pillar")EditWallPillarMenu();
		}
		else CreateLevelMenu();
		Repaint();
	}

	void SetEditorColor(){
		GUI.color = new Color(0.6f,0.6f,0.8f);
		Texture2D windowTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		windowTexture.SetPixel(0, 0, new Color(0.4f, 0.4f, 0.6f));
		windowTexture.Apply();
		GUI.DrawTexture(new Rect(0, 0, 300, 300), windowTexture, ScaleMode.StretchToFill);
	}


	//CREATE LEVEL MENU____________________________________________________________________________________________________
	void CreateLevelMenu(){
		GUI.Label (new Rect(0, 0, 300, 50), "Please select one of the following to make \nchanges using the Room Editor:", EditorStyles.whiteBoldLabel);

		GUI.Label (new Rect(0, 35, 300, 50), "Room GameObject:", EditorStyles.whiteMiniLabel);
		GUI.Label (new Rect(0, 50, 300, 20), "Creates new rooms around selected room", EditorStyles.helpBox);

		GUI.Label (new Rect(0, 70, 300, 50), "Wall Section GameObject:", EditorStyles.whiteMiniLabel);
		GUI.Label (new Rect(0, 85, 300, 20), "Rotates and replaces wall sections", EditorStyles.helpBox);

		GUI.Label (new Rect(0, 105, 300, 50), "Wall Pillar GameObject:", EditorStyles.whiteMiniLabel);
		GUI.Label (new Rect(0, 120, 300, 20), "Creates walls and replaces pillars", EditorStyles.helpBox);

		GUI.Label (new Rect(0, 140, 300, 50), "Wall Block GameObject:", EditorStyles.whiteMiniLabel);
		GUI.Label (new Rect(0, 155, 300, 20), "Rotate and replace wall blocks", EditorStyles.helpBox);

		GUI.Label (new Rect(0, 175, 300, 50), "Hazard GameObject:", EditorStyles.whiteMiniLabel);
		GUI.Label (new Rect(0, 190, 300, 20), "Toggles different hazards", EditorStyles.helpBox);

		GUI.Label (new Rect(0, 210, 300, 50), "Level GameObject:", EditorStyles.whiteMiniLabel);
		GUI.Label (new Rect(0, 225, 300, 20), "Menu to finialize the", EditorStyles.helpBox);


		GUI.Label (new Rect(0, 242, 300, 50), "Otherwise click the button below to create \na new level:", EditorStyles.whiteBoldLabel);


		if (GUI.Button(new Rect(0, 270, 300, 30), "Create New Level")){
			Debug.Log("[LEVEL EDITOR] Creating new Level.");

			roomX = 0;
			roomZ = 0;

			newLevelObject = new GameObject("Level");
			roomGroupObject = new GameObject("Rooms");
			roomGroupObject.transform.parent = newLevelObject.transform;



			newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Large"), Vector3.zero + new Vector3(0,1000,0), Quaternion.identity) as GameObject;
			newRoomObject.transform.parent = roomGroupObject.transform;
			newRoomObject.name = "[0,0] Landing Pad";

			exteriorObject = Instantiate(Resources.Load("_Room Editor/Components/Basic Exterior"), newRoomObject.transform.position + new Vector3(0,-0.1f,0), Quaternion.identity) as GameObject; 
			exteriorObject.transform.parent = newRoomObject.transform;
			exteriorObject.name = "Exterior";



			GameObject landingPadMarker = Instantiate(Resources.Load("_Room Editor/Components/Landing Pad Marker"), Vector3.zero, Quaternion.identity) as GameObject; 
			landingPadMarker.transform.parent = newRoomObject.transform;
			landingPadMarker.name = "Landing Pad Marker";
			landingPadMarker.transform.eulerAngles = new Vector3(90,0,0);


			roomScript = newRoomObject.GetComponent<Room>();
			roomScript.xIndex = 0;
			roomScript.zIndex = 0;

			Selection.activeGameObject = newRoomObject;
		}
	}




	//CREATE ROOM MENU_____________________________________________________________________________________________________
	void CreateNewRoomMenu(){
		GUI.Label (new Rect(0, 0, 300, 30), "", EditorStyles.helpBox);
		selectedRoomTemplate = (RoomTemplateOptions) EditorGUILayout.EnumPopup("Replace room:", selectedRoomTemplate);
		GUI.Label (new Rect(0, 0, 300, 20), "Replace Room:", EditorStyles.whiteBoldLabel);


		if (GUI.Button(new Rect(0, 30, 300, 20), "Replace")){
			Debug.Log("[LEVEL EDITOR] Replacing selected Room.");
			switch(selectedRoomTemplate){
				case RoomTemplateOptions.Normal : newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Normal"), Selection.activeGameObject.transform.position, Quaternion.identity) as GameObject; break;
				case RoomTemplateOptions.Small : newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Small"), Selection.activeGameObject.transform.position, Quaternion.identity) as GameObject; break;
				case RoomTemplateOptions.Large : newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Large"), Selection.activeGameObject.transform.position, Quaternion.identity) as GameObject; break;
				case RoomTemplateOptions.Wide : newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Wide"), Selection.activeGameObject.transform.position, Quaternion.identity) as GameObject; break;
				case RoomTemplateOptions.Long : newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Long"), Selection.activeGameObject.transform.position, Quaternion.identity) as GameObject; break;
			}

			boundaryObject = Instantiate(Resources.Load("_Room Editor/Components/Room Boundary"), newRoomObject.transform.position, Quaternion.identity) as GameObject;
			boundaryObject.transform.parent = newRoomObject.transform;
			boundaryObject.name = "Boundary";

			navMeshPlaneObject = Instantiate(Resources.Load("_Room Editor/Components/Nav Mesh Floor"), newRoomObject.transform.position + new Vector3(0,0.06f,0), Quaternion.identity) as GameObject; 
			navMeshPlaneObject.transform.parent = newRoomObject.transform;
			navMeshPlaneObject.transform.eulerAngles = new Vector3(90,0,0);
			navMeshPlaneObject.name = "Nav Mesh Floor";

			roomScript = newRoomObject.GetComponent<Room>();
			roomScript.xIndex = Selection.activeTransform.GetComponent<Room>().xIndex;
			roomScript.zIndex = Selection.activeTransform.GetComponent<Room>().zIndex;

			newRoomObject.name = "["+roomScript.xIndex+","+roomScript.zIndex+"]";

			DestroyImmediate(Selection.activeGameObject);
			Selection.activeGameObject = newRoomObject;
		}

		GUI.Label (new Rect(0, 60, 300, 20), "Create Room:", EditorStyles.whiteBoldLabel);
		if (GUI.Button(new Rect(125, 80, 50, 50), "North")){
			CreateRoom(0,1);
		}
		if (GUI.Button(new Rect(125, 180, 50, 50), "South")){
			CreateRoom(0,-1);
		}
		if (GUI.Button(new Rect(175, 130, 50, 50), "East")){
			CreateRoom(1,0);
		}
		if (GUI.Button(new Rect(75, 130, 50, 50), "West")){
			CreateRoom(-1,0);
		}

		GUI.Label (new Rect(0, 260, 300, 20), "Click when you are finished editing the room:", EditorStyles.whiteBoldLabel);
		if (GUI.Button(new Rect(50, 275, 200, 25), "Remove unused Components")){
			CleanRoom(); CleanRoom(); CleanRoom(); CleanRoom(); CleanRoom(); CleanRoom(); CleanRoom();
		}
	}

	void CreateRoom(int xModifier, int zModifier){
		Debug.Log("[LEVEL EDITOR] Creating new Room.");
		roomScript = Selection.activeTransform.GetComponent<Room>();
		int newRoomX = roomScript.xIndex + xModifier;
		int newRoomZ = roomScript.zIndex + zModifier;

		if((newRoomX == 0) && (newRoomZ == 0))Debug.Log("[LEVEL EDITOR] A room already exists in that location.");
		else{
			if(GameObject.Find("["+newRoomX+","+newRoomZ+"]")){
				Debug.Log("[LEVEL EDITOR] A room already exists in that location.");
			}
			else{
				Debug.Log("[LEVEL EDITOR] Creating a new room.");
				newRoomObject = Instantiate(Resources.Load("_Room Editor/Default Rooms/Room Normal"), Vector3.zero + new Vector3(newRoomX * 150,0,newRoomZ * 150), Quaternion.identity) as GameObject;
				if(!roomGroupObject)roomGroupObject = Selection.activeTransform.parent.gameObject;
				newRoomObject.transform.parent = roomGroupObject.transform;
				newRoomObject.name = "["+newRoomX+","+newRoomZ+"]";
				
				boundaryObject = Instantiate(Resources.Load("_Room Editor/Components/Room Boundary"), newRoomObject.transform.position, Quaternion.identity) as GameObject;
				boundaryObject.transform.parent = newRoomObject.transform;
				boundaryObject.name = "Boundary";

				navMeshPlaneObject = Instantiate(Resources.Load("_Room Editor/Components/Nav Mesh Floor"), newRoomObject.transform.position + new Vector3(0,0.06f,0), Quaternion.identity) as GameObject; 
				navMeshPlaneObject.transform.parent = newRoomObject.transform;
				navMeshPlaneObject.transform.eulerAngles = new Vector3(90,0,0);
				navMeshPlaneObject.name = "Nav Mesh Floor";

				roomScript = newRoomObject.GetComponent<Room>();
				roomScript.xIndex = newRoomX;
				roomScript.zIndex = newRoomZ;
				
				Selection.activeGameObject = newRoomObject;
			}
		}
	}
	
	void CleanRoom(){
		Debug.Log("[LEVEL EDITOR] Cleaning selected Room.");
		Transform[] childObjects =  Selection.activeTransform.GetComponentsInChildren<Transform>();
		foreach (Transform child in childObjects) {
			//Debug.Log("[LEVEL EDITOR] " +child.name +" : " +child.gameObject.activeSelf);
			if(child.name == "Walls") foreach(Transform grandChild in child) if(grandChild.gameObject.activeSelf == false)   DestroyImmediate(grandChild.gameObject);
			if(child.name == "Wall Halves") foreach(Transform grandChild in child) if(grandChild.gameObject.activeSelf == false)   DestroyImmediate(grandChild.gameObject);
			if(child.name == "Wall Bases") foreach(Transform grandChild in child) if(grandChild.gameObject.activeSelf == false)   DestroyImmediate(grandChild.gameObject);
			if(child.name == "Doors") foreach(Transform grandChild in child) if(grandChild.gameObject.activeSelf == false)   DestroyImmediate(grandChild.gameObject);
			if(child.name == "Windows") foreach(Transform grandChild in child) if(grandChild.gameObject.activeSelf == false)   DestroyImmediate(grandChild.gameObject);
			if(child.name == "Lasers") if(child.gameObject.activeSelf == false) DestroyImmediate(child.gameObject);
			if(child.name == "Colour Surfaces") if(child.gameObject.activeSelf == false) DestroyImmediate(child.gameObject);

			if(child.name == "Tiles") {
				foreach(Transform grandChild in child){
					if(grandChild.gameObject.activeSelf == false){
						DestroyImmediate(grandChild.gameObject);
					}
					else{
						if(grandChild.Find("Lower Base")) DestroyImmediate(grandChild.Find("Lower Base").gameObject);
					}
				}
			}
		}
		//Remove unused walls
		
		
	}
	
	
	//EDIT COMPONENT MENU's________________________________________________________________________________________________
	void EditTileMenu(){

	}



	void EditWallPillarMenu(){
		GUI.Label (new Rect(0, 0, 300, 20), "Replace Pillar Component", EditorStyles.whiteBoldLabel);
		if (GUI.Button(new Rect(0, 20, 300, 30), "Next Pillar Object")){
			TogglePillarObject();
		}

		GUI.Label (new Rect(0, 60, 300, 20), "Create New Wall Section", EditorStyles.whiteBoldLabel);
		if (GUI.Button(new Rect(125, 80, 50, 50), "North")){
			newWallPillarObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Pillar"), Selection.activeTransform.position + new Vector3(0,0,2), Quaternion.identity) as GameObject;
			newWallPillarObject.transform.parent = Selection.activeTransform.parent.parent;
			newWallSectionObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Section"), Selection.activeTransform.position + new Vector3(0,0,1), Quaternion.identity) as GameObject;
			newWallSectionObject.transform.eulerAngles = new Vector3(0,90,0);
			newWallSectionObject.transform.parent = Selection.activeTransform.parent.parent;
			Selection.activeGameObject = newWallPillarObject.transform.Find("Pillar Normal").gameObject;
		}
		if (GUI.Button(new Rect(125, 180, 50, 50), "South")){
			newWallPillarObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Pillar"), Selection.activeTransform.position + new Vector3(0,0,-2), Quaternion.identity) as GameObject;
			newWallPillarObject.transform.parent = Selection.activeTransform.parent.parent;
			newWallSectionObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Section"), Selection.activeTransform.position + new Vector3(0,0,-1), Quaternion.identity) as GameObject;
			newWallSectionObject.transform.eulerAngles = new Vector3(0,90,0);
			newWallSectionObject.transform.parent = Selection.activeTransform.parent.parent;
			Selection.activeGameObject = newWallPillarObject.transform.Find("Pillar Normal").gameObject;
		}
		if (GUI.Button(new Rect(175, 130, 50, 50), "East")){
			newWallPillarObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Pillar"), Selection.activeTransform.position + new Vector3(2,0,0), Quaternion.identity) as GameObject;
			newWallPillarObject.transform.parent = Selection.activeTransform.parent.parent;
			newWallSectionObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Section"), Selection.activeTransform.position + new Vector3(1,0,0), Quaternion.identity) as GameObject;
			newWallSectionObject.transform.eulerAngles = new Vector3(0,180,0);
			newWallSectionObject.transform.parent = Selection.activeTransform.parent.parent;
			Selection.activeGameObject = newWallPillarObject.transform.Find("Pillar Normal").gameObject;
		}
		if (GUI.Button(new Rect(75, 130, 50, 50), "West")){
			newWallPillarObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Pillar"), Selection.activeTransform.position + new Vector3(-2,0,0), Quaternion.identity) as GameObject;
			newWallPillarObject.transform.parent = Selection.activeTransform.parent.parent;
			newWallSectionObject = Instantiate(Resources.Load("_Room Editor/Components/Wall Section"), Selection.activeTransform.position + new Vector3(-1,0,0), Quaternion.identity) as GameObject;
			newWallSectionObject.transform.eulerAngles = new Vector3(0,180,0);
			newWallSectionObject.transform.parent = Selection.activeTransform.parent.parent;
			Selection.activeGameObject = newWallPillarObject.transform.Find("Pillar Normal").gameObject;
		}
	}

	void TogglePillarObject(){

	}

	void EditWallSectionMenu(){
		GUI.Label (new Rect(0, 0, 300, 20), "Replace Wall Section", EditorStyles.whiteBoldLabel);
		if (GUI.Button(new Rect(0,  20, 100, 50), "Wall")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
							case "1 Wall Basic" 					: wallSectionComponent.transform.parent.Find("2 Wall Panel Rectangle").gameObject.SetActive(true); break;
							case "2 Wall Panel Rectangle" 			: wallSectionComponent.transform.parent.Find("3 Wall Curved Shelf").gameObject.SetActive(true); break;
							case "3 Wall Curved Shelf" 				: wallSectionComponent.transform.parent.Find("4 Wall Shelf").gameObject.SetActive(true); break;
							case "4 Wall Shelf" 					: wallSectionComponent.transform.parent.Find("5 Wall Panel Rectangle Double").gameObject.SetActive(true); break;
							case "5 Wall Panel Rectangle Double" 	: wallSectionComponent.transform.parent.Find("6 Wall Curved Top").gameObject.SetActive(true); break;
							case "6 Wall Curved Top" 				: wallSectionComponent.transform.parent.Find("7 Wall Sloped").gameObject.SetActive(true); break;
							case "7 Wall Sloped" 					: wallSectionComponent.transform.parent.Find("8 Wall Sloped Base").gameObject.SetActive(true); break;
							case "8 Wall Sloped Base" 				: wallSectionComponent.transform.parent.Find("1 Wall Basic").gameObject.SetActive(true); break;
							default									: wallSectionComponent.transform.parent.parent.Find("Walls").Find("1 Wall Basic").gameObject.SetActive(true); break;
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

		if(GUI.Button(new Rect(100,  20, 100, 50), "Wall Half")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Wall Half Basic" 					: wallSectionComponent.transform.parent.Find("2 Wall Half End Curves").gameObject.SetActive(true); break;
						case "2 Wall Half End Curves" 				: wallSectionComponent.transform.parent.Find("3 Wall Half End Curve Left").gameObject.SetActive(true); break;
						case "3 Wall Half End Curve Left" 			: wallSectionComponent.transform.parent.Find("4 Wall Half End Curve Right").gameObject.SetActive(true); break;
						case "4 Wall Half End Curve Right" 			: wallSectionComponent.transform.parent.Find("5 Wall Half Curved").gameObject.SetActive(true); break;
						case "5 Wall Half Curved" 					: wallSectionComponent.transform.parent.Find("6 Wall Half Sloped").gameObject.SetActive(true); break;
						case "6 Wall Half Sloped" 					: wallSectionComponent.transform.parent.Find("1 Wall Half Basic").gameObject.SetActive(true); break;
						default										: wallSectionComponent.transform.parent.parent.Find("Wall Halves").Find("1 Wall Half Basic").gameObject.SetActive(true); break;
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}
		
		if(GUI.Button(new Rect(200,  20, 100, 50), "Wall Base")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Wall Base" 						: wallSectionComponent.transform.parent.Find("2 Wall Base Curved Right").gameObject.SetActive(true); break;
						case "2 Wall Base Curved Right" 		: wallSectionComponent.transform.parent.Find("3 Wall Base Curved Left").gameObject.SetActive(true); break;
						case "3 Wall Base Curved Left" 			: wallSectionComponent.transform.parent.Find("1 Wall Base").gameObject.SetActive(true); break;
						default									: wallSectionComponent.transform.parent.parent.Find("Wall Bases").Find("1 Wall Base").gameObject.SetActive(true); break;
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

		if(GUI.Button(new Rect(50,  70, 100, 50), "Door")){
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Door Curved" 					: wallSectionComponent.transform.parent.Find("2 Door Curved Frame").gameObject.SetActive(true); break;
						case "2 Door Curved Frame" 				: wallSectionComponent.transform.parent.Find("3 Door Curved Wide").gameObject.SetActive(true); break;
						case "3 Door Curved Wide" 				: wallSectionComponent.transform.parent.Find("4 Door Curved Wide Frame").gameObject.SetActive(true); break;
						case "4 Door Curved Wide Frame" 		: wallSectionComponent.transform.parent.Find("5 Door Rectangle Wide").gameObject.SetActive(true); break;
						case "5 Door Rectangle Wide" 			: wallSectionComponent.transform.parent.Find("6 Door Rectangle").gameObject.SetActive(true); break;
						case "6 Door Rectangle" 				: wallSectionComponent.transform.parent.Find("7 Door Rectangle Frame").gameObject.SetActive(true); break;
						case "7 Door Rectangle Frame" 			: wallSectionComponent.transform.parent.Find("1 Door Curved").gameObject.SetActive(true); break;
						default									: wallSectionComponent.transform.parent.parent.Find("Doors").Find("1 Door Curved").gameObject.SetActive(true); break;
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

		if(GUI.Button(new Rect(150,  70, 100, 50), "Window")){
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Window Small Curved" 					: wallSectionComponent.transform.parent.Find("2 Window Small Curved Double").gameObject.SetActive(true); break;
						case "2 Window Small Curved Double" 				: wallSectionComponent.transform.parent.Find("3 Window Large Curved").gameObject.SetActive(true); break;
						case "3 Window Large Curved" 				: wallSectionComponent.transform.parent.Find("4 Window Medium Curved").gameObject.SetActive(true); break;
						case "4 Window Medium Curved" 		: wallSectionComponent.transform.parent.Find("5 Window Rectangle Thick Frame").gameObject.SetActive(true); break;
						case "5 Window Rectangle Thick Frame" 			: wallSectionComponent.transform.parent.Find("6 Window Large Curved Wall").gameObject.SetActive(true); break;
						case "6 Window Large Curved Wall" 				: wallSectionComponent.transform.parent.Find("7 Window Narrow Double Curved Wall").gameObject.SetActive(true); break;
						case "7 Window Narrow Double Curved Wall" 			: wallSectionComponent.transform.parent.Find("8 Window Narrow Curved Wall").gameObject.SetActive(true); break;
						case "8 Window Narrow Curved Wall" 		: wallSectionComponent.transform.parent.Find("9 Window Square Large").gameObject.SetActive(true); break;
						case "9 Window Square Large" 			: wallSectionComponent.transform.parent.Find("10 Window Rectangle Double").gameObject.SetActive(true); break;
						case "10 Window Rectangle Double" 				: wallSectionComponent.transform.parent.Find("11 Window Rectangle Double Frame").gameObject.SetActive(true); break;
						case "11 Window Rectangle Double Frame" 			: wallSectionComponent.transform.parent.Find("1 Window Small Curved").gameObject.SetActive(true); break;

						default									: wallSectionComponent.transform.parent.parent.Find("Windows").Find("1 Window Small Curved").gameObject.SetActive(true); break;
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}





		GUI.Label (new Rect(0, 140, 300, 20), "Toggle Wall Component", EditorStyles.whiteBoldLabel);

		if(GUI.Button(new Rect(50,  160, 100, 50), "Colour Surface")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						wallSectionComponent.transform.parent.parent.Find("Colour Surfaces").gameObject.SetActive(!wallSectionComponent.transform.parent.parent.Find("Colour Surfaces").gameObject.activeSelf);
					}
				}
			}
		}
		
		if(GUI.Button(new Rect(150,  160, 100, 50), "Laser")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						wallSectionComponent.transform.parent.parent.Find("Lasers").gameObject.SetActive(!wallSectionComponent.transform.parent.parent.Find("Lasers").gameObject.activeSelf);
					}
				}
			}
		}

		GUI.Label 	 (new Rect(0,   210, 200, 20), "Rotate Wall Section", EditorStyles.whiteBoldLabel);
		if(GUI.Button(new Rect(150, 230, 100, 40), "Clockwise")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						wallSectionComponent.transform.parent.parent.eulerAngles += new Vector3(0,45,0);
					}
				}
			}
		}
		if(GUI.Button(new Rect(50, 230, 100, 40), "Anti-Clockwise")){
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						wallSectionComponent.transform.parent.parent.eulerAngles += new Vector3(0,-45,0);
					}
				}
			}
		}
	}


}
