using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {


	public Card[] hand,inPlay;
	protected int shields;
	public int rank;
	private string name;
	private int BP; 
	private bool human = true;
	private AbstractAI AI = null;
	Logger log = new Logger("PlayerNull");
	private int connectionID;

	public Player(Card[] _hand, int _shields, int _rank, string _name, int id = -1){
		hand = _hand;
		shields = _shields;
		rank = _rank;
		name = _name;
		BP = 5;
		log.setSource("Player: " + name);
		connectionID = id;
	}

	public Logger getLogger(){
		return log;
	}

	public void setHand(Card[] newHand){
		hand = newHand;
	}
	public void setInPlayHand(Card[] newHand){
		inPlay = newHand;
	}
	
	public void addCard(Card[] cardsToAdd, bool isInPlay = false) {
		if(cardsToAdd == null){
			DebugX.Log("ERROR trying to add null cards");
			return;
		}

		if(isInPlay){

			if(inPlay == null){
				DebugX.Log("inPlay is getting its first cards");
				inPlay = cardsToAdd;
				return;
			}		
			int newSize = inPlay.Length + cardsToAdd.Length;
			Card[] temp = new Card[newSize];
			for(int i = 0; i< inPlay.Length; i++){
				temp[i] = inPlay[i];
			}
			for(int i = inPlay.Length; i < newSize; i++) {
				temp[i] = cardsToAdd[i-inPlay.Length];
			}

			inPlay = temp;
		}
		else {
			if(hand == null){
				hand = cardsToAdd;
				return;
			}		
			int newSize = hand.Length + cardsToAdd.Length;
			Card[] temp = new Card[newSize];
			for(int i = 0; i< hand.Length; i++){
				temp[i] = hand[i];
			}
			for(int i = hand.Length; i < newSize; i++) {
				temp[i] = cardsToAdd[i-hand.Length];
			}

			hand = temp;

		}
	}

	public void addShields(int newShields) {
		shields += newShields;
		log.log ("Received " + newShields + " shields.");
		changeRank();
	}

	public void removeShields(int _remove){
		if (_remove > shields) {
			log.log ("Lost " + shields + " shields, down to the minimum of zero.");
			shields = 0;
		} else {
			log.log ("Lost " + _remove + "shields.");
			shields = shields - _remove;
		}
	}

	public Card[] getHand(bool isInPlay = false){
		if(isInPlay) {
			DebugX.Log("Returning inPlay");
			return inPlay;
		}
		else{
			return hand;
		}
	}

	public string getName(){
		return name;
	}
	//Deletes a card from a hand.
	public int getFreeBids(string quest) {
		int freeBids = 0;
		if(inPlay != null) {
			for(int i = 0; i< inPlay.Length; i++) {
				if(inPlay[i].getFreeBid(quest) != -1){
					DebugX.Log("free bid found");
					freeBids = freeBids + inPlay[i].getFreeBid(quest);
				}
			}
		}

		return freeBids;
	}

	public int getBP(string quest){
		int extraBP = 0;

		if(inPlay != null){
			for(int i = 0; i < inPlay.Length; i++) {
				extraBP = extraBP + inPlay[i].getBP(quest);
			}
			return BP + extraBP;
		}
		return BP;

	}
	
	public int getShields() { return shields; }
	public int getRank() { 
		return rank;
	}
	public string getRankString() { 
		if(rank == 0) {
			return "Squire"; 
		}
		else if(rank == 1) {
			return "Knight";
		}
		else if(rank == 2) {
			return "Champion Knight";
		}
		else if(rank == 3) {
			return "Knight Of The Round Table";
		}
		else {
			return "ERROR";
		}
		
	}

	public void discardCard(Card [] card){
		if(card == null) {DebugX.Log("Removing nothing"); return;}

		int j;

		for(int k = 0; k < card.Length; k++)
		{
			int n = hand.Length;

			Card[] temp = new Card[n-1];
			j = -1;
			for(int i = 0; i < n; i++){
				if(hand[i] == card[k]){
					j = i;
					break;
				}
			}
			if(j == -1){
				DebugX.Log("Requested card to delete is not in hand");
				return;
			}


			if(j == 0){
				for(int i = 0; i < n-1; i++)
				{
					temp[i] = hand[i];
				}	
			}
			else if(j == hand.Length){
				for(int i = 1; i < n; i++)
				{
					temp[i-1] = hand[i];
				}
			}
			else
			{
				for(int i = 0; i < j; i++){
					temp[i] = hand[i];
				}
				for(int i = j+1; i < hand.Length; i++){
					temp[i-1] = hand[i];
				}
			}
			hand = null;
			hand = temp;
		}

		return;	
	}

	public void changeRank(){
		if (shields >= 5 && shields < 7) {
			rank = 1;
			BP = 10;
			log.log (name + " ranks up to Knight!");
			if (shields > 5)
				shields -= 5;
		} else if (shields >= 7 && shields < 10) {
			rank = 2;
			BP = 20;
			log.log (name + " ranks up to Champion Knight!");
			if (shields > 7)
				shields -= 7;
		} else if (shields >= 10) {
			rank = 3;
			log.log (name + " is eligible for victory!");
		}

	}

	public bool isHuman(){
		return human;
	}

	public void assumingDirectControl(AbstractAI _AI){
		human = false;
		AI = _AI;
	}

	public AbstractAI getAI(){
		return AI;
	}
	
	public int removeAlly(string quest){
		if(inPlay == null){
			return -1;
		}
		int returnValue = inPlay[0].getFreeBid(quest);
		if(inPlay.Length == 1) {
			inPlay = null;
			return returnValue;
		}
		Card [] temp = new Card[inPlay.Length-1];

		for(int i = 1; i < inPlay.Length; i++){
			temp[i-1] = inPlay[i];
		}
		setInPlayHand(temp);

		return returnValue;
	}

	public int getConnectionID(){
		return connectionID;
	}

	public int getNumOfTypeOfCard(string cardType){
		int result = 0;
		DebugX.Log("hand size: " + hand.Length);
		if(hand == null){
			return 0;
		}
		for(int i = 0; i < hand.Length; i++){
			if(hand[i] != null){
				DebugX.Log("("+i+"): " + hand[i].getType()); 

				if(hand[i].getType().Equals(cardType)){
					result ++;
				}

			}
		}
		return result;
	}

}
