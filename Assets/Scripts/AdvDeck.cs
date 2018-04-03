using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvDeck : Deck  {

	
	public AdvDeck(){
	}
	


	//Creates a stack of all adventure cards
	public void initDeck(){
		CardLibrary lib = new CardLibrary();
		lib.init();
		Card[] tempCardArr = new Card[125];
		
		for(int i = 0; i < 2; i ++) { tempCardArr[i] = lib.getCard("excalibur"); }
		for(int i = 2; i < 8; i ++) { tempCardArr[i] = lib.getCard("lance"); }
		for(int i = 8; i < 16; i ++) { tempCardArr[i] = lib.getCard("battleax"); }
		for(int i = 16; i < 32; i ++) { tempCardArr[i] = lib.getCard("sword"); }
		for(int i = 32; i < 43; i ++) { tempCardArr[i] = lib.getCard("horse"); }
		for(int i = 43; i < 49; i ++){ tempCardArr[i] = lib.getCard("dagger"); }
		
		
		for(int i = 49; i < 50; i ++) { tempCardArr[i] = lib.getCard("dragon"); }
		for(int i = 50; i < 52; i ++) { tempCardArr[i] = lib.getCard("giant"); }
		for(int i = 52; i < 56; i ++) { tempCardArr[i] = lib.getCard("mordred"); }
		for(int i = 56; i < 58; i ++) { tempCardArr[i] = lib.getCard("greenknight"); }
		for(int i = 58; i < 61; i ++) { tempCardArr[i] = lib.getCard("blackknight"); }
		for(int i = 61; i < 67; i ++) { tempCardArr[i] = lib.getCard("evilknight"); }
		for(int i = 67; i < 75; i ++) { tempCardArr[i] = lib.getCard("saxonknight"); }
		for(int i = 75; i < 82; i ++) { tempCardArr[i] = lib.getCard("robberknight"); }
		for(int i = 82; i < 87; i ++) { tempCardArr[i] = lib.getCard("saxons"); }
		for(int i = 87; i < 91; i ++) { tempCardArr[i] = lib.getCard("boar"); }
		for(int i = 91; i < 99; i ++) { tempCardArr[i] = lib.getCard("thieves"); }
		
		for(int i = 99; i < 101; i ++) { tempCardArr[i] = lib.getCard("tovalor"); }
		for(int i = 101; i < 103; i ++) { tempCardArr[i] = lib.getCard("toquestingbeast"); }
		for(int i = 103; i < 105; i ++) { tempCardArr[i] = lib.getCard("totemptation"); }
		for(int i = 105; i < 107; i ++) { tempCardArr[i] = lib.getCard("tomorganlefey"); }
		
		for(int i = 108; i < 109; i ++) { tempCardArr[i] = lib.getCard("galahad"); }
		for(int i = 109; i < 110; i ++) { tempCardArr[i] = lib.getCard("arthur"); }
		for(int i = 110; i < 111; i ++) { tempCardArr[i] = lib.getCard("pellinore"); }
		for(int i = 111; i < 112; i ++) { tempCardArr[i] = lib.getCard("guinevere"); }
		for(int i = 112; i < 113; i ++) { tempCardArr[i] = lib.getCard("iseult"); }
		for(int i = 113; i < 114; i ++) { tempCardArr[i] = lib.getCard("gawain"); }
		for(int i = 114; i < 115; i ++) { tempCardArr[i] = lib.getCard("lancelot"); }
		for(int i = 115; i < 116; i ++) { tempCardArr[i] = lib.getCard("percival"); }
		for(int i = 116; i < 117; i ++) { tempCardArr[i] = lib.getCard("tristan"); }		

		for (int i = 117; i < 125; i++) {
			tempCardArr [i] = lib.getCard("amour");
		}
		
		for(int i = 0; i< 125; i++)
		{
			deck.Push(tempCardArr[i]);
		}
		
		shuffle();
		
		return;
	}
}
