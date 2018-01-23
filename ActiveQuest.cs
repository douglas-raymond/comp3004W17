using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuest{

	QuestCard quest;
	Foe [] foes;
	Player [] players;
	int playerNum;
	int stages;
	int sponsor;
	
	public ActiveQuest(QuestCard _quest, int _sponsor)
	{
		quest = _quest;
		stages = _quest.getStages();
		sponsor = _sponsor;
	}
	
	public void setStages(Foe[] newFoes)
	{
		foes = newFoes;
		return;
		
	}
	public void setPlayers(Player[] newPlayers)
	{
		players = newPlayers;
		playerNum = players.Length;
		return;
	}
	
	public int getStageNum()
	{
		return stages;
	}
	
	
}
