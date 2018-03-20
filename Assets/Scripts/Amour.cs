using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amour : AdvCard {

	protected int freeBids;
	
	public Amour(string _name, int _BP, int _bids, Sprite _sprite)
	{
		name = _name;
		BP = _BP;
		freeBids = _bids;
		sprite = _sprite;
	}
	
	public override int getFreeBid(string quest = "null") {
		return freeBids;}
}
