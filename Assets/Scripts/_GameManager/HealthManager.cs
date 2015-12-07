using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour
{

    public 		int 			maxHealth;
    public 		int 			playerHealth;
    public 		bool 			playerDead = false;

    private 	PlayerBodyparts bodyParts;
    private 	HUD_Healthbar 	healthUI;

    private		GameState 		gState;

    private 	UpgradeHealth 	upgradeInfo;


    void Start()
    {
		// Set up references, unnecessary for use in the Upgrade Screen
        if (Application.loadedLevelName != "Upgrades Screen")
        {
            healthUI = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
            if(GameObject.FindWithTag("Player")){
				bodyParts = GameObject.FindWithTag("Player").GetComponent<PlayerBodyparts>();
			}
        }
        else
        {
            upgradeInfo = GameObject.Find("Health Upgrade Info").GetComponent<UpgradeHealth>();
            upgradeInfo.currentHealth = maxHealth;
        }
    }


    void Update()
    {
		// in case the player spawns late, set up reference here
		if(GameObject.FindWithTag("Player")){
			bodyParts = GameObject.FindWithTag("Player").GetComponent<PlayerBodyparts>();
		}

		// mark player as dead if health is less than or equal to zero
		if (playerHealth <= 0)
        {
            playerDead = true;
        }
    }

    public void takeDamage(int damage)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damage;
        }

        if (playerHealth <= 0)
        {
            if (Application.loadedLevelName != "Upgrades Screen") bodyParts.selfDestruct();
        }
    }

	// Upgrade Health function
    public void IncreaseMaxHealth()
    {
        maxHealth = maxHealth + 1;
		playerHealth = maxHealth;
    }
}
