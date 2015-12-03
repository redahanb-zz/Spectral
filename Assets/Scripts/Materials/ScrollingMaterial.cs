using UnityEngine;
using System.Collections;

public class ScrollingMaterial : MonoBehaviour {

	Renderer render;
	public Vector2 scrollDirection;

	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		render.material.mainTextureOffset = render.material.mainTextureOffset + scrollDirection * Time.deltaTime;
	}
}
