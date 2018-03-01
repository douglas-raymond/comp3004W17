using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAI{

	int strategy;

	protected AbstractAI(){
	}

	//When tournament is played, this is called so AI decides whether/what to submit. Returns the cards it wants to play. If empty array, AI refuses to play.
	public Card[] doIParticipateInTournament(Card[] hand, Player[] players){
		int BPtotal = 0;
		Card[] submit = new Card[12];
		int count = 0;
		sortHand (hand);
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
		return submit;
	}

	public bool doISponsorAQuest (Card[] hand, Player[] players, QuestCard quest){ //strategy 1 and 2 run this the exact same way, so no switch statement needed... yet
		int hasTest = 0; //either 0 or 1, and we use int rather than bool to use it in quest stage calculations
		int lowestBP = 0;
		int totalFoes = 0;
		sortHand (hand);
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
		sortHand (hand);
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
	/*
	public Card[] nextBid(){

	}

	public Card[] discardAfterWinningTest(){

	}

	public Card[] sponsorQuestSetup(QuestCard quest){

	}

	public Card[] playQuestStage(QuestCard quest){
		
	}
*/
	private void sortHand(Card[] hand){
		bool sorted = false;
		int swaps;
		if (hand.GetLength (0) < 2) {
			return;
		}
		Card temp;
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
	}
}

