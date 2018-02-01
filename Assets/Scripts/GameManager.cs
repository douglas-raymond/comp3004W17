using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	
	//Initialize the two decks
	AdvDeck advDeck = new AdvDeck();
	StoryDeck storyDeck = new StoryDeck();
	UI ui;
	
	int playerCount = 3;
	
	Player[] players;
	
	//Game states. There will eventually be many possible states, but for right now these two exist.
	enum state {WAITINGFORINPUT, GOTINPUT};
	state gameState = state.WAITINGFORINPUT;

	int activePlayerMeta;
	int activePlayerSub;
	
	ActiveQuest activeQuest;
	// Use this for initialization
	void Start () {
		ui = new UI(this);
		//Create all the players and add it to the players array
		players = new Player[playerCount];
		
		for(int i = 0; i < playerCount; i++){
			players[i] = new Player(new Card[12], 0, 0, "Player " + i);
		}
			
		//Init the decks
		advDeck.initDeck();
		storyDeck.initDeck();
		
		//Deal hands to all the players
		
		
		//Asks player oen for a card selection. THis is for testing purposes
		gameStart();
	}
	
	private void gameStart(){
		activePlayerMeta++;
		dealHands(playerCount-1);
		Debug.Log("gameStart is run. Running drawQuestCard");
		drawQuestCard();
	}
	
	private void drawQuestCard(){
		Card drawnCard = storyDeck.drawCard();
		Debug.Log(drawnCard + " has been drawn");
		if(drawnCard.getType().Equals("quest")) {
			activePlayerSub = activePlayerMeta;
			activeQuest = new ActiveQuest((QuestCard)drawnCard);
			getSponsor();
		}
		else {
			drawQuestCard();
		}
	}
	
	public void getSponsor(){	
		Debug.Log("activePlayerSub: " + activePlayerSub);
		
		activePlayerSub = (activePlayerSub+1) % playerCount;
		Debug.Log("activePlayerSubMod: " + activePlayerSub % playerCount);
		if(activePlayerSub != activePlayerMeta)
		{
			Debug.Log("Getting sponsor...");
			ui.askYesOrNo(players[activePlayerSub]);
		}
		else
		{
			Debug.Log("Sponsor not found.");
			activeQuest = null;
			drawQuestCard();
		}
	}
	
	public void getPlayers(){	
		activePlayerSub ++;
		activePlayerSub = activePlayerSub % (playerCount-1);
		Debug.Log(activePlayerSub);
		if(players[activePlayerSub] == activeQuest.getSponsor())
		{
			activePlayerSub ++;
		}
		
		ui.askJoinOrDecline(players[activePlayerSub]);
	}
	public void gotPlayer(Player newPlayer){
		activeQuest.addPlayer(newPlayer);
		if(activePlayerSub == activePlayerMeta)
		{
			Debug.Log("Done getting players");
			return;
		}
		else
		{
			getPlayers();
		}
	}
	
	public void startQuestSetup(){
		activeQuest.setSponsor(players[activePlayerSub]);
		ui.askForMultipleCardSelection(players[activePlayerSub], activeQuest.getStageNum());
	}
	
	public void endQuestSetup(Card[] stages){
		Debug.Log("endQuestSetup");
		activeQuest.setStages(stages);
		gameState = state.GOTINPUT;
		for(int i = 0; i < stages.Length; i++){
			activeQuest.getSponsor().discardCard(stages[i]);
			Debug.Log(stages[i]);
		}
		
		activePlayerSub = activePlayerMeta;
		getPlayers();
	}
	
	
	//Gets a selected card and does something with it
	public void gotSingleCardSelection(Card card){
		gameState = state.GOTINPUT;
	}
	
	
	//Pass in a player count, it will give each player a hand of 12 adventure cards
	private void dealHands(int playerCount){
		for(int i = 0; i < playerCount+1; i++){
			Card[] newHand = new Card[12];
			for(int j = 0; j < 11; j++){
				newHand[j] = advDeck.drawCard();
			}
			players[i].setHand(newHand);
		}
		return;
	}	
}
