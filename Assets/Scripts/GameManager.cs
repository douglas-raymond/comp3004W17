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
	int activePlayerOther; 
	
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
	
		activePlayerMeta = nextPlayer(activePlayerMeta);
		dealHands(playerCount);
		log.log ("Dealing hands, drawing first quest");
		drawQuestCard();
		activePlayerSub = -1;
		activePlayerSub = activePlayerMeta;
	}
	private void drawQuestCard(){
		
		activePlayerMeta = nextPlayer(activePlayerMeta);
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
			
			activeQuest = new ActiveQuest((QuestCard)storyCard);
			activePlayerSub = activePlayerMeta;
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
			activeQuest = null;
			drawQuestCard();
		}
		else
		{
			cyclingThroughPlayers = true;
			log.log ("Getting sponsor");
			ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);	
			activePlayerSub = nextPlayer(activePlayerSub);
			
		}
	}
	public void startQuestSetup(){
		activePlayerSub = activePlayerMeta;
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
			getPlayers();
		}
		else {

			activeQuest.getSponsor().discardCard(stageWeapons);
			activeQuest.setStage(activeQuest.getCurrentStageNum()+1);
			startStageWeaponSetup();
		}
	}	
	public void getPlayers(){	
		activePlayerSub = nextPlayer(activePlayerSub);
		ui.askYesOrNo(players[activePlayerSub], "Do you want to join this quest?", GameState.state.ASKINGFORPLAYERS);
	}
	public void gotPlayer(Player newPlayer){
		if(newPlayer != null)
		{
			activeQuest.addPlayer(newPlayer);
		}
		if(players[nextPlayer(activePlayerSub)] == activeQuest.getSponsor())
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
		activeQuest.setQuestAsInProgress();
		Debug.Log(activeQuest.getCurrentPlayer().getName());
		startStage();		
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
			nextPlayer(activePlayerMeta);
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
			
	}
	public void gotCardLimitReached(Card [] cards) {
		activeQuest.getPlayer(activePlayerOther).discardCard(cards);
		endStage();
	}
	public void questAttack(Card [] selection) {
		//Debug.Log("Quest Attack");
		Card[] toDispose;
		Card[] toKeepInPlay;
		
		int extraBP = 0;
		toDispose = null;
		toKeepInPlay = null;
		if(selection != null) {
			toDispose = null;
			toKeepInPlay = null;
			
			if(selection.Length > 0) {
				for(int i = 0; i < selection.Length; i++) {
					extraBP = extraBP + selection[i].getBP();
				}
			}
		
		
		
			for(int i = 0; i< selection.Length; i++){
				if(Object.ReferenceEquals(selection[i].GetType(), typeof(Ally)) || Object.ReferenceEquals(selection[i].GetType(), typeof(Amour))) {
					Debug.Log("Found an ally or amour");
					if(toKeepInPlay == null) { 
					
						toKeepInPlay = new Card[] {selection[i]};
						if(toKeepInPlay == null){
							Debug.Log("ERROR 1");
						}
					}
					else {
						Debug.Log("OTHER CARDS");
						Card[] temp = new Card[toKeepInPlay.Length+1];
						for(int j = 0; j <toKeepInPlay.Length; j++){
							temp[j] = toKeepInPlay[j];
						}
						temp[toKeepInPlay.Length] = selection[i];
						toKeepInPlay = temp;
					}
				}
				else {
					Debug.Log("Found an ally or amour");
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
			if(toDispose != null) {
				if(toDispose.Length > 0) {
					activeQuest.getCurrentPlayer().discardCard(toDispose);
				}
			}
			
			activeQuest.getCurrentPlayer().addCard(toKeepInPlay, true);
			if(toKeepInPlay != null) {
				activeQuest.getCurrentPlayer().discardCard(toKeepInPlay);
			}
			if(activeQuest.isStageDone()) {
				endStage();
			}
			else {
				activeQuest.nextPlayer();
				startStage();
			}
						
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
	private void endStage() {
		Debug.Log("State is over.");
		drawXNumberOfCards(1);
		activeQuest.nextPlayer();
		startStage();
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
	
	private int nextPlayer(int activePlayer) {
		int temp = activePlayer;
		temp ++;
		if(temp == playerCount)
		{
			temp = 0;
		}
		Debug.Log("New current player is "+ temp);
		return temp;
	}
	public Player getCurrentPlayer() {
		if(activeQuest != null && activeQuest.isInProgress()) {
			return activeQuest.getCurrentPlayer();
		}
		else if(activePlayerSub != -1) {
			return players[activePlayerSub];
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

	private void drawXNumberOfCards(int numOfCardsToDraw)
	{
		
		for(int i = 0 ; i< activeQuest.getPlayerNum(); i ++){
			if(activeQuest.getPlayer(i).getHand().Length + numOfCardsToDraw > 12){
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
}
