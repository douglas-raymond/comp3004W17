﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkedUI {

	private GameManager gm;
	public int numPlayersConnected;

	public NetworkedUI(){
	}
	public NetworkedUI(GameManager _gm){
	}
	public void AssignGM(GameManager _gm){
		gm = _gm;
	}
	public void setNumPlayersConnected(int i){
		numPlayersConnected = i;
	}

	public void showCard(Card cardToShow){
		
		ShowCardMessage temp = new ShowCardMessage();
		temp.card = cardToShow.getName ();
		//NetworkServer.SendToAll (Msg.showCard, temp);
		NetworkServer.SendToClient(gm.getCurrentPlayer().getConnectionID(), Msg.showCard, temp);
	}

	public void askForCards(Player player, ActiveQuest newQuest, state newState, state oldState, string instructions, string button1, string button2, bool getFoes, bool getWeap, bool getAlly, bool getAmour, bool getTest, bool getMordred, int n = -1) {
		AskForCardsMessage temp = new AskForCardsMessage( player.getHand ().Length);
		for (int i = 0; i < player.getHand ().Length; i++) {
			temp.hand [i] = player.getHand () [i].getName();
		}
		temp.shield = player.getShields ();
		temp.rank = player.getRank();
		temp.name = player.getName();
		if (newQuest != null) {
			if (newQuest.getQuest() != null) {
				temp.BP = player.getBP (newQuest.getQuest ().getName ());
			} else {
				temp.BP = player.getBP ("null");
			}
		} else {
			temp.BP = player.getBP ("null");
		}
		//quest
		if (newQuest != null) {
			if (newQuest.getQuest () != null) {
				temp.questCard = newQuest.getQuest ().getName ();
			} else {
				temp.questCard = null;
			}
		} else {
			temp.questCard = null;
		}
		//other
		temp.newState = newState;
		temp.oldState = oldState;
		temp.instructions = instructions;
		temp.button1 = button1;
		temp.button2 = button2;
		temp.getFoes = getFoes;
		temp.getWeap = getWeap;
		temp.getAlly = getAlly;
		temp.getAmour = getAmour;
		temp.getTest = getTest;
		temp.getMordred = getMordred;
		temp.stage = n;
		NetworkServer.SendToClient (player.getConnectionID(), Msg.askForCards, temp);
		//NetworkServer.SendToClient (1, Msg.askForCards, temp);
	}

	public void displayAlert (string input){
		DisplayAlertMessage temp = new DisplayAlertMessage ();
		temp.input = input;
		//NetworkServer.SendToAll (Msg.displayAlert, temp);
		NetworkServer.SendToClient(gm.getCurrentPlayer().getConnectionID(), Msg.displayAlert, temp);
	}

	public void showStage(ActiveQuest activeQuest){
		ShowStageMessage m = new ShowStageMessage();
		if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Foe))){
			m.foe = true;
			m.test = false;
		} else {
			m.foe = false;
			m.test = true;
		}
		m.highestBid = activeQuest.getHighestBid ();
		m.questCard = activeQuest.getCurrentStage ().getName ();
		NetworkServer.SendToClient(gm.getCurrentPlayer().getConnectionID(), Msg.showStage, m);
	}

	public void endQuest(){
		EmptyMessage temp = new EmptyMessage ();
		NetworkServer.SendToAll (Msg.endQuest, temp);
		//NetworkServer.SendToClient(gm.getCurrentPlayer().getConnectionID(), Msg.endQuest, temp);
	}

	public void drawingQuestCard(){
		EmptyMessage temp = new EmptyMessage ();
		NetworkServer.SendToAll (Msg.drawingQuestCard, temp);
	}

	public void foeReveal(ActiveQuest activeQuest){
		int counter = 0;
		for (int i = 0; i < activeQuest.getStageWeapons(activeQuest.getCurrentStageNum()).Length; i++){
			if (activeQuest.getStageWeapons (activeQuest.getCurrentStageNum ()) [i] != null){
				counter++;
			}
		}
		FoeRevealMessage m = new FoeRevealMessage(counter, activeQuest.getPlayerNum());
		for (int w = 0; w < counter; w++){
			Debug.Log ("w len: " + counter);
			m.weapons [w] = activeQuest.getStageWeapons (activeQuest.getCurrentStageNum ()) [w].getName();
		}
		for (int n = 0; n < activeQuest.getPlayerNum (); n++) {
			m.names [n] = activeQuest.getPlayerArr ()[n].getName();
		}

		m.stage = activeQuest.getCurrentStage().getName();
		m.numPlayers = activeQuest.getPlayerNum();
			NetworkServer.SendToAll (Msg.foeReveal, m);
			//NetworkServer.SendToClient (1, Msg.foeReveal, m);

	}

	public void askYesOrNo(Player player, string message, state messageState){
		AskYesOrNoMessage m = new AskYesOrNoMessage (player.getHand ().Length);
		for (int i = 0; i < player.getHand ().Length; i++) {
			m.hand [i] = player.getHand () [i].getName();
		}
		m.shields = player.getShields ();
		m.rank = player.getRank();
		m.name = player.getName();
		m.BP = player.getBP("null");
		m.message = message;
		m.messageState = messageState;
		NetworkServer.SendToClient (player.getConnectionID(), Msg.askYesOrNo, m);
		//NetworkServer.SendToClient (1, Msg.askYesOrNo, m);
	}

	public void askForPlayerChoice(Player player, state newState, string instructions, Player[] players){
		AskForPlayerChoiceMessage m = new AskForPlayerChoiceMessage (player.getHand().Length, players.Length);
		for (int h = 0; h < player.getHand ().Length; h++) {
			m.hand [h] = player.getHand () [h].getName ();
		}
		for (int p = 0; p < players.Length; p++) {
			m.names [p] = players [p].getName ();
		}
		m.instructions = instructions;
		m.newState = newState;
		NetworkServer.SendToClient (player.getConnectionID(), Msg.askForPlayerChoice, m);
		//NetworkServer.SendToClient (1, Msg.askForPlayerChoice, m);
	}
		
	public void updatePlayers(Player[] players){
		for (int i = 0; i < players.Length; i++) {
			UpdatePlayerMessage message = new UpdatePlayerMessage (players [i].getHand ().Length);
			for (int c = 0; c < players [i].getHand ().Length; c++) {
				message.hand [c] = players [i].getHand () [c].getName ();
			}
			message.rank = players [i].getRank ();
			message.BP = players [i].getBP ("null");
			message.name = players [i].getName ();
			message.shields = players [i].getShields ();
			NetworkServer.SendToClient (players [i].getConnectionID (), Msg.updatePlayer, message);
			//NetworkServer.SendToClient (1, Msg.updatePlayer, message);
		}
	}
}
