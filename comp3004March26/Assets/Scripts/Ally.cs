using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : AdvCard {

	protected int altBP;
	protected int freeBids;
	
	public Ally(string _name, int _BP, int _altBP, int _freeBids, Sprite _sprite)
	{
		name = _name;
		BP = _BP;
		altBP = _altBP;
		freeBids = _freeBids;
		sprite = _sprite;
		
	}
	
	public override int getFreeBid() {return freeBids;}
}
