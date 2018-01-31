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

	}
	
	public void setSponsor(Player player){
		sponsor = player;
	}
	public void setStages(Card[] newFoes){
		foes = newFoes;
		return;
	}
	
	public void setPlayers(Player[] newPlayers){
		players = newPlayers;
		playerNum = players.Length;
		return;
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
