using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAI{

	int strategy;
	Player player;

	protected AbstractAI(){
	}

	protected AbstractAI(Player _player, int _strategy){
		player = _player;
		strategy = _strategy;
	}

	//When tournament is played, this is called so AI decides whether/what to submit. Returns the cards it wants to play. If empty array, AI refuses to play.
	public Card[] doIParticipateInTournament(Card[] hand, Player[] players){
		int BPtotal = 0;
		Card[] submit = new Card[12];
		int count = 0;
		hand = sortHand (hand);
		switch (strategy) {
		case 1:
			break;
		case 2:
			for (int i = hand.Length - 1; i >= 0; i--) {
				if (hand [i].getType () == "test" || hand [i].getType () == "foe") {
					continue;
				}
				if (BPtotal < 50 && BPtotal + hand [i].getBP () <= 50) { //if we have less than 50, and adding this wouldn't go over 50, add it, then find the next card cost down
					submit [count] = hand [i];
					count++;
					BPtotal = BPtotal + hand [i].getBP ();
				} 
				if (BPtotal == 50) {
					break;
				}
			}
			break;
		}
		player.discardCard (submit);
		return submit;
	}

	public bool doISponsorAQuest (Card[] hand, Player[] players, QuestCard quest){ //strategy 1 and 2 run this the exact same way, so no switch statement needed... yet
		int hasTest = 0; //either 0 or 1, and we use int rather than bool to use it in quest stage calculations
		int lowestBP = 0;
		int totalFoes = 0;
		hand = sortHand (hand);
		for (int i = 0; i < players.GetLength (0); i++) {
			switch (players [i].getRank ()) {
			case 0:
				if ((quest.getStages () + players [i].getShields ()) >= 5) {
					return false;
				}
				break;
			case 1:
				if ((quest.getStages () + players [i].getShields ()) >= 7) {
					return false;
				}
				break;
			case 2:
				if ((quest.getStages () + players [i].getShields ()) >= 10) {
					return false;
				}
				break;
			default:
				return false;
			}
		}
		for (int i = 0; i < hand.GetLength (0); i++) {
			if (hand [i].getType() == "test") {
				hasTest = 1;
				break;
			}
			if (hand [i].getType() == "foe" && hand[i].getBP()>lowestBP) {
				totalFoes++;
				lowestBP = hand [i].getBP ();
			}
		}
		if (quest.getStages () > totalFoes + hasTest) {
			return false;
		}
		return true;
	}

	public bool doIParticipateInQuest (Card[] hand, QuestCard quest){
		bool c1 = false;
		bool c2 = false;
		int lowestBP = 0;
		int count = 0;
		hand = sortHand (hand);
		switch (strategy) {
		case 1:
			return false;
			break;
		case 2:
			for (int i = 0; i < hand.GetLength (0); i++) {
				if (lowestBP == 0 && hand [i].getBP () > 0 && hand [i].getType () != "foe") {
					lowestBP = hand [i].getBP ();
					count++;
				} else if (hand [i].getBP () >= lowestBP + 10 && hand [i].getType () != "foe") {
					lowestBP = hand [i].getBP ();
					count++;
				}
			}
			if (count >= quest.getStages ()) {
				c1 = true;
			}
			count = 0;
			for (int i = 0; i < hand.GetLength (0); i++) {
				if (hand [i].getType () == "foe" && hand [i].getBP () < 25) {
					count++;
				}
			}
			if (count >= 2) {
				c2 = true;
			}
			return (c1 && c2);
			break;
		}
		return false;
	}

	//
	public Card[] nextBid(Card[] hand, ActiveQuest quest){
		Card[] submit = new Card[12];
		if (quest.getHighestBid () > hand.Length) {
			return submit;
		}
		int count = 0;
		switch (strategy) {
		case 1:
			break;

		case 2:
			for (int i = 0; i < hand.Length; i++) {
				if (hand [i].getType () == "foe" && hand [i].getBP () < 25) {
					submit [count] = hand [i];
					count++;
				}
			}
			break;
		}
		return submit;
	}

	//NB Because of how bidding is handled, this method is redundant - nextBid handles that for us.
	/* 
	public void discardAfterWinningTest(Card[] hand, ActiveQuest quest){
		Card[] disco = new Card[1];
		switch (strategy) {
		case 1:
			break;

		case 2:
			if (quest.getCurrentStageNum () == 0) {
				for (int i = 0; i < hand.Length; i++) {
					if (hand [i].getType () == "foe" && hand [i].getBP () < 25) {
						disco [0] = hand [i];
						player.discardCard (disco);
					}
				}
			} else {
				for (int i = 0; i < hand.Length; i++) {
					for (int j = 0; j < hand.Length; j++) {
						if (i == j) {
							continue;
						}
						if (hand [i].getName () == hand [j].getName ()) {
							disco [0] = hand [j];
							player.discardCard (disco);
							hand [j] = new AdvCard (null, -1);
						}
					}
				}
			}
			break;
		}

	}
	*/
	//AI submits cards for current quest stage (as a sponsor).
	public void sponsorQuestSetup(Card[] hand, ActiveQuest quest){
		Card[] submit = new Card[quest.getStageNum()];
		Card[] submitWeapons = new Card[10];
		hand = sortHand (hand);
		int BPtotal = 0;
		int count = 0;
		switch (strategy) {
		case 1:
			break;

		case 2:
			for (int i = 0; i < quest.getStageNum (); i++) {
				for (int j = 0; j < hand.Length; j++) {
					if (i == (quest.getStageNum () - 2)) {
						if (hand [j].getType () == "test") {
							submit [i] = hand [j];
							hand [j] = new AdvCard (null, -1);
							break;
						}
					}
					if (hand [j].getType () == "foe") {
						submit [i] = hand [j];
						hand [j] = new AdvCard (null, -1);
						break;
					}
				}
			}
			if (submit [quest.getStageNum () - 1].getBP () < 40) {
				BPtotal = submit [quest.getStageNum () - 1].getBP ();
				for (int i = 0; i < hand.Length; i++) {
					if (hand [i].getType () != "weapon") {
						continue;
					}
					submitWeapons [count] = hand [i];
					count++;
					BPtotal = BPtotal + hand [i].getBP();
					if (BPtotal >= 40) {
						break;
					}
				}
			}
			quest.setStages (submit);
			quest.setStage (quest.getStageNum () - 1);
			quest.setStageWeapons (submitWeapons);
			quest.setStage (0);
			player.discardCard (submit);
			player.discardCard (submitWeapons);
			break;
		}
	}

	//AI submits cards for current quest stage (as a player).
	public Card[] playQuestStage(Card[] hand, ActiveQuest quest){
		if (quest.getCurrentStage ().getType () == "test") {
			return nextBid (hand, quest);
		}
		Card[] submit = new Card[12];
		int BPhurdle = 0;
		int count = 0;
		hand = sortHandByType(sortHand (hand));
		switch (strategy) {
		case 1:
			break;

		case 2:
			if (quest.getCurrentStageNum () == quest.getStageNum ()) {
				for (int i = 0; i < hand.Length; i++) {
					if (hand [i].getType () == "foe" || hand [i].getType () == "test") {
						continue;
					} else {
						submit [count] = hand [i];
						count++;
					}
				}
			}else {
				BPhurdle = (quest.getCurrentStageNum () + 1) * 10;

				for (int i = 0; i < hand.Length; i++) {
					if (hand [i].getType () == "foe" || hand [i].getType () == "test") {
						continue;
					} else if (player.getBP () + hand [i].getBP () > BPhurdle) {
						submit [count] = hand [i];
						count++;
					}
				}
			}
			break;
		}
		player.discardCard (submit);
		return submit;
	}
	
	private Card[] sortHand(Card[] hand){
		bool sorted = false;
		int swaps;
		if (hand.GetLength (0) < 2) {
			return hand;
		}
		Card temp;
		Card[] submit = new Card[hand.Length];
		while(!sorted){
			swaps = 0;
			for(int i=0; i<hand.GetLength(0)-1; i++){
				if (hand [i].getBP () > hand [i + 1].getBP ()) {
					temp = hand [i + 1];
					hand [i + 1] = hand [i];
					hand [i] = temp;
					swaps++;
				}
			}
			if (swaps == 0) {
				sorted = true;
			}
		}
		return submit;
	}

	private Card[] sortHandByType(Card[] hand){
		Card[] submit = new Card[12];
		int count = 0;
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType () == "amour") {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType () == "ally") {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType () == "weapon") {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType () == "foe") {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType () == "test") {
				submit [count] = hand [i];
				count++;
			}
		}
		return submit;
	}
}

