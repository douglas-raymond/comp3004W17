using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : AdvCard {

	protected int altBP;
	protected int freeBids;

	public Ally(string _name, int _BP, int _altBP, int _freeBids, Sprite _sprite)
	{
		type = "ally";
		name = _name;
		BP = _BP;
		altBP = _altBP;
		freeBids = _freeBids;
		sprite = _sprite;

	}

	public override int getFreeBid(string quest) {
		if(quest.Equals("questingbeast") && name.Equals("pellinore")){
			return freeBids;
		}
		else if(!quest.Equals("questingbeast") && name.Equals("pellinore")){
			return 0;
		}

		return freeBids;
	}
	public override int getBP(string quest) {		
		Debug.Log("name: " + name + ", quest: " + quest);
		if(quest.Equals("holygrail") && name.Equals("percival")){
			return altBP;
		}
		else if(!quest.Equals("holygrail") && name.Equals("percival")){
			return BP;
		}

		if(quest.Equals("greenknighttest") && name.Equals("gawain")){
			return altBP;
		}
		else if(!quest.Equals("greenknighttest") && name.Equals("gawain")){
			return BP;
		}

		return BP;
	}
}
