using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class Msg {
	public static short showHand = 555;
	public static short showCard = 556;
	public static short askForCards = 557;
	public static short displayAlert = 558;
	public static short showStage = 559;
	public static short endQuest = 560;
	public static short drawingQuestCard = 561;
	public static short askForPlayerChoice = 562;
	public static short foeReveal = 563;
}

public class ShowHandMessage : MessageBase {
	public string[] hand;
}

public class ShowCardMessage : MessageBase {
	public string card;
}

public class AskForCardsMessage : MessageBase{
	//player
	public string[] hand;
	public int shield;
	public int rank;
	public string name;
	public int BP;
	//other
	public state newState;
	public string instructions; 
	public string button1;
	public string button2;
	public bool getFoes;
	public bool getWeap;
	public bool getAlly;
	public bool getAmour;
	public bool getTest;
	public bool getMordred;
	public int stage;
}

public class DisplayAlertMessage : MessageBase {
	public string input;
}

public class ShowStageMessage : MessageBase {
	public bool foe;
	public bool test;
	public string questCard;
	public int highestBid;
}

public class AskForPlayerChoiceMessage : MessageBase {
	//player
	public string[] hand;
	public int shield;
	public int rank;
	public string name;
	public int BP;
	//other
	public state newState;
	public string instructions;
}

public class FoeRevealMessage : MessageBase {
	public string[] weapons;
	public string stage;
	public int numPlayers;
	public string[] names;
}

