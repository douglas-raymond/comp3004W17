using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary {
	
	
	Weapon excalibur;
	Weapon lance;
	Weapon battleax;
	Weapon sword;
	Weapon horse;
	Weapon dagger;

	//Foes
	Foe dragon;
	Foe giant;
	Foe mordred;
	Foe greenknight;
	Foe blackknight;
	Foe evilknight;
	Foe saxonknight;
	Foe robberknight;
	Foe saxons;
	Foe boar;
	Foe thieves;

	//Tests
	Test tovalor;
	Test toquestingbeast;
	Test totemptation;
	Test tomorganlefey;

	//Allies

	Ally galahad;
	Ally arthur;
	Ally pellinore;
	Ally guinevere;
	Ally iseult;
	Ally gawain;
	Ally lancelot; 
	Ally percival;
	Ally tristan;

	//Amour amour amour amour amour
	Amour amour;
		
	//Quests 
	
	QuestCard holygrail;
	QuestCard enchantedforest;
	QuestCard arthursenemies;
	QuestCard saxonraiders;
	QuestCard boarhunt;
	QuestCard questingbeast;
	QuestCard queenshonor;
	QuestCard slaydragon;
	QuestCard rescuemaiden;
	QuestCard greenknighttest;
	
	//Tourneys
	TourneyCard camelot;
	TourneyCard orkney;
	TourneyCard tintagel;
	TourneyCard york;
	
	EventCard chivdeed;
	EventCard prosperity;
	
	private Sprite getCardImage(string cardTitle) {
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

		if (cardTitle.Equals ("amour")) { return Resources.Load<Sprite> ("Cards/Amour");}

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
		
		if(cardTitle.Equals("chivdeed")){ return Resources.Load<Sprite>("Cards/E Chivalrous Deed");}
		if(cardTitle.Equals("prosperity")){ return Resources.Load<Sprite>("Cards/E Prosperity Throughout the Realm");}

		else return null;
	}
	
	public Card getCard(string cardTitle) {
		if(cardTitle.Equals("excalibur")){ return excalibur;}
		if(cardTitle.Equals("lance")){ return lance;}
		if(cardTitle.Equals("battleax")){ return battleax;}
		if(cardTitle.Equals("sword")){ return sword;}
		if(cardTitle.Equals("horse")){ return horse;}
		if(cardTitle.Equals("dagger")){ return dagger;}
		if(cardTitle.Equals("dragon")){ return dragon;}
		if(cardTitle.Equals("giant")){ return giant;}
		if(cardTitle.Equals("mordred")){ return mordred;}
		if(cardTitle.Equals("greenknight")){ return greenknight;}
		if(cardTitle.Equals("blackknight")){ return blackknight;}
		if(cardTitle.Equals("evilknight")){ return evilknight;}
		if(cardTitle.Equals("saxonknight")){ return saxonknight;}
		if(cardTitle.Equals("robberknight")){ return robberknight;}
		if(cardTitle.Equals("saxons")){ return saxons;}
		if(cardTitle.Equals("boar")){ return boar;}
		if(cardTitle.Equals("thieves")){ return thieves;}
		if(cardTitle.Equals("tovalor")){ return tovalor;}
		if(cardTitle.Equals("toquestingbeast")){ return toquestingbeast;}
		if(cardTitle.Equals("totemptation")){ return totemptation;}
		if(cardTitle.Equals("tomorganlefey")){ return tomorganlefey;}
		if(cardTitle.Equals("galahad")){ return galahad;}
		if(cardTitle.Equals("arthur")){ return arthur;}
		if(cardTitle.Equals("pellinore")){ return pellinore;}
		if(cardTitle.Equals("guinevere")){ return guinevere;}
		if(cardTitle.Equals("iseult")){ return iseult;}
		if(cardTitle.Equals("gawain")){ return gawain;}
		if(cardTitle.Equals("lancelot")){ return lancelot;}
		if(cardTitle.Equals("percival")){ return percival;}
		if(cardTitle.Equals("tristan")){ return tristan;}

		if (cardTitle.Equals ("amour")) { return amour;}

		if(cardTitle.Equals("holygrail")){ return holygrail;}
		if(cardTitle.Equals("enchantedforest")){ return enchantedforest;}
		if(cardTitle.Equals("arthursenemies")){ return arthursenemies;}
		if(cardTitle.Equals("saxonraiders")){ return saxonraiders;}
		if(cardTitle.Equals("boarhunt")){ return boarhunt;}
		if(cardTitle.Equals("questingbeast")){ return questingbeast;}
		if(cardTitle.Equals("queenshonor")){ return queenshonor;}
		if(cardTitle.Equals("slaydragon")){ return slaydragon;}
		if(cardTitle.Equals("rescuemaiden")){ return rescuemaiden;}
		if(cardTitle.Equals("greenknighttest")){ return greenknighttest;}
		if(cardTitle.Equals("camelot")){ return camelot;}
		if(cardTitle.Equals("orkney")){ return orkney;}
		if(cardTitle.Equals("tintagel")){ return tintagel;}
		if(cardTitle.Equals("york")){ return york;}
		
		if(cardTitle.Equals("chivdeed")){ return chivdeed;}
		if(cardTitle.Equals("prosperity")){ return prosperity;}

		else return null;
	}
	
	public void init() {
		excalibur = new Weapon("excalibur", 30, getCardImage("excalibur"));
		lance = new Weapon("lance", 20, getCardImage("lance"));
		battleax = new Weapon("battleax", 15, getCardImage("battleax"));
		sword = new Weapon("sword", 10, getCardImage("sword"));
		horse = new Weapon("horse", 10, getCardImage("horse"));
		dagger = new Weapon("dagger", 5, getCardImage("dagger"));

		//Foes
		dragon = new Foe("dragon", 50, 70, getCardImage("dragon"));
		giant = new Foe("giant", 40, 0, getCardImage("giant"));
		mordred = new Foe("mordred", 30, 0, getCardImage("mordred"));
		greenknight = new Foe("greenknight", 25, 40, getCardImage("greenknight"));
		blackknight = new Foe("blackknight", 25, 35, getCardImage("blackknight"));
		evilknight = new Foe("evilknight", 20, 30, getCardImage("evilknight"));
		saxonknight = new Foe("saxonknight", 15, 25, getCardImage("saxonknight"));
		robberknight = new Foe("robberknight", 15, 0, getCardImage("robberknight"));
		saxons = new Foe("saxons", 10, 20, getCardImage("saxons"));
		boar = new Foe("boar", 5, 15, getCardImage("boar"));
		thieves = new Foe("thieves", 5, 0, getCardImage("thieves"));	

		//Tests
		tovalor = new Test("tovalor", 0, 0, getCardImage("tovalor"));
		toquestingbeast = new Test("toquestingbeast", 0, 4, getCardImage("toquestingbeast"));
		totemptation = new Test("totemptation", 0, 0, getCardImage("totemptation"));
		tomorganlefey = new Test("tomorganlefey", 0, 3, getCardImage("tomorganlefey"));

		//Allies

		galahad = new Ally("galahad", 15, 0, 0, getCardImage("galahad"));
		arthur = new Ally("arthur", 10, 0, 2, getCardImage("arthur"));
		pellinore = new Ally("pellinore", 10, 0, 4, getCardImage("pellinore"));
		guinevere = new Ally("guinevere", 0, 0, 3, getCardImage("guinevere"));
		iseult = new Ally("iseult", 0, 0, 2, getCardImage("iseult"));
		gawain = new Ally("gawain", 10, 0, 0, getCardImage("gawain"));
		lancelot = new Ally("lancelot", 15, 0, 0, getCardImage("lancelot"));
		percival = new Ally("percival", 5, 0, 0, getCardImage("percival"));
		tristan = new Ally("tristan", 10, 0, 0, getCardImage("tristan"));

		//Amour amour amour amour amour
		amour = new Amour ("amour", 10, 2, getCardImage("amour"));
			
		//Quests 
		
		holygrail = new QuestCard("holygrail", "quest", 5, new Foe[]{dragon, giant, mordred, greenknight, blackknight, evilknight, saxonknight, robberknight, saxons, boar, thieves}, getCardImage("holygrail"));
		enchantedforest = new QuestCard("enchantedforest", "quest", 3, new Foe[] {evilknight}, getCardImage("enchantedforest"));
		arthursenemies = new QuestCard("arthursenemies", "quest", 3, new Foe[0], getCardImage("arthursenemies"));
		saxonraiders = new QuestCard("saxonraiders", "quest", 2, new Foe[]{saxons, saxonknight}, getCardImage("saxonraiders"));
		boarhunt = new QuestCard("boarhunt", "quest", 2, new Foe[] {boar}, getCardImage("boarhunt"));
		questingbeast = new QuestCard("questingbeast", "quest", 4, new Foe[0], getCardImage("questingbeast"));
		queenshonor = new QuestCard("queenshonor", "quest", 4, new Foe[]{dragon, giant, mordred, greenknight, blackknight, evilknight, saxonknight, robberknight, saxons, boar, thieves}, getCardImage("queenshonor"));
		slaydragon = new QuestCard("slaydragon", "quest", 3, new Foe[]{dragon}, getCardImage("slaydragon"));
		rescuemaiden = new QuestCard("rescuemaiden", "quest", 3, new Foe[]{blackknight}, getCardImage("rescuemaiden"));
		greenknighttest = new QuestCard("greenknighttest", "quest", 4, new Foe[]{greenknight}, getCardImage("greenknighttest"));
		
		//Tourneys
		camelot = new TourneyCard("camelot", "tourney", 3, getCardImage("camelot"));
		orkney = new TourneyCard("orkney", "tourney", 2, getCardImage("orkney"));
		tintagel = new TourneyCard("tintagel", "tourney", 1, getCardImage("tintagel"));
		york = new TourneyCard("york", "tourney", 0, getCardImage("york"));
		
		//Events
		chivdeed = new EventCard("chivdeed", "event", getCardImage("chivdeed"));
		prosperity = new EventCard("prosperity", "event", getCardImage("prosperity"));

	}
}
