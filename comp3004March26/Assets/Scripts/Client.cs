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

	void Start(){
		myClient = new NetworkClient ();
		myClient.RegisterHandler (Msg.confirmConnect, Init);
		myClient.Connect("127.0.0.1", 4444);
	}

	public void Init(NetworkMessage m){
		Debug.Log ("init");
		gm = new NetworkedGM (myClient, connectionID);
		ui = new UI (gm);
		myClient.RegisterHandler(Msg.showHand, showHand);
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
	}
		
	public void showHand(NetworkMessage m){
		ShowHandMessage message = m.ReadMessage<ShowHandMessage>();
		string[] stringHand = message.hand;
		Card[] hand = MessageToHand (stringHand);
		ui.showHand(hand);
	}

	public void askForCards(NetworkMessage m){
		AskForCardsMessage nM = m.ReadMessage<AskForCardsMessage> ();
		Player tempPlayer = MessageToPlayer (nM.hand, nM.shield, nM.rank, nM.name, nM.BP);
		//populate tempPlayer with nM
		ui.askForCards (tempPlayer, nM.newState, nM.oldState, nM.instructions, nM.button1, nM.button2, nM.getFoes, nM.getWeap, nM.getAlly, nM.getAmour, nM.getTest, nM.getMordred, nM.stage);
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
		Player tempPlayer = MessageToPlayer (message.hand, message.shields, message.rank, message.name, message.BP);
		gm.PostUpdate (tempPlayer);
	}

	public void PassOtherPlayerInfo(NetworkMessage m){
		StringMessage message = m.ReadMessage<StringMessage> ();
		gm.PostUpdate (message.value);
	}

	public void askYesOrNo(NetworkMessage m){
		AskYesOrNoMessage message = m.ReadMessage<AskYesOrNoMessage> ();
		Player tempPlayer = MessageToPlayer (message.hand, message.shields, message.rank, message.name, message.BP);
		ui.askYesOrNo (tempPlayer, message.message, message.messageState);
	}

	public void askForPlayerChoice(NetworkMessage m){
		AskForPlayerChoiceMessage message = m.ReadMessage<AskForPlayerChoiceMessage> ();
		Player[] playerList = new Player[message.names.Length];
		for (int i = 0; i < message.names.Length; i++){
			playerList [i] = MessageToPlayer (null, 0, 0, message.names[i], 0);
		}
		Player tempPlayer = MessageToPlayer (message.hand, message.shield, message.rank, message.name, message.BP);
		ui.askForPlayerChoice (tempPlayer, message.newState, message.instructions, playerList);
	}

	public void mouseOverShowHand(NetworkMessage m){
		GetCurrentPlayerMessage message = m.ReadMessage<GetCurrentPlayerMessage> ();
		Player tempPlayer = MessageToPlayer (message.hand, message.shields, message.rank, message.name, message.BP);
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

	//Helpers
	public Card[] MessageToHand(string[] hand){
		Debug.Log ("Message To Hand");
		Card[] tempHand = new Card[hand.Length];
		for (int i = 0; i < hand.Length; i++) {
			tempHand [i] = MessageToCard (hand [i]);
		}
		return tempHand;
	}

	public Player MessageToPlayer(string[] hand, int shield, int rank, string name, int BP){
		Debug.Log ("Message To Player");
		Card[] tempHand = MessageToHand (hand);
		Player newPlayer = new Player (tempHand, shield, rank, name);
		return newPlayer;
	}

	public ActiveQuest MessageToActiveQuest(string[] weapons, string stage, int numPlayers, string[] names){
		Debug.Log ("Message To Active Quest");
		QuestCard tempQuest = new QuestCard (null, null, 1, null, null);
		ActiveQuest tempActiveQuest = new ActiveQuest (tempQuest);
		for (int i = 0; i < names.Length; i++) {
			tempActiveQuest.addPlayer(MessageToPlayer(null, 0, 0, names[i], 0));
		}
		tempActiveQuest.setStage (1);
		Card[] tempStage = new Card[1];
		tempStage [0] = MessageToCard (stage);
		tempActiveQuest.setStages (tempStage);
		tempActiveQuest.setStageWeapons (MessageToHand (weapons));
		return tempActiveQuest;
	}

	public ActiveQuest MessageToActiveQuestStage(bool foe, bool test, string questCard, int highestBid){
		QuestCard tempQuest = null;
		Sprite tempSprite = Resources.Load <Sprite> ("Cards/A Merlin");
		if (foe) {
			tempQuest = new QuestCard (questCard, "Foe", 1, null, tempSprite);
		} else {
			tempQuest = new QuestCard (questCard, "Test", 1, null, tempSprite);
		}
		ActiveQuest tempActiveQuest = new ActiveQuest (tempQuest);
		tempActiveQuest.setStage (1);
		tempActiveQuest.placeBid (null, highestBid);
		return tempActiveQuest;
	}

	public Card MessageToCard(string card){
		Debug.Log ("Message to Card");
		string[] types = new string[]{"A", "E", "F", "Q", "R", "T", "To", "W"};
		Sprite tempSprite = null;
		for (int i = 0; i < types.Length; i++){
			tempSprite = Resources.Load <Sprite> ("Cards/"+types[i]+" "+card);
			if (tempSprite != null){
				break;
			}
		}
		Card tempCard = new Card (card, tempSprite);
		return tempCard;
	}
}
