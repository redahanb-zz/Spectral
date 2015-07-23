using UnityEngine;
using System.Collections;

public class HUD_Healthbar : MonoBehaviour {

	GameObject[] healthbarIcons;
	public int panelSize;
	
	// Use this for initialization
	void Start () {
		buildHealthbarUI (5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void buildHealthbarUI(int size) {
		// Take size from player inventory, instantiate UI box for each slot, plus bookend graphics
		healthbarIcons = new GameObject[size];
		Vector3 panelPosition;
		
		// draw shoulder buttons
		GameObject L_bookend = Instantiate (Resources.Load("UI/HUD/Health/HUD_Bookend_Health_L"), new Vector3(-1*((size*panelSize)/2) - panelSize/2, -10, 0), Quaternion.identity) as GameObject;
		L_bookend.transform.SetParent(gameObject.transform, false);
		L_bookend.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1*((size*panelSize)/2) - panelSize/2, -10) ;
		
		GameObject R_bookend = Instantiate (Resources.Load("UI/HUD/Health/HUD_Bookend_Health_R"), new Vector3(1*((size*panelSize)/2) + panelSize/2, -10, 0), Quaternion.identity) as GameObject;
		R_bookend.transform.SetParent(gameObject.transform, false);
		R_bookend.GetComponent<RectTransform>().anchoredPosition = new Vector2(1*((size*panelSize)/2) + panelSize/2, -10) ;
		
		for(int i = 0; i < size ; i++){
			// position their rect-transforms according to their index and the size of the array
			panelPosition = new Vector3( -1*((size*panelSize)/2) + panelSize/2 + i*panelSize, -10, 0);
			healthbarIcons[i] = Instantiate(Resources.Load("UI/HUD/Health/HUD_Panel_Health"), panelPosition, Quaternion.identity) as GameObject;
			
			// set the parent and keep the position relative to the parent
			healthbarIcons[i].transform.SetParent(gameObject.transform, false);
			healthbarIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-1*((size*panelSize)/2) + panelSize/2 + i*panelSize, -10);
			
			// name them as an index for the UI array
			healthbarIcons[i].gameObject.name = "Cell " + i.ToString();
		}
		
	}
}
