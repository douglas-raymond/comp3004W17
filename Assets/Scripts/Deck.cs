using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Deck  {

	protected Stack<Card> deck = new Stack<Card>();
	protected Stack<Card> discardDeck = new Stack<Card>();
	Logger log = new Logger("Deck");
	//Shuffles the stack of cards into a new order
	public void shuffle() {
		Card[] tempDeck = new Card[deck.Count];
		int n = deck.Count;
		for(int i = 0; i< n; i++)
		{
			tempDeck[i] = deck.Pop();
		}
		
		arrayShuffle(tempDeck);
		for(int i = 0; i< n; i++) {
			deck.Push(tempDeck[i]);
		}
	}
	
	//remotes a card from the stack and returns it
	public Card drawCard()
	{
		if(deck.Count > 0) {
			return deck.Pop();
		}
		else {
			refillDeck();
			return deck.Pop();
		}
	}
	
	public int getCount(){ return deck.Count;}
	
	//returns acard to the stack
	public void returnCard(Card card)
	{
		deck.Push(card);
		return;
	}
	
	
	public Card getCard(string cardToGet) {
		
		Card[] temp = deck.ToArray();
		Card toReturn = null;
		for(int i = 0; i< temp.Length ; i++) {
			if(temp[i].getName().Equals(cardToGet)) {
				toReturn = temp[i];
				temp[i] = null;
				break;
			}
		}
		if(toReturn == null) {return null;}
		else{
			deck = null;
			deck = new Stack<Card>();
			for(int i = 0; i< temp.Length; i++) {
				if(temp[i] != null) {
					deck.Push(temp[i]);
				}
			}
		}
		
		return toReturn;
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
	public void discardCard(Card [] discardedCard) {
		for(int i = 0; i <discardedCard.Length; i++) {
			discardDeck.Push(discardedCard[i]);
		}
	}
	private void refillDeck() {
		log.log("Deck empty. Refilling");
		while(discardDeck.Count > 0) {
			deck.Push(discardDeck.Pop());
		}
		shuffle();
	}
}
