using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonChildColorTint : MonoBehaviour {

	Button b;
	RawImage rwImg;

	// Use this for initialization
	void Start () {
		b 		= transform.parent.GetComponent<Button>();
		rwImg 	= GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		rwImg.color = b.image.color;
	}
}
