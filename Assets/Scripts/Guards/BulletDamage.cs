using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

	public AudioClip shootSound;
	public AudioClip damageSound;

	// Use this for initialization
	void Start () {
		AudioSource.PlayClipAtPoint (shootSound, Camera.main.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other){
		if (other.collider.tag == "Player") {
			// Access player health and call takeDamage function
			GameObject.Find("Health Manager").GetComponent<HealthManager>().takeDamage(1);
			GameObject bloodSplatter = Instantiate(Resources.Load("BloodSplatter"), other.transform.position, Quaternion.identity) as GameObject;
			AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
			Destroy(bloodSplatter, 2.0f);
		}
		Destroy (gameObject);
	}
}
