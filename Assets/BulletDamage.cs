using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other){
		if (other.collider.name == "Player") {
			// Access player health and call takeDamage function
			GameObject.Find("Health Manager").GetComponent<HealthManager>().takeDamage(1);
		}
		Destroy (gameObject);
	}
}
