using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuest{

	QuestCard quest;
	Card [] foes;
	Player [] players;
	int playerNum;
	int stages;
	Player sponsor;
	int currentStage;
	Player currentPlayer;
	
	public ActiveQuest(QuestCard _quest)
	{
		quest = _quest;
		stages = _quest.getStages();
		playerNum = 0;
		currentStage = 0;
	}
	
	public void addPlayer(Player newPlayer) {
		int n = playerNum;
		Player[] temp = new Player[playerNum+1];
		for(int i = 0; i < playerNum; i++)
		{
			temp[i] = players[i];
		}
		temp[playerNum] = newPlayer;
		
		players = temp;
		playerNum ++;
		currentPlayer = players[0];
	}
	
	public void deletePlayer(Player player) {
		if(playerNum == 1) {
			playerNum = 0;
			currentPlayer = null;
			players = null;
			quest = null;
			return;
		}
		Debug.Log("Old size: " + players.Length);
		int indexToDelete = getPlayerInt(player);
		if(indexToDelete == -1) {
			Debug.Log("Not found");
			return;
		}
		
		Player [] newArr = new Player[players.Length-1];
		playerNum --;
		if(indexToDelete == players.Length-1) {
			Debug.Log("Tail");
			for(int i = 0; i < newArr.Length; i++) {
				newArr[i] = players[i];	
			}
			currentPlayer = newArr[newArr.Length-1];
			nextPlayer();
		}
		else if(indexToDelete == 0) {
			Debug.Log("Head");
			for(int i = 0; i < newArr.Length; i++) {
				newArr[i] = players[i+1];
			}
			currentPlayer = newArr[0];
		}
		else {
			currentPlayer = players[indexToDelete+1];
			Debug.Log("Middle");
			for(int i = 0; i < indexToDelete; i++) {
				newArr[i] = players[i];
			}
			for(int i = indexToDelete + 1; i < players.Length; i++) {
				newArr[i-1] = players[i];
			}
		}
		
		players = newArr;
		Debug.Log("New size: " + players.Length);
	}
	private void finishQuest() {
		if(playerNum == 0) { return;}
		for(int i = 0; i< players.Length; i ++)
		{
			players[i].addShields(stages);
		}
		quest = null;
	}

	public void nextPlayer() {
		if(playerNum == 0)
		{
			Debug.Log("Quest lost, No players left");
			quest = null;
		}
		int currentPlayerIndex = getPlayerInt(currentPlayer);

		if(currentPlayerIndex == players.Length-1){
			currentPlayer = players[0];
			if(currentStage + 1 == stages){
				quest = null;
				finishQuest();
				return;
			}
			else {
				currentStage++;
			}
			
		}
		else {
			currentPlayer = players[currentPlayerIndex+1];
		}
		
	}
	//Getters and setters
	public int getPlayerInt(Player player) {
		int index = -1;
		for(int i = 0; i < players.Length; i++)
		{
			if(players[i] == currentPlayer)
			{
				index = i;
				break;
			}
		}
		return index;
	}
	public void setSponsor(Player player){
		sponsor = player;
	}
	public void setStages(Card[] newFoes){
		foes = newFoes;
		return;
	}
	public Player getCurrentPlayer() {
		return currentPlayer;
	}
	public int getStageNum() {
		return stages;
	}
	public int getPlayerNum() {
		return playerNum;
	}
	public Player getSponsor() {
		return sponsor;
	}
	public Card getCurrentStage() {
		return foes[currentStage];
	}
	public Card getQuest() {
		return quest;
	}
}
