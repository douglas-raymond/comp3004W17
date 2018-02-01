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
	
	public ActiveQuest(QuestCard _quest)
	{
		quest = _quest;
		stages = _quest.getStages();
		playerNum = 0;

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
	}
	
	public int getStageNum()
	{
		return stages;
	}
	public Player getSponsor()
	{
		return sponsor;
	}
	
}
