using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions {
	public Card[] removeCard(Card [] cards, Card card) {
		Card[] temp = new Card[cards.Length-1];
		string cardsToKeep = "";
		int cardIndex = -1;
		
		for(int i = 0; i < cards.Length; i++) {
			if(cards[i].getName().Equals(card.getName())){
				cardIndex = i;
				break;
			}
		}
		if(cardIndex == -1) {
			return null;
		}
		if(cardIndex == 0) {
			for(int i = 1; i < cards.Length; i++){
				temp[i-1] = cards[i];
			}
		}
		else if(cardIndex+1 == cards.Length){
			for(int i = 0; i < cards.Length-1; i++) {
				temp[i] = cards[i];
			}
		}
		else{
			for(int i = 0; i< cardIndex; i++ ){
				temp[i] = cards[i];
			}
			for(int i = cardIndex+1; i< cards.Length; i++ ){
				temp[i-1] = cards[i];
			}
		}
		
		return temp;
	}

	public Player[] removePlayers(Player[] players, int player) {
		Player[] temp = new Player[players.Length-1];
		
		if(players[0].getName().Equals(players[player].getName())) {
			for(int i = 1; i < players.Length; i++){
				temp[i-1] = players[i];
			}
		}
		else if(players[temp.Length].getName().Equals(players[player].getName())){
			for(int i = 0; i < players.Length-1; i++) {
				temp[i] = players[i];
			}
		}
		else{
			for(int i = 0; i< player; i++ ){
				temp[i] = players[i];
			}
			for(int i = player+1; i< players.Length; i++ ){
				temp[i-1] = players[i];
			}
		}
		
		return temp;
	}	
}
