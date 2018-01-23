using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Deck  {

	protected Stack<Card> deck = new Stack<Card>();
	
	//Shuffles the stack of cards into a new order
	public void shuffle() {
		Card[] tempDeck = new Card[deck.Count];
		int n = deck.Count;
		for(int i = 0; i< n; i++)
		{
			tempDeck[i] = deck.Pop();
		}
		
		arrayShuffle(tempDeck);
		for(int i = 0; i< n; i++)
		{
			deck.Push(tempDeck[i]);
		}
	}
	
	//remotes a card from the stack and returns it
	public Card drawCard()
	{
		return deck.Pop();
	}
	
	//returns acard to the stack
	public void returnCard(Card card)
	{
		deck.Push(card);
		return;
	}
	private Card[] arrayShuffle(Card[] deck){
	    //Random r = new Random();
		//  Based on Java code from wikipedia:
		//  http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
		
		Card[] tempDeck = deck;
        for (int n = deck.Length - 1; n > 0; --n)
        {
            int k = Random.Range(0, n+1);
            Card temp = tempDeck[n];
            tempDeck[n] = tempDeck[k];
            tempDeck[k] = temp;
        }
		
		return tempDeck;
    }
}
