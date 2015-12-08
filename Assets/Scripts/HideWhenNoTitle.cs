using UnityEngine;
using System.Collections;

public class HideWhenNoTitle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(GameObject.Find("HideTitleObject")){
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
