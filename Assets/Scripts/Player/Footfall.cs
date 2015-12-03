using UnityEngine;
using System.Collections;

public class Footfall : MonoBehaviour {

	public float 	fadeSpeed;
	public float 	growSpeed;
	public float 	startTime;

	Color 			tempColor;
	SpriteRenderer 	sRenderer;
	TimeScaler 		tScaler;

	void Awake()
	{
		// record birthtime so guard can tell which footstep is the most recent
		startTime = Time.time;
	}

	void Start () 
	{
		transform.localScale = new Vector3 (0,0,0);
		sRenderer = GetComponent<SpriteRenderer> ();
		tScaler = GameObject.Find ("Time Manager").GetComponent<TimeScaler> ();
		if (tScaler.GetNoiseDampening()) {
			fadeSpeed = 3.0f;
			Destroy (gameObject, 1.25f);
		} 
		else {
			Destroy (gameObject, 2.5f);
		}
	}

	void Update () 
	{
		// fade to transparancy
		tempColor = sRenderer.color;
		tempColor.a -= (Time.deltaTime * fadeSpeed);
		sRenderer.color = tempColor;

		// grow in size
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1,1,1), Time.deltaTime*growSpeed );
	}
}
