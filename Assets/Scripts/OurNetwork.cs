using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class Msg {
	//UI
	public static short showHand = 555;
	public static short showCard = 556;
	public static short askForCards = 557;
	public static short displayAlert = 558;
	public static short showStage = 559;
	public static short endQuest = 560;
	public static short drawingQuestCard = 561;
	public static short askForPlayerChoice = 562;
	public static short foeReveal = 563;
	public static short confirmConnect = 580;
	public static short askYesOrNo = 582;
	//GM
	public static short startQuestSetup = 564;
	public static short setUserInputState = 565;
	public static short gotPlayer = 566;
	public static short gotPlayerTourney = 567;
	public static short questAttack = 568;
	public static short forfeitQuest = 569;
	public static short gotTournamentCards = 570;
	public static short bidPhase = 571;
	public static short endStage = 572;
	public static short endStageWeaponSetup = 573;
	public static short gotMordredTarget = 574;
	public static short endQuestSetup = 575;
	public static short gotCardLimitReached = 576;
	public static short getSponsor = 578;
	//getters
	public static short getUserInputState = 577;
	public static short getCurrentPlayer = 579;
	public static short getOtherPlayerInfo = 581;
	public static short mouseOverShowHand = 583;
	public static short mouseOverShowOther = 584;
	public static short checkCardSelection = 585;
	public static short checkCardRemoval = 586;
	public static short checkButtonClick = 587;
	//update
	public static short updatePlayer = 588;
	public static short idlePlayer = 600;
}

public class ShowHandMessage : MessageBase {
	public ShowHandMessage(){}
	public ShowHandMessage(int i){
		hand = new string[i];
	}
	public string[] hand;
}

public class ShowCardMessage : MessageBase {
	public string card;
}

public class AskForCardsMessage : MessageBase{
	//player
	public AskForCardsMessage(int i, int p){
		hand = new string[i];
		inPlay = new string[p];
	}
	public AskForCardsMessage(){}
	public string[] hand;
	public string[] inPlay;
	public int shield;
	public int rank;
	public string name;
	public int BP;
	//activeQuest
	public string questCard;
	//other
	public state newState;
	public state oldState;
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

	public AskForPlayerChoiceMessage(){}
	public AskForPlayerChoiceMessage(int h, int p, int z){
		hand = new string[h];
		names = new string[p];
		inPlay = new string[z];
	}
	//player
	public string[] hand;
	public string[] inPlay;
	public int shield;
	public int rank;
	public string name;
	public int BP;
	//other
	public state newState;
	public string instructions;
	public string[] names;
}

public class FoeRevealMessage : MessageBase {
	
	public FoeRevealMessage(){}
	public FoeRevealMessage(int w, int n){
		weapons = new string[w];
		names = new string[n];
	}
	public string[] weapons;
	public string stage;
	public int numPlayers;
	public string[] names;
}

public class SetUserInputStateMessage : MessageBase {
	public state newState;
}

public class GotPlayerMessage : MessageBase {
	public string name;
}

public class GotPlayerTourneyMessage : MessageBase {
	public string name;
}

public class SelectionMessage : MessageBase {
	public SelectionMessage(){}
	public SelectionMessage(int n){
		selection = new string[n];
	}
	public string[] selection;
	public int connectionID;
	public string name;
}

public class EndQuestMessage : MessageBase {
	public string input;
}

public class GotMordredTargetMessage : MessageBase {
	public int connectionID;
	public string target;
}

public class GetUserInputStateMessage: MessageBase {
	public state newState;
}

public class GetCurrentPlayerMessage : MessageBase {
	
	public GetCurrentPlayerMessage(){}
	public GetCurrentPlayerMessage(int n, int p){
		hand = new string[n];
		inPlay = new string[p];
	}
		
	public string name;
	public string[] hand;
	public string[] inPlay;
	public int rank;
	public int BP;
	public int shields;
}

public class GetOtherPlayerMessage : MessageBase {
	public string name;
	public int connectionID;
}

public class AskYesOrNoMessage : MessageBase {
	public AskYesOrNoMessage(){}
	public AskYesOrNoMessage(int n, int p){
		hand = new string[n];
		inPlay = new string[p];
	}

	public string name;
	public string[] hand;
	public string[] inPlay;
	public int rank;
	public int BP;
	public int shields;

	public string message;
	public state messageState;
}

public class UpdatePlayerMessage : MessageBase {
	public UpdatePlayerMessage(){}
	public UpdatePlayerMessage(int n, int p){
		hand = new string[n];
		inPlay = new string[p];
	}

	public string name;
	public string[] hand;
	public string[] inPlay;
	public int rank;
	public int BP;
	public int shields;
}


