using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : MonoBehaviour {
	//REMOVE

	//Initialize logging functionality
	Logger log = new Logger("GameManager");
	CardLibrary lib = new CardLibrary();
	//Initialize the two decks
	//DiscardDeck advDiscard = new DiscardDeck();
	//DiscardDeck storyDiscard = new DiscardDeck();
	AdvDeck advDeck;
	StoryDeck storyDeck;
	NetworkedUI ui;
	Client testClient;
	Server testServer;

	TestRiggingFunctions rigging;
	HelperFunctions hp;
	/*
	0 = regular play
	1 = prof given scenario 1
	2 = prof given scenario 2
	3 = testing tourneys
	4 =
	5 =
	6 = testing kings call
	7 = testing AI strategy 2 setting up quest
	8 = testing AI strategy 2 playing in quest
	9 = testing AI strategy 2 playing in tourney
	10 = testing AI strategy 1 setting up quest
	11 = testing AI strategy 1 playing in quest
	12 = testing AI strategy 1 playing in tourney
	13 = testing pox
	14 = testing plague
	15 = testing recognition
	16 = testing queen's favor
	17 = testing court called to camelot
	18 = testing kings call to arms
	*/
	int testingScenario = 1;
	int playerCount = 4;

	int aiStrat=2;
	Player[] players;
	
	//Game states. There will eventually be many possible states, but for right now these two exist.
	state userInputState = state.STANDBY;
	state gameState = state.STANDBY;
	int counter = 0;
	
	public int activePlayerMeta;
	public int activePlayerSub;
	public int activePlayerOther; 
	
	int testScenarioStep = 1;
	
	ActiveQuest activeQuest;
	ActiveTourney tourney;
	bool cyclingThroughPlayers;
	
	Card[] tempCardSelection;
	Player[] tempPlayerArr;

	bool recognitionActive = false;
	// Use this for initialization
	public GameManager(NetworkedUI nUI) {
		//Init the decks
		advDeck = new AdvDeck();
		storyDeck = new StoryDeck();
		rigging = new TestRiggingFunctions();
		advDeck.initDeck();
		storyDeck.initDeck();
		log.log ("decks initialized");
		//testingScenario = PlayerPrefs.GetInt("testScenario");
		//playerCount = PlayerPrefs.GetInt("humanPlayerNum")  + 1 ;
		//aiStrat=PlayerPrefs.GetInt("aiStrategy");

		hp = new HelperFunctions();
		playerCount = rigging.getRiggedPlayerCount(testingScenario);
		players = new Player[playerCount];
		for(int i = 0; i < playerCount; i++){
			if(testingScenario == 12 || testingScenario == 13 || testingScenario == 14){
				players[i] = new Player(new Card[12], 4, 0, "Player " + (i), i+1);
			}
			else {
				players[i] = new Player(new Card[12], 0, 0, "Player " + (i), i+1);
			}
		}

		if(testingScenario == 18){
			players[0] = new Player(new Card[12], 4, 0, "Player 1", 1);
		}
		dealHands(playerCount);
		if(rigging.getRiggedAiStratagy(testingScenario) != -1){
			if(rigging.getRiggedAiStratagy(testingScenario) == 1){
				players[rigging.getRiggedAiIndex(testingScenario)].assumingDirectControl(new Strategy1AI(players[rigging.getRiggedAiIndex(testingScenario)]));
			}
			if(rigging.getRiggedAiStratagy(testingScenario) == 2){
				players[rigging.getRiggedAiIndex(testingScenario)].assumingDirectControl(new Strategy2AI(players[rigging.getRiggedAiIndex(testingScenario)]));
			}
		}

		log.Init ();
		log.log ("Testing scenario is " + testingScenario);
		log.log ("There are " + PlayerPrefs.GetInt ("aiPlayerNum") + " AI players.");
		log.log ("There are " + PlayerPrefs.GetInt ("humanPlayerNum") + " Human players.");
		log.log ("Current test scenario is " + PlayerPrefs.GetInt ("testScenario"));



		ui = nUI;
		log.log ("Created UI");
		//Create all the players and add it to the players array

		log.log ("Created player array");


		log.log ("Dealt cards");

	}
	public void gameStart(){
		activePlayerMeta = -1;

		log.log ("Dealing hands, drawing first quest");
		activePlayerSub = activePlayerMeta;
		drawQuestCard();

		ui.updatePlayers (players);
		//activePlayerSub = activePlayerMeta;
	}
	public void drawQuestCard(){
		gameState = state.DRAWINGSTORYCARD;
		if(activePlayerMeta == -1){
			activePlayerMeta = 0;
		}
		else {
			activePlayerMeta = nextPlayer(activePlayerMeta);
		}
		Card drawnCard = rigging.drawRiggedCard(testingScenario, storyDeck, testScenarioStep);
		testScenarioStep ++;
		ui.showCard(drawnCard);
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
		//log.log("Drew a " + storyCard.getName());
		counter = 0;

		switch (storyCard.getType()) {

		case "quest":
			if(recognitionActive){
				activeQuest = new ActiveQuest((QuestCard)storyCard, 2);
				recognitionActive = false;
			}
			else{
				activeQuest = new ActiveQuest((QuestCard)storyCard, 0);
			}
			activePlayerSub = activePlayerMeta;
			cyclingThroughPlayers = false;
			userInputState = state.ASKINGFORSPONSORS;

			getSponsor();

			break;

		case "tourney":

			activePlayerSub = activePlayerMeta;
			userInputState = state.ASKINGFORPLAYERSTOURNEY;
			createTourney (storyCard);
			break;

		case "event":
			//Event handling. Pretty much done because events are handled in the cards themselves.
			storyCard.runEvent (players, activePlayerMeta, players.Length, advDeck, this);
			break;
		default:
			drawQuestCard ();
			break;
		}
	}

	public void getSponsor(){	
		if(counter == players.Length && cyclingThroughPlayers == true)
		{
			log.log ("Sponsor not found");
			storyDeck.discardCard(new Card[]{activeQuest.getQuest()});
			activeQuest = null;
			drawQuestCard();
		}
		else
		{
			if(activePlayerSub == activePlayerMeta && cyclingThroughPlayers == false) {
				cyclingThroughPlayers = true;
				counter = 1;
			}
			else{
				activePlayerSub = nextPlayer(activePlayerSub);
				counter ++;
			}
			ui.showCard(activeQuest.getQuest());

			log.log ("Getting sponsor");
			if(players[activePlayerSub].isHuman()){
				ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);	
			}
			else{
				if(players[activePlayerSub].getAI().doISponsorAQuest(players, (QuestCard)activeQuest.getQuest())){
					startQuestSetup();
				}
				else {
					getSponsor();
				}
			}


		}
	}

	public void startQuestSetup(){
		Debug.Log("setting player " + activePlayerSub + " as sponsor");
		ui.displayAlert ("Player "+activePlayerSub+ " has chosen to sponsor.", true);
		//activePlayerSub = activePlayerMeta;
		activeQuest.setSponsor(players[activePlayerSub]);
		//ui.askForStageSelection(activeQuest.getSponsor(), activeQuest.getStageNum());
		if(activeQuest.getSponsor().isHuman()){
			ui.askForCards(
				activeQuest.getSponsor(), 
				activeQuest,
				GameState.state.ASKINGFORSTAGES, 
				getUserInputState(),
				"Select up to " + activeQuest.getStageNum() + " stages", 
				"null",
				"Forfeit", 
				true, 
				false, 
				false,
				false,
				true,
				true,
				activeQuest.getStageNum()
			);
		}
		else{
			activeQuest.getSponsor().getAI().sponsorQuestSetup(activeQuest);
			counter = 1;
			getPlayers();
		}
	}

	public void endQuestSetup(Card[] stages){
		log.log("Quest setup over");
		activeQuest.setStages(stages);

		for(int i = 0; i < activeQuest.getStageNum(); i ++) {
			if(Object.ReferenceEquals (activeQuest.getStage(i).GetType (), typeof(Foe))) {
				activeQuest.setStage(i);
				break;
			}
		}
		startStageWeaponSetup();
	}
	
	public void startStageWeaponSetup(){
		if(Object.ReferenceEquals (activeQuest.getStage(activeQuest.getCurrentStageNum()).GetType (), typeof(Test))) {
			if(activeQuest.getCurrentStageNum() == activeQuest.getStageNum()) {
				endStageWeaponSetup(null);
			}
			else {
				//activeQuest.setStage(getCurrentStageNum+1);
				endStageWeaponSetup(null);
				return;
			}
		}
		else {
			userInputState = state.ASKINGFORSTAGEWEAPONS;
			ui.askForCards(
				activeQuest.getSponsor(), 
				activeQuest,
				GameState.state.ASKINGFORSTAGEWEAPONS, 
				getUserInputState(),
				"Select weapons to enhance this stage", 
				"Done", 
				"null",
				false, 
				true, 
				false,
				false,
				false,
				false
			);
			ui.showCard(activeQuest.getCurrentStage());
		}
	}

	public void endStageWeaponSetup(Card[] stageWeapons){

		string errorMessage = " ";
		if(stageWeapons == null) {
			activeQuest.setStageWeapons(new Card[] {null});
		}
		else {
			activeQuest.setStageWeapons(stageWeapons);
		}
		if(activeQuest.getCurrentStageNum() == activeQuest.getStageNum()-1) {
			activeQuest.setStage(0);
			bool validQuest = true;
			int prevBP = -1;
			for(int i = 0; i < activeQuest.getStageNum(); i++) {
				if(Object.ReferenceEquals (activeQuest.getStage(i).GetType(), typeof(Foe))) {  
					int currBP;
					currBP = activeQuest.getStageBP(i);
					if(currBP < prevBP) {
						validQuest = false;
						errorMessage = "Invalid selection. Stage's BP must be in increasing order.";
					}	
					prevBP  = activeQuest.getStageBP(i);
				}
			}
			int numberOfTests = 0;
			for(int i = 0; i < activeQuest.getStageNum(); i++) {
				if(Object.ReferenceEquals (activeQuest.getStage(i).GetType (), typeof(Test))) {  
					numberOfTests++;
				}
			}
			if(numberOfTests > 1) { 
				validQuest = false;
				if(errorMessage.Equals(" ")) {
					errorMessage = "Invalid selection. Only one test card is allowed in a quest.";
				}
				else {
					errorMessage = "Invalid selection. Stage's BP must be in increasing order, and only one test card is permitted in a quest.";
				}
			}

			if(validQuest) {
				for(int i = 0; i <activeQuest.getStageNum(); i++) {
					advDeck.discardCard(new Card[] {activeQuest.getStage(i)});
					advDeck.discardCard(activeQuest.getStageWeapons(i));
					activeQuest.getSponsor().discardCard(new Card[] {activeQuest.getStage(i)});
					activeQuest.getSponsor().discardCard(activeQuest.getStageWeapons(i));
				}
				activeQuest.setStage(0);
				counter = 1;
				getPlayers();
			}
			else {
				ui.displayAlert(errorMessage);
				activeQuest.resetQuest();
				ui.askForCards(
					activeQuest.getSponsor(),  
					activeQuest,
					GameState.state.ASKINGFORSTAGES, 
					getUserInputState(),
					"Select up to " + activeQuest.getStageNum() + " stages", 
					"Forfeit", 
					"null",
					true, 
					false, 
					false,
					false,
					true,
					false,
					activeQuest.getStageNum()
				);
				return;
			}
			//counter = 1;
			//activePlayerSub = activePlayerMeta;

			//getPlayers();
		}
		else {
			advDeck.discardCard(stageWeapons);
			activeQuest.getSponsor().discardCard(stageWeapons);
			activeQuest.setStage(activeQuest.getCurrentStageNum()+1);
			startStageWeaponSetup();
		}
	}

	public void getPlayers(){	
		activePlayerSub = nextPlayer(activePlayerSub);
		log.log("Asking " + players[activePlayerSub].getName() + " if they want to join the quest");
		if(players[activePlayerSub].isHuman()){
			ui.askYesOrNo(players[activePlayerSub], "Do you want to join this quest?", GameState.state.ASKINGFORPLAYERS);
		}
		else if(players[activePlayerSub].getAI().doIParticipateInQuest((QuestCard)activeQuest.getQuest())){
			gotPlayer(players[activePlayerSub]);
		}
		else {
			gotPlayer(null);
		}
	}

	public void getPlayersTourney(){	
		Debug.Log("Asking " + players[activePlayerSub].getName() + " if they want to join the tournament");
		log.log("Asking " + players[activePlayerSub].getName() + " if they want to join the tournament");

		userInputState = state.ASKINGFORPLAYERSTOURNEY;
		if(players[activePlayerSub].isHuman()){
			ui.askYesOrNo(players[activePlayerSub], "Do you want to join this tournament?", GameState.state.ASKINGFORPLAYERSTOURNEY);
			Debug.Log("After: " + activePlayerSub);
		}
		else if(players[activePlayerSub].getAI().doIParticipateInTournament(players[activePlayerMeta], tourney, players)){
			gotPlayerTourney(players[activePlayerSub]);

		}
		else {
			gotPlayerTourney(null);

		}
	}

	public void gotPlayer(Player newPlayer){
		counter ++;

		if(newPlayer != null) {
			log.log(newPlayer.getName() + " joined quest.");
			ui.displayAlert (newPlayer.getName () + " joined quest.", true);
			activeQuest.addPlayer(newPlayer);
		}
		if(counter == players.Length)
		{
			log.log("Done looking for players.");
			startQuest();
			counter = 0;
		}
		else
		{
			getPlayers();
		}
	}

	public void gotPlayerTourney(Player newPlayer){
		Debug.Log("gotPlayerTourney");
		counter ++;
		Debug.Log("After: " + activePlayerSub);
		if(newPlayer != null) {
			tourney.addPlayer(newPlayer);
			log.log(newPlayer.getName() + " has join the tournament");
		}


		if(counter == players.Length)
		{
			log.log("Done looking for tournament players.");
			startTourney();
			counter = 0;
		}
		else {
			Debug.Log("Before: " + activePlayerSub);
			activePlayerSub = nextPlayer(activePlayerSub);

			getPlayersTourney();
		}

	}

	/*Gets a selected card and does something with it
	*/
	//Pass in a player count, it will give each player a hand of 12 adventure cards
	private void dealHands(int playerCount){

		Card[][] riggedHands = rigging.dealRiggedHand(testingScenario, players, advDeck);
		for(int i = 0; i < riggedHands.Length; i++){
			players[i].setHand(riggedHands[i]);
		}

		return;
	}
	
	public void startQuest() {
		Debug.Log("startQuest");
		if(activeQuest.getPlayerNum() == 0){
			storyDeck.discardCard(new Card[]{activeQuest.getQuest()});
			activeQuest = null;
			drawQuestCard();
			return;
		}
		gameState = state.QUESTSTARTING;
		drawXNumberOfCards(1);
		if(userInputState != state.ASKINGFORCARDSTODISCARD){
			log.log("Starting quest " + activeQuest.getQuest().getName());
			gameState = state.QUESTINPROGRESS;
			activeQuest.setQuestAsInProgress();
			startStage();		
		}
	}

	public void startTourney(){
		if (tourney.getPlayerNum () == 0) {
			endTourney ();
			return;
		}
		drawXNumberOfCardsTourney (1);
		if(tourney.getCurrentPlayer ().isHuman()){
			ui.askForCards (
				tourney.getCurrentPlayer (),
				activeQuest,
				state.ASKINGFORCARDSINTOURNEY,
				getUserInputState(),
				"Select Ally, Weapon or Amour cards to play",
				"ENTER TOURNAMENT!",
				"null",
				false,
				true,
				true,
				true,
				false,
				true);
			//Ask players for cards
		}
		else {
			gotTournamentCards(tourney.getCurrentPlayer().getAI().playTournament(tourney));
		}
		return;
	}

	public void gotTournamentCards(Card[] selection){
		if(containsMordred(selection) != null) {
			askForMordredTarget(selection, containsMordred(selection));
			return;
		}	
		int totalBP =0;
		string cardsBeingPlayed = tourney.getCurrentPlayer().getName() + " has selected ";
		if(selection != null) {
			for(int i = 0; i < selection.Length; i++) {
				cardsBeingPlayed =  cardsBeingPlayed + ", " + selection[i].getName() + " ("+ selection[i].getBP()+")";
				totalBP = totalBP + selection[i].getBP();
			}
		}
		log.log(cardsBeingPlayed);
		Debug.Log(cardsBeingPlayed);
		totalBP += tourney.getCurrentPlayer().getBP("null");

		tourney.setPlayerBP(totalBP);

		if (tourney.getPlayerInt(tourney.getCurrentPlayer()) == tourney.getPlayerNum ()-1) {
			endTourney ();
		} else {
			tourney.nextPlayer ();
			startTourney ();
		}
	}

	public void startStage() {
		Debug.Log("startStage");
		Debug.Log("current player: " + activeQuest.getCurrentPlayer().getName());
		if(activeQuest.getQuest() == null) {
			endQuest("Quest over");
			return;
		}
		else if(activeQuest.getPlayerNum() == 0) {
			endQuest("All players dead");
			return;
		}
		else if(activeQuest.getCurrentPlayer().isHuman()){
			Debug.Log(activeQuest.getCurrentStage());
			ui.showStage(activeQuest);
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Foe))) {
				//log.log(activeQuest.getCurrentPlayer().getName() + " is now facing a foe of type " + activeQuest.getCurrentStage().getName() + " enhanced with " + activeQuest.getStageWeaponString());
				Debug.Log(activeQuest.getCurrentPlayer().getName());
				ui.askForCards(
					activeQuest.getCurrentPlayer(), 
					activeQuest,
					GameState.state.ASKINGFORCARDSINQUEST, 
					getUserInputState(),
					"Select cards to play, then press FIGHT", 
					"FIGHT",
					"Give up", 
					false, 
					true, 
					true,
					true,
					false,
					true);
			}
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Test))) {
				log.log(activeQuest.getCurrentPlayer().getName() + " is now bidding in the " + activeQuest.getCurrentStage().getName() + " test");	
				Debug.Log(activeQuest.getCurrentPlayer().getName() + " is now bidding in the " + activeQuest.getCurrentStage().getName() + " test");				
				ui.askForCards(
					activeQuest.getCurrentPlayer(),
					activeQuest,								
					GameState.state.ASKINGFORCARDSINBID, 
					getUserInputState(),
					"Select cards to bit, then press BID", 
					"BID",
					"Give up", 
					true, 
					true, 
					true,
					true,
					true,
					true);
			}
		}
		else {
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Foe))){
				questAttack(activeQuest.getCurrentPlayer().getAI().playQuestStage(activeQuest));
			}
			else if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Test))) {
				bidPhase(activeQuest.getCurrentPlayer().getAI().nextBid(activeQuest));
			}
		}
		return;
	}

	public void endQuest(string text = "Quest over") {
		Debug.Log("endQuest");
		gameState = state.QUESTWRAPUP;

		drawXNumberOfCards(activeQuest.getTotalCardsUsed(), activeQuest.getSponsor());

		if(userInputState != state.ASKINGFORCARDSTODISCARD) {
			Debug.Log (userInputState);
			log.log("Quest is over. Players will be awarded " + activeQuest.getStageNum() + " shields");
			storyDeck.discardCard(new Card[]{activeQuest.getQuest()});
			ui.endQuest();
			ui.drawingQuestCard();
			activeQuest.finishQuest();
			activePlayerMeta = nextPlayer(activePlayerMeta);
			drawQuestCard();

		}
	}

	public void endTourney(){
		gameState = state.TOURNEYWRAPUP;
		if(tourney.getPlayerNum() == 0) {
			drawQuestCard(); 
			return;
		}
		tourney.awardShields();
		log.log(tourney.getWinner().getName() + " won the tournament and is awarded " + tourney.getAwardNum() + " shields");
		ui.displayAlert(tourney.getWinner().getName() + " won the tournament and is awarded " + tourney.getAwardNum() + " shields", true);
		tourney = null;
		drawQuestCard();
	}

	public void bidPhase(Card [] selection) {	
		Debug.Log("bidPhase");
		if(containsMordred(selection) != null) {
			askForMordredTarget(selection, containsMordred(selection));
			return;
		}	

		log.log("Free bids: " + activeQuest.getCurrentPlayerFreeBids());
		if(selection == null) {
			log.log("placing a bid of: 0");
			Debug.Log("placing a bid of: 0");
		}
		else {
			log.log("placing a bid of: "  + selection.Length);
			Debug.Log("placing a bid of: "  + selection.Length);
		}
		activeQuest.placeBid(selection, activeQuest.getCurrentPlayerFreeBids());
		activeQuest.setTentativeBet(selection);
		if(activeQuest.isStageDone()) {
			log.log("Stage is over.");
			endStage();
		}
		else {
			activeQuest.nextPlayer();
			log.log("Moving onto " + activeQuest.getCurrentPlayer().getName());
			startStage();
		}
		/*			
		}
		else {
			log.log("Bid is too low.");
			ui.displayAlert("Bid too low. Bid more cards of forfeit the quest.");
			if(	activeQuest.getCurrentPlayer().isHuman())
			{
				ui.askForCards(
								activeQuest.getCurrentPlayer(), 
								activeQuest,
								GameState.state.ASKINGFORCARDSINBID, 
								"Select cards to bid, then press BID", 
								"BID",
								"Give up", 
								true, 
								true, 
								true,
								true,
								true,
								true);
								
								
			}
			else {
				forfeitQuest();
			}
		}
		*/

	}

	public void askForCardLimitReached(Player player, int cardsToDeleteNum, bool getFoes = true, bool getWeap = true, bool getAlly = true, bool getAmour = true, bool getTest = true) {
		Debug.Log("askForCardLimitReached");
		log.log(player.getName() + "'s card limit reached. Asking to discard " + cardsToDeleteNum + " cards.");
		ui.askForCards(
			player, 
			activeQuest,
			GameState.state.ASKINGFORCARDSTODISCARD, 
			getUserInputState(),
			"Card limit reached. Please select "+ cardsToDeleteNum + " cards to discard.", 
			"null",
			"null", 
			getFoes, 
			getWeap, 
			getAlly,
			getAmour,
			getTest,
			false,
			cardsToDeleteNum);

		return;

	}

	public void gotCardLimitReached(Card [] cards) {
		Debug.Log("gotCardLimitReached");
		if(gameState == state.PROSPERITY){
			//players[activePlayerOther].discardCard(cards);
			activePlayerMeta = nextPlayer(activePlayerMeta);
			for(int i = 0; i < cards.Length; i++){
				if(Object.ReferenceEquals(cards[i].GetType(), typeof(Amour))){
					Debug.Log("Setting amour to inPlay");
					players[activePlayerOther].addCard(new Card[]{cards[i]}, true);
				}

				players[activePlayerOther].discardCard(new Card[]{cards[i]});

			}
			advDeck.discardCard(cards);
			drawXGeneralNumberOfCards(2, state.PROSPERITY, players);
		}
		if(gameState == state.QUEENSFAVOR){
			//players[activePlayerOther].discardCard(cards);
			for(int i = 0; i < cards.Length; i++){
				if(Object.ReferenceEquals(cards[i].GetType(), typeof(Amour))){
					Debug.Log("Setting amour to inPlay");
					players[activePlayerOther].addCard(new Card[]{cards[i]}, true);
				}

				players[activePlayerOther].discardCard(new Card[]{cards[i]});
			}
			advDeck.discardCard(cards);
			drawXGeneralNumberOfCards(2, state.PROSPERITY, tempPlayerArr);
		}
		else if(gameState == state.KINGSCALL){
			Player[] nextPlayers;
			if(tempPlayerArr.Length-1 > 0){
				nextPlayers = new Player[tempPlayerArr.Length-1];
				for(int i = 0; i < nextPlayers.Length; i++){
					nextPlayers[i] = tempPlayerArr[i+1];
				}
			}
			else{
				nextPlayers = null;
			}
			kingsCall(nextPlayers);
		}
		else {
			advDeck.discardCard(cards);
			if(activePlayerOther == -1){
				//activeQuest.getSponsor().discardCard(cards);
				for(int i = 0; i < cards.Length; i++){
					if(Object.ReferenceEquals(cards[i].GetType(), typeof(Amour))){
						Debug.Log("Setting amour to inPlay");
						activeQuest.getSponsor().addCard(new Card[]{cards[i]}, true);
					}

					activeQuest.getSponsor().discardCard(new Card[]{cards[i]});
				}
			}
			else {
				//activeQuest.getPlayer(activePlayerOther).discardCard(cards);
				for(int i = 0; i < cards.Length; i++){
					if(Object.ReferenceEquals(cards[i].GetType(), typeof(Amour))){
						Debug.Log("Setting amour to inPlay");
						activeQuest.getPlayer(activePlayerOther).addCard(new Card[]{cards[i]}, true);
					}

					activeQuest.getPlayer(activePlayerOther).discardCard(new Card[]{cards[i]});

				}
			}
			userInputState = state.STANDBY;
			if(gameState == state.QUESTSTARTING){
				startQuest();
			}
			else if(gameState == state.QUESTINPROGRESS){
				endStage();
			}
			else if(gameState == state.QUESTWRAPUP){
				endQuest();
			}
		}
	}
	
	public Card containsMordred(Card[] selection) {
		if(selection == null) {
			return null;
		}
		for(int i = 0; i < selection.Length; i++) {
			if(selection[i].getName().Equals("mordred")) {return selection[i];}
		}
		return null;
	}

	private void askForMordredTarget(Card[] selection, Card mordredCard){
		advDeck.discardCard(new Card[]{mordredCard});
		tempCardSelection = hp.removeCard(selection, mordredCard);
		if(activeQuest != null){
			ui.askForPlayerChoice(activeQuest.getCurrentPlayer(), state.ASKINGFORMORDREDTARGET, "Select player you wish to remove an ally from", hp.removePlayers(activeQuest.getPlayerArr(), activeQuest.getPlayerInt(activeQuest.getCurrentPlayer()))); 	
			//Player current = activeQuest.getCurrentPlayer();
		}
		else if(tourney != null) {
			ui.askForPlayerChoice(tourney.getCurrentPlayer(), state.ASKINGFORMORDREDTARGET, "Select player you wish to remove an ally from", hp.removePlayers(tourney.getPlayerArr(), tourney.getPlayerInt(tourney.getCurrentPlayer()))); 
		}
	}

	public void gotMordredTarget(string target) {
		bool mordredResult = false;
		if(activeQuest != null){
			mordredResult =  activeQuest.mordredSpecialAbility(activeQuest.findPlayer(target));
			ui.showStage(activeQuest);
		}
		else if(tourney != null) {
			mordredResult =  tourney.mordredSpecialAbility(tourney.findPlayer(target));
		}
		
		if(!mordredResult){
			ui.displayAlert("This player has no allies in play! Mordred discarded");
		}
		Card [] temp = tempCardSelection;
		tempCardSelection = null;
		if(activeQuest != null){
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Test))){
				bidPhase(temp);
			}
			else {
				questAttack(temp);
			}
		}
		else if(tourney != null){
			gotTournamentCards(temp);
		}
	}

	public void questAttack(Card [] selection) {
		if(containsMordred(selection) != null) {
			askForMordredTarget(selection, containsMordred(selection));
			return;
		}
		log.log(activeQuest.getCurrentPlayer().getName() + " is attempting to over come the foe");
		Card[] toDispose;
		Card[] toKeepInPlay;
		
		int extraBP = 0;
		toDispose = null;
		toKeepInPlay = null;
		if(selection != null) {
			toDispose = null;
			toKeepInPlay = null;
			string cardsBeingPlayed = activeQuest.getCurrentPlayer().getName() + " has selected ";
			if(selection.Length > 0) {
				for(int i = 0; i < selection.Length; i++) {
					cardsBeingPlayed =  cardsBeingPlayed + ", " + selection[i].getName() + " ("+ selection[i].getBP()+")";
					extraBP = extraBP + selection[i].getBP();
				}
			}
		
			log.log(cardsBeingPlayed);
		
			for(int i = 0; i< selection.Length; i++){
				if(Object.ReferenceEquals(selection[i].GetType(), typeof(Ally)) || Object.ReferenceEquals(selection[i].GetType(), typeof(Amour))) {
					if(toKeepInPlay == null) { 
						toKeepInPlay = new Card[] {selection[i]};
					}
					else {
						Card[] temp = new Card[toKeepInPlay.Length+1];
						for(int j = 0; j <toKeepInPlay.Length; j++){
							temp[j] = toKeepInPlay[j];
						}
						temp[toKeepInPlay.Length] = selection[i];
						toKeepInPlay = temp;
					}
				}
				else {
					if(toDispose == null) { 
						toDispose = new Card[] {selection[i]};
					}
					else {
						Card[] temp = new Card[toDispose.Length+1];
						for(int j = 0; j <toDispose.Length; j++){
							temp[j] = toDispose[j];
						}
						temp[toDispose.Length] = selection[i];
						toDispose = temp;
					}
				}
			}
		
		}
		
		if(activeQuest.getStageBP(activeQuest.getCurrentStageNum()) <= activeQuest.getCurrentPlayer().getBP(activeQuest.getQuest().getName()) + extraBP) {
			log.log("With a total BP of " + (activeQuest.getCurrentPlayer().getBP(activeQuest.getQuest().getName()) + extraBP) + " " + activeQuest.getCurrentPlayer().getName() + " overcame " + activeQuest.getCurrentStage().getName());
			if(toDispose != null) {
				if(toDispose.Length > 0) {
					activeQuest.getCurrentPlayer().discardCard(toDispose);
					advDeck.discardCard(toDispose);
				}
			}
			
			activeQuest.getCurrentPlayer().addCard(toKeepInPlay, true);
			if(toKeepInPlay != null) {
				activeQuest.getCurrentPlayer().discardCard(toKeepInPlay);
			}
			if(activeQuest.getPlayerInt(activeQuest.getCurrentPlayer()) == activeQuest.getPlayerNum()-1) {
				userInputState = state.SHOWINGFOE;
				ui.foeReveal(activeQuest);
			}
			else {
				activeQuest.nextPlayer();
				startStage();
			}
						
		}
		else
		{
			log.log("With a total BP of " + (activeQuest.getCurrentPlayer().getBP(activeQuest.getQuest().getName()) + extraBP) + " " + activeQuest.getCurrentPlayer().getName() + " fell to " + activeQuest.getCurrentStage().getName());
			
			if (activeQuest.getPlayerInt (activeQuest.getCurrentPlayer ()) == activeQuest.getPlayerNum () - 1) {
				
				userInputState = state.SHOWINGFOE;
				forfeitQuest ();
				ui.foeReveal(activeQuest);
			}
			else {
				forfeitQuest();
				startStage();
			}
			
		}
	}

	public void endStage() {
		Debug.Log("endStage");
		log.log("Stage is over.");
		if(userInputState != state.ASKINGFORCARDSTODISCARD) {
			//activeQuest.endBidding();
		}
		drawXNumberOfCards(1);
		if(activeQuest.getPlayerNum() == 0) {
			endQuest("All players dead");
			return;
		}
		if(userInputState != state.ASKINGFORCARDSTODISCARD) {
			if(activeQuest.getCurrentStageNum() != activeQuest.getStageNum()){
				activeQuest.setPlayerNum(0);
				activeQuest.nextStage();
				startStage();
			}
			else {
				endQuest();
			}
		}
	}

	public void createTourney(Card tourneyCard){
		log.log("Tournament at " + tourneyCard.getName() + " has begun");
		tourney = new ActiveTourney(tourneyCard);
		ui.showCard(tourneyCard);
		activePlayerSub = activePlayerMeta;
		getPlayersTourney();
	}

	public void forfeitQuest() {
		//log.log(activeQuest.getCurrentPlayer().getName() + " has forfeited quest");
		activeQuest.deletePlayer(activeQuest.getCurrentPlayer());
		if(userInputState == state.ASKINGFORCARDSINBID){
			//activeQuest.nextPlayer();
			//startStage();
		}
		//
	}
	
	private int nextPlayer(int activePlayer) {
		int temp = activePlayer;
		temp ++;
		if(temp == playerCount) {
			temp = 0;
		}
		return temp;
	}

	public Player getCurrentPlayer() {
		if(activeQuest != null && activeQuest.isInProgress()) {
			return activeQuest.getCurrentPlayer();
		}
		else if(userInputState == state.ASKINGFORPLAYERS || userInputState == state.ASKINGFORSPONSORS || userInputState == state.ASKINGFORPLAYERSTOURNEY) {
			Debug.Log(userInputState);
			return players[activePlayerSub];
		}
		else if(userInputState == state.ASKINGFORSTAGES || userInputState == state.ASKINGFORSTAGEWEAPONS ) {
			Debug.Log(userInputState);
			return activeQuest.getSponsor();
		}
		else if(userInputState == state.ASKINGFORCARDSTODISCARD) {
			Debug.Log(userInputState);
			return activeQuest.getPlayer(activePlayerOther);
		}
		else if(userInputState == state.ASKINGFORCARDSINTOURNEY) {
			Debug.Log(userInputState);
			return tourney.getCurrentPlayer();
		}
		else {
			return players[activePlayerMeta];
		}
	}
	
	private Card[] cleanUpArray(Card [] oldArr){
		int newN = 0;
		if(oldArr == null) { return null; }
		for(int i = 0; i< oldArr.Length; i++)
		{
			if(oldArr[i] == null)
			{
				newN = i;
				break;
			}
		}
		Card [] newArr = new Card[newN];
		for(int i = 0; i < newArr.Length; i++)
		{
			newArr[i] = oldArr[i];
		}
		return newArr;

	}

	private void drawXNumberOfCards(int numOfCardsToDraw, Player player = null) {		
		if(player == null) {
			for(int i = 0 ; i< activeQuest.getPlayerNum(); i ++){
				//log.log("Drawing " + numOfCardsToDraw + " cards for " + activeQuest.getPlayer(i).getName());
				if(activeQuest.getPlayer(i).getHand().Length + numOfCardsToDraw > 12){
					userInputState = state.ASKINGFORCARDSTODISCARD;
					askForCardLimitReached(activeQuest.getPlayer(i), (activeQuest.getPlayer(i).getHand().Length + numOfCardsToDraw) - 12);
					activePlayerOther = i;
					return;
				}
			}

			for(int i = 0 ; i< activeQuest.getPlayerNum(); i ++){
				for(int j = 0; j < numOfCardsToDraw; j++) {
					activeQuest.getPlayer(i).addCard(new Card[]{advDeck.drawCard()});
				}
			}
		}
		else {
			Debug.Log ("Drawing " + numOfCardsToDraw + " cards for " + player.getName ());
			log.log("Drawing " + numOfCardsToDraw + " cards for " + player.getName());
			if(player.getHand().Length + numOfCardsToDraw > 12){
				log.log(player.getName() + "'s hand exceeds the 12 card limit. Asking to discard.");
				userInputState = state.ASKINGFORCARDSTODISCARD;
				askForCardLimitReached(player, (player.getHand().Length + numOfCardsToDraw) - 12);
				activePlayerOther = activeQuest.getPlayerInt(player);
				return;
			}		

			for(int j = 0; j < numOfCardsToDraw; j++) {
				Card newCard = advDeck.drawCard();
				log.log("Giving " + player.getName() + " a " + newCard.getName() + " card");
				player.addCard(new Card[]{newCard});
			}	
		}
	}

	public void drawXGeneralNumberOfCards(int numOfCardsToDraw, state newGameState, Player[] playersAffected){
		tempPlayerArr = playersAffected;
		gameState = newGameState;
		for(int i = 0 ; i< playersAffected.Length; i ++){
			log.log("Drawing " + numOfCardsToDraw + " cards for " + playersAffected[i].getName());
			if(playersAffected[i].getHand().Length + numOfCardsToDraw > 12){
				userInputState = state.ASKINGFORCARDSTODISCARD;
				askForCardLimitReached(playersAffected[i], (playersAffected[i].getHand().Length + numOfCardsToDraw) - 12);
				activePlayerOther = i;
				return;
			}
		}

		for(int i = 0 ; i< playersAffected.Length; i ++){
			for(int j = 0; j < numOfCardsToDraw; j++) {
				playersAffected[i].addCard(new Card[]{advDeck.drawCard()});
			}
		}

		drawQuestCard();

	}

	private void drawXNumberOfCardsTourney(int numOfCardsToDraw, Player player = null) {	

		if(player == null) {
			for(int i = 0 ; i< tourney.getPlayerNum(); i ++){
				log.log("Drawing " + numOfCardsToDraw + " cards for " + tourney.getPlayer(i).getName());
				if(tourney.getPlayer(i).getHand().Length + numOfCardsToDraw > 12){
					userInputState = state.ASKINGFORCARDSTODISCARD;
					askForCardLimitReached(tourney.getPlayer(i), (tourney.getPlayer(i).getHand().Length + numOfCardsToDraw) - 12);
					activePlayerOther = i;
					return;
				}
			}

			for(int i = 0 ; i< tourney.getPlayerNum(); i ++){
				for(int j = 0; j < numOfCardsToDraw; j++) {
					tourney.getPlayer(i).addCard(new Card[]{advDeck.drawCard()});
				}
			}
		}
		else {
			log.log("Drawing " + numOfCardsToDraw + " cards for " + player.getName());
			if(player.getHand().Length + numOfCardsToDraw > 12){
				log.log(player.getName() + "'s hand exceeds the 12 card limit. Asking to discard.");
				userInputState = state.ASKINGFORCARDSTODISCARD;
				askForCardLimitReached(player, (player.getHand().Length + numOfCardsToDraw) - 12);
				activePlayerOther = tourney.getPlayerInt(player);
				return;
			}		

			for(int j = 0; j < numOfCardsToDraw; j++) {
				Card newCard = advDeck.drawCard();
				log.log("Giving " + player.getName() + " a " + newCard.getName() + " card");
				player.addCard(new Card[]{newCard});
			}	
		}
	}

	public state getUserInputState(){
		return userInputState;
	}
	
	public void setUserInputState(state newState){
		userInputState = newState;
	}
	
	public string getOtherPlayerInfo(Player currPlayer) {
		string stringToReturn = "";
		for(int i = 0 ;i < players.Length; i++){
			if(!(currPlayer.getName().Equals(players[i].getName()))){
				stringToReturn = stringToReturn + players[i].getName() + System.Environment.NewLine + 
					"Rank: " + players[i].getRank() + System.Environment.NewLine + 
					"Shields: " + players[i].getShields() + System.Environment.NewLine + 
					"Hand size: " + players[i].getHand().Length + System.Environment.NewLine + System.Environment.NewLine;
			}
		}

		return stringToReturn;
	}

	public void kingsCall(Player [] playersToDiscard){

		tempPlayerArr = playersToDiscard;
		if(playersToDiscard == null){
			drawQuestCard();
			return;
		}
		Debug.Log("In kings call. " + playersToDiscard.Length + "players left");
		gameState = state.KINGSCALL;
		if(tempPlayerArr[0].getNumOfTypeOfCard("weapon") >= 2){
			askForCardLimitReached(tempPlayerArr[0], 2, false, true, false, false, false);
			return;
		}
		else if(tempPlayerArr[0].getNumOfTypeOfCard("foe") >= 1){
			askForCardLimitReached(tempPlayerArr[0], 1, true, false, false, false, false);
			return;
		}
		else{
			Player[] nextPlayers;
			if(tempPlayerArr.Length-1 > 0){
				nextPlayers = new Player[tempPlayerArr.Length-1];
				for(int i = 0; i < nextPlayers.Length; i++){
					nextPlayers[i] = tempPlayerArr[i+1];
				}
			}
			else{
				nextPlayers = null;
			}

			kingsCall(nextPlayers);
			return;
		}
	}

	public void recognition(){
		recognitionActive = true;
		drawQuestCard();
		return;
	}

	public Player IdentifyPlayer(string name){
		for (int i = 0; i < playerCount; i++) {
			if (players [i].getName () == name) {
				Debug.Log ("clearly " + name + " is " + players [i].getName ());
				return players [i];
			}
		}
		Debug.Log ("Player Unidentified");
		return null;
	}

	public Card[] IdentifySelection(string[] selection, int id){
		Card[] newSelection = null;
		if (selection.Length == 0){ return newSelection;}
		if (selection == null){ return newSelection;}
		newSelection = new Card[selection.Length];
		Player temp = getPlayerWithID (id);
		for (int i = 0; i < selection.Length; i++) {
			for (int c = 0; c < temp.getHand().Length; c++) {
				if (temp.getHand () [c].getName () == selection [i]) 
				{
					newSelection [i] = temp.getHand () [c];
					Debug.Log ("selection found: "+newSelection [i].getName ());
					break;
				}
			}
		}
		return newSelection;
	}

	public Player getPlayerWithID(int connectionID){
		for (int i = 0; i < players.Length; i++) {
			if (players [i].getConnectionID () == connectionID) {
				return players [i];
			}
		}
		return null;
	}

	//This is begging to be abstracted, to do later
}