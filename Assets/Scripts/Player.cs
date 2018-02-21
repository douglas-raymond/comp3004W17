using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public Card[] hand,inPlay;
	protected int shields;
	public int rank;
	private string name;
	private int BP; 

	public Player(Card[] _hand, int _shields, int _rank, string _name){
		hand = _hand;
		shields = _shields;
		rank = _rank;
		name = _name;
		BP = 5;
	}
	public void setHand(Card[] newHand){
		hand = newHand;
	}
	public void addShields(int newShields)
	{
		shields += newShields;
		changeRank();
	}
	public Card[] getHand(){
		return hand;
	}	
	public string getName(){
		return name;
	}
	//Deletes a card from a hand.
	
	public int getBP(){
		return BP;
	}
	public void discardCard(Card [] card){
		if(card == null) {Debug.Log("Removing nothing"); return;}
		int n = hand.Length;
		int j = -1;
		Card[] temp = new Card[n-1];
		for(int k = 0; k < card.Length; k++)
		{
			for(int i = 0; i < n; i++){
				if(hand[i] == card[k]){
					j = i;
					break;
				}
			}
			if(j == -1){
				Debug.Log("Requested card to delete is not in hand");
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
		}
		hand = null;
		hand = temp;
		return;	
	}

	public void changeRank(){
		if (rank == 0 && shields >= 5) {
			rank = 1;
			if (shields > 5)
				shields -= 5;
		} else if (rank == 1 && shields >= 7) {
			rank = 2;
			if (shields > 7)
				shields -= 7;
		} else if (rank == 2 && shields >= 10) {
			rank = 3;
		}
			
	}
	
	public bool isNextRank(){
		return true;
	}
}
