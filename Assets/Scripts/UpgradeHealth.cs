using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeHealth : MonoBehaviour {

	public 	bool 	healthVisible = false;
	public 	int 	currentHealth = 5;

	private int 	currentIndex = 0, 
					nextIndex = 0;

	private bool 	displayPanel = true, fadeIn = true;

	private Color 	activeColor, 
					inactiveColor, 
					nextUpgradeColorVisible, 
					nextUpgradeColorTransparent;

	private Vector3 hiddenPos, 
					visiblePos;

	private 		RawImage[] heartImages;
	private 		RectTransform healthInfoTransform;

	// Use this for initialization
	void Start () {

		healthInfoTransform = transform.parent.parent.GetComponent<RectTransform>();

		hiddenPos = healthInfoTransform.position;
		visiblePos = healthInfoTransform.position + new Vector3(340,0,0);
		healthInfoTransform.position = hiddenPos;

		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);

		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);

		heartImages = new RawImage[9];
		heartImages[0] = transform.Find("Heart 1").GetComponent<RawImage>();
		heartImages[1] = transform.Find("Heart 2").GetComponent<RawImage>();
		heartImages[2] = transform.Find("Heart 3").GetComponent<RawImage>();
		heartImages[3] = transform.Find("Heart 4").GetComponent<RawImage>();
		heartImages[4] = transform.Find("Heart 5").GetComponent<RawImage>();
		heartImages[5] = transform.Find("Heart 6").GetComponent<RawImage>();
		heartImages[6] = transform.Find("Heart 7").GetComponent<RawImage>();
		heartImages[7] = transform.Find("Heart 8").GetComponent<RawImage>();
		heartImages[8] = transform.Find("Heart 9").GetComponent<RawImage>();

		ClearHearts();
	}



	
	// Update is called once per frame
	void Update () {

		MovePanel();
		if(Vector3.Distance(healthInfoTransform.position,visiblePos) < 5){
			FillHearts();
			healthVisible = true;
		}
		if(Vector3.Distance(healthInfoTransform.position,visiblePos) > 330){
			ClearHearts();
			healthVisible = false;
		}

		//Debug Input
		if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
		if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;

	}

	void MovePanel(){
		if(displayPanel){
			healthInfoTransform.position = Vector3.Lerp(healthInfoTransform.position, visiblePos, 0.1f);

		}
		else{
			healthInfoTransform.position = Vector3.Lerp(healthInfoTransform.position, hiddenPos, 0.1f);
		}
	}

	void FillHearts(){
		if(heartImages[currentIndex].color.a < 0.95f){
			heartImages[currentIndex].color = Color.Lerp(heartImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (currentHealth))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(heartImages[nextIndex].color.a < 0.9f) heartImages[nextIndex].color = Color.Lerp(heartImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
					else fadeIn = false;
				}
				else{
					if(heartImages[nextIndex].color.a > 0.1f) heartImages[nextIndex].color = Color.Lerp(heartImages[nextIndex].color, nextUpgradeColorTransparent, 0.07f);
					else fadeIn = true;
				}
			}
		}
	}

	void ClearHearts(){
		currentIndex = 0;
		nextIndex = 0;

		for(int i = 0; i < 9; i++)
			heartImages[i].color = inactiveColor;
	}

	void IncreaseHealth(){
		currentHealth = currentHealth + 1;
	}
}
