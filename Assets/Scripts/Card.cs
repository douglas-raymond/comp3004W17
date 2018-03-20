using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {

	protected string name;
	protected string type;
	protected Card(){	}
	protected Sprite sprite;
	Logger log = new Logger("Card");
	
	public Card(string _name, Sprite _sprite)
	{
		name = _name;
		sprite = _sprite;
	}
	
	public string getName()
	{
		return name;
	}
	public string getType() { return type; }
	public Sprite getSprite()
	{
		return sprite;
	}
	public virtual int getBP(string quest="null") {return -1;}
	public virtual int getAltBP() {return -1;}
	public virtual int getMinBid() {return -1;}
	public virtual int getFreeBid(string quest) {return -1;}
	public virtual int getBonusShields() {return -1;}
	public virtual void runEvent(Player[] playerList, int activePlayer, int playerCount, AdvDeck advDeck, GameManager gm){return;}


}
