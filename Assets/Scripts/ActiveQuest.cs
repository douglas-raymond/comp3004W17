using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuest {

	QuestCard quest;
	Card [] stages;
	Card [][] stageWeapons;

	Player[] players;
	Logger log = new Logger("ActiveQuest");

	int stageNum;
	Player sponsor;
	int currentStage;
	Player currentPlayer;
	int highestBid;
	//Player[] bidders;
	public int [] bids;
	Card [][] tentativeBet;
	int totalCardsUsed;
	Player highestBidder;
	Player[] playersCompletedStage;

	bool inProgress;

	int extraShields;
	public ActiveQuest(QuestCard _quest, int _extraShields) {
		log.Init ();
		quest = _quest;
		stageNum = _quest.getStages();
		players = null;
		currentStage = 0;
		highestBid = -1;
		inProgress = false;
		totalCardsUsed = 0;
		highestBidder = null;
		extraShields = _extraShields;
	}

	public void addPlayer(Player newPlayer) {

		int n = 0;

		if(players != null){n = players.Length;}
		Player[] temp = new Player[n+1];
		playersCompletedStage = new Player[n+1];
		for(int i = 0; i < n; i++)
		{
			temp[i] = players[i];
		}
		temp[n] = newPlayer;

		players = temp;
		currentPlayer = players[0];

		bids = new int[n+1];
		tentativeBet = new Card[n+1][];
	}

	public void addPlayerToStageCompleteArray(Player newPlayer) {
		int n = players.Length;
		Player[] temp = new Player[players.Length+1];
		for(int i = 0; i < players.Length; i++)
		{
			temp[i] = players[i];
		}
		temp[players.Length] = newPlayer;

		playersCompletedStage = temp;
	}

	public void deletePlayer(Player player) {
		if(players.Length == 1) {
			currentPlayer = null;
			players = null;
			quest = null;
			return;
		}
		int indexToDelete = getPlayerInt(player);
		if(indexToDelete == -1) {
			return;
		}

		Player [] newArr = new Player[players.Length-1];
		if(indexToDelete == players.Length-1) {
			for(int i = 0; i < newArr.Length; i++) {
				newArr[i] = players[i];	
			}
			currentPlayer = newArr[newArr.Length-1];
			nextStage();
		}
		else if(indexToDelete == 0) {
			for(int i = 0; i < newArr.Length; i++) {
				newArr[i] = players[i+1];
			}
			currentPlayer = newArr[0];
		}
		else {
			currentPlayer = players[indexToDelete+1];
			for(int i = 0; i < indexToDelete; i++) {
				newArr[i] = players[i];
			}
			for(int i = indexToDelete + 1; i < players.Length; i++) {
				newArr[i-1] = players[i];
			}
		}

		players = newArr;
	}
	public void finishQuest() {
		if(players == null) { return;}
		for(int i = 0; i< players.Length; i ++)
		{
			players[i].addShields(stageNum + extraShields);
		}
		quest = null;
		inProgress = false;
	}


	public void nextPlayer() {
		int playerNum = 0;
		if(players != null){
			playerNum = players.Length;
		}
		if(playerNum == 0) {
			log.log("Quest lost, No players left");
			quest = null;
		}

		int currentPlayerIndex = getPlayerInt(currentPlayer);
		addPlayerToStageCompleteArray(currentPlayer);
		if(players == null) {
			nextStage();
			return;
		}
		if(currentPlayerIndex == players.Length-1){

			currentPlayer = players[0];
			//nextStage();

		}
		else {
			currentPlayer = players[currentPlayerIndex+1];
		}

	}
	private Player calculateHighestBidder(){
		int temp = -1;
		Player tempPlayer;
		if(players == null) {
			return null;
		}
		for(int i = 0; i < players.Length; i++){
			if(temp < bids[i]) {
				temp = bids[i];
				highestBidder = players[i]; 
				tempPlayer = highestBidder;
				log.log("Highest bidder is " + highestBidder.getName());
				Debug.Log("Highest bidder is " + highestBidder.getName());
			}
		}
		Debug.Log("Highest temp is " + temp);
		highestBid = temp;


		return highestBidder;
	}


	public void endBidding() {
		Debug.Log("endBidding");
		//if(Object.ReferenceEquals(stages[currentStage].GetType(), typeof(Test))) {
		if(calculateHighestBidder() != null){

			calculateHighestBidder().discardCard(tentativeBet[getPlayerInt(calculateHighestBidder())]);
			Player winningPlayer = calculateHighestBidder();
			Debug.Log(winningPlayer.getName());
			int winningBid = bids[getPlayerInt(winningPlayer)];
			players = new Player[] {winningPlayer};
			bids = new int[] {winningBid};
			currentPlayer = players[0];
			log.log("highest bidder is " + calculateHighestBidder().getName());
		}
		else{
			resetQuest();
			return;
		}
		//}
	}
	public void nextStage() {
		Debug.Log("nextStage");
		if(currentStage + 1 == stages.Length){
			quest = null;
			//finishQuest();
			return;
		}
		else {
			currentStage++;
			if(Object.ReferenceEquals(stages[currentStage].GetType(), typeof(Test))) {
				highestBid = stages[currentStage].getMinBid();
				if(players.Length == 1 && getHighestBid() == 0) {
					highestBid = 3;
				}
			}
		}
	}
	//Getters and setters



	public void resetQuest() {
		currentStage = 0;
		stageWeapons = null;
		stages = null;
		stageWeapons = new Card[stageNum][];
		stages = new Card[stageNum];
		inProgress = false;
		totalCardsUsed = 0;
	}
	public void setSponsor(Player player){
		sponsor = player;
	}
	public void setStages(Card[] newStages){
		stages = newStages;
		totalCardsUsed = stages.Length;
		stageWeapons = new Card[stages.Length][];
		return;
	}
	public void setStageWeapons(Card[] newStageWeapons){
		Debug.Log(newStageWeapons.Length);
		stageWeapons[currentStage] = newStageWeapons;
		if(newStageWeapons != null){
			totalCardsUsed = totalCardsUsed + newStageWeapons.Length;
		}
		return;
	}
	public void setStage(int i) {
		currentStage = i;
	}
	public void setTentativeBet(Card [] bet) {
		tentativeBet[getPlayerInt(getCurrentPlayer())] = bet;

	}


	public bool placeBid(Card [] bid, int freeBids) {
		int totalBet = 0;
		if(bid == null) {
			totalBet = freeBids;
		}
		else {
			totalBet = bid.Length + freeBids;
		}

		if(highestBid >= totalBet){
			return false;
		}

		log.log("Set player of index " + getPlayerInt(currentPlayer) + " and name " + currentPlayer.getName() + " to have a bid of " + totalBet);
		tentativeBet[getPlayerInt(currentPlayer)] = bid;
		bids[getPlayerInt(currentPlayer)] = totalBet;
		for(int i = 0; i< bids.Length; i++){
			Debug.Log(bids[i]);
		}
		highestBid = totalBet;
		return true;

	}
	public void setPlayerNum(int i){
		currentPlayer = players[i];
	}
	public int getStageNum() {
		return stageNum;
	}

	public Player getSponsor() {
		return sponsor;
	}
	public Card getStage(int i) {
		return stages[i];
	}
	public Card getCurrentStage() {
		return stages[currentStage];
	}
	public int getCurrentStageNum(){
		return currentStage;
	}
	public int getHighestBid(){

		if(highestBid == -1 && Object.ReferenceEquals(stages[currentStage].GetType(), typeof(Test)))
		{
			highestBid = stages[currentStage].getMinBid();
		}
		if(players.Length == 1 && highestBid == 0) {
			highestBid = 3;
		}
		return highestBid;
	}
	public Card getQuest() {

		return quest;
	}
	public Card[] getStageWeapons(int i) {
		return stageWeapons[i];
	}
	private bool isStageSpecial(int i) {
		Card temp = stages[i];
		for(int j = 0; j < quest.getSpecialNum(); j++) {
			if(quest.getSpecialFoe(j).getName().Equals(temp.getName())) { return true; }
		}
		return false;
	}
	public int getStageBP(int i) {
		int baseBP;
		if(isStageSpecial(i)) { 
			baseBP = stages[i].getAltBP();
		}
		else {
			baseBP = stages[i].getBP();
		}
		int extraBP = 0;
		if(stageWeapons[i][0] != null)
		{
			for(int j = 0; j< stageWeapons[i].Length; j++)
			{
				extraBP = extraBP + stageWeapons[i][j].getBP();
			}
		}
		return (baseBP + extraBP);
	}
	public int getTotalCardsUsed() {
		return totalCardsUsed;
	}
	public bool isInProgress() { return inProgress;}
	public void setQuestAsInProgress(){
		inProgress = true;
		currentStage = 0;
	}
	public bool isStageDone() {
		if(getPlayerInt(currentPlayer) == players.Length-1) {
			Debug.Log(getPlayerInt(currentPlayer));
			Debug.Log(players.Length-1);
			return true;
		}
		else {
			return false;
		}
	}
	public string getStageWeaponString() {
		string stringToReturn = "";
		if(stageWeapons[currentStage][0] != null) {
			for(int i = 0; i < stageWeapons[currentStage].Length; i++){
				stringToReturn = stringToReturn + stageWeapons[currentStage][i].getName();
				if((i+1) == stageWeapons[currentStage].Length){
					stringToReturn = stringToReturn +  ", ";
				}
				else {
					stringToReturn = stringToReturn +  " ";
				}
			}
		}
		else {
			return null;
		}
		return stringToReturn;
	}

	public string getPlayersString() {
		string stringToReturn = "";
		for(int i = 0; i < players.Length; i++){
			stringToReturn = stringToReturn + players[i].getName();
			if((i+1) == players.Length){
				stringToReturn = stringToReturn +  ", ";
			}
			else {
				stringToReturn = stringToReturn +  " ";
			}
		}
		return stringToReturn;
	}
	public bool mordredSpecialAbility(Player target){

		int targetIndex = getPlayerInt(target);
		int freeBidsToRemove = target.removeAlly(quest.getName());
		log.log("freeBidsToRemove: " + freeBidsToRemove);
		if(freeBidsToRemove == -1) {
			return false;
		}
		if(Object.ReferenceEquals(stages[currentStage].GetType(), typeof(Test))){
			log.log("Removing from " + target.getName() + players[targetIndex].getName()  + "'s bid of "+ bids[targetIndex]);
			bids[targetIndex] = bids[targetIndex] - freeBidsToRemove;
			calculateHighestBidder();
		}

		return true;
	}

	public int getCurrentPlayerFreeBids(){
		return currentPlayer.getFreeBids(quest.getName());
	}
	public Player getCurrentPlayer() {
		return currentPlayer;
	}

	public Player findPlayer(string target) {
		for(int i = 0; i < players.Length; i++) {
			if(players[i].getName().Equals(target)) {
				return players[i];
			}
		}
		return null;
	}

	public int getPlayerInt(Player player) {
		int index = -1;
		Debug.Log (players.Length);
		for(int i = 0; i < players.Length; i++)
		{
			if(players[i].getName().Equals(player.getName()))
			{
				index = i;
				break;
			}
		}
		return index;
	}

	public Player getPlayer(int i) {
		return players[i];
	}


	public Player[] getPlayerArr(){
		return players;
	}


	public int getPlayerNum() {
		if(players == null) {
			return 0;
		}
		return players.Length;
	}

}
