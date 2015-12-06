//Name:			InteractRange.cs
//Project:		Spectral: The Silicon Domain
//Author(s)		Conor Hughes - conormpkhughes@yahoo.com
//Description:	Rotates and positions the range indicator to the players position.

using UnityEngine;
using System.Collections;

public class InteractRange : MonoBehaviour {

	private Transform player;			//The player transform
	private Renderer rangeRenderer;		//The rangle indicator renderer component

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
