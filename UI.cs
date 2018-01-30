using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

	// Use this for initialization
	
	//The current player giving input
	Player activePlayer;
	//Cards to display
	GameObject[] handButtons;
	//public Card inputCard;
	GameManager gm;
	
	enum state {STANDBY, ASKINGFORSTAGES, ASKINGFORSINGLECARD, ASKINGFORSPONSORS};
	
	state gameState = state.STANDBY;
	
	Card[] multipleCardInput;
	public UI(GameManager _gm)
	{
		gm = _gm;
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//Prints out a given hand
	public GameObject[] showHand(Card[] hand){
		int n = hand.Length-1;
		GameObject[] handButtons = new GameObject[n+1];
		int buffer = Screen.width/(n*2);
		int offsetX = (Screen.width - n*buffer)/6;
		for(int i = 0; i< n; i++)
		{			
			handButtons[i] = (GameObject)Instantiate(Resources.Load("CardButton"), new Vector2(offsetX + i*buffer, Screen.height/7), Quaternion.identity);			
			handButtons[i].GetComponent<CardButtonUI>().setCard(hand[i]);
			handButtons[i].GetComponent<CardButtonUI>().setUI(this);
		}
		return handButtons;
	}	
	
	//Ask player for input
	public void getCardSelection(Card selected)
	{
		if (gameState == state.ASKINGFORSINGLECARD)
		{
			gotSingleCardSelection(selected);
		}
		if (gameState == state.ASKINGFORSTAGES)
		{
			gotMultipleCardSelection(selected);
		}
	}
	public void askForSingleCardSelection(Player player){
		
		activePlayer = player;
		handButtons = showHand(player.getHand());
		gameState = state.ASKINGFORSTAGES;
		return;
	}
	
	private void gotSingleCardSelection(Card selected){
		activePlayer.discardCard(selected);
		gm.gotSingleCardSelection(selected);
		for(int i=0; i< handButtons.Length; i++){
			Destroy(handButtons[i]);
		}
		return;
	}
	
	public void askForMultipleCardSelection(Player player, int n){
		
		activePlayer = player;
		handButtons = showHand(player.getHand());
		gameState = state.ASKINGFORSTAGES;
		multipleCardInput = new Card[n];
		return;
	}
	
	private void gotMultipleCardSelection(Card selected){
		for(int i = 0; i < multipleCardInput.Length; i++)
		{
			if(multipleCardInput[i] == null)
			{
				multipleCardInput[i] = selected;
				if(i == multipleCardInput.Length-1)
				{
					Debug.Log("Card Limit Reached");
					gameState = state.STANDBY;
					gm.gotMultipleCardSelection(multipleCardInput, activePlayer);
					for(int j=0; j< handButtons.Length; j++){
						Destroy(handButtons[j]);
					}
				
					multipleCardInput = null;
					return;
				}
				return;
			}
			
		}
	}
}
