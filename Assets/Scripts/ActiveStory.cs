using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStory  {

	protected Player currentPlayer;
	protected Player[] players;


	public void nextPlayer() {
		if(players.Length == 0) {
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

	public Player findPlayer(string target) {
		for(int i = 0; i < players.Length; i++) {
			if(players[i].getName().Equals(target)) {
				return players[i];
			}
		}
		return null;
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



	public Player[] getPlayerArr(){
		return players;
	}


	public int getPlayerNum() {
		if(players == null) {
			return 0;
		}
		return players.Length;
	}


}
