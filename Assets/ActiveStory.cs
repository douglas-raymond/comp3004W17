using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveStory  {
/* fill out common func*/
	protected Player[] players;
	protected HelperFunctions hp;
	public string type;
	
	public abstract Player getCurrentPlayer() ;
	
	public string getType() {return type;}
	public void addPlayer(Player newPlayer) {

		int n = 0;
		if(players != null){n = players.Length;}
		
		/*
		Player[] temp = new Player[n+1];
		for(int i = 0; i < n; i++)
		{
			temp[i] = players[i];
		}
		temp[n] = newPlayer;
*/
		Player[] temp = hp.addPlayer(players, newPlayer);
		players = temp;
		
		addPlayerExtraBehaviour(n);
	}


	protected abstract void addPlayerExtraBehaviour(int n);
	//public abstract void deletePlayer(Player player){


	public abstract void nextPlayer() ;

	public Player findPlayer(string target) {
		for(int i = 0; i < players.Length; i++) {
			if(players[i].getName().Equals(target)) {
				return players[i];
			}
		}
		return null;
	}
	
	public  Player getPlayer(int i) {
		Debug.Log("returning " + players[i].getName());
		return players[i];
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

	public abstract bool mordredSpecialAbility(Player target);

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
