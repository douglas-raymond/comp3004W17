using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;

public class UI : MonoBehaviour {

	// Use this for initialization
	
	//The current player giving input
	Player activePlayer;
	//Cards to display
	GameObject[] currButtons;
	GameObject[] cardsToShow;
	//public Card inputCard;
	GameManager gm;
	

	state gameState = state.STANDBY;
	
	Card[] multipleCardInput;
	
	GameObject header;  //This gives th current instruction to the player
	GameObject headerCurrPlayer; //This says which player's turn it is
	GameObject cardCenter;
	
	GameObject enemyBP;
	GameObject playerBP;
	Vector2 canvasSize;
	
	GameObject canvas;
	float panelWidth;
	float panelHeight;
	
	float panelPosX;
	float panelPosY;
	public UI(GameManager _gm) {
		
		canvas = GameObject.Find("Canvas");
		panelWidth = canvas.GetComponent<RectTransform>().rect.width * canvas.GetComponent<RectTransform>().localScale.x;
		panelHeight = canvas.GetComponent<RectTransform>().rect.height * canvas.GetComponent<RectTransform>().localScale.y;
		panelPosX = canvas.GetComponent<RectTransform>().position.x;
		panelPosY = canvas.GetComponent<RectTransform>().position.y;
		gm = _gm;
		
		header  = createHeaderMessage(panelPosX, panelPosY + panelHeight/4, "Current action required");	
		headerCurrPlayer  = createHeaderMessage(panelPosX, panelPosY + panelHeight/5, "Current player's turn");	
		
	//	canvasSize = new Vector2(Canvas.GetComponent<RectTransform>().transform.Width,0);

		
		
	}
	
	void Start () { }
	
	// Update is called once per frame
	void Update () {
	}
	
	//Prints out a given hand
	public GameObject[] showHand(Card[] hand){
		int n = hand.Length;
		if(cardsToShow != null)
		{
			for(int i = 0; i < cardsToShow.Length; i ++) {
				Destroy(cardsToShow[i]);
				cardsToShow[i] = null;
			}
		}
		cardsToShow = new GameObject[n];
		float buffer = panelWidth/(n*2);
		float offsetX = (panelWidth - n*buffer)/6;
		for(int i = 0; i< n; i++)
		{	
			Vector2 pos = 	new Vector2(panelPosX - panelWidth/2 + offsetX + i*buffer, panelPosY -  panelHeight/6);
			cardsToShow[i] = (GameObject)Instantiate(Resources.Load("UICardButton"), pos , Quaternion.identity);			
			//cardsToShow[i].GetComponent<CardButtonUI>().setCard(hand[i]);
			cardsToShow[i].GetComponent<CardButtonUI>().init(hand[i], this, pos);
		}
		return cardsToShow;
	}	
	
	//Ask player for input
	public void getCardSelection(Card selected){
		/*
		This method is called when a card is clicked. If we are expecting only a single care selection, 
		then it calls the gotSingleCardSelection() method which sends the card back to the GameManager.
		
		Other wise gotMultipleCardSelection() is called, which updates the multipleCardInput() array that is 
		sent once it is filled up with selections.
		*/
		
		/*
		if (gameState == state.ASKINGFORSINGLECARD)	{
			changeHeaderMessage("Select a card to play", header);
			gotSingleCardSelection(selected);
		}
		*/
		if (gameState == state.ASKINGFORSTAGES) {
			gotMultipleCardSelection(selected);
		}
		if (gameState == state.ASKINGFORCARDSINQUEST) {
			gotBattleCardSelection(selected);
		}
	}
	
	/*
	public void askForSingleCardSelection(Player player){
		clearScreen();
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	//Say who's turn it is.
		activePlayer = player; //Updates the current player
		currButtons = showHand(player.getHand()); //Display the hand
		gameState = state.ASKINGFORSTAGES; 
		
		return;
	}
	
	/*
	private void gotSingleCardSelection(Card selected){
		activePlayer.discardCard(new Card[] {selected}); 
		gm.gotSingleCardSelection(selected);
		clearScreen();
		return;
	}
	*/

	public void askForBattleCardSelection(Player player){
		
		activePlayer = player;
		currButtons = showHand(getOnlyTypeFromDeck(player.getHand(), false, true, true)); //Display the cards
		gameState = state.ASKINGFORCARDSINQUEST;
		multipleCardInput = new Card[player.getHand().Length]; //Get multipleCardInput ready to hold the new card choices
		changeHeaderMessage("Select cards to play, then press FIGHT", header);
		return;
	}
	
	private void gotBattleCardSelection(Card selected){
		for(int i = 0; i < multipleCardInput.Length; i++) {  //Find the next empty spot in multipleCardInput
			if(multipleCardInput[i] == null) { 
				multipleCardInput[i] = selected; //Add the new selected card to multipleCardInput
				break;
			}
		}
		int extraBP = 0;
		for(int i = 0; i< multipleCardInput.Length; i++)
		{
			if(multipleCardInput[i] == null){break;}
			extraBP = extraBP + multipleCardInput[i].getBP();
		}
		changeHeaderMessage("Player BP: " + (activePlayer.getBP() + extraBP), playerBP);
	}
	
	private Card[] cleanUpArray(Card [] oldArr){
		int newN = 0;
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
	
	private Card[] getOnlyTypeFromDeck(Card[] deck, bool getFoes, bool getWeap, bool getAlly){
		Card[] tempHand = new Card[deck.Length];
		int counter = 0;
		for(int i = 0; i < deck.Length; i++ )
		{
			if(Object.ReferenceEquals(deck[i].GetType(), typeof(Foe)) && getFoes)
			{
				tempHand[counter] = deck[i];
				counter++;
			}
			else if(Object.ReferenceEquals(deck[i].GetType(), typeof(Weapon)) && getWeap)
			{
				tempHand[counter] = deck[i];
				counter++;
			}
			else if(Object.ReferenceEquals(deck[i].GetType(), typeof(Ally)) && getAlly)
			{
				tempHand[counter] = deck[i];
				counter++;
			}
		}
		
		Card[] newTempHand = new Card[counter];
		
		for(int i = 0; i < counter; i++)
		{
			newTempHand[i] = tempHand[i];
		}
		return newTempHand;
	}
	
	public void askForStageSelection(Player player, int n){
		activePlayer = player;
		Card [] foesOnly = getOnlyTypeFromDeck(player.getHand(), true, false, false);
		currButtons = showHand(foesOnly); //Display the cards
		gameState = state.ASKINGFORSTAGES;
		multipleCardInput = new Card[n]; //Get multipleCardInput ready to hold the new card choices
		changeHeaderMessage("Select card 1 out of " + multipleCardInput.Length, header);
		return;
	}
	
	private void gotMultipleCardSelection(Card selected){
		for(int i = 0; i < multipleCardInput.Length; i++) {  //Find the next empty spot in multipleCardInput
			if(multipleCardInput[i] == null) { 
				multipleCardInput[i] = selected; //Add the new selected card to multipleCardInput
				if(i == multipleCardInput.Length-1) {  //If all the cards has been chosen
					
					changeHeaderMessage("Stages selected", header); //Update header
					gameState = state.STANDBY;
					gm.endQuestSetup(multipleCardInput); //Send cards back to GameManager
					multipleCardInput = null;
					return;
				}
				changeHeaderMessage("Select card " + (i+2) + " out of " + multipleCardInput.Length, header);
				return;
			}
		}
	}

	public void askYesOrNo(Player player, string message, state messageState) {
		clearButtons();
		gameState = messageState;
		activePlayer = player;
		
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		changeHeaderMessage(message, header);
		
		//Display yes or no buttons
		currButtons = new GameObject[2];
		
		currButtons[0] = createButtonMessage(panelPosX - panelWidth/3, panelPosY + panelHeight/5, "Yes");
		currButtons[1] = createButtonMessage(panelPosX + panelWidth/3, panelPosY + panelHeight/5, "No");
	}
	
	public void gotButtonClick(string input) {
		//This method is called when a button is clicked
		if(gameState == state.ASKINGFORSPONSORS) { //If the game is current looking for sponsors
			if(input.Equals("Yes")) { //If the current player wants to be sponsor
				gameState = state.STANDBY; 
				clearButtons();
				gm.startQuestSetup(); //Tell GameManager to set the current player as sponsor
			}
			
			else {
				gm.getSponsor(); //Other wise have GameManager call getSponsor for the next player.
			}
		}
		else if(gameState == state.ASKINGFORPLAYERS){
			if(input.Equals("Yes")) { //If the current player wants to be sponsor 
				gameState = state.STANDBY; 
				clearButtons();
				gm.gotPlayer(activePlayer); //Tell GameManager to set the current player as sponsor
			}
			else {
				clearButtons();
				gm.gotPlayer(null); //Other wise have GameManager call getSponsor for the next player.
			}
		}
		else if(gameState == state.ASKINGFORCARDSINQUEST)
		{
			gm.questAttack(cleanUpArray(multipleCardInput));
		}
	}

	//Header functions
	private void clearButtons(){
		if(currButtons == null)
		{
			return;
		}
		for(int i = 0; i < currButtons.Length; i ++) {
			Destroy(currButtons[i]);
			
		}
		
		currButtons = null;
		//GameObject [] temp = new GameObject[0];
		//currButtons = temp;
	}
	
	private void clearDeckOnScreen(){
		if(cardsToShow == null)
		{
			return;
		}
		for(int i = 0; i < cardsToShow.Length; i ++) {
			Destroy(cardsToShow[i]);
		}
		
		cardsToShow = null;
		//GameObject [] temp = new GameObject[0];
		//currButtons = temp;
	}
	
	private GameObject createButtonMessage(float x, float y, string newText = "Button") {
		GameObject tempButton = (GameObject)Instantiate(Resources.Load("UIButton"), new Vector2(x, y), Quaternion.identity);			
		tempButton.GetComponent<ButtonUI>().init(this);
		tempButton.GetComponentInChildren<Text>().text = newText;
		return tempButton;
	}
	
	private GameObject createHeaderMessage(float x, float y, string input = "Header") {
		GameObject header;
		header = (GameObject)Instantiate(Resources.Load("UIHeader"), new Vector2(x, y), Quaternion.identity);	
		header.GetComponent<HeaderUI>().init();
		changeHeaderMessage(input, header);
		return header;
	}
	
	private void changeHeaderMessage(string input, GameObject header){
		header.GetComponent<Text>().text = input;
	}
	
	public void showCard(Card cardToShow){
		if(cardCenter != null) { Destroy(cardCenter); }
		//cardCenter = (GameObject)Instantiate(Resources.Load("UICard"), new Vector2((float)(panelWidth/3.5), (float)(panelHeight/2.5)), Quaternion.identity);	
		
		cardCenter = (GameObject)Instantiate(Resources.Load("UICard"), new Vector2(panelPosX - panelWidth/35, panelPosY + panelHeight/4), Quaternion.identity);			
		//cardCenter.GetComponent<CardButtonUI>().setCard(cardToShow);
		cardCenter.GetComponent<CardButtonUI>().init(cardToShow, this, new Vector2(panelPosX - panelWidth/35, panelPosY + panelHeight/4));
	}

	public void showStage(Card currStage){
		clearButtons();
		if(playerBP == null) { playerBP = createHeaderMessage(panelWidth/4, panelHeight/2, " ");}
		if(enemyBP == null) { enemyBP = createHeaderMessage(panelWidth - panelWidth/4, panelHeight/2, " ");}
		
		
		showCard(currStage);
		
		changeHeaderMessage("Player BP: " + activePlayer.getBP(), playerBP);
		changeHeaderMessage("Enemy BP: " + currStage.getBP(), enemyBP);
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		changeHeaderMessage("Select the cards you wish to play to defeat this foe.", header);	
		currButtons = new GameObject[1];
		currButtons[0] = createButtonMessage(panelPosX, panelPosY - panelHeight/10, "FIGHT");
		gameState = state.ASKINGFORSINGLECARD;
		
		
	}
	
}
