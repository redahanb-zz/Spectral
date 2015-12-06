using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

	public 		AudioClip 		shootSound;
	public 		AudioClip 		damageSound;


	void Start () 
	{
		// Play shot sound on spawning
		AudioSource.PlayClipAtPoint (shootSound, Camera.main.transform.position);
	}

	void Update () 
	{
	
	}

	void OnCollisionEnter(Collision other)
	{
		// if the bullet collides with the player, deal damage
		if (other.collider.tag == "Player") {
			// Access player health and call takeDamage function
			GameObject.Find("Health Manager").GetComponent<HealthManager>().takeDamage(1);
			// Instantiate the bloodsplatter particle prefab
			GameObject bloodSplatter = Instantiate(Resources.Load("BloodSplatter"), other.transform.position, Quaternion.identity) as GameObject;
			// Player damage sound
			AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
			// Destroy the particle effect after two seconds
			Destroy(bloodSplatter, 2.0f);
		}
		// Destroy the bullet on collision
		Destroy (gameObject);
	}
}
