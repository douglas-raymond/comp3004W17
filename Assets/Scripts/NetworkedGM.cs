using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkedGM : MonoBehaviour{
	
	NetworkClient client;
	int connectionID;

	bool upToDate;
	state inputState;
	Player currentPlayer;
	string otherPlayerInfromation;

	public NetworkedGM(NetworkClient c, int i){
		client = c;
		connectionID = i;
		upToDate = false;
		currentPlayer = null;
	}

	public state getUserInputState(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.getUserInputState, m);

		upToDate = false;
		return inputState;
	}

	public void startQuestSetup(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.startQuestSetup, m);
	}

	public void getSponsor(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.getSponsor, m);
	}

	public void setUserInputState(state newState){
		SetUserInputStateMessage m = new SetUserInputStateMessage ();
		m.newState = newState;
		client.Send (Msg.setUserInputState, m);
	}

	public void gotPlayer(Player newPlayer){
		GotPlayerMessage m = new GotPlayerMessage ();
		if (newPlayer != null) {
			m.name = newPlayer.getName ();
		} else {
			m.name = null;
		}
		client.Send (Msg.gotPlayer, m);
	}

	public void gotPlayerTourney(Player newPlayer){
		GotPlayerMessage m = new GotPlayerMessage ();
		if (newPlayer != null) {
			m.name = newPlayer.getName ();
		} else {
			m.name = null;
		}
		client.Send (Msg.gotPlayerTourney, m);
	}

	public void questAttack(Card [] selection) {
		int len = 0;
		if (selection != null) {
			len = selection.Length;
		} 
		SelectionMessage m = new SelectionMessage (len);
		for (int i = 0; i < len; i++) {
			m.selection [i] = selection [i].getName();
		}
		m.connectionID = connectionID;
		client.Send (Msg.questAttack, m);
	}

	public void forfeitQuest() {
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.forfeitQuest, m);
	}

	public void bidPhase(Card [] selection) {	
		int len = 0;
		if (selection != null) {
			len = selection.Length;
		} 
		SelectionMessage m = new SelectionMessage (len);
		for (int i = 0; i < len; i++) {
			m.selection [i] = selection [i].getName();
		}
		m.connectionID = connectionID;
		client.Send (Msg.bidPhase, m);
	}

	public void endQuest(string text = "Quest over") {
		EndQuestMessage m = new EndQuestMessage ();
		m.input = text;
		client.Send (Msg.endQuest, m);
	}

	public void endStage(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.endStage, m);
	}

	public void endStageWeaponSetup(Card[] stageWeapons){
		int len = 0;
		if (stageWeapons != null) {
			len = stageWeapons.Length;
		} 
		SelectionMessage m = new SelectionMessage(len);
		for (int i = 0; i < len; i++) {
			m.selection [i] = stageWeapons [i].getName ();
		}
		m.connectionID = connectionID;
		client.Send (Msg.endStageWeaponSetup, m);
	}	

	public void gotMordredTarget(string target) {
		GotMordredTargetMessage m = new GotMordredTargetMessage ();
		m.connectionID = connectionID;
		m.target = target;
		client.Send (Msg.gotMordredTarget, m);
	}

	public void endQuestSetup(Card[] stages){
		int len = 0;
		if (stages != null) {
			len = stages.Length;
		}
		SelectionMessage m = new SelectionMessage (len);
		for (int i = 0; i < len; i++) {
			m.selection [i] = stages [i].getName();
		}
		m.connectionID = connectionID;
		client.Send (Msg.endQuestSetup, m);
	}

	public void gotTournamentCards(Card[] cards){
		int len = 0;
		if (cards != null) {
			len = cards.Length;
		} 
		SelectionMessage m = new SelectionMessage(len);
		for (int i = 0; i < len; i++) {
			m.selection [i] = cards [i].getName ();
		}
		m.connectionID = connectionID;
		client.Send (Msg.gotTournamentCards, m);
	}

	public void gotCardLimitReached(Card[] cards){
		int len = 0;
		if (cards != null) {
			len = cards.Length;
		} 
		SelectionMessage m = new SelectionMessage(len);
		for (int i = 0; i < len; i++) {
			m.selection [i] = cards [i].getName ();
		}
		m.connectionID = connectionID;
		client.Send (Msg.gotCardLimitReached, m);
	}

	public Player getCurrentPlayer(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.getCurrentPlayer, m);

		while (!upToDate) {
		}
		upToDate = false;
		return currentPlayer;
	}

	public string getOtherPlayerInfo(Player currPlayer) {
		GetOtherPlayerMessage m = new GetOtherPlayerMessage ();
		m.name = currPlayer.getName ();
		m.connectionID = connectionID;
		client.Send (Msg.getOtherPlayerInfo, m);

		while (!upToDate) {
		}
		upToDate = false;
		return otherPlayerInfromation;
	}

	public void LoadMouseOverShowHand (){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.mouseOverShowHand, m);
	}

	public void LoadMouseOverShowOtherPlayer (){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.mouseOverShowOther, m);
	}

	public void CheckCardSelection(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.checkCardSelection, m);
	}

	public void CheckCardRemoval(){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.checkCardRemoval, m);
	}

	public void CheckButtonClick (){
		IntegerMessage m = new IntegerMessage ();
		m.value = connectionID;
		client.Send (Msg.checkButtonClick, m);
	}

	//helpers
	IEnumerator BusyWait(){
		yield return new WaitForSeconds (1.0f);
	}

	public void PostUpdate(state s){
		inputState = s;
		upToDate = true;
	}

	public void PostUpdate(Player p){
		currentPlayer = p;
		upToDate = true;
	}

	public void PostUpdate(string s){
		otherPlayerInfromation = s;
		upToDate = true;
	}
}
