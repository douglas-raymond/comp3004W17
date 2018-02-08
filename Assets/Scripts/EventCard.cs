using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCard : StoryCard {
	
	Logger log = new Logger("EventCard", "c:/comp3004");

	public EventCard(string _name, string _type)
	{
		name = _name;
		type = _type;
		log.log ("creating card " + name);
	}
	
	void runEvent()
	{
		return;
	}
}
