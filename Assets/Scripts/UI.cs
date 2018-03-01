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
	GameObject[] currButtons, cardsToShow, currIcons, currPlayerHand;
	GameObject[] stageWinners;
	//public Card inputCard;
	GameManager gm;
	Logger log = new Logger("UI");
	

	// gameState = state.STANDBY;
	
	GameObject[] multipleCardInput;
	
	GameObject instructionHeader;  //This gives th current instruction to the player
	GameObject headerCurrPlayer; //This says which player's turn it is
	GameObject messageHeader; //Gives any messages to the player
	
	GameObject cardCenter;
	GameObject enemyBP, playerBP, highestBid, currentBid;
	GameObject otherPlayerHeader;

	Vector2 canvasSize;
	
	GameObject canvas;
	
	int multipleCardInputMaxNum;
	//Use these variables to place this on UI, this assures that the relative positions will change the same in different resolutions
	float panelWidth, panelHeight, panelPosX, panelPosY;
	
	ShowHandUI showHandUI;
	ShowOtherPlayerUI showOtherPlayerUI;
	public UI(GameManager _gm) {
		canvas = GameObject.Find("Canvas");
		panelWidth = canvas.GetComponent<RectTransform>().rect.width * canvas.GetComponent<RectTransform>().localScale.x;
		panelHeight = canvas.GetComponent<RectTransform>().rect.height * canvas.GetComponent<RectTransform>().localScale.y;
		panelPosX = canvas.GetComponent<RectTransform>().position.x;
		panelPosY = canvas.GetComponent<RectTransform>().position.y;
		gm = _gm;
		
		instructionHeader  = createHeaderMessage(panelPosX, panelPosY + panelHeight/3 + panelHeight/15, new Vector3(0,0,0), "Current action required");	
		headerCurrPlayer  = createHeaderMessage(panelPosX, panelPosY + panelHeight/3, new Vector3(0,0,0), "Current player's turn");	
		messageHeader  = createHeaderMessage(panelPosX, panelPosY - panelHeight/3, new Vector3(0,0,0), " ");	
		
		
		GameObject showHandUITemp = (GameObject)Instantiate(Resources.Load("UIShowHand"), new Vector2(panelPosX + panelWidth/3, panelPosY + panelHeight/4) , Quaternion.identity);
		showHandUITemp.GetComponent<ShowHandUI>().init(this);
		
		GameObject showOtherPlayerUI = (GameObject)Instantiate(Resources.Load("UIShowOtherPlayer"), new Vector2(panelPosX + panelWidth/3, panelPosY + panelHeight/4 - panelHeight/20) , Quaternion.identity);
		showOtherPlayerUI.GetComponent<ShowOtherPlayerUI>().init(this);
	}
	
	//Prints out a given hand
	public GameObject[] showHand(Card[] hand){
		int n = hand.Length;
		log.log ("getting cards to show");
		if(hand.Length == null){return null;}
		if(cardsToShow != null)
		{
			for(int i = 0; i < cardsToShow.Length; i ++) {
				Destroy(cardsToShow[i]);
				cardsToShow[i] = null;
			}
		}
		cardsToShow = new GameObject[n];
		float cardWidth = panelWidth/21;
		float cardSpacing = cardWidth/4;
		float totalDeckWidth = (n-1)*cardSpacing + (n-1)*cardWidth;
		log.log ("setting up area");
		for(int i = 0; i< n; i++)
		{	
			Vector2 pos = 	new Vector2(panelPosX - totalDeckWidth/2 + i*cardWidth + i*cardSpacing, panelPosY -  panelHeight/6);
			cardsToShow[i] = (GameObject)Instantiate(Resources.Load("UICardButton"), pos , Quaternion.identity);			
			cardsToShow[i].GetComponent<CardButtonUI>().init(hand[i], this, pos, i);
		}
		return cardsToShow;
	}	
	
	//Ask player for input
	public GameObject[] showCards(Card[] hand, Vector2 startPos, Vector2 scale){
		if(hand[0] == null) { return null; }
		if(hand == null) { return null; }
		int n = hand.Length;
		if(currIcons != null)
		{
			for(int i = 0; i < currIcons.Length; i ++) {
				Destroy(currIcons[i]);
				currIcons[i] = null;
			}
		}
		currIcons = new GameObject[n];
		float buffer = panelWidth/20;
		float cardWidth = panelWidth/15;
		float offsetX = (panelWidth - buffer)/6;
		for(int i = 0; i< n; i++)
		{	
			Vector2 pos = 	new Vector2(startPos.x + offsetX + i*buffer, startPos.y);
			currIcons[i] = (GameObject)Instantiate(Resources.Load("UICard"), pos , Quaternion.identity);			
			currIcons[i].GetComponent<CardUI>().init(hand[i], this, pos, scale);
		}
		return cardsToShow;
	}
	public void gotCardSelection(GameObject selected){
		/*
		This method is called when a card is clicked. Depending on the current gm.getUserInputState(), a difference received mthod will be called.
		*/
		if (gm.getUserInputState() == state.ASKINGFORSTAGES) {
			gotStageSelection(selected, selected.GetComponent<CardButtonUI>().getPos());
		}
		else if (gm.getUserInputState() == state.ASKINGFORCARDSINQUEST) {
			gotBattleCardSelection(selected, selected.GetComponent<CardButtonUI>().getPos());
		}
		else if (gm.getUserInputState() == state.ASKINGFORCARDSINBID) {
			gotBidCardSelection(selected, selected.GetComponent<CardButtonUI>().getPos());
		}
		else if (gm.getUserInputState() == state.ASKINGFORSTAGEWEAPONS) {
			gotStageWeaponSelection(selected, selected.GetComponent<CardButtonUI>().getPos());
		}
		
		else if (gm.getUserInputState() == state.ASKINGFORCARDSTODISCARD) {
			gotCardToDiscardSelection(selected, selected.GetComponent<CardButtonUI>().getPos());
		}
		else if (gm.getUserInputState() == state.ASKINGFORCARDSINTOURNEY) {
			gotTourneyCardSelection(selected, selected.GetComponent<CardButtonUI>().getPos());
		}
	}
	
	public void removeCardSelection(GameObject selected){
		/*
		This method is called when a card is clicked. Depending on the current gameState, a difference received mthod will be called.
		*/
		
		if(selected == null) { return;}
		int n = multipleCardInput.Length;
		int j = -1;
		GameObject[] temp = new GameObject[n-1];
		j = selected.GetComponent<CardButtonUI>().getIndexInSelection();

		if(j == 0){
			for(int i = 0; i < n-1; i++)
			{
				temp[i] = multipleCardInput[i];
			}	
		}
		else if(j == multipleCardInput.Length){
			for(int i = 1; i < n; i++)
			{
				temp[i-1] = multipleCardInput[i];
			}
		}
		else
		{
			for(int i = 0; i < j; i++){
				temp[i] = multipleCardInput[i];
			}
			for(int i = j+1; i < multipleCardInput.Length; i++){
				temp[i-1] = multipleCardInput[i];
			}
		}

		multipleCardInput = temp;
		Destroy(selected.GetComponent<CardButtonUI>().getSelectedCardIcon());
		
		for(int i = 0; i < multipleCardInput.Length; i++) {
			multipleCardInput[i].GetComponent<CardButtonUI>().setIndexInSelection(i);
		}
		if (gm.getUserInputState() == state.ASKINGFORCARDSINQUEST) {
			changeHeaderMessage("Player BP: " + getPlayersBP(), playerBP);
		}
		if (gm.getUserInputState() == state.ASKINGFORCARDSINBID) {
			int currentBidCounter;
			if(multipleCardInput.Length == null){currentBidCounter = 0;}
			else{currentBidCounter = multipleCardInput.Length;}
			
			currentBidCounter = currentBidCounter + activePlayer.getFreeBids();
			changeHeaderMessage("Current bid: " + currentBidCounter, currentBid);
		}
		return;
	}
	public void gotButtonClick(string input) {
		//This method is called when a button is clicked
		if(gm.getUserInputState() == state.ASKINGFORSPONSORS) { //If the game is current looking for sponsors
			log.log("currently asking for sponsors");
			if(input.Equals("Yes")) { //If the current player wants to be sponsor
				log.log("player clicked yes");
				gm.setUserInputState(state.STANDBY); 
				clearGameObjectArray(currButtons);
				log.log ("telling GM to set up quest");
				gm.startQuestSetup(); //Tell GameManager to set the current player as sponsor
			}
			else {
				log.log ("telling GM to ask next player");
				gm.getSponsor(); //Other wise have GameManager call getSponsor for the next player.
			}
		}
		else if(gm.getUserInputState() == state.ASKINGFORPLAYERS){
			log.log ("currently asking for players");
			if(input.Equals("Yes")) { //If the current player wants to be sponsor 
				log.log("player clicked yes");
				gm.setUserInputState(state.STANDBY); 
				clearGameObjectArray(currButtons);
				log.log ("telling GM we have an active player");
				gm.gotPlayer(activePlayer); //Tell GameManager to set the current player as sponsor
			}
			else {
				clearGameObjectArray(currButtons);
				log.log ("telling GM we don't have an active player");
				gm.gotPlayer(null); //Other wise have GameManager call getSponsor for the next player.
			}
		}
		else if(gm.getUserInputState() == state.ASKINGFORPLAYERSTOURNEY){
			log.log ("currently asking for players for tournament");
			if(input.Equals("Yes")) { //If the current player wants to be sponsor 
				log.log("player clicked yes");
				gm.setUserInputState(state.STANDBY); 
				clearGameObjectArray(currButtons);
				log.log ("telling GM we have an active player");
				gm.gotPlayerTourney(activePlayer); //Tell GameManager to set the current player as sponsor
			}
			else {
				clearGameObjectArray(currButtons);
				log.log ("telling GM we don't have an active player");
				gm.gotPlayerTourney(null); //Other wise have GameManager call getSponsor for the next player.
			}
		}
		else if(gm.getUserInputState() == state.ASKINGFORCARDSINQUEST){
			log.log ("asking for cards in quest");
			if(input.Equals("FIGHT")) {
				gm.questAttack(gameObjectArrayToCardArray(multipleCardInput));
			}
			else if(input.Equals("Give up")) {
				gm.forfeitQuest();
			}
		}
		else if(gm.getUserInputState() == state.ASKINGFORCARDSINTOURNEY){
			log.log ("asking for cards in tournament");
			if(input.Equals("ENTER TOURNAMENT!")) {
				clearGameObjectArray(currButtons);
				clearGameObjectArray(cardsToShow);
				gm.gotTournamentCards(gameObjectArrayToCardArray(multipleCardInput));
			}
		}
		else if(gm.getUserInputState() == state.ASKINGFORCARDSINBID){
			log.log ("got for cards in bid");
			if(input.Equals("BID")) {
				gm.bidPhase(gameObjectArrayToCardArray(multipleCardInput));
			}
			else if(input.Equals("Give up")) {
				gm.forfeitQuest();
			}
		}
		else if(gm.getUserInputState() == state.ASKINGFORSTAGES) {
			log.log ("asking for stages");
			clearGameObjectArray(currButtons);
			clearGameObjectArray(cardsToShow);
			gm.endQuest("Quest forfeited");
			displayAlert("Quest forfeited");
		}
		else if(gm.getUserInputState() == state.SHOWINGFOE) {
			clearGameObjectArray(currButtons);
			clearGameObjectArray(cardsToShow);
			clearGameObjectArray(stageWinners);
			Destroy(playerBP);
			gm.endStage();
		}
		else if(gm.getUserInputState() == state.ASKINGFORSTAGEWEAPONS) {
			clearGameObjectArray(currButtons);
			clearGameObjectArray(cardsToShow);
			if(multipleCardInput == null)
			{
				gm.endStageWeaponSetup(null);
			}
			else{
				gm.endStageWeaponSetup(gameObjectArrayToCardArray(multipleCardInput));
			}
		}
	}
	
	public void askForCards(Player player, state newState, string instructions, string button1, string button2, bool getFoes, bool getWeap, bool getAlly, bool getAmour, bool getTest, int n = -1) {
		clearGameObjectArray(cardsToShow);
		clearGameObjectArray(currButtons);
		if(gm.getUserInputState() == state.ASKINGFORCARDSTODISCARD){clearGameObjectArray(currIcons);}
		multipleCardInputMaxNum = n;
		activePlayer = player;
		Card [] cards = getOnlyTypeFromDeck(player.getHand(), getFoes, getWeap, getAlly, getAmour, getTest);
		if(cards == null) { return; }
		cardsToShow = showHand(cards); //Display the cards
		gm.setUserInputState(newState);
		Debug.Log(gm.getUserInputState());
		multipleCardInput = null; //Get multipleCardInput ready to hold the new card choices
		changeHeaderMessage(instructions, instructionHeader);
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		if(!button1.Equals("null")){
			createButtonMessage(panelPosX, panelPosY - panelHeight/20, button1);
		}
		if(!button2.Equals("null")){
			createButtonMessage(panelPosX - panelWidth/5, panelPosY + panelHeight/5, button2);
		}
		return;
	}
	
	private void gotBattleCardSelection(GameObject selected, Vector2 pos){
		//Card selected = selectedObj.GetComponent<CardButtonUI>().getCard();
		if(!checkIfArrayContainsCard(multipleCardInput, selected)) {
			addNewCardToMultipleCardArray(selected, pos);
			changeHeaderMessage("Player BP: " + getPlayersBP(), playerBP);
		}
		else {
			displayAlert("Cannot have two weapons of the same type!");
			return;
		}
	}
	
	private void gotTourneyCardSelection(GameObject selected, Vector2 pos){
		//Card selected = selectedObj.GetComponent<CardButtonUI>().getCard();
		if(!checkIfArrayContainsCard(multipleCardInput, selected)) {
			addNewCardToMultipleCardArray(selected, pos);
		}
		else {
			displayAlert("Cannot have two weapons of the same type!");
			return;
		}
	}
	
	public void foeReveal(ActiveQuest activeQuest) {
		if(enemyBP == null) { enemyBP = createHeaderMessage(panelPosX + panelWidth/3, panelHeight/2, new Vector3(0,0,0), " ");}
		Destroy(playerBP);
		showCards(activeQuest.getStageWeapons(activeQuest.getCurrentStageNum()), new Vector2(panelPosX + panelWidth/10, panelPosY) , new Vector2(10,10));
		showCard(activeQuest.getCurrentStage());
		clearGameObjectArray(cardsToShow);
		clearGameObjectArray(currButtons);
		
		
		stageWinners = new GameObject[activeQuest.getPlayerNum()+1];
		stageWinners[0] = createHeaderMessage(panelPosX - panelWidth/3, panelHeight/2, new Vector3(0,0,0), "Winners");
		createButtonMessage(panelPosX, panelPosY - panelHeight/10, "OK");
		for(int i = 1; i< activeQuest.getPlayerNum()+1; i++){
			stageWinners[i] = createHeaderMessage(panelPosX - panelWidth/3, panelHeight/2 - i *(panelHeight/15), new Vector3(0,0,0), activeQuest.getPlayer(i-1).getName());
		}
		
	}
	private void gotBidCardSelection(GameObject selected, Vector2 pos) {

		
		int currentBidCounter;
		if(multipleCardInput == null) { currentBidCounter = 1;}
		else {
			currentBidCounter = multipleCardInput.Length + 1;
			}
		addNewCardToMultipleCardArray(selected, pos);
		currentBidCounter = currentBidCounter + activePlayer.getFreeBids();
		changeHeaderMessage("Current bid: " + currentBidCounter, currentBid);
	}
	public void drawingQuestCard() {
		clearGameObjectArray(cardsToShow);
		clearGameObjectArray(currButtons);
		changeHeaderMessage("Drawing new card", instructionHeader);
		changeHeaderMessage(" ", messageHeader);
		changeHeaderMessage(" ", headerCurrPlayer);
		
	}
	private void gotStageSelection(GameObject selected, Vector2 pos){
		//Card selected = selectedObj.GetComponent<CardButtonUI>().getCard();
		addNewCardToMultipleCardArray(selected, pos);
		
		if(multipleCardInput.Length == multipleCardInputMaxNum) {  //If all the cards has been chosen
			changeHeaderMessage("Stages selected", instructionHeader); //Update header
			gm.setUserInputState(state.STANDBY);
			clearGameObjectArray(cardsToShow);	
			
			Card [] temp = gameObjectArrayToCardArray(multipleCardInput);
			gm.endQuestSetup(gameObjectArrayToCardArray(multipleCardInput)); //Send cards back to GameManager
			return;
		}
		//multipleCardInput[multipleCardInput.Length-1].GetComponent<CardButtonUI>().setSelectedCardIcon((GameObject)Instantiate(Resources.Load("UISelectedCard"), pos, Quaternion.identity));	
		changeHeaderMessage("Select card " + (multipleCardInput.Length) + " out of " + multipleCardInputMaxNum, instructionHeader);
		return;

	}
	private void gotStageWeaponSelection(GameObject selected, Vector2 pos){
		//Card selected = selectedObj.GetComponent<CardButtonUI>().getCard();
		if(!checkIfArrayContainsCard(multipleCardInput, selected)) {
			addNewCardToMultipleCardArray(selected, pos);
		}
		else
		{
			displayAlert("Cannot have two weapons of the same type!");
			return;
		}
	}
	
	private void gotCardToDiscardSelection(GameObject selected, Vector2 pos){
		//Card selected = selectedObj.GetComponent<CardButtonUI>().getCard();
		addNewCardToMultipleCardArray(selected, pos);
		
		if(multipleCardInput.Length == multipleCardInputMaxNum) {  //If all the cards has been chosen
			//gm.setUserInputState(state.STANDBY);
			clearGameObjectArray(cardsToShow);	
			Card [] temp = gameObjectArrayToCardArray(multipleCardInput);
			gm.gotCardLimitReached(gameObjectArrayToCardArray(multipleCardInput)); //Send cards back to GameManager
			return;
		}
		//multipleCardInput[multipleCardInput.Length-1].GetComponent<CardButtonUI>().setSelectedCardIcon((GameObject)Instantiate(Resources.Load("UISelectedCard"), pos, Quaternion.identity));	
		changeHeaderMessage("Select card " + (multipleCardInput.Length) + " out of " + multipleCardInputMaxNum, instructionHeader);
		return;

	}
	public void askYesOrNo(Player player, string message, state messageState) {
		/*
		This method creates two buttons, yes or no. When one of these are clicked, gotButtonClick will be called and
		will have the appropriate action done according to the current state.
		*/
		clearGameObjectArray(currButtons);
		clearGameObjectArray(currIcons);
		clearGameObjectArray(cardsToShow);
		gm.setUserInputState(messageState);
		activePlayer = player;
		
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		changeHeaderMessage(message, instructionHeader);
		
		//Display yes or no buttons
		createButtonMessage(panelPosX - panelWidth/5, panelPosY + panelHeight/5, "Yes");
		createButtonMessage(panelPosX + panelWidth/5, panelPosY + panelHeight/5, "No");
	}
	
	//UI clean up
	private void clearGameObjectArray(GameObject [] arr){
		//Clears any buttons on screen
		GameObject [] toDelete =  GameObject.FindGameObjectsWithTag("CardSelected");
		for(int i =0; i< toDelete.Length; i++)
		{
			Destroy(toDelete[i]);
		}
		if(arr == null)
		{
			return;
		}
		for(int i = 0; i < arr.Length; i ++) {
			Destroy(arr[i]);
		}
		arr = null;
	}

	//Creating and modifying headers and buttons
	private GameObject createButtonMessage(float x, float y, string newText = "Button") {
		GameObject tempButton = (GameObject)Instantiate(Resources.Load("UIButton"), new Vector2(x, y), Quaternion.identity);			
		tempButton.GetComponent<ButtonUI>().init(this);
		tempButton.GetComponentInChildren<Text>().text = newText;
		int n = 0;
		if(currButtons != null) {
			
			n = currButtons.Length;
		}
		
			GameObject[] temp = new GameObject[n+1];
			for(int i = 0; i< n; i++)
			{
				temp[i] = currButtons[i];
			}
			temp[n] = tempButton;
			currButtons = temp;
		return tempButton;
	}
	
	private GameObject createHeaderMessage(float x, float y, Vector3 color, string input = "Header") {
		GameObject header;
		header = (GameObject)Instantiate(Resources.Load("UIHeader"), new Vector2(x, y), Quaternion.identity);	
		header.GetComponent<HeaderUI>().init(color);
		changeHeaderMessage(input, header);
		
		Renderer tempRenderer = header.GetComponent<Renderer>();
		
			tempRenderer.sortingOrder = 2;
		return header;
	}
	
	private void changeHeaderMessage(string input, GameObject header){
		header.GetComponent<TextMesh>().text = input;
	}
	
	public void showCard(Card cardToShow){
		if(cardCenter != null) { Destroy(cardCenter); }
		//cardCenter = (GameObject)Instantiate(Resources.Load("UICard"), new Vector2((float)(panelWidth/3.5), (float)(panelHeight/2.5)), Quaternion.identity);	
		
		cardCenter = (GameObject)Instantiate(Resources.Load("UICard"), new Vector2(panelPosX, panelPosY), Quaternion.identity);			
		//cardCenter.GetComponent<CardButtonUI>().setCard(cardToShow);
		cardCenter.GetComponent<CardUI>().init(cardToShow, this, new Vector2(panelPosX, panelPosY + panelHeight/6), new Vector2(15, 15));
	}

	public void showStage(ActiveQuest activeQuest){
		clearGameObjectArray(cardsToShow);
		clearGameObjectArray(currButtons);
		if(activeQuest.getQuest() == null)
		{
			gm.setUserInputState(state.STANDBY);
			clearGameObjectArray(cardsToShow);
		}
			
		
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Foe))) {
			showCard(activeQuest.getQuest());
			Destroy(currentBid);
			Destroy(highestBid);			
			if(playerBP == null){playerBP = createHeaderMessage(panelPosX - panelWidth/3, panelHeight/2, new Vector3(0,0,0), " ");}

			changeHeaderMessage("Player BP: " + activePlayer.getBP(), playerBP);


			changeHeaderMessage("Select the cards you wish to play in this stage", instructionHeader);	
			//createButtonMessage(panelPosX, panelPosY - panelHeight/10, "FIGHT");
			gm.setUserInputState(state.ASKINGFORCARDSINQUEST);
		}
		else if(Object.ReferenceEquals(activeQuest.getCurrentStage().GetType(), typeof(Test))) {
			showCard(activeQuest.getCurrentStage());

			
			if(currentBid == null) { currentBid = createHeaderMessage(panelPosX - panelWidth/3, panelHeight/2, new Vector3(0,0,0), " ");}
			if(highestBid == null) { highestBid = createHeaderMessage(panelPosX + panelWidth/3, panelHeight/2, new Vector3(0,0,0), " ");}
			changeHeaderMessage("Current bid: " + activePlayer.getFreeBids(), currentBid);
			changeHeaderMessage("Highest bid: " + activeQuest.getHighestBid(), highestBid);
			gm.setUserInputState(state.ASKINGFORCARDSINBID);
		}
		createButtonMessage(panelPosX - panelWidth/5, panelPosY + panelHeight/5, "Give up");
		

	}
	
	//Other utilities
	public void displayAlert(string input) {
		changeHeaderMessage(input, messageHeader);
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
	
	private Card[] getOnlyTypeFromDeck(Card[] deck, bool getFoes, bool getWeap, bool getAlly, bool getAmour, bool getTest){
		if(deck.Length == null){
			return null;
			}
		Card[] tempHand = new Card[deck.Length];
		int counter = 0;
		for(int i = 0; i < deck.Length; i++ )
		{
			if(deck[i] != null){
				if (Object.ReferenceEquals (deck [i].GetType (), typeof(Foe)) && getFoes) {
					tempHand [counter] = deck [i];
					counter++;
				} else if (Object.ReferenceEquals (deck [i].GetType (), typeof(Weapon)) && getWeap) {
					tempHand [counter] = deck [i];
					counter++;
				} else if (Object.ReferenceEquals (deck [i].GetType (), typeof(Ally)) && getAlly) {
					tempHand [counter] = deck [i];
					counter++;
				} else if (Object.ReferenceEquals (deck [i].GetType (), typeof(Amour)) && getAmour) {
					tempHand [counter] = deck [i];
					counter++;
				}
				else if (Object.ReferenceEquals (deck [i].GetType (), typeof(Test)) && getTest) {
					tempHand [counter] = deck [i];
					counter++;
				}
			}
		}
		
		Card[] newTempHand = new Card[counter];
		
		for(int i = 0; i < counter; i++)
		{
			newTempHand[i] = tempHand[i];
		}
		return newTempHand;
	}
	
	public void endQuest() {
		clearGameObjectArray(currButtons);
		Destroy(enemyBP);
		Destroy(playerBP);
		
		enemyBP = null;
		playerBP = null;
		
		Destroy(currentBid);
		Destroy(highestBid);
		
		currentBid = null;
		highestBid = null;
	}
	private void addNewCardToMultipleCardArray(GameObject selected, Vector2 pos) {
		selected.GetComponent<CardButtonUI>().setSelectedCardIcon((GameObject)Instantiate(Resources.Load("UISelectedCard"), pos, Quaternion.identity));	
		if(multipleCardInput == null){
			multipleCardInput = new GameObject[]{selected};
			multipleCardInput[0].GetComponent<CardButtonUI>().setIndexInSelection(0);
		}
		else {
			GameObject [] temp = new GameObject[multipleCardInput.Length+1];
			for(int i = 0 ; i < multipleCardInput.Length; i++) {
				temp[i] = multipleCardInput[i];
			}
			temp[temp.Length-1] = selected;
			temp[temp.Length-1].GetComponent<CardButtonUI>().setIndexInSelection(temp.Length-1);
			multipleCardInput = null;
			multipleCardInput = temp;
		}
		return;
	}
	private bool checkIfArrayContainsCard(GameObject[] arr, GameObject cardToFind) {
		if(arr == null) { return false;}
		for(int i = 0; i < arr.Length; i++){
			if(arr[i].GetComponent<CardButtonUI>().getCard().getName().Equals(cardToFind.GetComponent<CardButtonUI>().getCard().getName())){
				return true;
			}
		}
		return false;
	}
	private int getPlayersBP() {
		int extraBP = 0;
		for(int i = 0; i< multipleCardInput.Length; i++)
		{
			if(multipleCardInput[i] == null){break;}
			extraBP = extraBP + multipleCardInput[i].GetComponent<CardButtonUI>().getCard().getBP();
		}
		return activePlayer.getBP() + extraBP;
	}
	private Card[] gameObjectArrayToCardArray(GameObject[] arr){ 
		if(arr == null){ return null;}
		Card[] newArr = new Card[arr.Length];
		for(int i = 0; i< arr.Length; i++) {
			newArr[i] = arr[i].GetComponent<CardButtonUI>().getCard();
		}
		
		return newArr;
	
	}
	
	
	
	public string[] mouseOverShowHandIcon() {
		Player currPlayer = gm.getCurrentPlayer();
		GameObject blackScreen = GameObject.FindGameObjectWithTag("BlackFilter");
		Renderer blackScreenRenderer = blackScreen.GetComponent<Renderer>();
		
		
		Card [] tempCardsToShow = gm.getCurrentPlayer().getHand();
		Card [] tempCardsInPlayToShow = gm.getCurrentPlayer().getHand(true);
		int n1, n2;
		if(tempCardsToShow != null){
			n1 = tempCardsToShow.Length;
		}
		else { n1 = 0; }
		if(tempCardsInPlayToShow != null){
			n2 = tempCardsInPlayToShow.Length;
		}

		else { n2 = 0; }
		
		currPlayerHand = new GameObject[n1 + n2];
		float cardWidth = panelWidth/10;
		float cardSpacing = cardWidth/3;
		float totalDeckWidth = (n1-1)*cardSpacing + (n1-1)*cardWidth;
		string[] mouseOverShowHandUIHeaders = new string[]{
			currPlayer.getName(),
			"Rank: " + currPlayer.getRankString(),
			"Shields: " + currPlayer.getShields()
			
		};
		for(int i = 0; i< n1/2; i++) {	
			if(tempCardsToShow[i] != null){
				Vector2 pos = new Vector2(panelPosX - totalDeckWidth/4 + i*cardWidth + (i+1)*cardSpacing, panelPosY);
				currPlayerHand[i] = (GameObject)Instantiate(Resources.Load("UICard"), pos , Quaternion.identity);			
				currPlayerHand[i].GetComponent<CardUI>().init(tempCardsToShow[i], this, pos, new Vector2(15,15));
				//currPlayerHand[i].GetComponent<SpriteRenderer>().sortingLayerID  = blackScreenRenderer.sortingLayerID;				
				currPlayerHand[i].GetComponent<SpriteRenderer>().sortingOrder  = blackScreenRenderer.sortingOrder+1;			
			}
		}
		for(int i = n1/2; i< n1; i++) {	
			if(tempCardsToShow[i] != null){
				Vector2 pos = new Vector2(panelPosX - totalDeckWidth/4 + (i-n1/2)*cardWidth + ((i+1)-n1/2)*cardSpacing, panelPosY- cardWidth*2);
				currPlayerHand[i] = (GameObject)Instantiate(Resources.Load("UICard"), pos , Quaternion.identity);			
				currPlayerHand[i].GetComponent<CardUI>().init(tempCardsToShow[i], this, pos, new Vector2(15,15));
				
				currPlayerHand[i].GetComponent<SpriteRenderer>().sortingOrder = 4;
			}
		}
		
		for(int i = n1; i< n1+n2; i++) {	

			if(tempCardsInPlayToShow[i-n1] != null){
				
				Vector2 pos = new Vector2(panelPosX - panelPosX/3 + (i-n1) * cardWidth, panelPosY + panelHeight/3 - panelHeight/30);
				
				//Vector2 pos = new Vector2(panelPosX, panelPosY);
				
				currPlayerHand[i] = (GameObject)Instantiate(Resources.Load("UICard"), pos , Quaternion.identity);		
				currPlayerHand[i].GetComponent<CardUI>().init(tempCardsInPlayToShow[i-n1], this, pos, new Vector2(7,7));
				currPlayerHand[i].GetComponent<SpriteRenderer>().sortingLayerID = blackScreenRenderer.sortingLayerID;
				currPlayerHand[i].GetComponent<SpriteRenderer>().sortingOrder = blackScreenRenderer.sortingOrder+1;
			}
		}
		return mouseOverShowHandUIHeaders;
	}
	
	public void mouseLeaveShowHandIcon() {

		if(currPlayerHand.Length != null) {
			for(int i = 0; i< currPlayerHand.Length; i++) {
				Destroy(currPlayerHand[i]);
			}
			
			//currPlayerHand = null;
		}
	}
	
	public void mouseOverShowOtherPlayerIcon() {

		otherPlayerHeader = createHeaderMessage(panelPosX + panelWidth/3, panelPosY - panelHeight/25, new Vector3(0,0,0), gm.getOtherPlayerInfo(gm.getCurrentPlayer()));
	}
	
	public void mouseLeaveShowOtherPlayerIcon() {

		if(otherPlayerHeader != null) {
			Destroy(otherPlayerHeader);
		}
	}
}
