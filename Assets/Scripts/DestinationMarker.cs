using UnityEngine;
using System.Collections;

public class DestinationMarker : MonoBehaviour {

	Renderer r;

	// Use this for initialization
	void Start () {
		r = GetComponent<Renderer>();
		r.material.color = new Color(r.material.color.a, r.material.color.g, r.material.color.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
		r.material.color = Color.Lerp(r.material.color, new Color(r.material.color.a, r.material.color.g, r.material.color.b, 1), 0.06f);
	}
}
