/// <summary>
/// Shooting - script to handle the guards shooting 
/// </summary>

using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public 			int 				shotDmg;
	public 			float 				reloadTime;
	public 			bool				shotBlocked = false;

	private 		GuardSensing		sensing;
	private 		Animator 			anim;
	private 		GameObject 			player;
	private 		GameObject			torso;
	private 		HealthManager		pHealth;
	public 			GameObject 			gunBarrel;

	private 		float 				counter = 0.6f;


	void Start () 
	{
		sensing 		= 		GetComponent<GuardSensing> ();
		player 			= 		GameObject.FindWithTag("Player");
		torso 			= 		GameObject.Find ("ppl_chest");
		anim 			= 		GetComponent<Animator> ();
		pHealth 		= 		GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
	}
	

	void Update () 
	{
		// in case player wasn't found in Start(), find player now
		if (player == null) {
			player = GameObject.FindWithTag ("Player");
		}

		// countdown from reload time if player is in sight
		if(sensing.playerInSight){
			if(counter >= 0){
				counter -= Time.deltaTime;
			}
		}

		// stop aiming weapon if player is dead
		if(pHealth.playerDead) {
			anim.SetBool("InSight", false);
			anim.SetFloat("Aim Weight", 0.0f);
		}
	}

	// animator IK function to ensure the guard is aiming directly at the player
	void OnAnimatorIK(int layerIndex) {
		float aimWeight = anim.GetFloat ("Aim Weight");
		// set aim weight for aiming weapon
		if(aimWeight > 1.0f){
			aimWeight = 1.0f;

			// Testing Line of Sight from GunBarrel
//			RaycastHit hit;
//			Debug.DrawRay(gunBarrel.transform.position, torso.transform.position);
//			if(Physics.Raycast(gunBarrel.transform.position, torso.transform.position - gunBarrel.transform.position, out hit, 8.0f))
//			{
//				if(hit.collider.tag != "Player"){
//					print ("can't hit player from weapon!");
//					shotBlocked = true;
//				} else {
//					shotBlocked = false;
//				}
//			}
		}

		if (player && !pHealth.playerDead && sensing.playerInSight) {
			// if there is a player, aim the guard's right hand at the player's torso
			anim.SetIKPosition (AvatarIKGoal.RightHand, torso.transform.position);
			anim.SetIKPositionWeight (AvatarIKGoal.RightHand, aimWeight);
		} else if (pHealth.playerDead) {
			// stop aiming at player is he is dead
			anim.SetBool ("InSight", false);
			anim.SetFloat ("Aim Weight", 0.0f);
		} else {
			// stop aiming at player is he is dead
			anim.SetBool ("InSight", false);
			anim.SetFloat ("Aim Weight", 0.0f);
		}
	}


	// function to fire weapon 
	public void Shoot() {
		// if reloading is complete and player is alive and weapon is fully raised, FIRE
		if (counter <= 0 && !pHealth.playerDead && anim.GetFloat("Aim Weight") >= 1.0f) {
			// reset reload timer
			counter = reloadTime;
			// instantiate bullet and add force along upward vector (local up is global forward)
			GameObject bullet = Instantiate (Resources.Load ("Enemies/Bullet"), gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.up * 1.5f, ForceMode.Impulse);
		} else if(pHealth.playerDead) {
			// stop aiming weapon if player is dead
			anim.SetBool("InSight", false);
			anim.SetFloat("Aim Weight", 0.0f);
		}
	}
}
