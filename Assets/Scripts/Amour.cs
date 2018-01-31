using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amour : AdvCard {

	protected int bids;
	
	public Amour(string _name, int _BP, int _bids, Sprite _sprite)
	{
		name = _name;
		BP = _BP;
		bids = _bids;
		sprite = _sprite;
	}
}
