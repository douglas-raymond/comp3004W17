using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public Card[] hand;
	protected int shields;
	public int rank;
	
	public Player(Card[] _hand, int _shields, int _rank)
	{
		hand = _hand;
		shields = _shields;
		rank = _rank;
	}
	public void setHand(Card[] newHand)
	{
		hand = newHand;
	}
	
	public void printHand()
	{
		for(int i =0; i < hand.Length-1; i++)
		{
			Debug.Log(hand[i].getName());
		}
	}
	bool isNextRank()
	{
		return true;
	}
}
