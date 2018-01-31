using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvDeck : Deck  {


	//Creates a stack of all adventure cards
	public void initDeck(){

		Card[] tempCardArr = new Card[117];
		
		Weapon excalibur = new Weapon("excalibur", 30, getCardImage("excalibur"));
		Weapon lance = new Weapon("lance", 20, getCardImage("lance"));
		Weapon battleax = new Weapon("battleax", 15, getCardImage("battleax"));
		Weapon sword = new Weapon("sword", 10, getCardImage("sword"));
		Weapon horse = new Weapon("horse", 10, getCardImage("horse"));
		Weapon dagger = new Weapon("dagger", 5, getCardImage("dagger"));
		
		//Foes
		Foe dragon = new Foe("dragon", 50, 70, getCardImage("dragon"));
		Foe giant = new Foe("giant", 40, 0, getCardImage("giant"));
		Foe mordred = new Foe("mordred", 30, 0, getCardImage("mordred"));
		Foe greenknight = new Foe("greenknight", 25, 40, getCardImage("greenknight"));
		Foe blackknight = new Foe("blackknight", 25, 35, getCardImage("blackknight"));
		Foe evilknight = new Foe("evilknight", 20, 30, getCardImage("evilknight"));
		Foe saxonknight = new Foe("saxonknight", 15, 25, getCardImage("saxonknight"));
		Foe robberknight = new Foe("robberknight", 15, 0, getCardImage("robberknight"));
		Foe saxons = new Foe("saxons", 10, 20, getCardImage("saxons"));
		Foe boar = new Foe("boar", 5, 15, getCardImage("boar"));
		Foe thieves = new Foe("thieves", 5, 0, getCardImage("thieves"));	
		
		//Tests
		Test tovalor = new Test("tovalor", 0, 0, getCardImage("tovalor"));
		Test toquestingbeast = new Test("toquestingbeast", 0, 4, getCardImage("toquestingbeast"));
		Test totemptation = new Test("totemptation", 0, 0, getCardImage("totemptation"));
		Test tomorganlefey = new Test("tomorganlefey", 0, 3, getCardImage("tomorganlefey"));

		//Allies
		
		Ally galahad = new Ally("galahad", 15, 0, 0, getCardImage("galahad"));
		Ally arthur = new Ally("arthur", 10, 0, 2, getCardImage("arthur"));
		Ally pellinore = new Ally("pellinore", 10, 0, 4, getCardImage("pellinore"));
		Ally guinevere = new Ally("guinevere", 0, 0, 3, getCardImage("guinevere"));
		Ally iseult = new Ally("iseult", 0, 0, 2, getCardImage("iseult"));
		Ally gawain = new Ally("gawain", 10, 0, 0, getCardImage("gawain"));
		Ally lancelot = new Ally("lancelot", 15, 0, 0, getCardImage("lancelot"));
		Ally percival = new Ally("percival", 5, 0, 0, getCardImage("percival"));
		Ally tristan = new Ally("tristan", 10, 0, 0, getCardImage("tristan"));
		
		for(int i = 0; i < 2; i ++) { tempCardArr[i] = excalibur; }
		for(int i = 2; i < 8; i ++) { tempCardArr[i] = lance; }
		for(int i = 8; i < 16; i ++) { tempCardArr[i] = battleax; }
		for(int i = 16; i < 32; i ++) { tempCardArr[i] = sword; }
		for(int i = 32; i < 43; i ++) { tempCardArr[i] = horse; }
		for(int i = 43; i < 49; i ++){ tempCardArr[i] = dagger; }
		
		
		for(int i = 49; i < 50; i ++) { tempCardArr[i] = dragon; }
		for(int i = 50; i < 52; i ++) { tempCardArr[i] = giant; }
		for(int i = 52; i < 56; i ++) { tempCardArr[i] = mordred; }
		for(int i = 56; i < 58; i ++) { tempCardArr[i] = greenknight; }
		for(int i = 58; i < 61; i ++) { tempCardArr[i] = blackknight; }
		for(int i = 61; i < 67; i ++) { tempCardArr[i] = evilknight; }
		for(int i = 67; i < 75; i ++) { tempCardArr[i] = saxonknight; }
		for(int i = 75; i < 82; i ++) { tempCardArr[i] = robberknight; }
		for(int i = 82; i < 87; i ++) { tempCardArr[i] = saxons; }
		for(int i = 87; i < 91; i ++) { tempCardArr[i] = boar; }
		for(int i = 91; i < 99; i ++) { tempCardArr[i] = thieves; }
		
		for(int i = 99; i < 101; i ++) { tempCardArr[i] = tovalor; }
		for(int i = 101; i < 103; i ++) { tempCardArr[i] = toquestingbeast; }
		for(int i = 103; i < 105; i ++) { tempCardArr[i] = totemptation; }
		for(int i = 105; i < 107; i ++) { tempCardArr[i] = tomorganlefey; }
		
		for(int i = 108; i < 109; i ++) { tempCardArr[i] = galahad; }
		for(int i = 109; i < 110; i ++) { tempCardArr[i] = arthur; }
		for(int i = 110; i < 111; i ++) { tempCardArr[i] = pellinore; }
		for(int i = 111; i < 112; i ++) { tempCardArr[i] = guinevere; }
		for(int i = 112; i < 113; i ++) { tempCardArr[i] = iseult; }
		for(int i = 113; i < 114; i ++) { tempCardArr[i] = gawain; }
		for(int i = 114; i < 115; i ++) { tempCardArr[i] = lancelot; }
		for(int i = 115; i < 116; i ++) { tempCardArr[i] = percival; }
		for(int i = 116; i < 117; i ++) { tempCardArr[i] = tristan; }		
		
		for(int i = 0; i< 116; i++)
		{
			deck.Push(tempCardArr[i]);
		}
		
		shuffle();
		
		return;
	}
}
