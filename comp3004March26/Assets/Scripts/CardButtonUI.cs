using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButtonUI : MonoBehaviour {

	// Use this for initialization
	
	string name;
	UI ui;
	Card card;
	
	Vector2 pos;
	int indexInSelection;
	bool currentlySelected;
	GameObject selectedIcon;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = pos;
		
	}
	public  void init(Card _card, UI _ui, Vector2 _pos, int _indexInSelection){
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		ui = _ui;
		card = _card;
		GetComponent<SpriteRenderer>().sprite = card.getSprite();
		pos = _pos;
		indexInSelection = _indexInSelection;
		return;
	}
	void OnMouseDown(){
		if(currentlySelected == false) {
			ui.gotCardSelection(gameObject);
			currentlySelected = true;
		}
		else
		{
			ui.removeCardSelection(gameObject);
			currentlySelected = false;
		}
	}
	public void setIndexInSelection(int i) {
		indexInSelection = i;
	}
	public int getIndexInSelection() {
		return indexInSelection;
	}
	public Card getCard()
	{
		return card;
	}
	public Vector2 getPos()
	{
		return transform.position;
	}
	public void setSelectedCardIcon(GameObject _selectedIcon) {
		selectedIcon = _selectedIcon;
	}
	public GameObject getSelectedCardIcon() {
		return  selectedIcon;
	}
}
