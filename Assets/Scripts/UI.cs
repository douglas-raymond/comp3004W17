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
			handButtons[i].GetComponent<SpriteRenderer>().sprite = getCardImage(hand[i].getName());
			handButtons[i].GetComponent<CardButtonUI>().setCard(hand[i]);
			handButtons[i].GetComponent<CardButtonUI>().setUI(this);
		}
		
		Debug.Log("handButtons "+  handButtons.Length);
		return handButtons;
	}	
	
	//Ask player for input
	public void getCardSelection(Card [] hand, Player player){
		
		activePlayer = player;
		handButtons = showHand(hand);
		return;
	}
	
	//Player has selected a card, stop showing the deck and send the chosen card to active player.
	public void cardClicked(Card selected)
	{
		
		activePlayer.cardChosen(selected);
		for(int i=0; i<handButtons.Length; i++){
			Destroy(handButtons[i]);
		}
		return;
	}
//Give the name of a card it will return the card image.
	private Sprite getCardImage(string cardTitle)
	{
		if(cardTitle.Equals("excalibur")){ return Resources.Load<Sprite>("Cards/W Excalibur");}
		if(cardTitle.Equals("lance")){ return Resources.Load<Sprite>("Cards/W Lance");}
		if(cardTitle.Equals("battleax")){ return Resources.Load<Sprite>("Cards/W Battle-ax");}
		if(cardTitle.Equals("sword")){ return Resources.Load<Sprite>("Cards/W Sword");}
		if(cardTitle.Equals("horse")){ return Resources.Load<Sprite>("Cards/W Horse");}
		if(cardTitle.Equals("dagger")){ return Resources.Load<Sprite>("Cards/W Dagger");}
		if(cardTitle.Equals("dragon")){ return Resources.Load<Sprite>("Cards/F Dragon");}
		if(cardTitle.Equals("giant")){ return Resources.Load<Sprite>("Cards/F Giant");}
		if(cardTitle.Equals("mordred")){ return Resources.Load<Sprite>("Cards/F Mordred");}
		if(cardTitle.Equals("greenknight")){ return Resources.Load<Sprite>("Cards/F Green Knight");}
		if(cardTitle.Equals("blackknight")){ return Resources.Load<Sprite>("Cards/F Black Knight");}
		if(cardTitle.Equals("evilknight")){ return Resources.Load<Sprite>("Cards/F Evil Knight");}
		if(cardTitle.Equals("saxonknight")){ return Resources.Load<Sprite>("Cards/F Saxon Knight");}
		if(cardTitle.Equals("robberknight")){ return Resources.Load<Sprite>("Cards/F Robber Knight");}
		if(cardTitle.Equals("saxons")){ return Resources.Load<Sprite>("Cards/F Saxons");}
		if(cardTitle.Equals("boar")){ return Resources.Load<Sprite>("Cards/F Boar");}
		if(cardTitle.Equals("thieves")){ return Resources.Load<Sprite>("Cards/F Thieves");}
		if(cardTitle.Equals("tovalor")){ return Resources.Load<Sprite>("Cards/T Test of Valor");}
		if(cardTitle.Equals("toquestingbeast")){ return Resources.Load<Sprite>("Cards/T Test of the Questing Beast");}
		if(cardTitle.Equals("totemptation")){ return Resources.Load<Sprite>("Cards/T Test of Temptation");}
		if(cardTitle.Equals("tomorganlefey")){ return Resources.Load<Sprite>("Cards/T Test of Morgan Le Fey");}
		if(cardTitle.Equals("galahad")){ return Resources.Load<Sprite>("Cards/A Sir Galahad");}
		if(cardTitle.Equals("arthur")){ return Resources.Load<Sprite>("Cards/A King Arthur");}
		if(cardTitle.Equals("pellinore")){ return Resources.Load<Sprite>("Cards/A King Pellinore");}
		if(cardTitle.Equals("guinevere")){ return Resources.Load<Sprite>("Cards/A Queen Guinevere");}
		if(cardTitle.Equals("iseult")){ return Resources.Load<Sprite>("Cards/A Queen Iseult");}
		if(cardTitle.Equals("gawain")){ return Resources.Load<Sprite>("Cards/A Sir Gawain");}
		if(cardTitle.Equals("lancelot")){ return Resources.Load<Sprite>("Cards/A Sir Lancelot");}
		if(cardTitle.Equals("percival")){ return Resources.Load<Sprite>("Cards/A Sir Percival");}
		if(cardTitle.Equals("tristan")){ return Resources.Load<Sprite>("Cards/A Sir Tristan");}
		else return null;
	}
}
