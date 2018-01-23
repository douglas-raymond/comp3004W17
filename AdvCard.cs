using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvCard : Card {

	// Use this for initialization
	protected int BP;

	protected AdvCard() {}
	public AdvCard(string _name, int _BP)
	{
		name = _name;
		BP = _BP;
	}
}
