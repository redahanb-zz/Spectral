using UnityEngine;
using System.Collections;

public class InteractRange : MonoBehaviour {

	Transform player;
	Renderer rangeRenderer;

	// Use this for initialization
	void Start () {
		rangeRenderer = GetComponent<Renderer>();
		player = transform.parent;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.position;
		transform.eulerAngles += new Vector3(0,0.5f,0);
	}
}
