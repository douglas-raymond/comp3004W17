using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
public class GameManager : MonoBehaviour {
	//Initialize logging functionality
	Logger log = new Logger("GameManager");
	
	//Initialize the two decks
	//DiscardDeck advDiscard = new DiscardDeck();
	//DiscardDeck storyDiscard = new DiscardDeck();
	AdvDeck advDeck;
	StoryDeck storyDeck;
	UI ui;

	//0 = no test, 1 = scenario 1, 2 = scenario 2
	int testingScenario = 0;
	int playerCount = 3;
	
	Player[] players;
	
	//Game states. There will eventually be many possible states, but for right now these two exist.
	state userInputState = state.STANDBY;
	state gameState = state.STANDBY;
	int counter = 0;
	
	public int activePlayerMeta;
	public int activePlayerSub;
	public int activePlayerOther; 
	
	ActiveQuest activeQuest;
	
	bool cyclingThroughPlayers;
	// Use this for initialization
	void Start () {
		
		if(testingScenario == 1) {
			playerCount = 4;
		}
		advDeck = new AdvDeck();
		storyDeck = new StoryDeck();
		log.Init ();
		ui = new UI(this);
		log.log ("created UI");
		//Create all the players and add it to the players array
		players = new Player[playerCount];
		log.log ("created player array");
		
		for(int i = 0; i < playerCount; i++){
			players[i] = new Player(new Card[12], 0, 0, "Player " + (i));
		}
		log.log ("dealt cards");
			
		//Init the decks
		advDeck.initDeck();
		storyDeck.initDeck();
		log.log ("decks initialized");

		gameStart();
	}
	private void gameStart(){
		activePlayerMeta = -1;
		dealHands(playerCount);
		log.log ("Dealing hands, drawing first quest");
		drawQuestCard();
		activePlayerSub = activePlayerMeta;
		//activePlayerSub = activePlayerMeta;
	}
	private void drawQuestCard(){
		gameState = state.DRAWINGSTORYCARD;
		if(activePlayerMeta == -1){
			activePlayerMeta = 0;
		}
		else {
			activePlayerMeta = nextPlayer(activePlayerMeta);
		}
		Card drawnCard = null;
		if(testingScenario == 0) {
			drawnCard = storyDeck.drawCard();
		}
		else if(testingScenario == 1) {
			drawnCard = storyDeck.getCard("boarhunt");
		}
		else {
			drawnCard = storyDeck.drawCard();
		}
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
			
			activeQuest = new ActiveQuest((QuestCard)storyCard);
			activePlayerSub = activePlayerMeta;
			cyclingThroughPlayers = false;
			userInputState = state.ASKINGFORSPONSORS;
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
			
			ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);	
			
			
		}
	}
	public void startQuestSetup(){
		Debug.Log("setting player " + activePlayerSub + "as sponsor");
		//activePlayerSub = activePlayerMeta;
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
				activePlayerSub = activePlayerMeta;
				getPlayers();
			}
			else {
				ui.displayAlert(errorMessage);
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
			activePlayerSub = activePlayerMeta;
			counter = 1;
			getPlayers();
		}
		else {
			advDeck.discardCard(stageWeapons);
			activeQuest.getSponsor().discardCard(stageWeapons);
			activeQuest.setStage(activeQuest.getCurrentStageNum()+1);
			startStageWeaponSetup();
		}
	}	
	public void getPlayers(){	
		ui.showCard(activeQuest.getQuest());
		activePlayerSub = nextPlayer(activePlayerSub);
		log.log("Asking " + players[activePlayerSub].getName() + " if they want to join the quest");
		ui.askYesOrNo(players[activePlayerSub], "Do you want to join this quest?", GameState.state.ASKINGFORPLAYERS);
	}
	public void gotPlayer(Player newPlayer){
		counter ++;
		if(newPlayer != null) {
			log.log("Player " + newPlayer.getName() + " joined quest.");
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
	/*Gets a selected card and does something with it
	*/
	//Pass in a player count, it will give each player a hand of 12 adventure cards
	private void dealHands(int playerCount){
		if(testingScenario == 0){
			for(int i = 0; i < playerCount; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				players[i].setHand(newHand);
			}
		}
		else if (testingScenario == 1) {
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("saxons");
			player1NewHand[1] = advDeck.getCard("boar");
			player1NewHand[2] = advDeck.getCard("sword");
			player1NewHand[3] = advDeck.getCard("dagger");
			for(int i = 4; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			for(int i = 0; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player3NewHand = new Card[12];
			player3NewHand[0] = advDeck.getCard("horse");
			player3NewHand[1] = advDeck.getCard("excalibur");
			for(int i = 2; i < player3NewHand.Length; i++){
				player3NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player4NewHand = new Card[12];
			player4NewHand[0] = advDeck.getCard("battleax");
			player4NewHand[1] = advDeck.getCard("lance");
			for(int i = 2; i < player4NewHand.Length; i++){
				player4NewHand[i] = advDeck.drawCard();
			}
			players[0].setHand(player1NewHand);
			players[1].setHand(player2NewHand);
			players[2].setHand(player3NewHand);
			players[3].setHand(player4NewHand);

		}
		return;
	}
	
	public void startQuest() {
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
	public void startStage() {
		if(activeQuest.getQuest() == null) {
			endQuest("Quest over");
			return;
		}
		else if(activeQuest.getPlayerNum() == 0) {
			endQuest("All players dead");
			return;
		}
		else{
			ui.showStage(activeQuest);
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Foe))) {
				log.log(activeQuest.getCurrentPlayer().getName() + " is now facing a foe of type " + activeQuest.getCurrentStage().getName() + " enhanced with " + activeQuest.getStageWeaponString());
				
				ui.askForCards(
								activeQuest.getCurrentPlayer(), 
								GameState.state.ASKINGFORCARDSINQUEST, 
								"Select cards to play, then press FIGHT", 
								"FIGHT",
								"null", 
								false, 
								true, 
								true,
								true,
								false);
			}
			if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Test))) {
				log.log(activeQuest.getCurrentPlayer().getName() + " is now bidding in the " + activeQuest.getCurrentStage().getName() + " test");				
				ui.askForCards(
								activeQuest.getCurrentPlayer(),  
								GameState.state.ASKINGFORCARDSINBID, 
								"Select cards to bit, then press BID", 
								"BID",
								"Give up", 
								true, 
								true, 
								true,
								true,
								true);
			}
		}
		return;
	}
	public void endQuest(string text = "Quest over") {
			gameState = state.QUESTWRAPUP;
			
			drawXNumberOfCards(activeQuest.getTotalCardsUsed(), activeQuest.getSponsor());
			
			if(userInputState != state.ASKINGFORCARDSTODISCARD) {
				log.log("Quest is over. Players will be awarded " + activeQuest.getStageNum() + " shields");
				storyDeck.discardCard(new Card[]{activeQuest.getQuest()});
				activeQuest = null;
				ui.endQuest();
				ui.drawingQuestCard();
				drawQuestCard();
				activePlayerMeta = nextPlayer(activePlayerMeta);
			}
	}
	public void bidPhase(Card [] selection) {	
		Debug.Log("Free bids: " + activeQuest.getCurrentPlayer().getFreeBids());
		if(activeQuest.placeBid(selection, activeQuest.getCurrentPlayer().getFreeBids())) {
			activeQuest.setTentativeBet(selection);
			if(activeQuest.isStageDone()) {
				endStage();
			}
			else {
				activeQuest.nextPlayer();
				startStage();
			}			
		}
		else {
			ui.displayAlert("Bid too low. Bid more cards of forfeit the quest.");
			ui.askForCards(
							activeQuest.getCurrentPlayer(), 
							GameState.state.ASKINGFORCARDSINBID, 
							"Select cards to bit, then press BID", 
							"BID",
							"Give up", 
							true, 
							true, 
							true,
							true,
							true);
		}
	}
	public void askForCardLimitReached(Player player, int cardsToDeleteNum) {
		Debug.Log(player.getName() + "'s card limit reached. Asking to discard " + cardsToDeleteNum + " cards.");
		ui.askForCards(
			player, 
			GameState.state.ASKINGFORCARDSTODISCARD, 
								"Card limit reached. Please select "+ cardsToDeleteNum + " cards to discard.", 
								"null",
								"null", 
								true, 
								true, 
								true,
								true,
								true,
								cardsToDeleteNum);
								
		return;
			
	}
	public void gotCardLimitReached(Card [] cards) {
		advDeck.discardCard(cards);
		if(activePlayerOther == -1){
			activeQuest.getSponsor().discardCard(cards);
		}
		else {
			activeQuest.getPlayer(activePlayerOther).discardCard(cards);
			
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
	public void questAttack(Card [] selection) {
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
		
		if(activeQuest.getStageBP(activeQuest.getCurrentStageNum()) <= activeQuest.getCurrentPlayer().getBP() + extraBP) {
			log.log("With a total BP of " + (activeQuest.getCurrentPlayer().getBP() + extraBP) + " " + activeQuest.getCurrentPlayer().getName() + " overcame " + activeQuest.getCurrentStage().getName());
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
			log.log("With a total BP of " + (activeQuest.getCurrentPlayer().getBP() + extraBP) + " " + activeQuest.getCurrentPlayer().getName() + " fell to " + activeQuest.getCurrentStage().getName());
			
			if(activeQuest.getPlayerInt(activeQuest.getCurrentPlayer()) == activeQuest.getPlayerNum()-1)  {
				
				userInputState = state.SHOWINGFOE;
				forfeitQuest();
				ui.foeReveal(activeQuest);
				
			}
			else {
				forfeitQuest();
				startStage();
			}
		}
	}
	public void endStage() {
		log.log("Stage is over.");
		drawXNumberOfCards(1);
		if(activeQuest.getPlayerNum() == 0) {
			endQuest("All players dead");
			return;
		}
		if(userInputState != state.ASKINGFORCARDSTODISCARD) {
			if(activeQuest.getCurrentStageNum() != activeQuest.getStageNum()){
				activeQuest.nextPlayer();
				activeQuest.nextStage();
				startStage();
			}
			else{
				endQuest();
			}
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
		//log.log(activeQuest.getCurrentPlayer().getName() + " has forfeited quest");
		activeQuest.deletePlayer(activeQuest.getCurrentPlayer());
		if(userInputState == state.ASKINGFORCARDSINBID){
			activeQuest.nextPlayer();
			startStage();
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
		else if(userInputState == state.ASKINGFORPLAYERS || userInputState == state.ASKINGFORSPONSORS) {
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
		activePlayerOther = -1;
		if(player == null) {
			for(int i = 0 ; i< activeQuest.getPlayerNum(); i ++){
				log.log("Drawing " + numOfCardsToDraw + " cards for " + activeQuest.getPlayer(i).getName());
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
	
	
	
	
	public state getUserInputState(){
		return userInputState;
	}
	
	public void setUserInputState(state newState){
		userInputState = newState;
	}
}
