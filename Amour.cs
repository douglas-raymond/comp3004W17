using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amour : AdvCard {

	protected int bids;
	
	public Amour(string _name, int _BP, int _bids)
	{
		name = _name;
		BP = _BP;
		bids = _bids;
	}
}
