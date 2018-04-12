using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Client : MonoBehaviour {
	NetworkClient myClient;
	int connectionID;
	//the client has it's own UI and abstacted gm;
	private UI ui;
	private NetworkedGM gm;
	public Text connectionText;
	private bool lobby;

	void Start(){
		lobby = false;
		myClient = new NetworkClient ();
		myClient.RegisterHandler (Msg.confirmConnect, Init);
		Debug.Log ("Attempting to connect to " + PlayerPrefs.GetString ("hostIP"));
		myClient.Connect(PlayerPrefs.GetString("hostIP"), PlayerPrefs.GetInt("hostPort"));
		connectionText.GetComponent<Text> ().text = "Connecting...";
	}

	public void Init(NetworkMessage m){
		if (!(lobby)) {
			connectionID = m.ReadMessage<IntegerMessage> ().value;
			connectionText.GetComponent<Text> ().text = " Connected - Player " + (connectionID - 1);
			gm = new NetworkedGM (myClient, connectionID);
			lobby = true;
		} else {
			ui = new UI (gm);
			myClient.RegisterHandler (Msg.showHand, showHand);
			myClient.RegisterHandler (Msg.showCard, showCard);
			myClient.RegisterHandler (Msg.askForCards, askForCards);
			myClient.RegisterHandler (Msg.displayAlert, displayAlert);
			myClient.RegisterHandler (Msg.showStage, showStage);
			myClient.RegisterHandler (Msg.endQuest, endQuest);
			myClient.RegisterHandler (Msg.drawingQuestCard, drawingQuestCard);
			myClient.RegisterHandler (Msg.askForPlayerChoice, askForPlayerChoice);
			myClient.RegisterHandler (Msg.foeReveal, foeReveal);
			myClient.RegisterHandler (Msg.getUserInputState, PassUserInputState);
			myClient.RegisterHandler (Msg.getCurrentPlayer, PassCurrentPlayer);
			myClient.RegisterHandler (Msg.getOtherPlayerInfo, PassOtherPlayerInfo);
			myClient.RegisterHandler (Msg.askYesOrNo, askYesOrNo);
			myClient.RegisterHandler (Msg.mouseOverShowHand, mouseOverShowHand);
			myClient.RegisterHandler (Msg.mouseOverShowOther, mouseOverShowOther);
			myClient.RegisterHandler (Msg.checkCardSelection, checkCardSelection);
			myClient.RegisterHandler (Msg.checkCardRemoval, checkCardRemoval);
			myClient.RegisterHandler (Msg.checkButtonClick, checkButtonClick);
			myClient.RegisterHandler (Msg.updatePlayer, updatePlayer);
			myClient.RegisterHandler (Msg.idlePlayer, idlePlayer);
		}
	}
		
	public void showHand(NetworkMessage m){
		ShowHandMessage message = m.ReadMessage<ShowHandMessage>();
		string[] stringHand = message.hand;
		Card[] hand = MessageToHand (stringHand);
		ui.showHand(hand);
	}

	public void askForCards(NetworkMessage m){
		AskForCardsMessage nM = m.ReadMessage<AskForCardsMessage> ();
		Player tempPlayer = MessageToPlayer (nM.hand, nM.inPlay, nM.shield, nM.rank, nM.name, nM.BP);
		QuestCard tempCard = new QuestCard (nM.questCard, "quest", 0, null, getCardImage (nM.questCard));
		ActiveQuest tempQuest = new ActiveQuest (tempCard, 0);
		//populate tempPlayer with nM
		ui.askForCards (tempPlayer, tempQuest, nM.newState, nM.oldState, nM.instructions, nM.button1, nM.button2, nM.getFoes, nM.getWeap, nM.getAlly, nM.getAmour, nM.getTest, nM.getMordred, nM.stage);
	}

	public void foeReveal(NetworkMessage m){
		FoeRevealMessage message = m.ReadMessage<FoeRevealMessage> ();
		ActiveQuest activeQuest = MessageToActiveQuest (message.weapons, message.stage, message.numPlayers, message.names);
		//turn message into an ActiveQuest
		ui.foeReveal(activeQuest);
	}

	public void drawingQuestCard(NetworkMessage m){
		ui.drawingQuestCard ();
	}

	public void showCard(NetworkMessage m){
		ShowCardMessage message = m.ReadMessage<ShowCardMessage> ();
		string tempCard = message.card;
		Card card = MessageToCard (tempCard);
		Debug.Log("show "+tempCard);
		ui.showCard (card);
	}
		
	public void showStage (NetworkMessage m){
		ShowStageMessage message = m.ReadMessage<ShowStageMessage> ();
		ActiveQuest temp = MessageToActiveQuestStage (message.foe, message.test, message.questCard, message.highestBid);
		ui.showStage (temp);
	}

	public void displayAlert(NetworkMessage m){
		DisplayAlertMessage message = m.ReadMessage<DisplayAlertMessage> ();
		ui.displayAlert (message.input);
	}

	public void endQuest(NetworkMessage m){
		ui.endQuest ();
	}

	public void PassUserInputState(NetworkMessage m){
		gm.PostUpdate (m.ReadMessage<GetUserInputStateMessage> ().newState);
	}

	public void PassCurrentPlayer(NetworkMessage m){
		GetCurrentPlayerMessage message = m.ReadMessage<GetCurrentPlayerMessage> ();
		Player tempPlayer = MessageToPlayer (message.hand, message.inPlay, message.shields, message.rank, message.name, message.BP);
		gm.PostUpdate (tempPlayer);
	}

	public void PassOtherPlayerInfo(NetworkMessage m){
		StringMessage message = m.ReadMessage<StringMessage> ();
		gm.PostUpdate (message.value);
	}

	public void askYesOrNo(NetworkMessage m){
		AskYesOrNoMessage message = m.ReadMessage<AskYesOrNoMessage> ();
		Player tempPlayer = MessageToPlayer (message.hand, message.inPlay, message.shields, message.rank, message.name, message.BP);
		ui.askYesOrNo (tempPlayer, message.message, message.messageState);
	}

	public void askForPlayerChoice(NetworkMessage m){
		AskForPlayerChoiceMessage message = m.ReadMessage<AskForPlayerChoiceMessage> ();
		Player[] playerList = new Player[message.names.Length];
		for (int i = 0; i < message.names.Length; i++){
			playerList [i] = MessageToPlayer (null, null, 0, 0, message.names[i], 0);
		}
		Player tempPlayer = MessageToPlayer (message.hand, message.inPlay, message.shield, message.rank, message.name, message.BP);
		ui.askForPlayerChoice (tempPlayer, message.newState, message.instructions, playerList);
	}

	public void mouseOverShowHand(NetworkMessage m){
		GetCurrentPlayerMessage message = m.ReadMessage<GetCurrentPlayerMessage> ();
		Player tempPlayer = MessageToPlayer (message.hand, message.inPlay, message.shields, message.rank, message.name, message.BP);
		ui.mouseOverShowHandIcon (tempPlayer);
	}

	public void mouseOverShowOther(NetworkMessage m){
		StringMessage message = m.ReadMessage<StringMessage> ();
		ui.mouseOverShowOtherPlayerIcon (message.value);
	}

	public void checkCardSelection(NetworkMessage m){
		state newState = m.ReadMessage<GetUserInputStateMessage> ().newState;
		ui.gotCardSelection (newState);
	}

	public void checkCardRemoval(NetworkMessage m){
		state newState = m.ReadMessage<GetUserInputStateMessage> ().newState;
		ui.removeCardSelection (newState);
	}

	public void checkButtonClick(NetworkMessage m){
		state newState = m.ReadMessage<GetUserInputStateMessage> ().newState;
		ui.gotButtonClick (newState);
	}

	public void updatePlayer (NetworkMessage m){
		UpdatePlayerMessage nM = m.ReadMessage<UpdatePlayerMessage> ();
		Player newPlayer = MessageToPlayer (nM.hand, nM.inPlay, nM.shields, nM.rank, nM.name, nM.BP);
		ui.UpdatePlayer (newPlayer);
	}

	public void idlePlayer(NetworkMessage m){
		StringMessage message = m.ReadMessage<StringMessage> ();
		string player = message.value;
		ui.updateCurrentPlayer(player);
	}

	//Helpers
	public Card[] MessageToHand(string[] hand){
		Card[] tempHand = null;
		if (hand == null) {
			return tempHand;
		}
		tempHand = new Card[hand.Length];
		for (int i = 0; i < hand.Length; i++) {
			tempHand [i] = MessageToCard (hand [i]);
		}
		return tempHand;
	}

	public Player MessageToPlayer(string[] hand, string[] inPlay, int shield, int rank, string name, int BP){
		Card[] tempHand = MessageToHand (hand);
		Card[] tempInPlay = MessageToHand (inPlay);
		Player newPlayer = new Player (tempHand, shield, rank, name);
		newPlayer.setInPlayHand (tempInPlay);
		return newPlayer;
	}

	public ActiveQuest MessageToActiveQuest(string[] weapons, string stage, int numPlayers, string[] names){
		QuestCard tempQuest = new QuestCard (null, null, 1, null, null);
		ActiveQuest tempActiveQuest = new ActiveQuest (tempQuest, 0);
		for (int i = 0; i < names.Length; i++) {
			tempActiveQuest.addPlayer(MessageToPlayer(null, null, 0, 0, names[i], 0));
		}
		tempActiveQuest.setStage (0);
		Card[] tempStage = new Card[1];
		tempStage [0] = MessageToCard (stage);
		tempActiveQuest.setStages (tempStage);
		tempActiveQuest.setStageWeapons (MessageToHand (weapons));
		return tempActiveQuest;
	}

	public ActiveQuest MessageToActiveQuestStage(bool foe, bool test, string questCard, int highestBid){
		Sprite tempSprite = getCardImage (questCard);
		QuestCard tempQuest = new QuestCard (questCard, "quest", 1, null, tempSprite);
		Card tempEncounter;
		if (foe) {
			tempEncounter = new Foe ("", 0, 0, tempSprite);
		} else {
			tempEncounter = new Test ("", 0, 0, tempSprite);
		}
		Card[] newStage = new Card[1];
		newStage [0] = tempEncounter;
		ActiveQuest tempActiveQuest = new ActiveQuest (tempQuest, 0);
		tempActiveQuest.setStages (newStage);
		tempActiveQuest.setStage (0);
		tempActiveQuest.setHighestBid (highestBid);
		return tempActiveQuest;
	}

	public Card MessageToCard(string card){
		Sprite tempSprite = null;
		tempSprite = getCardImage (card);
		if (tempSprite == null) {
			Debug.Log ("unable to find: " + card);
		}
		string type = GetCardType (card);
		if (type.Equals("weapon")) {
			Weapon tempCard = new Weapon (card, getBP(card), tempSprite);
			return tempCard;
		} else if (type.Equals("foe")) {
			Foe tempCard = new Foe (card, 0, 0, tempSprite);
			return tempCard;
		} else if (type.Equals("test")) {
			Test tempCard = new Test (card, 0, 0, tempSprite);
			return tempCard;
		} else if (type.Equals("ally")) {
			Ally tempCard = new Ally (card, getBP(card), getAltBP(card), getFreeBids(card), tempSprite);
			return tempCard;
		} else if (type.Equals("amour")) {
			Amour tempCard = new Amour (card, 10, 1, tempSprite);
			return tempCard;
		} 
		Debug.Log (card+ " not found " + type );
		return null;
	}

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
		if(cardTitle.Equals("courtcalled")){ return Resources.Load<Sprite>("Cards/E Court Called Camelot");}
		if(cardTitle.Equals("kingscall")){ return Resources.Load<Sprite>("Cards/E Kings Call to Arms");}
		if(cardTitle.Equals("recognition")){ return Resources.Load<Sprite>("Cards/E Kings Recognition");}	
		if(cardTitle.Equals("plague")){ return Resources.Load<Sprite>("Cards/E Plague");}
		if(cardTitle.Equals("pox")){ return Resources.Load<Sprite>("Cards/E Pox");}
		if(cardTitle.Equals("queensfavor")){ return Resources.Load<Sprite>("Cards/E Queen's Favor");}			
		else return null;
	}

	public string GetCardType(string card){
		if (card.Equals("excalibur") ||
			card.Equals("lance") ||
			card.Equals("sword") ||
			card.Equals("battleax") ||
			card.Equals("dagger") ||
			card.Equals("horse")) {
			return "weapon";
		} else if (card.Equals("galahad") ||
				card.Equals("arthur") ||
				card.Equals("pellinore") ||
				card.Equals("guinevere") ||
				card.Equals("iseult") ||
				card.Equals("gawain") ||
				card.Equals("lancelot") ||
				card.Equals("percival") ||
				card.Equals("tristan")) {
			return "ally";
		} else if (card.Equals("tomorganlefey") ||
			card.Equals("totemptation") ||
			card.Equals("toquestingbeast") ||
			card.Equals("tovalor")) {
			return "test";
		} else if (card.Equals("amour")) {
			return "amour";
		} else {
			return "foe";
		}
		return null;
	}

	public int getBP(string name){
		if (name.Equals ("excalibur")) {
			return 30;
		} else if (name.Equals ("lance")) {
			return 20;
		} else if (name.Equals ("sword")) {
			return 10;
		} else if (name.Equals ("battleax")) {
			return 15;
		} else if (name.Equals ("dagger")) {
			return 5;
		} else if (name.Equals ("horse")) {
			return 10;
		} else if (name.Equals ("galahad")) {
			return 15;
		} else if (name.Equals ("pellinore")) {
			return 10;
		} else if (name.Equals ("guinevere")) {
			return 0;
		} else if (name.Equals ("iseult")) {
			return 0;
		} else if (name.Equals ("gawain")) {
			return 10;
		} else if (name.Equals ("lancelot")) {
			return 15;
		} else if (name.Equals ("percival")) {
			return 5;
		} else if (name.Equals ("tristan")) {
			return 10;
		} else if (name.Equals ("arthur")) {
			return 10;
		} else {
			return 0;
		}
	}
	public int getAltBP(string name){
		if (name.Equals ("gawain")) {
			return 20;
		} else if (name.Equals ("percival")) {
			return 20;
		}  else {
			return 0;
		}
	}

	public int getFreeBids(string name){
		if (name.Equals ("arthur")) {
			return 2;
		} else if (name.Equals ("pellinore")) {
			return 4;
		} else if (name.Equals ("guinevere")) {
			return 3;
		} else {
			return 0;
		}
	}
		
}
