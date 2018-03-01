using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCard : StoryCard {

  
	Logger log = new Logger("EventCard");

	public EventCard(string _name, string _type, Sprite _sprite)
	{
		name = _name;
		type = _type;
		log.log ("creating card " + name);
		sprite = _sprite;
	}

	public override void runEvent(Player[] playerList, int activePlayer, int playerCount, AdvDeck advDeck, GameManager gm)
	{
		int lowestShields = 50; //arbitrarily high count
		int lowestRank = 10; //arbitrarily high count
		Card[] draws = new Card[2];
		Card[] empty = null;
		switch (name) {
		case "chivdeed":
			//player(s) in last receive 3 shields
			for (int i = 0; i < playerCount; i++) {
				if (playerList [i].getRank () < lowestRank) {
					lowestRank = playerList [i].getRank ();
				}
				if (playerList [i].getShields () < lowestShields) {
					lowestShields = playerList [i].getShields ();
				}
			}
			for (int j = 0; j < playerCount; j++) {
				if((playerList[j].getRank()==lowestRank) && (playerList[j].getShields()==lowestShields)){
					playerList[j].addShields(3);
				}
			}
			break;
			/*
		case "court called to camelot":
			//all players discard all allies
			for (int i = 0; i < playerCount; i++) {
				playerList [i].setInPlayHand (empty);
				playerList [i].getLogger ().log ("Discarding all cards in play.");
			}
			break;
		case "king's call to arms":
			//top player(s) must discard 1 weapon. if they can't they must discard 2 foes
			break;
		case "king's recognition":
			//next player(s) to complete a quest get 2 extra shields
			break;
		case "plague":
			//active player loses 2 shields if possible
			playerList [activePlayer].removeShields (2);
			break;
		case "pox":
			//all players but active players lose 1 shield if possible
			for(int i=0; i<playerCount; i++){
				if(i==activePlayer){continue;}
				else{
					playerList[i].removeShields(1);
				}
			}
			break;
			*/
		case "prosperity":
			//all players draw 2 adventure cards
			for(int i=0; i<playerCount; i++){
				log.log("prosperity through the kingdom called");
				
				gm.drawXGeneralNumberOfCards(2);
			}
			break;
			/*
		case "queen's favor":
			//player(s) in last draw 2 adventure cards
			for (int i = 0; i < playerCount; i++) {
				if (playerList [i].getRank () < lowestRank) {
					lowestRank = playerList [i].getRank ();
				}
				if (!(playerList [i].getRank () > lowestRank) && playerList [i].getShields () < lowestShields) {
					lowestShields = playerList [i].getShields ();
				}
			}
			for (int j = 0; j < playerCount; j++) {
				if((playerList[j].getRank()==lowestRank) && (playerList[j].getShields()==lowestShields)){
					draws[0] = advDeck.drawCard();
					draws[1] = advDeck.drawCard();
					playerList [j].getLogger ().log ("Player draws 2 adventure cards.");
					playerList[j].addCard(draws, false);
				}
			}
			break;
			*/
		}
		return;
	}
}