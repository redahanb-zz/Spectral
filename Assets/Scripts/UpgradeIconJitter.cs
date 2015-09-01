using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeIconJitter : MonoBehaviour {

	RectTransform rTransform;
	float randomDelay = 1, randomScale = 1, scaleRate = 0.01f;
	Vector3 originalSize;
	public bool debug = false;

	float distance;

	// Use this for initialization
	void Start () {


		rTransform = GetComponent<RectTransform>();

		originalSize = rTransform.sizeDelta;
		StartCoroutine(RandomizeScale());

	}
	
	// Update is called once per frame
	void Update () {

		distance =  Vector3.Distance(rTransform.localScale, rTransform.localScale * randomScale);

		if(rTransform.localScale.x > randomScale){
			rTransform.localScale -= new Vector3(scaleRate * distance,scaleRate * distance,scaleRate * distance);
		}
		else if(rTransform.localScale.x < randomScale){
			rTransform.localScale += new Vector3(scaleRate * distance,scaleRate * distance,scaleRate * distance);
		}

		//if(debug)print(distance);
	}

	IEnumerator RandomizeScale(){
		while(true){
			randomDelay = Random.Range(0.3f, 2);
			randomScale = Random.Range(0.9f, 1.1f);
			yield return new WaitForSeconds(randomDelay);
		}
	}
}
