using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
public class GameManager : MonoBehaviour {
	//Initialize logging functionality
	Logger log = new Logger("GameManager");
	
	//Initialize the two decks
	DiscardDeck advDiscard = new DiscardDeck();
	DiscardDeck storyDiscard = new DiscardDeck();
	AdvDeck advDeck;
	StoryDeck storyDeck;
	UI ui;

	
	int playerCount = 3;
	
	Player[] players;
	
	//Game states. There will eventually be many possible states, but for right now these two exist.
	enum state {WAITINGFORINPUT, GOTINPUT, QUESTINPROGRESS};
	state gameState = state.WAITINGFORINPUT;

	int activePlayerMeta;
	int activePlayerSub;
	
	ActiveQuest activeQuest;
	
	bool cyclingThroughPlayers;
	// Use this for initialization
	void Start () {
		advDeck = new AdvDeck(advDiscard);
		storyDeck = new StoryDeck(storyDiscard);
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
		Debug.Log("before: " + activePlayerMeta);
		activePlayerMeta = nextPlayer(activePlayerMeta);
		Debug.Log("after: " + activePlayerMeta);
		dealHands(playerCount);
		log.log ("Dealing hands, drawing first quest");
		drawQuestCard();
	}
	private void drawQuestCard(){
		Debug.Log("before: " + activePlayerMeta);
		activePlayerMeta = nextPlayer(activePlayerMeta);
		Debug.Log("after: " + activePlayerMeta);
		Card drawnCard = storyDeck.drawCard();
		evaluateStory(drawnCard);
	}

	//Track splitter that evaluates based on card type.
	public void evaluateStory(Card storyCard){
		switch (storyCard.getType()) {
		case "quest":
			activePlayerSub = activePlayerMeta;
			activeQuest = new ActiveQuest((QuestCard)storyCard);

			cyclingThroughPlayers = false;
			getSponsor();
			
			break;
			/*block these out until we can get the tourneys and events sorted
		case "tourney":
			createTourney (storyCard);
			break;
		case "event":
			//Event handling. Pretty much done because events are handled in the cards themselves.
			storyCard.runEvent (players, activePlayer);
			break; */
		default:
			drawQuestCard ();
			break;
		}
	}
	public void getSponsor(){	

		ui.showCard(activeQuest.getQuest());

		if(activePlayerSub == activePlayerMeta && cyclingThroughPlayers == true)
		{
			log.log ("Sponsor not found");
			Debug.Log("Sponsor not found.");
			activeQuest = null;
			drawQuestCard();
		}
		else
		{
			cyclingThroughPlayers = true;
			log.log ("Getting sponsor");
			Debug.Log("Getting sponsor...");
			activePlayerSub = nextPlayer(activePlayerSub);
			ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);	
			
			
		}
	}
	public void startQuestSetup(){
		Debug.Log("settign this player as sponser " + players[activePlayerSub].getName());
		activeQuest.setSponsor(players[activePlayerSub], activePlayerSub);
		//ui.askForStageSelection(activeQuest.getSponsor(), activeQuest.getStageNum());
		ui.askForCards(
			activeQuest.getSponsor(), 
			activeQuest.getStageNum(), 
			GameState.state.ASKINGFORSTAGES, 
			"Select up to " + activeQuest.getStageNum() + " stages", 
			"null",
			"Forfeit", 
			true, 
			false, 
			false,
			false
			);
	}	
	public void endQuestSetup(Card[] stages){
		Debug.Log("endQuestSetup");
		activeQuest.setStages(stages);
		gameState = state.GOTINPUT;
		startStageWeaponSetup();
	}
	
	public void startStageWeaponSetup(){
		ui.askForCards(
			activeQuest.getSponsor(), 
			activeQuest.getSponsor().getHand().Length, 
			GameState.state.ASKINGFORSTAGEWEAPONS, 
			"Select weapons to enhance this stage", 
			"Done", 
			"null",
			false, 
			true, 
			false,
			false
			);
		ui.showCard(activeQuest.getCurrentStage());
	}	
	public void endStageWeaponSetup(Card[] stageWeapons){
		if(stageWeapons == null) {
			Debug.Log("no stageWeapons selected");
			activeQuest.setStageWeapons(new Card[] {null});
		}
		else {
			Debug.Log(stageWeapons.Length + " stageWeapons selected");
			activeQuest.setStageWeapons(stageWeapons);
		}
		if(activeQuest.getCurrentStageNum() == activeQuest.getStageNum()-1) {
			Debug.Log("stage weapon selection over");
			activeQuest.setStage(0);
			bool validQuest = true;
			for(int i = 0; i < activeQuest.getStageNum(); i++) {
				int prevBP;
				if(i == 0) { prevBP = -1; }
				else {
					prevBP  = activeQuest.getStageBP(i-1);
				}
				
				int currBP;
				
				currBP = activeQuest.getStageBP(i);
				if(currBP < prevBP) {
					validQuest = false;
				}	
			}
				
			if(validQuest) {
				Debug.Log("Valid quest");
				for(int i = 0; i <activeQuest.getStageNum(); i++) {
					activeQuest.getSponsor().discardCard(new Card[] {activeQuest.getStage(i)});
					activeQuest.getSponsor().discardCard(activeQuest.getStageWeapons(i));
				}
				activePlayerSub = activeQuest.getSponsorNum();
				getPlayers();
			}
			else {
				Debug.Log("Invalid quest");
				ui.displayAlert("Invalid selection. Stage's BP must be in increasing order.");
				activeQuest.resetQuest();
				ui.askForCards(
					activeQuest.getSponsor(), 
					activeQuest.getStageNum(), 
					GameState.state.ASKINGFORSTAGES, 
					"Select up to " + activeQuest.getStageNum() + " stages", 
					"Forfeit", 
					"null",
					true, 
					false, 
					false,
					false
					);
				return;
			}
			//getPlayers();
		}
		else {
			activeQuest.setStage(activeQuest.getCurrentStageNum()+1);
			activeQuest.getSponsor().discardCard(stageWeapons);
			startStageWeaponSetup();
		}
	}	
	public void getPlayers(){	
		activePlayerSub = nextPlayer(activePlayerSub);
		//activePlayerSub = activePlayerSub % (playerCount-1);
		ui.askYesOrNo(players[activePlayerSub], "Do you want to join this quest?", GameState.state.ASKINGFORPLAYERS);
	}
	public void gotPlayer(Player newPlayer){
		if(newPlayer != null)
		{
			activeQuest.addPlayer(newPlayer);
		}
		Debug.Log("current player: " + players[activePlayerSub].getName());
		Debug.Log("next player: " + players[nextPlayer(activePlayerSub)].getName());
		Debug.Log("sponser: " + activeQuest.getSponsor().getName());
		
		if(players[nextPlayer(activePlayerSub)].getName().Equals(activeQuest.getSponsor().getName()))
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
		if(activeQuest.getQuest() == null) {
			endQuest("Quest over");
		}
		else if(activeQuest.getPlayerNum() == 0) {
			endQuest("All players dead");
		}
		else{
			ui.showStage(activeQuest);
			ui.askForCards(
							activeQuest.getCurrentPlayer(), 
							activeQuest.getCurrentPlayer().getHand().Length, 
							GameState.state.ASKINGFORCARDSINQUEST, 
							"Select cards to play, then press FIGHT", 
							"FIGHT",
							"Give up", 
							false, 
							true, 
							true,
							true);
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
		if(selection != null) {
			if(selection.Length > 0) {
				for(int i = 0; i < selection.Length; i++) {
					extraBP = extraBP + selection[i].getBP();
				}
			}
		}
		if(activeQuest.getStageBP(activeQuest.getCurrentStageNum()) <= activeQuest.getCurrentPlayer().getBP() + extraBP) {
			if(selection != null) {
				if(selection.Length > 0) {
					activeQuest.getCurrentPlayer().discardCard(selection);
				}
			}
			activeQuest.nextPlayer();
			startStage();			
		}
		else
		{
			ui.displayAlert("Too weak to defeat foe!");
			ui.askForCards(
							activeQuest.getCurrentPlayer(), 
							activeQuest.getCurrentPlayer().getHand().Length, 
							GameState.state.ASKINGFORCARDSINQUEST, 
							"Select cards to play, then press FIGHT", 
							"FIGHT",
							"Give up", 
							false, 
							true, 
							true,
							true
							);
		}
	}
	
	public void forfeitQuest() {
		//Debug.Log("Quest Attack");
		activeQuest.deletePlayer(activeQuest.getCurrentPlayer());
		startStage();
	}
	
	private int nextPlayer(int activePlayer)
	{
		int temp = activePlayer;
		temp ++;
		if(temp == playerCount)
		{
			temp = 0;
		}
		return temp;
	}
}
