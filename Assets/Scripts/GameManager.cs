using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	
	//Initialize the two decks
	AdvDeck advDeck = new AdvDeck();
	StoryDeck storyDeck = new StoryDeck();
	UI ui = new UI();
	
	int playerCount = 3;
	
	Player[] players;
	//Game states. There will eventually be many possible states, but for right now these two exist.
	enum state {WAITINGFORINPUT, GOTINPUT};
	state gameState = state.WAITINGFORINPUT;

	// Use this for initialization
	void Start () {
		//Create all the players and add it to the players array
		players = new Player[playerCount];
		for(int i = 0; i < playerCount; i++){
			players[i] = new Player(new Card[12], 0, 0, ui, this);
		}
			
		//Init the decks
		advDeck.initDeck();
		storyDeck.initDeck();
		
		//Deal hands to all the players
		dealHands(playerCount-1);
		
		//Asks player oen for a card selection. THis is for testing purposes
		players[0].askForCardChoice();
	}
	
	// Update is called once per frame
	void Update () {
		//When it receives input immediately ask for input again. This is for testing card deletion works
		if(gameState == state.GOTINPUT)
		{
			players[0].askForCardChoice();
			gameState = state.WAITINGFORINPUT;
		}
	}
	
	//Gets a selected card and does something with it
	public void getInput(Card card){
		gameState = state.GOTINPUT;
	}
	
	//Pass in a player count, it will give each player a hand of 12 adventure cards
	private void dealHands(int playerCount){
		for(int i = 0; i < playerCount+1; i++){
			Card[] newHand = new Card[12];
			for(int j = 0; j < 11; j++){
				newHand[j] = advDeck.drawCard();
			}
			players[i].setHand(newHand);
		}
		return;
	}	
}
