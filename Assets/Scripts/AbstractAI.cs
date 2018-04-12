using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAI{

	protected int strategy;
	protected Player player;
	protected Card[] hand;
	protected HelperFunctions hp;
	protected Logger log = new Logger("AINull");

	//When tournament is played, this is called so AI decides whether/what to submit. Returns the cards it wants to play. If empty array, AI refuses to play.
	abstract public bool doIParticipateInTournament(Player currPlayer, ActiveTourney tourney, Player[] players);
	abstract public Card[] playTournament(ActiveTourney tourney);
	public  bool doISponsorAQuest (Player[] players, QuestCard quest){
		Debug.Log("AI is considering if it should sponser a quest");
		int hasTest = 0; //either 0 or 1, and we use int rather than bool to use it in quest stage calculations
		int lowestBP = 0;
		int totalFoes = 0;
		//hand = sortHand ();
		bool result = true;

		for (int i = 0; i < players.Length; i++) {
			if(players [i].getRank() == 0){
				if ((quest.getStages() + players [i].getShields ()) >= 5) {
					return false;
				}
			}
			else if(players [i].getRank() == 1){
				if ((quest.getStages() + players [i].getShields ()) >= 7) {
					return false;
				}
			}
			else if(players [i].getRank() == 2){
				if ((quest.getStages() + players [i].getShields ()) >= 10) {
					return false;
				}
			}
		}

		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType().Equals("test")) {
				hasTest = 1;
				break;
			}
			if (hand[i].getType().Equals("foe") && hand[i].getBP()>lowestBP) {
				totalFoes++;
				lowestBP = hand [i].getBP ();
			}
		}
		if (quest.getStages() < totalFoes + hasTest) {
			Debug.Log("As " + quest.getStages() + " < " + totalFoes + " + " + hasTest + ", AI has accepted sponsorship");
			return true;
		}
		Debug.Log("As " + quest.getStages() + " > " + totalFoes + " + " + hasTest + ", AI has declined sponsorship");
		return false;
	}	
	abstract public bool doIParticipateInQuest (QuestCard quest);
	abstract public Card[] nextBid(ActiveQuest quest);	

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
	public abstract void sponsorQuestSetup(ActiveQuest quest);
	//AI submits cards for current quest stage (as a player).
	public abstract Card[] playQuestStage(ActiveQuest quest);
	protected Card[] sortHand(Card[] handToSort){
		bool sorted = false;
		int swaps;
		if(handToSort == null){
			return null;
		}
		if (handToSort.Length < 2) {
			return handToSort;
		}
		Card temp;
		Card[] submit = handToSort;
		while(!sorted){
			swaps = 0;
			for(int i=0; i < submit.Length-1; i++){
				if (submit[i].getBP() > submit [i + 1].getBP()) {
					temp = submit [i + 1];
					submit [i + 1] = submit [i];
					submit [i] = temp;
					swaps++;
				}
			}
			if (swaps == 0) {
				sorted = true;
			}
		}
		return submit;
	}
	protected Card[] sortHandByType(){
		Card[] submit = new Card[hand.Length];
		int count = 0;
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType ().Equals("amour")) {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType ().Equals("ally")) {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType ().Equals("weapon")) {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType ().Equals("foe")) {
				submit [count] = hand [i];
				count++;
			}
		}
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType ().Equals("test")) {
				submit [count] = hand [i];
				count++;
			}
		}
		return submit;
	}	
	protected Card[] getOnlyType(string type1, string type2 = "null", string type3 = "null") {
		Card[] result = null;
		for(int i = 0; i< hand.Length; i++){
			if(hand[i].getType().Equals(type1) || hand[i].getType().Equals(type2) || hand[i].getType().Equals(type3)){
				result = hp.addCard(result, hand[i]);
			}
		}
		return result;
	}
}

