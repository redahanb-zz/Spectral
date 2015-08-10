using UnityEngine;
using UnityEditor;

public class MeshCombiner : EditorWindow {

	GameObject meshObject, selectedMeshGroup;

	string newMeshName = "Mesh Name";


	[MenuItem ("Window/Level Editor/Mesh Combiner")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		MeshCombiner window = (MeshCombiner)EditorWindow.GetWindow (typeof (MeshCombiner));
		window.minSize = new Vector2(300,500);
		window.maxSize = new Vector2(300,500);
		window.maximized = true;
		window.Show();
	}

	void OnGUI(){
		if(Selection.activeGameObject)CreateNewMesh();
		else DisplayHelp();
	}

	void CreateNewMesh(){
		GUILayout.Label ("Create New Mesh from selection", EditorStyles.whiteBoldLabel);
		newMeshName = EditorGUILayout.TextField ("New Mesh Name", newMeshName);
		if (GUILayout.Button("Combine!"))CombineSelectedMeshes();
	}

	void DisplayHelp(){
		GUILayout.Label("Select one or more Gameobjects to begin combining meshes", EditorStyles.helpBox);
	}

	void CombineSelectedMeshes(){
		meshObject = new GameObject();
		meshObject.name = "New Mesh Object";
		meshObject.AddComponent<MeshFilter>();
		meshObject.AddComponent<MeshRenderer>();

		selectedMeshGroup = new GameObject();
		selectedMeshGroup.name = "Selected Mesh Group";
		selectedMeshGroup.AddComponent<MeshFilter>();
		selectedMeshGroup.AddComponent<MeshRenderer>();


		foreach(Transform t in Selection.transforms){
			t.parent =  selectedMeshGroup.transform;
		}

		selectedMeshGroup.transform.position = Vector3.zero;
		selectedMeshGroup.transform.rotation = Quaternion.identity;

		MeshFilter[] meshFilters = selectedMeshGroup.GetComponentsInChildren<MeshFilter>();

		CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
		int index = 0;
		for (var i = 0; i < meshFilters.Length; i++){
			if (meshFilters[i].sharedMesh == null) continue;
			combine[index].mesh = meshFilters[i].sharedMesh;
			combine[index++].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].transform.GetComponent<Renderer>().enabled = false;
		}

		selectedMeshGroup.GetComponent<MeshFilter>().sharedMesh = new Mesh();
		selectedMeshGroup.GetComponent<MeshFilter>().sharedMesh.CombineMeshes (combine);
		selectedMeshGroup.GetComponent<Renderer>().material = meshFilters[1].GetComponent<Renderer>().sharedMaterial;

		while(selectedMeshGroup.transform.childCount != 0){
			DestroyImmediate(selectedMeshGroup.transform.GetChild(0).gameObject);
		}
	
		selectedMeshGroup.name = newMeshName;
		Selection.activeGameObject = selectedMeshGroup;

		DestroyImmediate(meshObject);

		SaveMeshToAssets();
	}

	void SaveMeshToAssets(){

		//Create Mesh Folder
		AssetDatabase.CreateFolder("Assets/Meshes/Room Components", newMeshName);
		
		//Save Mesh
		Mesh newMesh = selectedMeshGroup.GetComponent<MeshFilter>().sharedMesh;
		AssetDatabase.CreateAsset( newMesh, "Assets/Meshes/Room Components/"+newMeshName+"/" +newMeshName+" .asset");
		Debug.Log(AssetDatabase.GetAssetPath(newMesh));

		//Save All Assets
		AssetDatabase.SaveAssets();

		//Create Prefab
		//Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Resources/_ROOMS/"+roomName+".prefab");
		//EditorUtility.ReplacePrefab(Selection.activeGameObject.transform.parent.parent.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
		
	}
}
