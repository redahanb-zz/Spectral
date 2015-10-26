using UnityEngine;
using System.Collections;

public class PlayerBodyparts : MonoBehaviour {

	public GameObject[] bodyparts = new GameObject[38];
	Animator anim;
	Vector3 impulse;
	Vector3 worldPos;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		//Invoke ("selfDestruct", 0.55f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void selfDestruct(){
		anim.enabled = false;
		foreach(GameObject part in bodyparts){
			worldPos = part.transform.position;
			part.transform.parent = null;
			part.transform.position = worldPos;
			part.AddComponent<Rigidbody>();
			part.AddComponent<BoxCollider>();
			impulse = new Vector3(Random.Range(-0.25f,0.25f),Random.Range(0.25f,0.4f),Random.Range(-0.25f,0.25f));
			part.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
			//part.GetComponent<Rigidbody>().useGravity = false;
			//Destroy (part, Random.Range(1.0f,2.0f));
		}
		Invoke ("delayDestruct", 1); // destroy player object
	}

	void delayDestruct(){
		//Destroy (this.gameObject);
		gameObject.SetActive (false);
	}
}
