using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
public class GameManager : MonoBehaviour {
	//Initialize logging functionality
	Logger log = new Logger("GameManager", "c:/comp3004");
	
	//Initialize the two decks
	AdvDeck advDeck = new AdvDeck();
	StoryDeck storyDeck = new StoryDeck();
	UI ui;

	
	int playerCount = 3;
	
	Player[] players;
	
	//Game states. There will eventually be many possible states, but for right now these two exist.
	enum state {WAITINGFORINPUT, GOTINPUT, QUESTINPROGRESS};
	state gameState = state.WAITINGFORINPUT;

	int activePlayerMeta;
	int activePlayerSub;
	
	ActiveQuest activeQuest;
	// Use this for initialization
	void Start () {
		log.Init ();
		ui = new UI(this);
		log.log ("created UI");
		//Create all the players and add it to the players array
		players = new Player[playerCount];
		log.log ("created player array");
		
		for(int i = 0; i < playerCount; i++){
			players[i] = new Player(new Card[12], 0, 0, "Player " + (i+1));
		}
		log.log ("dealt cards");
			
		//Init the decks
		advDeck.initDeck();
		storyDeck.initDeck();
		log.log ("decks initialized");

		gameStart();
	}
	private void gameStart(){
		activePlayerMeta++;
		dealHands(playerCount);
		log.log ("Dealing hands, drawing first quest");
		drawQuestCard();
	}
	private void drawQuestCard(){
		Card drawnCard = storyDeck.drawCard();
		
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

		ui.showCard(activeQuest.getQuest());
		activePlayerSub = (activePlayerSub+1) % playerCount;
		if(activePlayerSub != activePlayerMeta)
		{
			log.log ("Getting sponsor");
			Debug.Log("Getting sponsor...");
			ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);
		}
		else
		{
			log.log ("Sponsor not found");
			Debug.Log("Sponsor not found.");
			activeQuest = null;
			drawQuestCard();
		}
	}
	public void startQuestSetup(){
		activeQuest.setSponsor(players[activePlayerSub]);
		ui.askForStageSelection(activeQuest.getSponsor(), activeQuest.getStageNum());
	}	
	public void endQuestSetup(Card[] stages){
		Debug.Log("endQuestSetup");
		activeQuest.setStages(stages);
		gameState = state.GOTINPUT;
		bool validQuest = true;
		for(int i = 0; i < stages.Length; i++)
		{
			int prevBP;
			if(i == 0) { prevBP = -1; }
			else {prevBP = stages[i-1].getBP();}
			if(stages[i].getBP() < prevBP)
			{
				validQuest = false;
			}	
		}
		
		if(validQuest)
		{
			Debug.Log("Valid quest");
			for(int i = 0; i < stages.Length; i++){
				activeQuest.getSponsor().discardCard(new Card[] {stages[i]});
			}
			activePlayerSub = activePlayerMeta;
			getPlayers();
		}
		else
		{
			Debug.Log("Invalid quest");
			ui.displayAlert("Invalid selection. Stage's BP must be in increasing order.");
			ui.askForStageSelection(activeQuest.getSponsor(), activeQuest.getStageNum());
		}
	}
	public void getPlayers(){	
		activePlayerSub ++;
		activePlayerSub = activePlayerSub % (playerCount-1);
		if(players[activePlayerSub] == activeQuest.getSponsor()) {
			activePlayerSub ++;
			activePlayerSub = activePlayerSub % (playerCount-1);
		}
		
		ui.askYesOrNo(players[activePlayerSub], "Do you want to join this quest?", GameState.state.ASKINGFORPLAYERS);
	}
	public void gotPlayer(Player newPlayer){
		if(newPlayer != null)
		{
			activeQuest.addPlayer(newPlayer);
		}
		if(activePlayerSub == activePlayerMeta)
		{
			Debug.Log("Done getting players");
			startStage();
		}
		else
		{
			getPlayers();
		}
	}
	/*Gets a selected card and does something with it
	*/
	//Pass in a player count, it will give each player a hand of 12 adventure cards
	private void dealHands(int playerCount){
		for(int i = 0; i < playerCount; i++){
			Card[] newHand = new Card[12];
			for(int j = 0; j < newHand.Length; j++){
				newHand[j] = advDeck.drawCard();
			}
			players[i].setHand(newHand);
		}
		return;
	}
	public void startStage() {
		if(activeQuest.getQuest() == null)
		{
			endQuest("Quest over");
		}
		else if(activeQuest.getPlayerNum() == 0)
		{
			endQuest("All players dead");
		}
		else{
			ui.showStage(activeQuest);
			ui.askForBattleCardSelection(activeQuest.getCurrentPlayer());
		}
		return;
	}
	public void endQuest(string text = "Quest over")
	{
			Debug.Log(text);
			activeQuest = null;
			ui.endQuest();
			ui.drawingQuestCard();
			drawQuestCard();
	}
	public void questAttack(Card [] selection) {
		//Debug.Log("Quest Attack");
		int extraBP = 0;
		if(selection.Length > 0)
		{
			for(int i = 0; i < selection.Length; i++)
			{
				extraBP = extraBP + selection[i].getBP();
			}

		}
		if(activeQuest.getCurrentStage().getBP() <= activeQuest.getCurrentPlayer().getBP() + extraBP)
		{
			if(selection.Length > 0)
			{
				activeQuest.getCurrentPlayer().discardCard(selection);
			}
			activeQuest.nextPlayer();
			startStage();			
		}
		else
		{
			ui.displayAlert("Too weak to defeat foe!");
			ui.askForBattleCardSelection(activeQuest.getCurrentPlayer());
		}
	}
	
	public void forfeitQuest() {
		//Debug.Log("Quest Attack");
		activeQuest.deletePlayer(activeQuest.getCurrentPlayer());
		startStage();
	}
}
