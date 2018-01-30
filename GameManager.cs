using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	
	//Initialize the two decks
	AdvDeck advDeck = new AdvDeck();
	StoryDeck storyDeck = new StoryDeck();
	StoryCard thisTurnCard;
	UI ui = new UI();
	
	int playerCount = 3; //so we have player 0, 1, and 2
	bool running = true;
	int activePlayer = 0; //initialize first player as active
	
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
		dealHands(playerCount);

		while (running) {
			thisTurnCard = StoryDeck.drawCard ();
			evaluateStory (thisTurnCard);
			/*check for promotions and victory here*/
			activePlayer = (activePlayer + 1) % playerCount; 
		}
	}

	//Track splitter that evaluates based on card type.
	public void evaluateStory(Card storyCard){
		switch (storyCard.getType ()) {
		case "quest":
			createActiveQuest (storyCard);
			break;
		case "tourney":
			createTourney (storyCard);
			break;
		case "event":
			doEvent (storyCard);
			break;
		}

	}

	//Event handling. Pretty much done because events are handled in the cards themselves.
	public void doEvent(EventCard storyCard){
		storyCard.runEvent ();
	}

	//#WIP: Tourney handling
	public void createTourney(Card storyCard){
		return;
	}

	//#WIP: Quest handling.
	public void createActiveQuest(Card storyCard){
		int sponsorPlayer = -1;
		for (int i = activePlayer; i < activePlayer + playerCount; i++) {
			if (offerSponsorship (players [i % playerCount])) {
				sponsorPlayer = i;
			}
		}
		if (sponsorPlayer == -1) {
			return;
		}
		ActiveQuest thisQuest = new ActiveQuest (storyCard, sponsorPlayer);
		/*tell the sponsor to set up the quest here*/
		/*run the quest for the other players here*/
		/*evaluate quest results here*/
		/*draw adventure cards for sponsor player here*/
		return;
			
	}

	//#PLACEHOLDER: UI message for "wanna sponsor? yes/no"
	public bool offerSponsorship(Player player){
		bool accept = false;
		/*send UI message to player offering sponsorship here...*/
		if (accept) {
			return true;
		}
		return false;
	
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
