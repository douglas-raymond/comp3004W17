using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTourney : ActiveStory
{
	int bonusShields;
	int totalShields;
	private Player currentPlayer;
	//private Player[] players;
	int [] bps;
	Player winner;
	public ActiveTourney (Card T)
	{
		type = "tourney";
		bonusShields = T.getBonusShields();
		totalShields=0;
		players=null;
		winner = null;
		hp = new HelperFunctions();
	}

	public override Player getCurrentPlayer() {
		return currentPlayer;
	}

	protected override void addPlayerExtraBehaviour(int n){
		currentPlayer = players[0];
		bps = new int [n+1];
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

	public override bool mordredSpecialAbility(Player target){

		int targetIndex = getPlayerInt(target);
		target.removeAlly("tourney");
		DebugX.Log(target.getName() + "'s bp was " + bps[targetIndex]);
		bps[targetIndex] = target.getBP("null");

		DebugX.Log(target.getName() + "'s bp is now " + bps[targetIndex]);



		return true;
	}

	
	public override void nextPlayer() {
		if(players.Length == 0) {
			DebugX.Log("Quest lost, No players left");

		}
		int currentPlayerIndex = getPlayerInt(currentPlayer);

		if(currentPlayerIndex == players.Length-1){

			currentPlayer = players[0];

		}
		else {
			currentPlayer = players[currentPlayerIndex+1];
		}

	}
}