using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDeck : Deck  {

	public StoryDeck(){
	}
	
	

	//Creates a stack of all the story cards
	public void initDeck(){
		CardLibrary lib = new CardLibrary();
		lib.init();
		Card[] tempCardArr = new Card[25];	
		//Story deck
		
		for(int i = 0; i < 1; i ++) { tempCardArr[i] = lib.getCard("holygrail"); }
		for(int i = 1; i < 2; i ++) { tempCardArr[i] = lib.getCard("greenknighttest"); }
		for(int i = 2; i < 3; i ++) { tempCardArr[i] = lib.getCard("questingbeast"); }
		for(int i = 3; i < 4; i ++) { tempCardArr[i] = lib.getCard("queenshonor"); }
		for(int i = 4; i < 5; i ++) { tempCardArr[i] = lib.getCard("rescuemaiden"); }
		for(int i = 5; i < 6; i ++){ tempCardArr[i] = lib.getCard("enchantedforest"); }
		for(int i = 6; i < 8; i ++){ tempCardArr[i] = lib.getCard("arthursenemies"); }
		for(int i = 8; i < 9; i ++){ tempCardArr[i] = lib.getCard("slaydragon"); }
		for(int i = 9; i < 11; i ++){ tempCardArr[i] = lib.getCard("boarhunt"); }
		for(int i = 11; i < 13; i ++){ tempCardArr[i] = lib.getCard("saxonraiders"); }
		
		for(int i = 13; i < 14; i ++){ tempCardArr[i] = lib.getCard("camelot"); }
		for(int i = 14; i < 15; i ++){ tempCardArr[i] = lib.getCard("orkney"); }
		for(int i = 15; i < 16; i ++){ tempCardArr[i] = lib.getCard("tintagel"); }
		for(int i = 16; i < 17; i ++){ tempCardArr[i] = lib.getCard("york"); }
		for(int i = 17; i < 18; i ++){ tempCardArr[i] = lib.getCard("prosperity"); }
		for(int i = 18; i < 19; i ++){  tempCardArr[i] = lib.getCard("chivdeed"); }
		for(int i = 19; i < 20; i ++){  tempCardArr[i] = lib.getCard("courtcalled"); }
		for(int i = 20; i < 21; i ++){  tempCardArr[i] = lib.getCard("kingscall"); }
		for(int i = 21; i < 22; i ++){  tempCardArr[i] = lib.getCard("recognition"); }
		for(int i = 22; i < 23; i ++){  tempCardArr[i] = lib.getCard("plague"); }
		for(int i = 23; i < 24; i ++){  tempCardArr[i] = lib.getCard("pox"); }
		for(int i = 24; i < 25; i ++){  tempCardArr[i] = lib.getCard("queensfavor"); }
		for(int i = 0; i< 25; i++)
		{
			
			deck.Push(tempCardArr[i]);
		}
		
		shuffle();
		
		return;
	}
}
