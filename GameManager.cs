using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	
	//Initialize the two decks
	AdvDeck advDeck = new AdvDeck();
	StoryDeck storyDeck = new StoryDeck();
	int playerNum;
	Player[] players;
	
	/* Work in progress
	
	Player player0 = new Player(new Card[12], 0, 0);
	Player player1 = new Player(new Card[12], 0, 0);
	Player player2 = new Player(new Card[12], 0, 0);
	
	Player[] players = new Player[3];
	
	enum GameState {DRAWINGSTORYCARD, LOOKINGFORSPONSOR, BUILDINGQUEST, LOOKINGFORQUESTPLAYERS, PLAYINGQUEST, LOOKINGFORTOURNEYPLAYERS, PLAYINGTOURNEY};
	enum PlayerState {WAITINGFORPLAYERCARDS, WAITINGFORCOMPUTERACTIONS};
	enum MenuState {WRITINGMENU, NOTWRITINGMENU};
	
	GameState gameState;
	PlayerState playerState;
	MenuState menuState;
	
	int currPlayer;
	Card tempCard;
	ActiveQuest currQuest;
	*/
	// Use this for initialization
	void Start () {
		
		/* TESTING CODE IGNORE
		players[0] = player0;
		players[1] = player1;
		players[2] = player2;
		
		
		advDeck.initDeck();
		storyDeck.initDeck();
		
		dealHands(2);
		
		gameState = GameState.DRAWINGSTORYCARD;
		playerState = PlayerState.WAITINGFORCOMPUTERACTIONS;
		menuState = MenuState.WRITINGMENU;
		
		currPlayer = 0;
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		/* TESTING CODE IGNORE
		if(gameState == GameState.DRAWINGSTORYCARD) {
			tempCard = storyDeck.drawCard();
			
			Debug.Log(tempCard.getName());
			
			if(tempCard.getType().Equals("quest"))
			{
				gameState = GameState.LOOKINGFORSPONSOR;
				currPlayer = 0;
				playerState = PlayerState.WAITINGFORPLAYERCARDS;
			}
				
			if(tempCard.getType().Equals("tourney"))
			{
				gameState = GameState.LOOKINGFORTOURNEYPLAYERS;
				playerState = PlayerState.WAITINGFORPLAYERCARDS;
			}
			
			//storyDeck.returnCard(tempCard);
		}
		if(gameState == GameState.LOOKINGFORSPONSOR) {
			if (Input.GetKeyDown(KeyCode.Y))
            {
				Debug.Log("Sponsored by player " + currPlayer);
              
				gameState = GameState.BUILDINGQUEST;
				currQuest = new ActiveQuest((QuestCard)tempCard, currPlayer);
				tempCard = null;
            }
			else if(Input.GetKeyDown(KeyCode.N))
            {
                if(currPlayer == 2)
				{
					storyDeck.returnCard(tempCard);
					tempCard = null;
					Debug.Log("No sponsor, returning.");
				}
				else
				{
					currPlayer++;
				}	
            }		
		}
		if(gameState == GameState.BUILDINGQUEST) {
			Foe[] temp = new Foe[currQuest.getStageNum()];
			for(int i = 0; i < )
		}
	
	*/
	}
	
	//Pass in a player count, it will give each player a hand of 12 adventure cards
	private void dealHands(int playerCount){
		for(int i = 0; i < playerCount+1; i++)
		{
			Card[] newHand = new Card[12];
			for(int j = 0; j < 11; j++)
			{
				newHand[j] = advDeck.drawCard();
			}
			
			players[i].setHand(newHand);
		}
		return;
	}	
}
