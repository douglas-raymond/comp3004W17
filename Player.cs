using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public Card[] hand;
	protected int shields;
	public int rank;
	private string name;

	public Player(Card[] _hand, int _shields, int _rank, string _name){
		hand = _hand;
		shields = _shields;
		rank = _rank;
		name = _name;
	}
	public void setHand(Card[] newHand){
		hand = newHand;
	}
	
	public Card[] getHand(){
		return hand;
	}	
	public string getName(){
		return name;
	}
	//Deletes a card from a hand.
	public void discardCard(Card card){
		int n = hand.Length;
		int j = -1;
		for(int i = 0; i < n; i++){
			if(hand[i] == card){
				j = i;
				break;
			}
		}
		if(j == -1){
			Debug.Log("Requested card to delete is not in hand");
			return;
		}
		
		Card[] temp = new Card[n-1];
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
		return;
	}
	
	public bool isNextRank(){
		return true;
	}
}
