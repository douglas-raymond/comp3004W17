using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : AdvCard {

	protected int altBP;
	
	public Foe(string _name, int _BP, int _altBP)
	{
		name = _name;
		BP = _BP;
		altBP = _altBP;
	}
}
