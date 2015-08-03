using UnityEngine;
using System.Collections;

public class TEMP_ITEM_TESTING : MonoBehaviour {

	public GameObject[] cubes;
	float actionTime = 2.0f;
	bool done = false;

	PlayerInventory playerInventory;

	// Use this for initialization
	void Start () {
		cubes = GameObject.FindGameObjectsWithTag ("Pickup");
		playerInventory = GameObject.Find("Inventory Manager").GetComponent<PlayerInventory>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= actionTime && !done){
			for(int i = 0; i < cubes.Length; i++){
				playerInventory.addItem(cubes[i]);
			}
			done = true;
		}
	}
}
