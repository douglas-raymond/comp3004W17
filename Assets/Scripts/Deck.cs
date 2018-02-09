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
	
	public int getCount(){ return deck.Count;}
	
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


	protected Sprite getCardImage(string cardTitle)
	{
		if(cardTitle.Equals("excalibur")){ return Resources.Load<Sprite>("Cards/W Excalibur");}
		if(cardTitle.Equals("lance")){ return Resources.Load<Sprite>("Cards/W Lance");}
		if(cardTitle.Equals("battleax")){ return Resources.Load<Sprite>("Cards/W Battle-ax");}
		if(cardTitle.Equals("sword")){ return Resources.Load<Sprite>("Cards/W Sword");}
		if(cardTitle.Equals("horse")){ return Resources.Load<Sprite>("Cards/W Horse");}
		if(cardTitle.Equals("dagger")){ return Resources.Load<Sprite>("Cards/W Dagger");}
		if(cardTitle.Equals("dragon")){ return Resources.Load<Sprite>("Cards/F Dragon");}
		if(cardTitle.Equals("giant")){ return Resources.Load<Sprite>("Cards/F Giant");}
		if(cardTitle.Equals("mordred")){ return Resources.Load<Sprite>("Cards/F Mordred");}
		if(cardTitle.Equals("greenknight")){ return Resources.Load<Sprite>("Cards/F Green Knight");}
		if(cardTitle.Equals("blackknight")){ return Resources.Load<Sprite>("Cards/F Black Knight");}
		if(cardTitle.Equals("evilknight")){ return Resources.Load<Sprite>("Cards/F Evil Knight");}
		if(cardTitle.Equals("saxonknight")){ return Resources.Load<Sprite>("Cards/F Saxon Knight");}
		if(cardTitle.Equals("robberknight")){ return Resources.Load<Sprite>("Cards/F Robber Knight");}
		if(cardTitle.Equals("saxons")){ return Resources.Load<Sprite>("Cards/F Saxons");}
		if(cardTitle.Equals("boar")){ return Resources.Load<Sprite>("Cards/F Boar");}
		if(cardTitle.Equals("thieves")){ return Resources.Load<Sprite>("Cards/F Thieves");}
		if(cardTitle.Equals("tovalor")){ return Resources.Load<Sprite>("Cards/T Test of Valor");}
		if(cardTitle.Equals("toquestingbeast")){ return Resources.Load<Sprite>("Cards/T Test of the Questing Beast");}
		if(cardTitle.Equals("totemptation")){ return Resources.Load<Sprite>("Cards/T Test of Temptation");}
		if(cardTitle.Equals("tomorganlefey")){ return Resources.Load<Sprite>("Cards/T Test of Morgan Le Fey");}
		if(cardTitle.Equals("galahad")){ return Resources.Load<Sprite>("Cards/A Sir Galahad");}
		if(cardTitle.Equals("arthur")){ return Resources.Load<Sprite>("Cards/A King Arthur");}
		if(cardTitle.Equals("pellinore")){ return Resources.Load<Sprite>("Cards/A King Pellinore");}
		if(cardTitle.Equals("guinevere")){ return Resources.Load<Sprite>("Cards/A Queen Guinevere");}
		if(cardTitle.Equals("iseult")){ return Resources.Load<Sprite>("Cards/A Queen Iseult");}
		if(cardTitle.Equals("gawain")){ return Resources.Load<Sprite>("Cards/A Sir Gawain");}
		if(cardTitle.Equals("lancelot")){ return Resources.Load<Sprite>("Cards/A Sir Lancelot");}
		if(cardTitle.Equals("percival")){ return Resources.Load<Sprite>("Cards/A Sir Percival");}
		if(cardTitle.Equals("tristan")){ return Resources.Load<Sprite>("Cards/A Sir Tristan");}

		if (cardTitle.Equals ("amour")) {
			return Resources.Load<Sprite> ("Cards/Amour");
		}

		if(cardTitle.Equals("holygrail")){ return Resources.Load<Sprite>("Cards/Q Search For The Holy Grail");}
		if(cardTitle.Equals("enchantedforest")){ return Resources.Load<Sprite>("Cards/Q Journey Through The Enchanted Forest");}
		if(cardTitle.Equals("arthursenemies")){ return Resources.Load<Sprite>("Cards/Q Vanquish King Arthur's Enemies");}
		if(cardTitle.Equals("saxonraiders")){ return Resources.Load<Sprite>("Cards/Q Repel The Saxon Raiders");}
		if(cardTitle.Equals("boarhunt")){ return Resources.Load<Sprite>("Cards/Q Boar Hunt");}
		if(cardTitle.Equals("questingbeast")){ return Resources.Load<Sprite>("Cards/Q Search For The Questing Beast");}
		if(cardTitle.Equals("queenshonor")){ return Resources.Load<Sprite>("Cards/Q Defend The Queen's Honor");}
		if(cardTitle.Equals("slaydragon")){ return Resources.Load<Sprite>("Cards/Q Slay The Dragon");}
		if(cardTitle.Equals("rescuemaiden")){ return Resources.Load<Sprite>("Cards/Q Rescue The Fair Maiden");}
		if(cardTitle.Equals("greenknighttest")){ return Resources.Load<Sprite>("Cards/Q Test Of The Green Knight");}
		if(cardTitle.Equals("camelot")){ return Resources.Load<Sprite>("Cards/To Camelot");}
		if(cardTitle.Equals("orkney")){ return Resources.Load<Sprite>("Cards/To Orkney");}
		if(cardTitle.Equals("tintagel")){ return Resources.Load<Sprite>("Cards/To Tintagel");}
		if(cardTitle.Equals("york")){ return Resources.Load<Sprite>("Cards/To York");}

		else return null;
	}
}
