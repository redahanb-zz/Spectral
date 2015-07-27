using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	public string itemName;
	public Color itemColor;
	public Sprite itemIcon;
	public int itemValue;
	AudioSource impactSound;

	//ButtonManager buttonManager;

	// Use this for initialization
	void Start () {
		impactSound = GetComponent<AudioSource> ();
		transform.GetComponent<Renderer> ().material.color = itemColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(impactSound.mute && Time.time > 3.0f){
			impactSound.mute = false;
		}

//		if (!buttonManager) {
//			if(GameObject.FindWithTag("Player")) {
//				buttonManager = GameObject.FindWithTag("Player").GetComponent<ButtonManager> ();
//			}
//		}


	}

	void OnTriggerStay(Collider other) {
//		if (other.gameObject.tag == "Player") {
//			// switch A button icon on
//			buttonManager.pickupEnabled (true);
//
//			if (Input.GetKeyDown(KeyCode.K) || Input.GetButton("A_Button")) {
//				// Pick up item and add to inventory
//				other.GetComponent<PlayerInventory>().addItem(gameObject);
//				buttonManager.pickupEnabled(false);
//			}
//		}
	}

	void OnTriggerExit(Collider other) {
//		if (other.gameObject.tag == "Player") {
//			buttonManager.pickupEnabled(false);
//		}
	}

	public void pickUpItem() {
		transform.position = GameObject.FindWithTag ("Player").transform.position;
		gameObject.SetActive (false);
	}

	public void destroyItem() {
		Destroy (gameObject);
	}

	void OnCollisionEnter(Collision coll){
		if(GameObject.FindWithTag("Player") && coll.gameObject.tag != "Player"){
			soundOnImpact ();
		}
	}

	void soundOnImpact() {
		//print ("FLASHBANG");
		playEcho ();
		GameObject[] guards = GameObject.FindGameObjectsWithTag ("Guard");
		int x = 0;
		foreach(GameObject guard in guards){
			if(Vector3.Distance(transform.position, guard.transform.position) < 10.0f) {
				guard.GetComponent<NavMeshPatrolv2>().Investigate(transform.position);
				x++;
			}
		}
	}

	void playEcho() {
		Vector3 drawLoc = transform.position;
		drawLoc.y += 0.01f;
		GameObject echo = Instantiate (Resources.Load("FootfallFX"), drawLoc, Quaternion.Euler(90,0,0)) as GameObject;
		echo.GetComponent<SpriteRenderer> ().color = Color.red;
		AudioSource.PlayClipAtPoint (impactSound.clip, transform.position);
	}
	
}
