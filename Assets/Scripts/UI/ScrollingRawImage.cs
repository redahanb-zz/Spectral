using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingRawImage : MonoBehaviour {

	RawImage rImage;
	public Vector2 scrollDirection;
	// Use this for initialization
	void Start () {
		rImage = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		rImage.uvRect = new Rect(rImage.uvRect.x + scrollDirection.x,rImage.uvRect.y + scrollDirection.y,rImage.uvRect.width,rImage.uvRect.height);
	}
}
