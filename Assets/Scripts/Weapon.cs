using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AdvCard {

	public Weapon(string _name, int _BP, Sprite _sprite)
	{
		type = "weapon";
		name = _name;
		BP = _BP;
		sprite = _sprite;

	}
}
