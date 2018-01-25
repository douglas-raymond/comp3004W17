using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card {

	protected string name;
	protected string type;
	protected Card(){	}
	
	public Card(string _name)
	{
		name = _name;
	}
	
	public string getName()
	{
		return name;
	}
	public string getType()
	{
		return type;
	}
	

}
