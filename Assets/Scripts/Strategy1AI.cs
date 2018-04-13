using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy1AI : AbstractAI{

	int tounamentReward = 0;
	//When tournament is played, this is called so AI decides whether/what to submit. Returns the cards it wants to play. If empty array, AI refuses to play.
	public Strategy1AI(Player _player){
		hp = new HelperFunctions();
		player = _player;
		hand = player.getHand ();
		log.setSource("AIController: " + player.getName());
		player.assumingDirectControl (this);
	}
	public override bool doIParticipateInTournament(Player currPlayer, ActiveTourney tourney, Player[] players){
		DebugX.Log("AI is debating if it will enter the tourney");
		tounamentReward = tourney.getAwardNum();
		bool someoneWillEvolve = false;
		for(int i = 0; i< players.Length; i++){
			if(players[i] != player) {
				if(hp.willPlayerEvolve(players[i], tounamentReward)){
					DebugX.Log("AI has noticed that " + players[i] + " will evolve");
					someoneWillEvolve = true;
				}
			}
		}
		if(currPlayer == player && someoneWillEvolve){
			DebugX.Log("Joining tournament as I announce and someone will evolve");
			return true;
		}
		else {
			DebugX.Log("Not joining tournament");
			return false;
		}
	}
	public override Card[] playTournament(ActiveTourney tourney){
		bool someoneWillEvolve = false;
		bool iWillEvolve = false;
		Card[] tempHand = getOnlyType("ally", "weapon", "amour");
		tempHand = sortHand(tempHand);
		int BPtotal = 0;
		Card[] submit = null;

		for(int i = 0; i< tourney.getPlayerArr().Length; i++){
			if(tourney.getPlayerArr()[i] != player) {
				if(hp.willPlayerEvolve(tourney.getPlayerArr()[i], tounamentReward)){
					someoneWillEvolve = true;
				}
			}
		}

		iWillEvolve = hp.willPlayerEvolve(player, tounamentReward);
		if(iWillEvolve || someoneWillEvolve) {
			Card [] weaponHand = getOnlyType("weapon");
			weaponHand = sortHand(weaponHand);

			Card [] amourHand = getOnlyType("amour");
			amourHand = sortHand(amourHand);

			Card [] allyHand = getOnlyType("ally");
			allyHand = sortHand(allyHand);

			if(amourHand != null){
				for(int i = 0; i < amourHand.Length; i++){
					submit = hp.addCard(submit, amourHand[i]);
				}
			}
			if(allyHand != null){
				for(int i = 0; i < allyHand.Length; i++){
					DebugX.Log("Playing a " + allyHand[i].getName());
					submit = hp.addCard(submit, allyHand[i]);
				}
			}
			if(weaponHand != null){
				for(int i = 0; i < weaponHand.Length; i++){
					if(!hp.checkIfArrayContainsCard(submit, weaponHand[i])){
						DebugX.Log("Playing a " + weaponHand[i].getName());
						submit = hp.addCard(submit, weaponHand[i]);
					}
				}
			}
			player.discardCard(submit);
			return submit; 
		}
		else {
			for(int i = 0; i< hand.Length; i++){
				if(hp.numberOfCardInstancesInHand(hand, hand[i]) >= 2) {
					submit = hp.addCard(submit, hand[i]);
					player.discardCard(submit);
					return submit; 
				}
			}
		}
		return submit;
	}

	public override bool doIParticipateInQuest (QuestCard quest){
		DebugX.Log("Asking AI to participate in quest");
		bool c1 = false;
		bool c2 = false;
		int lowestBP = 0;
		int count = 0;
		Card [] tempHand = getOnlyType("weapon");

		tempHand = sortHand (tempHand);
		int totalIncrementsBy10 = -1;
		int lastBP = -1;

		//Handle c1
		for(int i = 0; i< tempHand.Length; i++){
			if(lastBP == -1){
				lastBP = tempHand[i].getBP();
				totalIncrementsBy10 = 1;
			}
			else if(tempHand[i].getBP() >= lastBP + 10){
				lastBP = tempHand[i].getBP();
				totalIncrementsBy10++;
			}
		}
		DebugX.Log("totalIncrementsBy10: "+ totalIncrementsBy10);
		DebugX.Log("lastBP: "+ lastBP);
		if(totalIncrementsBy10 >= quest.getStages()){
			c1 = true;
			DebugX.Log("condition 1 is true");
		}
		//Handle c2
		count = 0;
		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType().Equals("foe") && hand [i].getBP () <= 25) {
				count++;
			}
		}
		if (count >= 2) {
			DebugX.Log("condition 2 is true");
			c2 = true;
		}
		if(c1 && c2){
			DebugX.Log("Conditions met, participating in quest");
			return true;
		}
		else {
			DebugX.Log("Conditions not met.");
			return false;
		}
	}
	//
	public override Card[] nextBid(ActiveQuest quest){
		Card[] submit = null;
		if (quest.getHighestBid () > hand.Length) {
			DebugX.Log("Cannot place bid");
			//quest.deletePlayer(player);
			return submit;
		}

		for (int i = 0; i < hand.Length; i++) {
			if (hand [i].getType().Equals("foe") && hand[i].getBP() <= 20) {
				submit = hp.addCard(submit, hand [i]);
				if(submit.Length > quest.getHighestBid()) { return submit; }
				else {
					DebugX.Log("Bid too low");
					//quest.deletePlayer(player);
					return null;
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

		Card[] handWeaponsOnly = getOnlyType("weapon");
		handWeaponsOnly = sortHand(handWeaponsOnly);
		int BPtotal = 0;
		int count = 0;

		submit[submit.Length-1] = handFoesOnly[handFoesOnly.Length-1];
		//First determine if we have a test.
		Card test = null;
		for(int i = 0; i< hand.Length; i++){
			if(hand[i].getType().Equals("test")) {
				test = hand[i];
				break;
			}
		}

		for(int i = 0; i < submit.Length-1; i++){
			DebugX.Log(i + ": " + handFoesOnly[i] + " (" + handFoesOnly[i].getBP()+")");
			submit[i] = handFoesOnly[i];
		}
		int mainQuestStagesStart = 0;
		int currentMainStageIndex = submit.Length-2;
		if(test != null){
			DebugX.Log("Quest will cointain a test as AI has one");
			submit[submit.Length-2] = test;
			mainQuestStagesStart = 3;
		}
		else {
			mainQuestStagesStart = 2;
		}
		for(int i = submit.Length-mainQuestStagesStart; i >= 0; i-- ){
			submit[i] = handFoesOnly[currentMainStageIndex];
			currentMainStageIndex --;
		}
		DebugX.Log("AI is submitting");
		for(int i = 0; i < submit.Length; i++){
			DebugX.Log(submit[i].getName());
		}
		quest.setStages (submit);

		if(submit[submit.Length-1].getBP() < 50){
			int finalStageBP = submit[submit.Length-1].getBP(); 
			for(int i = 0; i < handWeaponsOnly.Length; i++){
				finalStageBP = finalStageBP + handWeaponsOnly[i].getBP();
				submitWeapons = hp.addCard(submitWeapons, handWeaponsOnly[i]);

				if(finalStageBP >= 50){
					break;
				}
			}

			quest.setStage (submit.Length-1);
			
			if(submitWeapons != null){
				DebugX.Log("For stage weapons, AI is submitting");
				for(int i = 0; i < submit.Length; i++){
					DebugX.Log(submit[i].getName());
				}
			}
			quest.setStageWeapons (submitWeapons);
			player.discardCard(submitWeapons);
			submitWeapons = null;

			handWeaponsOnly = getOnlyType("weapon");
			handWeaponsOnly = sortHand(handWeaponsOnly);
		}
		for(int i = 0; i < quest.getStageNum()-1; i ++){
			quest.setStage (i);
			quest.setStageWeapons (new Card[1]{null});
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


		int bp = 0;
		BPhurdle = quest.getCurrentStageNum ()+1  * 10;
		if(amourHand != null){
			for(int i = 0; i < amourHand.Length; i++){
				if(bp >= BPhurdle){
					player.discardCard (submit);
					return submit;
				}
				submit = hp.addCard(submit, amourHand[i]);
				if (quest.getCurrentStageNum () < quest.getStageNum()-1){						
					bp = bp + amourHand[i].getBP();
				}
			}
		}
		if(allyHand != null){
			for(int i = 0; i < allyHand.Length; i++){
				if(bp >= BPhurdle){
					player.discardCard (submit);
					return submit;
				}
				submit = hp.addCard(submit, allyHand[i]);
				if (quest.getCurrentStageNum () < quest.getStageNum()-1){					
					bp = bp + allyHand[i].getBP();
				}
			}
		}
		if(weaponHand != null){
			for(int i = 0; i < weaponHand.Length; i++){
				if(bp >= BPhurdle){
					player.discardCard (submit);
					return submit;
				}
				if(!hp.checkIfArrayContainsCard(submit, weaponHand[i])){
					submit = hp.addCard(submit, weaponHand[i]);
					if (quest.getCurrentStageNum () < quest.getStageNum()-1){
						bp = bp + weaponHand[i].getBP();
					}
				}
			}
		}
		player.discardCard (submit);
		return submit;
	}
}
