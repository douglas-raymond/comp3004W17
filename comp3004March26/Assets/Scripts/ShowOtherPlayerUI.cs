using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowOtherPlayerUI : MonoBehaviour {

	// Use this for initialization
	UI ui;
	
	
	GameObject blackScreen;
	
	float panelWidth;
	float panelHeight;
	float panelPosX;
	float panelPosY;
	GameObject[] headers;
	public void init(UI _ui) {
		ui = _ui;
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		GameObject canvas = GameObject.Find("Canvas");
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	
	public void PointerEnter()
    {

		ui.mouseOverShowOtherPlayerIcon();
		
    }
	
	public void PointerExit(){

        ui.mouseLeaveShowOtherPlayerIcon();
    }


}
