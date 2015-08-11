using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public int shotDmg;
	public float reloadTime;

	//LineRenderer laserLine;
	EnemySight enemySight;
	Animator anim;

	GameObject player;
	GameObject gunBarrel;

	float counter = 0.6f;


	// Use this for initialization
	void Start () {
		//laserLine = GetComponent<LineRenderer> ();
		enemySight = GetComponent<EnemySight> ();
		player = GameObject.FindWithTag("Player");
		anim = GetComponent<Animator> ();
		gunBarrel = GameObject.Find ("GunBarrel");
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindWithTag ("Player");
		}

		if(enemySight.alerted){
			if(counter >= 0){
				counter -= Time.deltaTime;
				//laserLine.enabled = false;
			}

			//turn off the line renderer 0.2 seconds after shot
			if (counter <= 2.8f) {
				//laserLine.enabled = false;
			}
		}
		//Debug.Log (counter);
	}

	void OnAnimatorIK(int layerIndex) {
		//print ("IK function called");
		float aimWeight = anim.GetFloat ("Right Arm Down-Up");
		anim.SetIKPosition (AvatarIKGoal.RightHand, player.transform.position + Vector3.up * 1.0f);
		anim.SetIKPositionWeight (AvatarIKGoal.RightHand, aimWeight*5.0f);
		print ("Aim weight = " + aimWeight);
	}


	public void Shoot() {
		if(counter <= 0){
			counter = reloadTime;

			//GameObject player = GameObject.FindWithTag("Player");

//			laserLine.SetPosition(0, gunBarrel.transform.position);
//			laserLine.SetPosition(1, player.transform.position + Vector3.up);
//			laserLine.enabled = true;

			//player.gameObject.GetComponent<Health>().takeDamage(shotDmg);

		}
	}
}
