/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// GameState component: handles the GameState for transitions between scenes, Saving and Loading savefiles
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameState : MonoBehaviour {

	public static GameState 	gameState;

	private HealthManager 		pHealth;
	private PlayerInventory		pInventory;
	private InventoryItem 		invItem;
	private HUD_Inventory 		invHUD;
	private TimeScaler 			pTime;
	private Color 				tempColor;
	
	// Singleton design pattern - there can only be one GameState Object: the one from the first scene loaded
	void Awake () 
	{
		print ("GameState awake!");
		if (gameState == null) {
			// If this is the first scene loaded, set this as the GameState object
			DontDestroyOnLoad (gameObject);
			gameState = this;
		} else {
			// If a GameState object already exists, destroy this object
			if(gameState != gameObject){
				Destroy(gameObject);
			}
		}

//		LoadGame ();
	}
	
	void Start()
	{
		print ("GameState start!");
		// Cache references to scripts that utilise GameState variables
		pHealth = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		pInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
		invHUD = GameObject.Find ("HUD_Inventory").GetComponent<HUD_Inventory> ();
		pTime = GameObject.Find("Time Manager").GetComponent<TimeScaler>();

		LoadGame ();
	}

	void Update () 
	{

	}

	public void SaveGame()
	{
		// Create a save file and empty savedata object
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		SaveData data = new SaveData ();

		// Store player stats into savedata
		data.currentHealth = pHealth.playerHealth;
		data.maxHealth = pHealth.maxHealth;

		data.inventorySize = pInventory.inventorySize;

		data.itemNames = new string[pInventory.inventorySize];
		//data.itemColors = new Vector3[pInventory.inventorySize];
		data.itemColours = new float[pInventory.inventorySize][];

		for(int x = 0; x < pInventory.inventorySize; x++){
			if(pInventory.playerInventory[x] != null){
				InventoryItem tempInfo = pInventory.playerInventory[x].GetComponent<InventoryItem>();
				data.itemNames[x] = tempInfo.name;
				tempColor = tempInfo.itemColor;
				//data.itemColors[x] = new Vector3(tempColor.r, tempColor.b, tempColor.g);
				float[] tempArray = new float[3]{tempColor.r, tempColor.b, tempColor.g};
				data.itemColours[x] = tempArray;
			}
		}

		// Commit the savedata into the savefile
		bf.Serialize (file, data);
		file.Close ();
		print ("Saving Game...");
	} // end SaveGame

	public void LoadGame()
	{
		BinaryFormatter bf = new BinaryFormatter();
		SaveData data;

		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			print ("Save file found!");
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			data = (SaveData)bf.Deserialize (file);
			file.Close ();

			// Update player stats from save file
			pHealth.maxHealth = data.maxHealth;
			pHealth.playerHealth = data.currentHealth;
			pInventory.inventorySize = data.inventorySize;
			pInventory.playerInventory = new GameObject[pInventory.inventorySize];

			for(int x = 0; x < data.inventorySize; x++){
				if(data.itemNames[x] != null)
				{
					GameObject tempItem = Instantiate( Resources.Load("Pickups/" + data.itemNames[x]), new Vector3(-50,-50,-50), Quaternion.identity ) as GameObject;
					invItem = tempItem.GetComponent<InventoryItem>();

					// Update item stats
					invItem.name = data.itemNames[x];
					invItem.itemName = data.itemNames[x];
					tempColor = new Color();
					tempColor.r = data.itemColours[x][0];
					tempColor.b = data.itemColours[x][1];
					tempColor.g = data.itemColours[x][2];
					tempColor.a = 1.0f;
					invItem.itemColor = tempColor;

					// Hide and disable, and add to player inventory
					pInventory.playerInventory[x] = tempItem;
					//pInventory.addItem(tempItem);
					tempItem.SetActive(false);
					//invHUD.updateIcon(x);
					}
			}
			invHUD.buildInventoryUI (pInventory.inventorySize);
			invHUD.UpdateAllIcons();
		} else {
			print ("No save file found... Initialising default player stats.");
			pHealth.maxHealth = 3;
			pHealth.playerHealth = 3;
			pInventory.inventorySize = 4;
			pInventory.playerInventory = new GameObject[4];
			invHUD.buildInventoryUI (4);

		}
	} // end LoadGame

	
	[Serializable]
	private class SaveData
	// Private class to serve as a container for all relevant data to be saved
	{
		// Health Data
		public int 				currentHealth;
		public int 				maxHealth;

		// Inventory Data
		public int 				inventorySize;

		public string[] 		itemNames;
		public int[] 			itemValues;
		//public Vector3[] 		itemColors;
		public float[][] 		itemColours;

		// Upgrade Data
		//public float 			timeSlowFactor;
		//public float 			timeSlowCharge;

	} // end SaveData class
	
}
