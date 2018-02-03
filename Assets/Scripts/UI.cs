using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	// Use this for initialization
	
	//The current player giving input
	Player activePlayer;
	//Cards to display
	GameObject[] currButtons;
	//public Card inputCard;
	GameManager gm;
	
	enum state {STANDBY, ASKINGFORSTAGES, ASKINGFORSINGLECARD, ASKINGFORSPONSORS, ASKINGFORPLAYERS};
	
	state gameState = state.STANDBY;
	
	Card[] multipleCardInput;
	
	GameObject header;  //This gives th current instruction to the player
	GameObject headerCurrPlayer; //This says which player's turn it is
	public UI(GameManager _gm) {
		gm = _gm;
		header  = createHeaderMessage(Screen.width/2, Screen.height - Screen.height/3, "Current action required");	
		headerCurrPlayer  = createHeaderMessage(Screen.width/2, Screen.height - Screen.height/4, "Current player's turn");	
	}
	
	void Start () { }
	
	// Update is called once per frame
	void Update () { }
	
	//Prints out a given hand
	public GameObject[] showHand(Card[] hand){
		
		int n = hand.Length-1;
		GameObject[] currButtons = new GameObject[n+1];
		int buffer = Screen.width/(n*2);
		int offsetX = (Screen.width - n*buffer)/6;
		for(int i = 0; i< n; i++)
		{			
			currButtons[i] = (GameObject)Instantiate(Resources.Load("CardButton"), new Vector2(offsetX + i*buffer, Screen.height/7), Quaternion.identity);			
			currButtons[i].GetComponent<CardButtonUI>().setCard(hand[i]);
			currButtons[i].GetComponent<CardButtonUI>().setUI(this);
		}
		return currButtons;
	}	
	
	//Ask player for input
	public void getCardSelection(Card selected){
		/*
		This method is called when a card is clicked. If we are expecting only a single care selection, 
		then it calls the gotSingleCardSelection() method which sends the card back to the GameManager.
		
		Other wise gotMultipleCardSelection() is called, which updates the multipleCardInput() array that is 
		sent once it is filled up with selections.
		*/
		if (gameState == state.ASKINGFORSINGLECARD)	{
			changeHeaderMessage("Select a card to play", header);
			gotSingleCardSelection(selected);
		}
		if (gameState == state.ASKINGFORSTAGES) {
			gotMultipleCardSelection(selected);
		}
	}
	
	public void askForSingleCardSelection(Player player){
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	//Say who's turn it is.
		activePlayer = player; //Updates the current player
		currButtons = showHand(player.getHand()); //Display the hand
		gameState = state.ASKINGFORSTAGES; 
		return;
	}
	
	private void gotSingleCardSelection(Card selected){
		activePlayer.discardCard(selected); 
		gm.gotSingleCardSelection(selected);
		clearScreen();
		return;
	}
	
	public void askForMultipleCardSelection(Player player, int n){
		activePlayer = player;
		currButtons = showHand(player.getHand()); //Display the cards
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
					Debug.Log("Card Limit Reached");
					changeHeaderMessage("Stages selected", header); //Update header
					gameState = state.STANDBY;
					gm.endQuestSetup(multipleCardInput); //Send cards back to GameManager
					clearScreen();
					multipleCardInput = null;
					return;
				}
				changeHeaderMessage("Select card " + (i+2) + " out of " + multipleCardInput.Length, header);
				return;
			}
		}
	}

	public void askYesOrNo(Player player) {
		gameState = state.ASKINGFORSPONSORS;
		activePlayer = player;
		
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		changeHeaderMessage("Do you want to sponsor this quest?", header);
		
		//Display yes or no buttons
		currButtons = new GameObject[2];
		currButtons[0] = createButtonMessage(Screen.width/3, Screen.height/2, "Yes");
		currButtons[1] = createButtonMessage(Screen.width - Screen.width/3, Screen.height/2, "No");
	}
	
	public void askJoinOrDecline(Player player) {
		clearScreen();
		gameState = state.ASKINGFORPLAYERS;
		activePlayer = player;
		
		changeHeaderMessage(activePlayer.getName() + "'s turn", headerCurrPlayer);	
		changeHeaderMessage("Do you want to join this quest?", header);
		
		//Display yes or no buttons
		GameObject [] currButtons = new GameObject[2];
		currButtons[0] = createButtonMessage(Screen.width/3, Screen.height/2, "Join");
		currButtons[1] = createButtonMessage(Screen.width - Screen.width/3, Screen.height/2, "Decline");
	
	}
	
	public void gotButtonClick(string input) {
		//This method is called when a button is clicked
		if(gameState == state.ASKINGFORSPONSORS) { //If the game is current looking for sponsors
			if(input.Equals("Yes")) { //If the current player wants to be sponsor
				gameState = state.STANDBY; 
				clearScreen();
				gm.startQuestSetup(); //Tell GameManager to set the current player as sponsor
			}
			
			else {
				gm.getSponsor(); //Other wise have GameManager call getSponsor for the next player.
			}
		}
		else if(gameState == state.ASKINGFORPLAYERS){
			if(input.Equals("Join")) { //If the current player wants to be sponsor 
				clearScreen();
				gm.gotPlayer(activePlayer); //Tell GameManager to set the current player as sponsor
			}
			
			else {
				gm.getPlayers(); //Other wise have GameManager call getSponsor for the next player.
			}
		}
	}

	//Header functions
	private void clearScreen(){
		for(int i = 0; i < currButtons.Length; i ++) {
			Destroy(currButtons[i]);
		}
		Debug.Log(currButtons.Length);
		GameObject [] temp = new GameObject[1];
		currButtons = temp;
	}
	
	private GameObject createButtonMessage(int x, int y, string newText = "Button") {
		GameObject tempButton = (GameObject)Instantiate(Resources.Load("UIButton"), new Vector2(x, y), Quaternion.identity);			
		tempButton.GetComponent<ButtonUI>().init(this);
		tempButton.GetComponentInChildren<Text>().text = newText;
		
		return tempButton;
	}
	
	private GameObject createHeaderMessage(int x, int y, string input = "Header") {
		GameObject header;
		header = (GameObject)Instantiate(Resources.Load("UIHeader"), new Vector2(x, y), Quaternion.identity);	
		header.GetComponent<HeaderUI>().init();
		changeHeaderMessage(input, header);
		return header;
	}
	
	private void changeHeaderMessage(string input, GameObject header){
		header.GetComponent<Text>().text = input;
	}
}
