using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public int shotDmg;
	public float reloadTime;
	
	EnemySight enemySight;
	Animator anim;

	GameObject player;
	public GameObject gunBarrel;

	float counter = 0.6f;


	// Use this for initialization
	void Start () {
		enemySight = GetComponent<EnemySight> ();
		player = GameObject.FindWithTag("Player");
		anim = GetComponent<Animator> ();
		//gunBarrel = transform.Find("GunBarrel").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindWithTag ("Player");
		}

		if(enemySight.alerted){
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
		anim.SetIKPosition (AvatarIKGoal.RightHand, player.transform.position + Vector3.up * 1.0f);
		anim.SetIKPositionWeight (AvatarIKGoal.RightHand, aimWeight);
		//print ("Aim weight = " + aimWeight);
	}


	public void Shoot() {
		if(counter <= 0){
			counter = reloadTime;

			GameObject bullet = Instantiate(Resources.Load("Enemies/Bullet"), gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up*1.5f, ForceMode.Impulse);

			//player.gameObject.GetComponent<Health>().takeDamage(shotDmg);

		}
	}
}
