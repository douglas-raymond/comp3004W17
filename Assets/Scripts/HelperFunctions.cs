using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions {
	public Player[] addPlayer(Player[] playerArr, Player newPlayer) {
		if(playerArr == null){
			return new Player[]{newPlayer};
		}
		Player[] newPlayerArr = new Player[playerArr.Length+1];
		
		for(int i = 0; i< playerArr.Length; i++){
			newPlayerArr[i] = playerArr[i];
		}
		
		newPlayerArr[playerArr.Length] = newPlayer;
		
		return newPlayerArr;
	}
	public Card[] addCard(Card[] cardArr, Card newCard) {
		if(cardArr == null){
			return new Card[]{newCard};
		}
		Card[] newCardArr = new Card[cardArr.Length+1];
		
		for(int i = 0; i< cardArr.Length; i++){
			newCardArr[i] = cardArr[i];
		}
		
		newCardArr[cardArr.Length] = newCard;
		
		return newCardArr;
	}
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
	public bool checkIfArrayContainsCard(Card[] arr, Card cardToFind) {
		if(arr == null) { return false;}
		for(int i = 0; i < arr.Length; i++){
			if(arr[i].getName().Equals(cardToFind.getName())){
				return true;
			}
		}
		return false;
	}
	public int shieldsToRank(int shields){
		if(shields < 5){
			return 0;
		}
		else if (shields >= 5 && shields < 7) {
			return 1;
		} 
		else if (shields >= 7 && shields < 10) {
			return 2;
		} 
		else if (shields >= 10) {
			return 3;
		}
		else{
			return 0;
		}
		
	}
	public bool willPlayerEvolve(Player player, int reward) {
		int prevShields = player.getShields();
		int prevRank = shieldsToRank(prevShields);
		if(prevRank < shieldsToRank(prevShields + reward)){
			return true;
		}
		else {return false;}
	}
	
	public int numberOfCardInstancesInHand(Card[] arr, Card cardToCount){
		int count = 0;
		for(int i = 0; i < arr.Length; i++){
			if(cardToCount.getName().Equals(arr[i].getName())){
				count ++;
			}
		}
		return count;
	}
}
