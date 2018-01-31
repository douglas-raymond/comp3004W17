using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonUI : MonoBehaviour {

	// Use this for initialization
	
	string name;
	UI ui;
	Card card;
	
	bool currentlySelected;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	public  void setCard(Card _card){
		card = _card;
		GetComponent<SpriteRenderer>().sprite = card.getSprite();
	}
	public  void setUI(UI _ui){
		ui = _ui;
	}
	void OnMouseDown(){
		if(currentlySelected == false)
		{
			ui.getCardSelection(card);
			currentlySelected = true;
		}
	}
}
