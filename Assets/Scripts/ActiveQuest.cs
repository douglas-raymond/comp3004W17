using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuest{

	QuestCard quest;
	Card [] stages;
	Card [][] stageWeapons;
	Player [] players;
	int playerNum;
	int stageNum;
	Player sponsor;
	int currentStage;
	Player currentPlayer;
	
	public ActiveQuest(QuestCard _quest)
	{
		quest = _quest;
		stageNum = _quest.getStages();
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
			players[i].addShields(stageNum);
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
			if(currentStage + 1 == stageNum){
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
	public void resetQuest() {
		currentStage = 0;
		stageWeapons = null;
		stages = null;
		stageWeapons = new Card[stageNum][];
		stages = new Card[stageNum];
	}
	public void setSponsor(Player player){
		sponsor = player;
	}
	public void setStages(Card[] newStages){
		stages = newStages;
		stageWeapons = new Card[stages.Length][];
		return;
	}
	public void setStageWeapons(Card[] newStageWeapons){
		
		stageWeapons[currentStage] = newStageWeapons;
		return;
	}
	public void setStage(int i) {
		currentStage = i;
	}
	public Player getCurrentPlayer() {
		return currentPlayer;
	}
	public int getStageNum() {
		return stageNum;
	}
	public int getPlayerNum() {
		return playerNum;
	}
	public Player getSponsor() {
		return sponsor;
	}
	public Card getStage(int i) {
		return stages[i];
	}
	public Card getCurrentStage() {
		return stages[currentStage];
	}
	public int getCurrentStageNum(){
		return currentStage;
	}
	public Card getQuest() {
		
		return quest;
	}
	public Card[] getStageWeapons(int i) {
		return stageWeapons[i];
	}
	private bool isStageSpecial(int i) {
		Card temp = stages[i];
		for(int j = 0; j < quest.getSpecialNum(); j++) {
			if(quest.getSpecialFoe(j).getName().Equals(temp.getName())) { return true; }
		}
		return false;
	}
	public int getStageBP(int i)
	{
		int baseBP;
		if(isStageSpecial(i)) { 
			baseBP = stages[i].getAltBP();
		}
		else {
			baseBP = stages[i].getBP();
		}
		
		Debug.Log("base BP: " + baseBP);
		int extraBP = 0;
		if(stageWeapons[i][0] != null)
		{
			for(int j = 0; j< stageWeapons[i].Length; j++)
			{
				extraBP = extraBP + stageWeapons[i][j].getBP();
			}
		}
		return (baseBP + extraBP);
	}
}
