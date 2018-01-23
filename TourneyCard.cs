using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourneyCard : StoryCard {

	protected int shields;
	public TourneyCard(string _name, string _type, int _shields)
	{
		name = _name;
		type = _type;
		shields = _shields;
	}
}
