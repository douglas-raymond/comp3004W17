using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCard : StoryCard {

	protected int stages;
	protected Foe[] special;
	Logger log = new Logger("QuestCard");
	
	public QuestCard(string _name, string _type, int _stages, Foe[] _special, Sprite _sprite)
	{
		name = _name;
		type = _type;
		stages = _stages;
		special = _special;
		sprite = _sprite;
		log.log ("creating card " + name);
	}
	
	public int getStages(){
		return stages;
		}
}
