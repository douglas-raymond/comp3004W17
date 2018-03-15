using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourney
{
	int playerNum;
	int bonusShields;
	int totalShields;
	Player currentPlayer;

	Player[] players;
	int [] bps;
	Player winner;
	public Tourney (Card T)
	{
		playerNum = 0;
		bonusShields = T.getBonusShields();
		totalShields=0;
		players=null;
		winner = null;
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
		bps = new int [players.Length];
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
			if(players[i].getName().Equals(player.getName()))
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
	public void setPlayerBP(int BP){
		bps[getPlayerInt(getCurrentPlayer())] = BP;
	}
	
	public void awardShields(){
		Player strongestPlayer = null;
		int strongestPlayerBP = -1;
		
		for(int i = 0; i <players.Length; i++){
			if(bps[i] > strongestPlayerBP){
				strongestPlayer = players[i];
				strongestPlayerBP = bps[i];
			}
		}
		winner = strongestPlayer;
		strongestPlayer.addShields (bonusShields+getPlayerNum());
	}
	
	public Player getWinner(){
			return winner;
	}
	public int getAwardNum(){
		return bonusShields + getPlayerNum();
	}
	
	public bool mordredSpecialAbility(Player target){
		
		int targetIndex = getPlayerInt(target);
		target.removeAlly();
		Debug.Log(target.getName() + "'s bp was " + bps[targetIndex]);
		bps[targetIndex] = target.getBP();
		
		Debug.Log(target.getName() + "'s bp is now " + bps[targetIndex]);
		
		

		return true;
	}
	
	public Player[] getAllOtherPlayers(Player player) {
		Player[] temp = new Player[players.Length-1];
		int playerIndex = getPlayerInt(player);
		
		if(players[0].getName().Equals(player.getName())) {
			for(int i = 1; i < players.Length; i++){
				temp[i-1] = players[i];
			}
		}
		else if(players[temp.Length].getName().Equals(player.getName())){
			for(int i = 0; i < players.Length-1; i++) {
				temp[i] = players[i];
			}
		}
		else{
			for(int i = 0; i< playerIndex; i++ ){
				temp[i] = players[i];
			}
			for(int i = playerIndex+1; i< players.Length; i++ ){
				temp[i-1] = players[i];
			}
		}
		
		return temp;
	}
	
	public Player findPlayer(string target) {
		for(int i = 0; i < players.Length; i++) {
			if(players[i].getName().Equals(target)) {
				return players[i];
			}
		}
		return null;
	}
}


