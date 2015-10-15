using UnityEngine;
using System.Collections;

public class Footfall : MonoBehaviour {

	public float fadeSpeed;
	public float growSpeed;

	Color tempColor;
	SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (0,0,0);
		sRenderer = GetComponent<SpriteRenderer> ();
		Destroy (gameObject, 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
		// fade to transparancy
		tempColor = sRenderer.color;
		tempColor.a -= (Time.deltaTime * fadeSpeed);
		sRenderer.color = tempColor;

		// grow in size
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1,1,1), Time.deltaTime*growSpeed );
	}
}
