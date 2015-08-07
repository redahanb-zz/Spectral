using UnityEngine;
using System.Collections;

public class SoundEffect : MonoBehaviour {

	public AudioClip soundEffect;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playClip(){
		AudioSource.PlayClipAtPoint (soundEffect, transform.position);
	}
}
