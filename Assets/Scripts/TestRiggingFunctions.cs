using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRiggingFunctions {
	public int getRiggedAiStratagy(int testingScenario){
		if(testingScenario == 7||testingScenario == 8||testingScenario == 9){
			return 2;
		}
		else if (testingScenario == 10||testingScenario == 11||testingScenario == 12){
			return 1;
		}
		else return -1;
	}
	public int getRiggedAiIndex(int testingScenario){
		if(testingScenario == 7 ||testingScenario == 8 || testingScenario == 10 ||testingScenario == 11 ){
			return 1;
		}
		else if(testingScenario == 9 || testingScenario == 12){
			return 0;
		}
		else {
			return -1;	
		}
	}
	public int getRiggedPlayerCount(int testingScenario){
		if(testingScenario == 1 || testingScenario == 2|| testingScenario == 3) {
			return 4;
		}
		else {
			return 3;
		}
	}
	public Card drawRiggedCard(int test, StoryDeck storyDeck, int testScenarioStep){
		Card drawnCard = null;
		if(test == 0) {
			drawnCard = storyDeck.drawCard();
		}
		else if(test == 1) {
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("boarhunt");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("prosperity");
 
			}
			else if(testScenarioStep == 3){
				drawnCard = storyDeck.getCard("holygrail");
 
			}
			else {

				drawnCard = storyDeck.getCard("boarhunt");
 
			}
		}
		else if(test == 2) {
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("boarhunt");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("holygrail");
 
			}
		}
		else if(test == 3){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("camelot");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("boarhunt");
 
			}
		}
		else if(test == 4){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("boarhunt");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("holygrail");
 
			}
		}
		else if(test == 5){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("greenknighttest");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("holygrail");
 
			}
		}
		else if(test == 6){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("kingscall");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("holygrail");
 
			}
		}
		else if(test == 7){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("boarhunt");
 
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("holygrail");
			}
		}
		else if(test == 8){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("saxonraiders");  
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("boarhunt");
			}
		}
		else if(test == 9){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("camelot");  
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("boarhunt");
			}
		}
		else if(test == 10){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("saxonraiders");  
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("boarhunt");
			}
		}
		else if(test == 11){
			if(testScenarioStep == 1){
				drawnCard = storyDeck.getCard("saxonraiders");  
			}
			else if(testScenarioStep == 2){
				drawnCard = storyDeck.getCard("boarhunt");
			}
		}
		else {
			drawnCard = storyDeck.drawCard();
		}
		
		return drawnCard;
	}
	public Card[][] dealRiggedHand(int test, Player[] players, AdvDeck advDeck){
		Card[][] result;
		if(test == 0 || test == 6 || test == 7 || test == 9){	
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
		}
		else if (test == 1 || test == 2 || test == 3){
			result = new Card[4][];
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("saxons");
			player1NewHand[1] = advDeck.getCard("boar");
			player1NewHand[2] = advDeck.getCard("sword");
			player1NewHand[3] = advDeck.getCard("toquestingbeast");
			for(int i = 4; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			for(int i = 0; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player3NewHand = new Card[12];
			player3NewHand[0] = advDeck.getCard("horse");
			player3NewHand[1] = advDeck.getCard("excalibur");
			for(int i = 2; i < player3NewHand.Length; i++){
				player3NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player4NewHand = new Card[12];
			player4NewHand[0] = advDeck.getCard("battleax");
			player4NewHand[1] = advDeck.getCard("lance");
			for(int i = 2; i < player4NewHand.Length; i++){
				player4NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
			result[2] = player3NewHand;
			result[3] = player4NewHand;

		}
		else if(test == 4) {
			result = new Card[3][];
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("saxons");
			player1NewHand[1] = advDeck.getCard("tovalor");
			for(int i = 2; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("amour");
			player2NewHand[1] = advDeck.getCard("amour");
			for(int i = 2; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player3NewHand = new Card[12];
			player3NewHand[0] = advDeck.getCard("mordred");
			for(int i = 1; i < player3NewHand.Length; i++){
				player3NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
			result[2] = player3NewHand;
		}
		else if(test == 5) {
			result = new Card[3][];
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("gawain");
			player1NewHand[1] = advDeck.getCard("percival");
			for(int i = 2; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("thieves");
			player2NewHand[1] = advDeck.getCard("thieves");
			player2NewHand[2] = advDeck.getCard("thieves");
			player2NewHand[3] = advDeck.getCard("saxons");
			player2NewHand[4] = advDeck.getCard("saxons");
			player2NewHand[5] = advDeck.getCard("saxons");
			player2NewHand[6] = advDeck.getCard("robberknight");
			player2NewHand[7] = advDeck.getCard("robberknight");
			player2NewHand[8] = advDeck.getCard("mordred");
			for(int i = 9; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player3NewHand = new Card[12];
			player3NewHand[0] = advDeck.getCard("mordred");
			for(int i = 1; i < player3NewHand.Length; i++){
				player3NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
			result[2] = player3NewHand;
		}
		else if(test == 7) {
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("thieves");
			player2NewHand[1] = advDeck.getCard("thieves");
			player2NewHand[2] = advDeck.getCard("thieves");
			player2NewHand[3] = advDeck.getCard("saxons");
			player2NewHand[4] = advDeck.getCard("saxons");
			player2NewHand[5] = advDeck.getCard("saxons");
			player2NewHand[6] = advDeck.getCard("robberknight");
			player2NewHand[7] = advDeck.getCard("robberknight");
			player2NewHand[8] = advDeck.getCard("tovalor");
			for(int i = 9; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			result[1] = player2NewHand;
		}
		else if(test == 8) {
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("thieves");
			player1NewHand[1] = advDeck.getCard("saxons");
			player1NewHand[2] = advDeck.getCard("boar");
			player1NewHand[3] = advDeck.getCard("toquestingbeast");
			for(int i = 4; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("thieves");
			player2NewHand[1] = advDeck.getCard("saxons");
			player2NewHand[2] = advDeck.getCard("horse");
			player2NewHand[3] = advDeck.getCard("horse");
			player2NewHand[4] = advDeck.getCard("lance");
			player2NewHand[5] = advDeck.getCard("excalibur");
			for(int i = 4; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
		}
		else if(test == 9) {
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("thieves");
			player1NewHand[1] = advDeck.getCard("saxons");
			player1NewHand[2] = advDeck.getCard("boar");
			player1NewHand[3] = advDeck.getCard("greenknight");
			for(int i = 4; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("thieves");
			player2NewHand[1] = advDeck.getCard("saxons");
			player2NewHand[2] = advDeck.getCard("horse");
			player2NewHand[3] = advDeck.getCard("horse");
			player2NewHand[4] = advDeck.getCard("lance");
			player2NewHand[5] = advDeck.getCard("excalibur");
			for(int i = 6; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
		}
		else if(test == 10) {
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("thieves");
			player1NewHand[1] = advDeck.getCard("saxons");
			player1NewHand[2] = advDeck.getCard("boar");
			player1NewHand[3] = advDeck.getCard("greenknight");
			for(int i = 4; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("thieves");
			player2NewHand[1] = advDeck.getCard("saxons");
			player2NewHand[2] = advDeck.getCard("horse");
			player2NewHand[3] = advDeck.getCard("horse");
			player2NewHand[4] = advDeck.getCard("lance");
			player2NewHand[5] = advDeck.getCard("excalibur");
			for(int i = 6; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
		}
		else if(test == 11) {
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
			Card[] player1NewHand = new Card[12];
			player1NewHand[0] = advDeck.getCard("thieves");
			player1NewHand[1] = advDeck.getCard("saxons");
			player1NewHand[2] = advDeck.getCard("toquestingbeast");
			player1NewHand[3] = advDeck.getCard("greenknight");
			for(int i = 4; i < player1NewHand.Length; i++){
				player1NewHand[i] = advDeck.drawCard();
			}
			
			Card[] player2NewHand = new Card[12];
			player2NewHand[0] = advDeck.getCard("thieves");
			player2NewHand[1] = advDeck.getCard("saxons");
			player2NewHand[2] = advDeck.getCard("horse");
			player2NewHand[3] = advDeck.getCard("horse");
			player2NewHand[4] = advDeck.getCard("lance");
			player2NewHand[5] = advDeck.getCard("excalibur");
			for(int i = 6; i < player2NewHand.Length; i++){
				player2NewHand[i] = advDeck.drawCard();
			}
			result[0] = player1NewHand;
			result[1] = player2NewHand;
		}
		else {
			result = new Card[3][];
			for(int i = 0; i < 3; i++){
				Card[] newHand = new Card[12];
				for(int j = 0; j < newHand.Length; j++){
					newHand[j] = advDeck.drawCard();
					//log.log("Gave " + players[i].getName() + " a " + newHand[j].getName() + " card");
				}
				result[i] = newHand;
			}
		}
		
		return result;
	}
}
