using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourneyCard : StoryCard {

	protected int shields;
	Logger log = new Logger("TourneyCard");
	
	public TourneyCard(string _name, string _type, int _shields, Sprite _sprite)
	{
		name = _name;
		type = _type;
		shields = _shields;
		sprite = _sprite;
	}

	public int getBonusShields(){
		return shields;
	}
}
