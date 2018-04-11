using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy2AI : AbstractAI{


	//When tournament is played, this is called so AI decides whether/what to submit. Returns the cards it wants to play. If empty array, AI refuses to play.
	public Strategy2AI(Player _player){
		hp = new HelperFunctions();
		player = _player;
		hand = player.getHand ();
		log.setSource("AIController: " + player.getName());
		player.assumingDirectControl (this);
	}
	public override bool doIParticipateInTournament(Player currPlayer, ActiveTourney tourney, Player[] players){
		if(currPlayer == player){
			Debug.Log("Joining tournament");
			return true;
		}
		else {
			Debug.Log("Not joining tournament");
			return false;
		}
	}
	public override Card[] playTournament(ActiveTourney tourney){
		Card[] tempHand = getOnlyType("ally", "weapon", "amour");
		tempHand = sortHand(tempHand);
		int BPtotal = 0;
		Card[] submit = null;
		for (int i = tempHand.Length - 1; i >= 0; i--) {
			if (BPtotal < 50 && BPtotal + tempHand[i].getBP () <= 50) { //if we have less than 50, and adding this wouldn't go over 50, add it, then find the next card cost down
				submit = hp.addCard(submit, tempHand[i]);
				BPtotal = BPtotal + tempHand [i].getBP ();
			} 
			if (BPtotal >= 50) {
				break;
			}
		}
		return submit;
	}

	public override bool doIParticipateInQuest (QuestCard quest){
		bool c1 = false;
		bool c2 = false;
		int count = 0;
		
		Card [] weaponHand = getOnlyType("weapon");
		weaponHand = sortHand(weaponHand);
		
		Card [] amourHand = getOnlyType("amour");
		amourHand = sortHand(amourHand);
		
		Card [] allyHand = getOnlyType("ally");
		allyHand = sortHand(allyHand);
		
		int totalIncrementsBy10 = -1;
		int lastBP = -1;
		int weaponHandLength = 0;
		int allyHandLength = 0;
		if(weaponHand != null){
			weaponHandLength = weaponHand.Length;
		}
		if(allyHand != null){
			allyHandLength = allyHand.Length;
		}
		//Handle c1
		if(weaponHandLength + allyHandLength >= quest.getStages()){
			c1 = true;
		}

		//Handle c2

		count = 0;
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType().Equals("foe") && hand [i].getBP () <= 20) {
				count++;
			}
		}
		if (count >= 2) {
			Debug.Log("condition 2 is true");
			c2 = true;
		}
		if(c1 && c2){
			Debug.Log("Conditions met, participating in quest");
			return true;
		}
		else {
			Debug.Log("Conditions not met.");
			return false;
		}
		return false;
	}
	//
	public override Card[] nextBid(ActiveQuest quest){
		Card[] submit = null;
		if (quest.getHighestBid () > hand.Length) {
			return submit;
		}

		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType().Equals("foe") && hand[i].getBP() <= 20) {
				submit = hp.addCard(submit, hand [i]);
				if(submit.Length > quest.getHighestBid()) { return submit; }
				else {
					//quest.deletePlayer(player);
					return null;
				}
			}
			else {
				if(quest.getCurrentTestPhase() == 1 && hp.numberOfCardInstancesInHand(hand, hand[i]) > 1){
					submit = hp.addCard(submit, hand [i]);
				}
			}
		}
		return null;
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
	public override void sponsorQuestSetup(ActiveQuest quest){
		Card[] submit = new Card[quest.getStageNum()];
		Card[] submitWeapons = null;
		Card[] handFoesOnly = getOnlyType("foe");
		handFoesOnly = sortHand(handFoesOnly);
		int BPtotal = 0;
		int count = 0;

		submit[submit.Length-1] = hand[hand.Length-1];
		//First determine if we have a test.
		Card test = null;
		for(int i = 0; i< hand.Length; i++){
			if(hand[i].getType().Equals("test")) {
				test = hand[i];
				break;
			}
		}
		for(int i = 0; i < submit.Length-1; i++){
			Debug.Log(i + ": " + handFoesOnly[i] + " (" + handFoesOnly[i].getBP()+")");
			submit[i] = handFoesOnly[i];
		}
		if(test != null){
			submit[submit.Length-2] = test;
		}
			
		quest.setStages (submit);
		for(int i = 0; i < quest.getStageNum(); i ++){
			quest.setStage (i);
			quest.setStageWeapons (new Card[1]{null});
		}
		//quest.setStage (quest.getStageNum () - 1);
		//quest.setStageWeapons ();
		if(submit[submit.Length-1].getBP() < 40){
			int differenceToMake = 40  - submit[submit.Length-1].getBP();
			Card[] handWeaponsOnly = getOnlyType("weapon");
			
			handWeaponsOnly = sortHand(handWeaponsOnly);
			Card[] finalStageWeapons = null;
			for(int i = 0; i < handWeaponsOnly.Length; i++){
				
				finalStageWeapons = hp.addCard(finalStageWeapons, handWeaponsOnly[i]);
				differenceToMake = differenceToMake - handWeaponsOnly[i].getBP();
				if(differenceToMake <= 0) {
					break;
				}
			}
			quest.setStage (quest.getStageNum()-1);
			quest.setStageWeapons (finalStageWeapons);
			
		}
		quest.setStage (0);
		player.discardCard (submit);
		player.discardCard (submitWeapons);
	}

	//AI submits cards for current quest stage (as a player).
	public override Card[] playQuestStage(ActiveQuest quest){
		if (quest.getCurrentStage ().getType ().Equals("test")) {
			return nextBid (quest);
		}
		Card[] submit = null;
		int BPhurdle = 0;
		
		int count = 0;
		//hand = sortHand ();
		//hand = sortHandByType ();
		
		Card [] weaponHand = getOnlyType("weapon");
		weaponHand = sortHand(weaponHand);
		
		Card [] amourHand = getOnlyType("amour");
		amourHand = sortHand(amourHand);
		
		Card [] allyHand = getOnlyType("ally");
		allyHand = sortHand(allyHand);
		
		int weaponHandLength = 0;
		int allyHandLength = 0;
		int amourHandLength = 0;
		if(weaponHand != null){
			weaponHandLength = weaponHand.Length;
		}
		if(allyHand != null){
			allyHandLength = allyHand.Length;
		}
		if(amourHand != null){
			amourHandLength = amourHand.Length;
		}

		
		if(allyHandLength > 0){
			for(int i = 0; i < allyHandLength; i++){
				submit = hp.addCard(submit, allyHand[i]);
			}
		}
		else if(amourHandLength > 0){
			for(int i = 0; i < amourHandLength; i++){
				submit = hp.addCard(submit, amourHand[i]);
			}
		}
		if(submit.Length < 2){
			for(int i = 0; i < weaponHandLength; i++){
				submit = hp.addCard(submit, weaponHand[i]);
				if(submit.Length == 2){
					break;
				}
			}
		}
		player.discardCard (submit);
		return submit;
	}
}

