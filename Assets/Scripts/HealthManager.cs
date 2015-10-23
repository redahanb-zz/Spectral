using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

	public int maxHealth;
	public int playerHealth;
	public bool playerDead = false;

	PlayerBodyparts bodyParts;

	// Use this for initialization
	void Start () {
//		maxHealth = 5;
//		playerHealth = 5;

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
