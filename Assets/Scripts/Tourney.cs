using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourney
{
	int playerNum;
	int strongestPlayerBP;
	int bonusShields;
	int totalShields;
	Player currentPlayer;
	Player strongestPlayer;
	Player[] players;
	public Tourney (Card T)
	{
		strongestPlayerBP = 0;
		playerNum = 0;
		bonusShields = T.getBonusShields();
		totalShields=0;
		players=null;
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
			return;
		}
		int indexToDelete = getPlayerInt(player);
		if(indexToDelete == -1) {
			return;
		}

		Player [] newArr = new Player[players.Length-1];
		playerNum --;
		if(indexToDelete == players.Length-1) {
			for(int i = 0; i < newArr.Length; i++) {
				newArr[i] = players[i];	
			}
			currentPlayer = newArr[newArr.Length-1];
		}
		else if(indexToDelete == 0) {
			for(int i = 0; i < newArr.Length; i++) {
				newArr[i] = players[i+1];
			}
			currentPlayer = newArr[0];
		}
		else {
			currentPlayer = players[indexToDelete+1];
			for(int i = 0; i < indexToDelete; i++) {
				newArr[i] = players[i];
			}
			for(int i = indexToDelete + 1; i < players.Length; i++) {
				newArr[i-1] = players[i];
			}
		}

		players = newArr;
	}

	public void nextPlayer() {
		if(playerNum == 0) {
			Debug.Log("Quest lost, No players left");

		}
		int currentPlayerIndex = getPlayerInt(currentPlayer);

		if(currentPlayerIndex == players.Length-1){

			currentPlayer = players[0];

		}
		else {
			currentPlayer = players[currentPlayerIndex+1];
		}

	}
	public Player getCurrentPlayer() {
		return currentPlayer;
	}

	public int getPlayerInt(Player player) {
		int index = -1;
		for(int i = 0; i < players.Length; i++)
		{
			if(players[i] == player)
			{
				index = i;
				break;
			}
		}
		return index;
	}

	public Player getPlayer(int i) {
		return players[i];
	}
	public int getPlayerNum() {
		return playerNum;
	}

	public void setStrongestPlayer(Player player,int BP){
		strongestPlayer = player;
		strongestPlayerBP = BP;
	}
	public Player getStrongestPlayer(){
		return strongestPlayer;
	}
	public int getStrongestBP(){
		return strongestPlayerBP;
	}
	public void awardShields(){
		strongestPlayer.addShields (bonusShields+getPlayerNum());
	}
	public int getAwardNum(){
		return bonusShields + getPlayerNum();
	}
}


