using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDeck : Deck  {


	//Creates a stack of all the story cards
	public void initDeck(){

		Card[] tempCardArr = new Card[17];	
		//Story deck
		//Quests 
	
		QuestCard holygrail = new QuestCard("holygrail", "quest", 5, new Foe[7]);
		QuestCard enchantedforest = new QuestCard("enchantedforest", "quest", 3, new Foe[1]);
		QuestCard arthursenemies = new QuestCard("arthursenemies", "quest", 3, new Foe[0]);
		QuestCard saxonraiders = new QuestCard("saxonraiders", "quest", 2, new Foe[2]);
		QuestCard boarhunt = new QuestCard("boarhunt", "quest", 2, new Foe[1]);
		QuestCard questingbeast = new QuestCard("boarhunt", "quest", 4, new Foe[0]);
		QuestCard queenshonor = new QuestCard("queenshonor", "quest", 4, new Foe[8] );
		QuestCard slaydragon = new QuestCard("slaydragon", "quest", 3, new Foe[1] );
		QuestCard rescuemaiden = new QuestCard("rescuemaiden", "quest", 3, new Foe[1]);
		QuestCard greenknighttest = new QuestCard("greenknighttest", "quest", 4, new Foe[1] );
		
		//Tourneys
		TourneyCard camelot = new TourneyCard("camelot", "tourney", 3);
		TourneyCard orkney = new TourneyCard("orkney", "tourney", 2);
		TourneyCard tintagel = new TourneyCard("tintagel", "tourney", 1);
		TourneyCard york = new TourneyCard("york", "tourney", 0);
		
		for(int i = 0; i < 1; i ++) { tempCardArr[i] = holygrail; }
		for(int i = 1; i < 2; i ++) { tempCardArr[i] = greenknighttest; }
		for(int i = 2; i < 3; i ++) { tempCardArr[i] = questingbeast; }
		for(int i = 3; i < 4; i ++) { tempCardArr[i] = queenshonor; }
		for(int i = 4; i < 5; i ++) { tempCardArr[i] = rescuemaiden; }
		for(int i = 5; i < 6; i ++){ tempCardArr[i] = enchantedforest; }
		for(int i = 6; i < 8; i ++){ tempCardArr[i] = arthursenemies; }
		for(int i = 8; i < 9; i ++){ tempCardArr[i] = slaydragon; }
		for(int i = 9; i < 11; i ++){ tempCardArr[i] = boarhunt; }
		for(int i = 11; i < 13; i ++){ tempCardArr[i] = saxonraiders; }
		
		for(int i = 13; i < 14; i ++){ tempCardArr[i] = camelot; }
		for(int i = 14; i < 15; i ++){ tempCardArr[i] = orkney; }
		for(int i = 15; i < 16; i ++){ tempCardArr[i] = tintagel; }
		for(int i = 16; i < 17; i ++){ tempCardArr[i] = york; }
		
		
		
		for(int i = 0; i< 16; i++)
		{
			deck.Push(tempCardArr[i]);
		}
		
		shuffle();
		
		return;
	}
}
