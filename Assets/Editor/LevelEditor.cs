using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class LevelEditor : EditorWindow {

	GameObject 	levelObject, layoutObject, layoutChunkObject, baseObject,
				floorObject, groundObject, roomObject, newRoomObject,
	wallObject, tileObject, wallSectionsObject,
				cameraTriggerObject, cameraPositionObject;
	
	Vector2 chunkIndex;

	string[] wallTypes = new string[2]{"Wall Basic", "Wall Shelf"};
	string currentWallType;


	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Level Editor")]
	static void Init () {
		LevelEditor window = (LevelEditor)EditorWindow.GetWindow (typeof (LevelEditor));
		window.minSize = new Vector2(300,300);
		window.maxSize = new Vector2(300,300);
		window.maximized = true;
		window.Show();
	}
	
	void OnGUI () {
		if(Selection.activeGameObject){
			if(Selection.activeGameObject.tag == "Layout")EditLevelLayoutMenu();
			else if(Selection.activeGameObject.tag == "Empty Tile")RoomEditor();
			else if(Selection.activeGameObject.tag == "Wall Section")WallSectionMenu();
			else if(Selection.activeGameObject.tag == "Wall Pillar")WallPillarMenu();
		}
		else CreateLevelLayoutMenu();

		Repaint();
	}

	void CreateLevelLayoutMenu(){
		GUI.color = new Color(0.1f, 0.8f,0.1f,1);
		if (GUI.Button(new Rect(75, 75, 150, 150), "Create \n New Level")){
			Debug.Log("[LEVEL EDITOR] Creating new Level.");

			//Level Object
			levelObject = new GameObject("Level");
			levelObject.transform.position = Vector3.zero;

			//Layout Object
			layoutObject = new GameObject("Layout");
			layoutObject.transform.parent = levelObject.transform;
			chunkIndex = new Vector2(0,0);

			//Ground Object
			groundObject = Instantiate(Resources.Load("Editor/Ground"), new Vector3(0,-80,0), Quaternion.identity) as GameObject;
			groundObject.transform.parent = levelObject.transform;
			groundObject.name = "Ground";

			//Initial Layout Chunk
			layoutChunkObject = Instantiate(Resources.Load("Editor/LevelLayoutChunk"), new Vector3(5,0,5), Quaternion.identity) as GameObject;
			layoutChunkObject.transform.parent = layoutObject.transform;
			layoutChunkObject.GetComponent<LevelChunk>().chunkIndex = chunkIndex;
			layoutChunkObject.name = "Chunk["+chunkIndex.x+","+chunkIndex.y +"]";
		}
		GUI.color = new Color(1,1,1,1);
	}

	void EditLevelLayoutMenu(){
		GUI.Label (new Rect(100, 20, 200, 40), "Extend Level Size:", EditorStyles.boldLabel);
		if (GUI.Button(new Rect(120,  60, 60, 60), "North"))CreateNewLayout("North");
		if (GUI.Button(new Rect(120, 180, 60, 60), "South"))CreateNewLayout("South");
		if (GUI.Button(new Rect(180, 120, 60, 60),  "East"))CreateNewLayout("East");
		if (GUI.Button(new Rect(60,  120, 60, 60),  "West"))CreateNewLayout("West");

		//Removes layout and replaces with Tiles
		if (GUI.Button(new Rect(0, 280, 300, 20), "Convert into Tiles")){
			Debug.Log("[LEVEL EDITOR] Converting Layout in Tiles.");

			baseObject = new GameObject("Base");
			baseObject.transform.parent = levelObject.transform;

			roomObject = new GameObject("Rooms");
			roomObject.transform.parent = levelObject.transform;

			tileObject = new GameObject("Tiles");
			tileObject.transform.parent = levelObject.transform;

			wallObject = new GameObject("Walls");
			wallObject.transform.parent = levelObject.transform;

			GameObject[] chunkObjects = GameObject.FindGameObjectsWithTag("Layout");
			foreach(GameObject g in chunkObjects){
				g.transform.Find("Base").parent = baseObject.transform;

				g.GetComponent<Renderer>().enabled = false;
				g.transform.Find("Tiles").gameObject.SetActive(true);
				GameObject[] editorTiles = GameObject.FindGameObjectsWithTag("Empty Tile");
				foreach(GameObject tile in editorTiles){
					tile.transform.parent = tileObject.transform;
				}
				DestroyImmediate(g);
			}
			DestroyImmediate(layoutObject);
		}



	}



	void CreateNewLayout(string direction){
		Vector3 newLayoutPosition = Vector3.zero;
		Vector2 indexModifier = Vector2.zero;

		chunkIndex = Selection.activeGameObject.GetComponent<LevelChunk>().chunkIndex;

		switch(direction){
			case "North" : newLayoutPosition = Selection.activeGameObject.transform.position + new Vector3(  0, 0,  10); indexModifier = new Vector2(0,1); break;
			case "South" : newLayoutPosition = Selection.activeGameObject.transform.position + new Vector3(  0, 0, -10); indexModifier = new Vector2(0,-1); break;
			case "East"  : newLayoutPosition = Selection.activeGameObject.transform.position + new Vector3( 10, 0,   0); indexModifier = new Vector2(1,0); break;
			case "West"  : newLayoutPosition = Selection.activeGameObject.transform.position + new Vector3(-10, 0,   0); indexModifier = new Vector2(-1,0); break;
			default:newLayoutPosition = Selection.activeGameObject.transform.position + new Vector3(0,0,10); break;
		}

		Vector2 nextChunkIndex = chunkIndex + indexModifier;
		bool canCreateChunk = true;

		foreach(Transform t in layoutObject.transform){
			if(t.GetComponent<LevelChunk>().chunkIndex  == nextChunkIndex){
				canCreateChunk = false;
				Debug.Log("[LEVEL EDITOR] There is already a tile in that location.");
				break;
			}
		}

		if(canCreateChunk){
			Debug.Log("[LEVEL EDITOR] Extending level layout " +direction);
			layoutChunkObject = Instantiate(Resources.Load("Editor/LevelLayoutChunk"), newLayoutPosition, Quaternion.identity) as GameObject;
			layoutChunkObject.transform.parent = layoutObject.transform;
			layoutChunkObject.name = "Chunk["+(nextChunkIndex.x) +","+ (nextChunkIndex.y) +"]";
			layoutChunkObject.GetComponent<LevelChunk>().chunkIndex = nextChunkIndex;
			Selection.activeGameObject = layoutChunkObject;
		}
	}








	void RoomEditor(){

		if (GUI.Button(new Rect(0, 240, 300, 20), "Create a solid block")){
			foreach(GameObject g in Selection.gameObjects){
				GameObject wallFillerObject = Instantiate(Resources.Load("Editor/Wall Filler"), g.transform.position + new Vector3(0,1.5f,0), Quaternion.identity) as GameObject;
				wallFillerObject.transform.parent = g.transform;
				g.GetComponent<Renderer>().enabled = false;
			}


		}

		if (GUI.Button(new Rect(0, 280, 300, 20), "Create room from Tile(s)")){
			newRoomObject = new GameObject("Room");

			newRoomObject.transform.position = Selection.activeTransform.position;
			newRoomObject.transform.parent = roomObject.transform;

			cameraTriggerObject = new GameObject("Camera Trigger");
			cameraTriggerObject.transform.parent = newRoomObject.transform;

			cameraPositionObject = Instantiate(Resources.Load("Editor/CameraPosRot"), Selection.activeTransform.position + new Vector3(-10, 10f,-10), Quaternion.identity) as GameObject;
			cameraPositionObject.transform.eulerAngles = new Vector3(30,45,0);
			cameraPositionObject.transform.parent = cameraTriggerObject.transform;

			floorObject = new GameObject("Floor");
			floorObject.transform.parent = newRoomObject.transform;

			wallSectionsObject = new GameObject("Walls");
			wallSectionsObject.transform.parent = newRoomObject.transform;

			foreach(Transform t in Selection.transforms){
				GameObject triggerObject = Instantiate(Resources.Load("Editor/Camera Trigger"), t.position + new Vector3(0,1.5f,0), Quaternion.identity) as GameObject;
				triggerObject.transform.parent = cameraTriggerObject.transform;
				triggerObject.GetComponent<Renderer>().enabled = false;
				t.parent = floorObject.transform;
				t.GetComponent<Renderer>().material = Resources.Load("Materials/Debug Tile") as Material;
				t.tag = "Room Tile";
			}
			CreatePillars();
		}
	}


	void CreatePillars(){
		foreach(GameObject g in Selection.gameObjects){
			RaycastHit hit;

			if (Physics.Raycast(g.transform.position + new Vector3(0.98f,10,0.98f), -Vector3.up, out hit, 100.0F)){
				Debug.Log("TAG: "+hit.transform.tag +" NAME: "+hit.transform.name);
				if(hit.transform.tag == "Room Tile"){
					GameObject pillarObject = Instantiate(Resources.Load("Editor/Wall Pillar"), 
					                                      g.transform.position + new Vector3(1,1.5f,1), 
					                                      Quaternion.identity) as GameObject;
					pillarObject.transform.parent = wallObject.transform;
				}
			}
			if (Physics.Raycast(g.transform.position + new Vector3(-0.98f,10,-0.98f), -Vector3.up, out hit, 100.0F)){
				if(hit.transform.tag == "Room Tile"){
					GameObject pillarObject = Instantiate(Resources.Load("Editor/Wall Pillar"), 
					                                      g.transform.position + new Vector3(-1,1.5f,-1), 
					                                      Quaternion.identity) as GameObject;
					pillarObject.transform.parent = wallObject.transform;
				}
			}
			if (Physics.Raycast(g.transform.position + new Vector3(0.98f,10,-0.98f), -Vector3.up, out hit, 100.0F)){
				if(hit.transform.tag == "Room Tile"){
					GameObject pillarObject = Instantiate(Resources.Load("Editor/Wall Pillar"), 
					                                      g.transform.position + new Vector3(1,1.5f,-1), 
					                                      Quaternion.identity) as GameObject;
					pillarObject.transform.parent = wallObject.transform;
				}
			}
			if (Physics.Raycast(g.transform.position + new Vector3(-0.98f,10,0.98f), -Vector3.up, out hit, 100.0F)){
				if(hit.transform.tag == "Room Tile"){
					GameObject pillarObject = Instantiate(Resources.Load("Editor/Wall Pillar"), 
					                                      g.transform.position + new Vector3(-1,1.5f,1), 
					                                      Quaternion.identity) as GameObject;
					pillarObject.transform.parent = wallObject.transform;
				}
			}
		}
	}



	void WallPillarMenu(){
		Vector3 newWallPos = Vector3.zero, newWallRot;
		GUI.Label (new Rect(100, 20, 200, 40), "Create New Wall Section:", EditorStyles.boldLabel);
		if (GUI.Button(new Rect(120,  60, 60, 60), "North")){
			newWallPos = new Vector3(0,0,1); 
			newWallRot = new Vector3(0,0,0); 
			CreateWallSection(newWallPos, newWallRot);
		}
		if (GUI.Button(new Rect(120, 180, 60, 60), "South")){
			newWallPos = new Vector3(0,0,-1); 
			newWallRot = new Vector3(0,0,0); 
			CreateWallSection(newWallPos, newWallRot);
		}
		if (GUI.Button(new Rect(180, 120, 60, 60),  "East")){
			newWallPos = new Vector3(1,0,0); 
			newWallRot = new Vector3(0,90,0); 
			CreateWallSection(newWallPos, newWallRot);
		}
		if (GUI.Button(new Rect(60,  120, 60, 60),  "West")){
			newWallPos = new Vector3(-1,0,0); 
			newWallRot = new Vector3(0,90,0); 
			CreateWallSection(newWallPos, newWallRot);
		}
	}

	void CreateWallSection(Vector3 pos, Vector3 rot){
		Vector3 newPos = Vector3.zero;
		foreach(Transform t in Selection.transforms){
			newPos = pos + t.transform.position;
			GameObject newWallObj = Instantiate(Resources.Load("Editor/Wall Section"),newPos,Quaternion.identity) as GameObject;
			newWallObj.transform.eulerAngles = rot;
			newWallObj.transform.parent = wallObject.transform;
		}
	}

	void WallSectionMenu(){

		//ROTATE SECTION_____________________________________________

		GUI.Label (new Rect(20, 5, 260, 40), "Rotate selected wall sections by 45º", EditorStyles.boldLabel);
		if (GUI.Button(new Rect(20,  20, 120, 30), "Counter-clockwise")){
			foreach(Transform t in Selection.transforms)t.eulerAngles = t.eulerAngles + new Vector3(0,-45,0);
		}
		if (GUI.Button(new Rect(160,  20, 120, 30), "Clockwise")){
			foreach(Transform t in Selection.transforms)t.eulerAngles = t.eulerAngles + new Vector3(0,45,0);
		}


		//REPLACE SECTION_____________________________________________
		GUI.Label (new Rect(20, 65, 260, 40), "Replace selected wall sections", EditorStyles.boldLabel);
		//Toggle Wall Components
		if (GUI.Button(new Rect(10,  80, 60, 30), "Wall")){
			//Debug.Log("[LEVEL EDITOR] Replacing wall section with Wall.");
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
							case "1 Wall Basic" : wallSectionComponent.transform.parent.Find("2 Wall Shelf").gameObject.SetActive(true); break;
							case "2 Wall Shelf" : wallSectionComponent.transform.parent.Find("1 Wall Basic").gameObject.SetActive(true); break;
							default: wallSectionComponent.transform.parent.parent.Find("Walls").Find("1 Wall Basic").gameObject.SetActive(true); break;
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

		if (GUI.Button(new Rect(80,  80, 60, 30), "Half Wall")){
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Wall Half" : wallSectionComponent.transform.parent.Find("2 Wall Half Raised").gameObject.SetActive(true); break;
						case "2 Wall Half Raised" : wallSectionComponent.transform.parent.Find("1 Wall Half").gameObject.SetActive(true); break;
						default: wallSectionComponent.transform.parent.parent.Find("Wall Halves").Find("1 Wall Half").gameObject.SetActive(true); break;

						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

		//Toggle Door Components
		if (GUI.Button(new Rect(150,  80, 60, 30), "Door")){
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Door Single" : wallSectionComponent.transform.parent.Find("2 Door Double").gameObject.SetActive(true); break;
						case "2 Door Double" : wallSectionComponent.transform.parent.Find("1 Door Single").gameObject.SetActive(true); break;
						default: wallSectionComponent.transform.parent.parent.Find("Doors").Find("1 Door Single").gameObject.SetActive(true); break;
							
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

		//Toggle Window Components
		if (GUI.Button(new Rect(220,  80, 60, 30), "Window")){
			foreach(Transform wallSection in Selection.transforms){
				foreach(Transform wallSectionComponent in wallSection.transform.parent){
					if(wallSectionComponent.gameObject.activeSelf == true){
						switch(wallSectionComponent.name){
						case "1 Window Large" : wallSectionComponent.transform.parent.Find("2 Window Medium").gameObject.SetActive(true); break;
						case "2 Window Medium" : wallSectionComponent.transform.parent.Find("3 Window Small Double").gameObject.SetActive(true); break;
						case "3 Window Small Double" : wallSectionComponent.transform.parent.Find("1 Window Large").gameObject.SetActive(true); break;
						default: wallSectionComponent.transform.parent.parent.Find("Windows").Find("1 Window Large").gameObject.SetActive(true); break;
							
						}
						wallSectionComponent.gameObject.SetActive(false);
						break;
					}
				}
			}
		}

	}

	void DisableAllWallComponents(Transform wallObject){
		foreach(Transform t in wallObject.Find("Walls")){
			t.gameObject.SetActive(false);
		}
		foreach(Transform t in wallObject.Find("Doors")){
			t.gameObject.SetActive(false);
		}
		foreach(Transform t in wallObject.Find("Windows")){
			t.gameObject.SetActive(false);
		}
	}
}