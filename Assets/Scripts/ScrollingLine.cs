using UnityEngine;
using System.Collections;

public class ScrollingLine : MonoBehaviour {
	Renderer render;
	// Use this for initialization
	void Start () {
		render = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		render.material.mainTextureOffset = render.material.mainTextureOffset + new Vector2(0.001f,0);
	}
}
