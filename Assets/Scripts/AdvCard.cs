using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvCard : Card {

	// Use this for initialization
	protected int BP;
	Logger log = new Logger("AdvCard");

	protected AdvCard() {}
	public AdvCard(string _name, int _BP)
	{
		name = _name;
		BP = _BP;
	}

	public override int getBP(string quest="null"){return BP;}
}
