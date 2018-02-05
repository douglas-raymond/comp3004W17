using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {

	protected string name;
	protected string type;
	protected Card(){	}
	protected Sprite sprite;
	
	public Card(string _name, Sprite _sprite)
	{
		name = _name;
		sprite = _sprite;
	}
	
	public string getName()
	{
		return name;
	}
	public string getType()
	{
		return type;
	}
	public Sprite getSprite()
	{
		return sprite;
	}
	public virtual int getBP() {return -1;}

}
