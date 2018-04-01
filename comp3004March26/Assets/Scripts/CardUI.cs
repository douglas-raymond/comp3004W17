using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI : MonoBehaviour {

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
	public  void init(Card _card, UI _ui, Vector2 _pos, Vector2 _scale){
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		ui = _ui;
		card = _card;
		GetComponent<SpriteRenderer>().sprite = card.getSprite();
		GetComponent<RectTransform>().localScale = _scale;
		pos = _pos;
		return;
	}

}
