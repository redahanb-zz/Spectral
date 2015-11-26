using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour
{

    public int maxHealth;
    public int playerHealth;
    public bool playerDead = false;

    PlayerBodyparts bodyParts;
    HUD_Healthbar healthUI;

    GameState gState;

    UpgradeHealth upgradeInfo;

    // Use this for initialization
    void Start()
    {


        // Pull health data from GameState object
        // maxHealth = GameState.data.maxHealth;
        // playerHealth = GameState.data.currentHealth;

        if (Application.loadedLevelName != "Upgrades Screen")
        {
            healthUI = GameObject.Find("HUD_Healthbar").GetComponent<HUD_Healthbar>();
            //healthUI.buildHealthbarUI(maxHealth);
            //healthUI.healthBarSize = maxHealth;
            bodyParts = GameObject.FindWithTag("Player").GetComponent<PlayerBodyparts>();
        }
        else
        {
            upgradeInfo = GameObject.Find("Health Upgrade Info").GetComponent<UpgradeHealth>();
            upgradeInfo.currentHealth = maxHealth;
        }


    }

    // Update is called once per frame
    void Update()
    {
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
            //print ("Player was killed...");
            if (Application.loadedLevelName != "Upgrades Screen") bodyParts.selfDestruct();
        }
    }

    public void IncreaseMaxHealth()
    {
        maxHealth = maxHealth + 1;
		playerHealth = maxHealth;
		print ("Upgrading health: HM");
    }
}
