/// <summary>
/// 2015
/// Ben Redahan, redahanb@gmail.com
/// Project: Spectral - The Silicon Domain (Unity)
/// GameState component: handles the GameState for transitions between scenes, Saving and Loading savefiles
/// </summary>

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameState : MonoBehaviour {

	public static GameState 	gameState;

	private HealthManager 		pHealth;
	private PlayerInventory		pInventory;
	private TimeScaler 			pTime;
	
	// Singleton design pattern - there can only be one GameState Object: the one from the first scene loaded
	void Awake () 
	{
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
		// Cache references to scripts that utilise GameState variables
		pHealth = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		pInventory = GameObject.Find ("Inventory Manager").GetComponent<PlayerInventory> ();
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

		} else {
			print ("No save file found... Initialising default player stats.");
			pHealth.maxHealth = 3;
			pHealth.playerHealth = 3;
			pInventory.inventorySize = 4;

		}
	} // end LoadGame

	
	[Serializable]
	private class SaveData
	// Private class to serve as a container for all relevant data to be saved
	{
		// Health Data
		public int currentHealth;
		public int maxHealth;

		// Inventory Data
		public int inventorySize;

		// Upgrade Data
		public float timeSlowFactor;
		public float timeSlowCharge;

	} // end SaveData class
}
