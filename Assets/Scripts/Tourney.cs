using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTourney : ActiveStory
{
	int bonusShields;
	int totalShields;

	int [] bps;
	Player winner;
	public ActiveTourney (Card T)
	{
		bonusShields = T.getBonusShields();
		totalShields=0;
		players=null;
		winner = null;
	}

	public Player getCurrentPlayer() {
		return currentPlayer;
	}

	public void addPlayer(Player newPlayer) {

		int n = 0;
		if(players != null){n = players.Length;}
		Player[] temp = new Player[n+1];
		for(int i = 0; i < n; i++)
		{
			temp[i] = players[i];
		}
		temp[n] = newPlayer;

		players = temp;
		currentPlayer = players[0];
		bps = new int [n+1];
	}


	public void setPlayerBP(int BP){
		Debug.Log("getPlayerInt: " + getPlayerInt(getCurrentPlayer()));
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
	public void deletePlayer(Player player) {
		if(players.Length == 1) {
			currentPlayer = null;
			players = null;
			return;
		}
		int indexToDelete = getPlayerInt(player);
		if(indexToDelete == -1) {
			return;
		}

		Player [] newArr = new Player[players.Length-1];
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
	public Player getWinner(){
		return winner;
	}
	public int getAwardNum(){
		return bonusShields + getPlayerNum();
	}

	public bool mordredSpecialAbility(Player target){

		int targetIndex = getPlayerInt(target);
		target.removeAlly("tourney");
		Debug.Log(target.getName() + "'s bp was " + bps[targetIndex]);
		bps[targetIndex] = target.getBP("null");

		Debug.Log(target.getName() + "'s bp is now " + bps[targetIndex]);



		return true;
	}
	public Player[] getPlayerArr(){
		return players;
	}

}