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
//	public void drawAdvCard(){
//		Debug.Log("before: " + activePlayerMeta);
//		activePlayerMeta = nextPlayer(activePlayerMeta);
//		Debug.Log("after: " + activePlayerMeta);
//		Card drawnCard = advDeck.drawCard();
//	}

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
			ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);	
			activePlayerSub = nextPlayer(activePlayerSub);
			
		}
	}
	public void startQuestSetup(){
		activeQuest.setSponsor(players[activePlayerSub]);
		//ui.askForStageSelection(activeQuest.getSponsor(), activeQuest.getStageNum());
		ui.askForCards(
			activeQuest.getSponsor(), 
			GameState.state.ASKINGFORSTAGES, 
			"Select up to " + activeQuest.getStageNum() + " stages", 
			"null",
			"Forfeit", 
			true, 
			false, 
			false,
			false,
			true,
			activeQuest.getStageNum()
			);
	}	
	public void endQuestSetup(Card[] stages){
		Debug.Log("endQuestSetup");
		activeQuest.setStages(stages);
		gameState = state.GOTINPUT;
		for(int i = 0; i < activeQuest.getStageNum(); i ++) {
			
			if(Object.ReferenceEquals (activeQuest.getStage(i).GetType (), typeof(Foe))) {
				activeQuest.setStage(i);
				break;
			}
		}
		
		startStageWeaponSetup();
	}
	
	public void startStageWeaponSetup(){
		ui.askForCards(
			activeQuest.getSponsor(), 
			GameState.state.ASKINGFORSTAGEWEAPONS, 
			"Select weapons to enhance this stage", 
			"Done", 
			"null",
			false, 
			true, 
			false,
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
			int prevBP = -1;
			for(int i = 0; i < activeQuest.getStageNum(); i++) {
				if(Object.ReferenceEquals (activeQuest.getStage(activeQuest.getCurrentStageNum()).GetType (), typeof(Foe))) {  
					int currBP;
					currBP = activeQuest.getStageBP(i);
					if(currBP < prevBP) {
						validQuest = false;
					}	
					prevBP  = activeQuest.getStageBP(i);
				}
			}
				
			if(validQuest) {
				Debug.Log("Valid quest");
				for(int i = 0; i <activeQuest.getStageNum(); i++) {
					activeQuest.getSponsor().discardCard(new Card[] {activeQuest.getStage(i)});
					activeQuest.getSponsor().discardCard(activeQuest.getStageWeapons(i));
				}
				activeQuest.setStage(0);
				activePlayerSub = activePlayerMeta;
				getPlayers();
			}
			else {
				Debug.Log("Invalid quest");
				ui.displayAlert("Invalid selection. Stage's BP must be in increasing order.");
				activeQuest.resetQuest();
				ui.askForCards(
					activeQuest.getSponsor(),  
					GameState.state.ASKINGFORSTAGES, 
					"Select up to " + activeQuest.getStageNum() + " stages", 
					"Forfeit", 
					"null",
					true, 
					false, 
					false,
					false,
					true,
					activeQuest.getStageNum()
					);
				return;
			}
			getPlayers();
		}
		else {
			for(int i = activeQuest.getCurrentStageNum()+1; i < activeQuest.getStageNum(); i ++) {
			if(Object.ReferenceEquals (activeQuest.getStage(i).GetType (), typeof(Foe))) {
				activeQuest.setStage(i);
				break;
			}
		}
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
		if(players[activePlayerSub] == activeQuest.getSponsor())
		{
			Debug.Log("Done getting players");
			startQuest();
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
	
	public void startQuest() {
		Debug.Log("starting quest");
		activeQuest.setStage(0);
		Debug.Log(activeQuest.getCurrentPlayer().getName());
		startStage();
		
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
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Foe))) {
				Debug.Log(activeQuest.getCurrentStageNum());
				ui.askForCards(
								activeQuest.getCurrentPlayer(), 
								GameState.state.ASKINGFORCARDSINQUEST, 
								"Select cards to play, then press FIGHT", 
								"FIGHT",
								"Give up", 
								false, 
								true, 
								true,
								true,
								false);
			}
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Test))) {			
				ui.askForCards(
								activeQuest.getCurrentPlayer(),  
								GameState.state.ASKINGFORCARDSINBID, 
								"Select cards to bit, then press BID", 
								"BID",
								"Give up", 
								false, 
								true, 
								true,
								true,
								false);
			}
		}
		return;
	}
	public void endQuest(string text = "Quest over") {
			Debug.Log(text);
			activeQuest = null;
			ui.endQuest();
			ui.drawingQuestCard();
			drawQuestCard();
	}
	public void bidPhase(Card [] selection) {		
		if(activeQuest.placeBid(selection, 0)) {
			activeQuest.setTentativeBet(selection);
			activeQuest.nextPlayer();
			startStage();			
		}
		else {
			ui.displayAlert("Bid too low. Bid more cards of forfeit the quest.");
			ui.askForCards(
							activeQuest.getCurrentPlayer(), 
							GameState.state.ASKINGFORCARDSINBID, 
							"Select cards to bit, then press BID", 
							"BID",
							"Give up", 
							false, 
							true, 
							true,
							true,
							false);
		}
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
							GameState.state.ASKINGFORCARDSINQUEST, 
							"Select cards to play, then press FIGHT", 
							"FIGHT",
							"Give up", 
							false, 
							true, 
							true,
							true,
							false
							);
		}
	}

	public void createTourney(TourneyCard tourneyCard){
		getPlayers ();

		switch (tourneyCard.getName ()) {

		case "camelot":
			break;

		case "orkney":
			break;

		case "tintagel":
			break;

		case "york":
			break;

		}
			
	}
	
	public void forfeitQuest() {
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
