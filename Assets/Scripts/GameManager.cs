using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;
public class GameManager : MonoBehaviour {
	
	
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
		Debug.Log("fssfsdfs");
		ui = new UI(this);
		//Create all the players and add it to the players array
		players = new Player[playerCount];
		
		for(int i = 0; i < playerCount; i++){
			players[i] = new Player(new Card[12], 0, 0, "Player " + (i+1));
		}
			
		//Init the decks
		advDeck.initDeck();
		storyDeck.initDeck();

		gameStart();
	}
	private void gameStart(){
		activePlayerMeta++;
		dealHands(playerCount);
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
			Debug.Log("Getting sponsor...");
			ui.askYesOrNo(players[activePlayerSub], "Do you want to sponsor this quest?", GameState.state.ASKINGFORSPONSORS);
		}
		else
		{
			Debug.Log("Sponsor not found.");
			activeQuest = null;
			drawQuestCard();
		}
	}
	public void startQuestSetup(){
		activeQuest.setSponsor(players[activePlayerSub]);
		ui.askForStageSelection(players[activePlayerSub], activeQuest.getStageNum());
	}	
	public void endQuestSetup(Card[] stages){
		Debug.Log("endQuestSetup");
		activeQuest.setStages(stages);
		gameState = state.GOTINPUT;
		for(int i = 0; i < stages.Length; i++){
			activeQuest.getSponsor().discardCard(new Card[] {stages[i]});
		}
		
		activePlayerSub = activePlayerMeta;
		getPlayers();
	}
	public void getPlayers(){	
		activePlayerSub ++;
		activePlayerSub = activePlayerSub % (playerCount-1);
		if(players[activePlayerSub] == activeQuest.getSponsor())
		{
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
	public void gotSingleCardSelection(Card card){
		gameState = state.GOTINPUT;
	}
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
			Debug.Log("Quest over");
			activeQuest = null;
			drawQuestCard();
			return;
		}
		if(activeQuest.getPlayerNum() == 0)
		{
			activeQuest = null;
			drawQuestCard();
		}
		ui.showStage(activeQuest.getCurrentStage());
		ui.askForBattleCardSelection(activeQuest.getCurrentPlayer());
		return;
	}
	public void questAttack(Card [] selection) {
		Debug.Log("Quest Attack");
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
		
	}

}
