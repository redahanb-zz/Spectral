using UnityEngine;
using System.Collections;

public class PlayerBodyparts : MonoBehaviour {

	public GameObject[] bodyparts = new GameObject[38];

	// Use this for initialization
	void Start () {
		//Invoke ("selfDestruct", 0.55f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void selfDestruct(){
		foreach(GameObject part in bodyparts){
			part.transform.parent = null;
			part.AddComponent<Rigidbody>();
			Vector3 impulse = new Vector3(Random.Range(-3.0f,3.0f),Random.Range(1.0f,2.0f),Random.Range(-3.0f,3.0f));
			part.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
			//part.GetComponent<Rigidbody>().useGravity = false;
			part.AddComponent<CapsuleCollider>();
			Destroy (part, Random.Range(0.5f,1.5f));
		}
		Invoke ("delayDestruct", 1);
	}

	void delayDestruct(){
		Destroy (this.gameObject);
	}
}
