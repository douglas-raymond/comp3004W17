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
	int currentPlayer;
	
	public ActiveQuest(QuestCard _quest)
	{
		quest = _quest;
		stages = _quest.getStages();
		playerNum = 0;
		currentStage = 0;

	}
	
	public void setSponsor(Player player){
		sponsor = player;
	}
	public void setStages(Card[] newFoes){
		foes = newFoes;
		return;
	}
	
	public void addPlayer(Player newPlayer){
		int n = playerNum;

		Player[] temp = new Player[playerNum+1];
		for(int i = 0; i < playerNum; i++)
		{
			temp[i] = players[i];
		}
		temp[playerNum] = newPlayer;
		
		players = temp;
		playerNum ++;
	}
	private void finishQuest()
	{
		if(playerNum == 0) { return;}
		for(int i = 0; i< players.Length; i ++)
		{
			players[i].addShields(stages);
		}
		quest = null;
	}
	public int getStageNum()
	{
		return stages;
	}
	public int getPlayerNum()
	{
		return playerNum;
	}
	public Player getSponsor()
	{
		return sponsor;
	}
	
	public Card getCurrentStage()
	{
		return foes[currentStage];
	}
	
	public Card getQuest()
	{
		return quest;
	}
	public void nextPlayer()
	{
		if(playerNum == 0)
		{
			Debug.Log("Quest lost, No players left");
			quest = null;
		}
		currentPlayer ++;
		if(currentPlayer == players.Length)
		{
			currentPlayer = 0;
			if(currentStage + 1 == stages)
			{
				Debug.Log("Quest over");
				return;
			}
			if(currentStage == stages)
			{
				finishQuest();
				return;
			}
			else{
				currentStage++;
			}
			
		}
		
	}
	public Player getCurrentPlayer()
	{
		return players[currentPlayer];
	}
}
