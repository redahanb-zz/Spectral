using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeHealth : MonoBehaviour {

	public 	bool 	healthVisible = false;
	public 	int 	currentHealth;

	int displayedHealth;

	private int 	currentIndex = 0, 
					nextIndex = 0;

	private bool 	fadeIn = true;

	public bool		displayPanel = false;

	private Color 	activeColor, 
					inactiveColor, 
					nextUpgradeColorVisible, 
					nextUpgradeColorTransparent;

	private Vector3 hiddenPos, 
					visiblePos;

	private 		RawImage[] heartImages;
	private 		RectTransform healthInfoTransform;
	private			HealthManager healthManager;

	Image			healthButtonImage;

	Color			buttonTargetColor = Color.white;

	GameObject 		upgradeButton;

	// Use this for initialization
	void Start () {
		healthButtonImage = GameObject.Find("Health Button").GetComponent<Image>();
		buttonTargetColor = Color.white;
		healthInfoTransform = transform.GetComponent<RectTransform>();
		healthManager = GameObject.Find ("Health Manager").GetComponent<HealthManager> ();
		currentHealth = healthManager.playerHealth;

		hiddenPos = healthInfoTransform.position;
		visiblePos = healthInfoTransform.position + new Vector3(340,0,0);
		healthInfoTransform.position = hiddenPos;

		activeColor = Color.white;
		inactiveColor = new Color(1,1,1,0.1f);

		nextUpgradeColorVisible 	= new Color(0,1,0,1);
		nextUpgradeColorTransparent = new Color(0,1,0,0.05f);

		upgradeButton = transform.Find("Upgrade Button").gameObject;

		heartImages = new RawImage[5];
		heartImages[0] = transform.Find("Current Health").Find("Health Bar").Find("Heart 1").GetComponent<RawImage>();
		heartImages[1] = transform.Find("Current Health").Find("Health Bar").Find("Heart 2").GetComponent<RawImage>();
		heartImages[2] = transform.Find("Current Health").Find("Health Bar").Find("Heart 3").GetComponent<RawImage>();
		heartImages[3] = transform.Find("Current Health").Find("Health Bar").Find("Heart 4").GetComponent<RawImage>();
		heartImages[4] = transform.Find("Current Health").Find("Health Bar").Find("Heart 5").GetComponent<RawImage>();
		ClearHearts();
	}

	// Update is called once per frame
	void Update () {
		displayedHealth = currentHealth - 3;

		if(currentHealth >= 7)Destroy(upgradeButton);

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
//		if(Input.GetKeyDown(KeyCode.Space))IncreaseHealth();
//		if(Input.GetKeyDown(KeyCode.N))displayPanel = !displayPanel;
	}

	void MovePanel(){
		if(displayPanel){
			healthInfoTransform.position = Vector3.Lerp(healthInfoTransform.position, visiblePos, 0.1f);
			buttonTargetColor = Color.green;
		}
		else{
			healthInfoTransform.position = Vector3.Lerp(healthInfoTransform.position, hiddenPos, 0.1f);
			buttonTargetColor = Color.white;
		}
		healthButtonImage.color = Color.Lerp(healthButtonImage.color,buttonTargetColor, Time.deltaTime * 5);
	}

	void FillHearts(){
		if(heartImages[currentIndex].color.a < 0.95f){
			heartImages[currentIndex].color = Color.Lerp(heartImages[currentIndex].color, activeColor, 0.2f);
		}
		else{
			if(currentIndex < (displayedHealth))currentIndex = currentIndex + 1;
			else{
				nextIndex = currentIndex + 1;
				if(fadeIn){
					if(currentHealth < 7) if(heartImages[nextIndex].color.a < 0.9f) heartImages[nextIndex].color = Color.Lerp(heartImages[nextIndex].color, nextUpgradeColorVisible, 0.07f);
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

		for(int i = 0; i < 5; i++)
			heartImages[i].color = inactiveColor;
	}

	public void IncreaseHealth(){
		print ("Upgrading health: button.");
		currentHealth = currentHealth + 1;
		healthManager.IncreaseMaxHealth ();
	}
}
