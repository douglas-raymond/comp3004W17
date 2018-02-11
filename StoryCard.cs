using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryCard : Card {

	protected StoryCard(){}
	
	public StoryCard(string _name, string _type)
	{
		name = _name;
		type = _type;
	}
}
