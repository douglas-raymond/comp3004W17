using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
public class EventCard : StoryCard {

  
	Logger log = new Logger("EventCard");
	HelperFunctions hp;
	public EventCard(string _name, string _type, Sprite _sprite)
	{
		name = _name;
		type = _type;
		log.log ("creating card " + name);
		sprite = _sprite;
		hp = new HelperFunctions();
	}

	public override void runEvent(Player[] playerList, int activePlayer, int playerCount, AdvDeck advDeck, GameManager gm)
	{
		int lowestShields = 50; //arbitrarily high count
		int lowestRank = 10; //arbitrarily high count
		Card[] draws = new Card[2];
		Card[] empty = null;
		
		int highestShields = -1;
		for(int i = 0; i < playerList.Length; i++){
			if(playerList[i].getShields() > highestShields){
				highestShields = playerList[i].getShields();
			}
		}
		Player [] playersAffected = null;
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
			
		case "courtcalled":
			//all players discard all allies
			Debug.Log("Playing courtcalled. Current Player is: " + playerList[activePlayer].getName());
			for (int i = 0; i < playerCount; i++) {
				playerList [i].setInPlayHand (null);
				playerList [i].getLogger ().log ("Discarding all cards in play.");
			}
			gm.drawQuestCard();
			break;
			
		case "kingscall":
			Debug.Log("Playing kingscall. Current Player is: " + playerList[activePlayer].getName());
			for (int i = 0; i < playerList.Length; i++) {
				Debug.Log(i);
				if(playerList[i].getShields() == highestShields){
					playersAffected = hp.addPlayer(playersAffected, playerList[i]);
				}
			}
			gm.kingsCall(playersAffected);
			
			//top player(s) must discard 1 weapon. if they can't they must discard 2 foes
			break;
			
		case "recognition":
			Debug.Log("Playing recognition. Current Player is: " + playerList[activePlayer].getName());
			gm.recognition();
			//next player(s) to complete a quest get 2 extra shields
			break;
			
		case "plague":
			//active player loses 2 shields if possible
			Debug.Log("Playing plague. Current Player is: " + playerList[activePlayer].getName());
			if(playerList[activePlayer].getShields() > 1){
				Debug.Log("Removing 2 shields from " + playerList[activePlayer].getName());
				playerList[activePlayer].removeShields (2);
			}
			gm.drawQuestCard();
			break;
			
		case "pox":
			//all players but active players lose 1 shield if possible
			Debug.Log("Playing pox. Current Player is: " + playerList[activePlayer].getName());
			for(int i=0; i<playerCount; i++){
				if(i !=activePlayer && playerList[i].getShields() > 0) {
					Debug.Log("Removing a shield from " + playerList[i].getName());
					playerList[i].removeShields(1);
				}
			}
			gm.drawQuestCard();
			break;
			
		case "prosperity":
			//all players draw 2 adventure cards
			for(int i=0; i<playerCount; i++){
				log.log("prosperity through the kingdom called");
				
				gm.drawXGeneralNumberOfCards(2, state.PROSPERITY, playerList);
			}
			break;
			
		case "queensfavor":
			Debug.Log("Playing queensfavor. Current Player is: " + playerList[activePlayer].getName());
			//player(s) in last draw 2 adventure cards
			for (int i = 0; i < playerCount; i++) {
				if (playerList [i].getShields () < lowestShields) {
					lowestShields = playerList [i].getShields ();
				}
			}
			for (int j = 0; j < playerCount; j++) {
				if(playerList[j].getShields() == lowestShields){
					playersAffected = hp.addPlayer(playersAffected, playerList[j]);
				}
			}
			
			gm.drawXGeneralNumberOfCards(2, state.QUEENSFAVOR, playersAffected);
			break;
			
		}
		return;
	}
}