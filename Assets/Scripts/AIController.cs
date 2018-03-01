using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : AbstractAI {

	int strategy;
	Player player;
	Card[] hand;
	Logger log = new Logger("AINull");

	public AIController(Player _player, int _strategy){
		player = _player;
		strategy = _strategy;
		hand = player.getHand ();
		log.setSource("AIController: " + player.getName());
		player.assumingDirectControl (this);
	}
}
