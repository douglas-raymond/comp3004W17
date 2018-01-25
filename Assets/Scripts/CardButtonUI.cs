using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonUI : MonoBehaviour {

	// Use this for initialization
	
	string name;
	UI ui;
	Card card;
	
	
	void Start () {
		//UI = GameObject.FindGameObjectWithTag("UI");
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	public  void setCard(Card _card){
		card = _card;
	}
		public  void setUI(UI _ui){
		ui = _ui;
	}
	void OnMouseDown(){
		ui.cardClicked(card);
	}
}
