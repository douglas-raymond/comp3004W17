using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonUI : MonoBehaviour {

	// Use this for initialization
	
	string name;
	UI ui;
	Card card;
	
	Vector2 pos;
	bool currentlySelected;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = pos;
		
	}
	public  void init(Card _card, UI _ui, Vector2 _pos){
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		ui = _ui;
		card = _card;
		GetComponent<SpriteRenderer>().sprite = card.getSprite();
		pos = _pos;
		return;
	}
	void OnMouseDown(){
		if(currentlySelected == false) {
			ui.gotCardSelection(this.gameObject);
			currentlySelected = true;
		}
	}
	public Card getCard()
	{
		return card;
	}
	public Vector2 getPos()
	{
		return transform.position;
	}
}
