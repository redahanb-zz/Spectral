using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public int 			shotDmg;
	public float 		reloadTime;
	
	EnemySight 			enemySight;
	GuardSensing		sensing;
	Animator 			anim;
	GameObject 			player;
	GameObject			torso;
	HealthManager		pHealth;
	public GameObject 	gunBarrel;

	float 				counter = 0.6f;


	// Use this for initialization
	void Start () {
		enemySight = GetComponent<EnemySight> ();
		sensing = GetComponent<GuardSensing> ();
		player = GameObject.FindWithTag("Player");
		torso = GameObject.Find ("ppl_chest");
		anim = GetComponent<Animator> ();
		pHealth = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		//gunBarrel = transform.Find("GunBarrel").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindWithTag ("Player");
		}

		if(enemySight.alerted || sensing.playerInSight){
			if(counter >= 0){
				counter -= Time.deltaTime;
			}
		}
	}

	void OnAnimatorIK(int layerIndex) {;
		float aimWeight = anim.GetFloat ("Aim Weight");
		if(aimWeight > 1.0f){
			aimWeight = 1.0f;
		}
		if (player) {
			anim.SetIKPosition (AvatarIKGoal.RightHand, torso.transform.position);
			anim.SetIKPositionWeight (AvatarIKGoal.RightHand, aimWeight);
		}

		//print ("Aim weight = " + aimWeight);
	}


	public void Shoot() {
		if (counter <= 0 && !pHealth.playerDead) {
			//print ("shooting!");
			counter = reloadTime;

			GameObject bullet = Instantiate (Resources.Load ("Enemies/Bullet"), gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.up * 1.5f, ForceMode.Impulse);
		} else if(pHealth.playerDead) {
			anim.SetBool("InSight", false);
			anim.SetFloat("Aim Weight", 0.0f);
		}
	}
}
