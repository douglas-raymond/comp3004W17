using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : AbstractAI {

	int strategy;
	Logger log = new Logger("AINull");

	public AIController(Player player, int _strategy){
		strategy = _strategy;
		log.setSource("AIController: " + player.getName());
		player.assumingDirectControl (this);
	}

	public Logger getLogger(){
		return log;
	}
}
