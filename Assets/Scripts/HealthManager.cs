using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	public int maxHealth;
	public int playerHealth;
	public bool playerDead = false;

	PlayerBodyparts bodyParts;
	HUD_Healthbar healthUI;

	// Use this for initialization
	void Start () {
		// Pull health data from GameState object
//		maxHealth = GameState.data.maxHealth;
//		playerHealth = GameState.data.currentHealth;
		healthUI = GameObject.Find ("HUD_Healthbar").GetComponent<HUD_Healthbar>();
		healthUI.buildHealthbarUI (maxHealth);
		healthUI.healthBarSize = maxHealth;
		bodyParts = GameObject.FindWithTag ("Player").GetComponent<PlayerBodyparts> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth <= 0) {
			playerDead = true;
		}
	}

	public void takeDamage(int damage){
		if(playerHealth > 0){
			playerHealth -= damage;
		}

		if(playerHealth <= 0){
			//print ("Player was killed...");
			bodyParts.selfDestruct();
		}
	}
}
