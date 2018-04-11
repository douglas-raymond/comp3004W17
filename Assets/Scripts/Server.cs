using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : MonoBehaviour {
	//the server has access to the gm and an abstacted ui
	private GameManager gm;
	private NetworkedUI ui;
	private int numPlayersConnected;
	public Text serverUpdate;

	Client testClient1;

	void Start(){
		Debug.Log ("here");
		ui = new NetworkedUI (null);
		gm = new GameManager (ui);
		ui.AssignGM (gm);
		numPlayersConnected = 1;
		NetworkServer.Listen(PlayerPrefs.GetInt("hostPort"));
		serverUpdate.GetComponent<Text> ().text = "Creating new lobby...";
		NetworkServer.RegisterHandler(MsgType.Connect, PlayerConnected);
		NetworkServer.RegisterHandler (Msg.startQuestSetup, startQuestSetup);
		NetworkServer.RegisterHandler (Msg.setUserInputState, setUserInputState);
		NetworkServer.RegisterHandler (Msg.gotPlayer, gotPlayer);
		NetworkServer.RegisterHandler (Msg.gotPlayerTourney, gotPlayerTourney);
		NetworkServer.RegisterHandler (Msg.questAttack, questAttack);
		NetworkServer.RegisterHandler (Msg.forfeitQuest, forfeitQuest);
		NetworkServer.RegisterHandler (Msg.gotTournamentCards, gotPlayerTournamentCards);
		NetworkServer.RegisterHandler (Msg.bidPhase, bidPhase);
		NetworkServer.RegisterHandler (Msg.endQuest, endQuest);
		NetworkServer.RegisterHandler (Msg.endStage, endStage);
		NetworkServer.RegisterHandler (Msg.endStageWeaponSetup, endStageWeaponSetup);
		NetworkServer.RegisterHandler (Msg.gotMordredTarget, gotMordredTarget);
		NetworkServer.RegisterHandler (Msg.endQuestSetup, endQuestSetup);
		NetworkServer.RegisterHandler (Msg.gotCardLimitReached, gotCardLimitReached);
		NetworkServer.RegisterHandler (Msg.getSponsor, getSponsor);
		NetworkServer.RegisterHandler (Msg.getUserInputState, getUserInputState);
		NetworkServer.RegisterHandler (Msg.getCurrentPlayer, getCurrentPlayer);
		NetworkServer.RegisterHandler (Msg.getOtherPlayerInfo, getOtherPlayerInfo);
		NetworkServer.RegisterHandler (Msg.mouseOverShowHand, mouseOverShowHand);
		NetworkServer.RegisterHandler (Msg.mouseOverShowOther, mouseOverShowOther);
		NetworkServer.RegisterHandler (Msg.checkCardSelection, checkCardSelection);
		NetworkServer.RegisterHandler (Msg.checkCardRemoval, checkCardRemoval);
		NetworkServer.RegisterHandler (Msg.checkButtonClick, checkButtonClick);
	}

	public void PlayerConnected(NetworkMessage m){
		Debug.Log ("Player Connected");
		serverUpdate.GetComponent<Text>().text = numPlayersConnected+" Players Connected - Waiting for more players...";
		IntegerMessage message = new IntegerMessage ();
		message.value = numPlayersConnected;
		NetworkServer.SendToClient (numPlayersConnected, Msg.confirmConnect, message);
		numPlayersConnected++;
		//this should be a variable
		if (numPlayersConnected == 5) {
			serverUpdate.GetComponent<Text>().text = numPlayersConnected+" Players Connected - Game Ongoing";
			EmptyMessage startMessage = new EmptyMessage ();
			NetworkServer.SendToAll (Msg.confirmConnect, startMessage);
			gm.gameStart ();
		}
	}

	public void startQuestSetup(NetworkMessage m){
		gm.startQuestSetup ();
	}

	public void setUserInputState(NetworkMessage m){
		SetUserInputStateMessage temp = m.ReadMessage<SetUserInputStateMessage> ();
		gm.setUserInputState (temp.newState);
	}

	public void gotPlayer(NetworkMessage m){
		GotPlayerMessage message = m.ReadMessage<GotPlayerMessage> ();
		gm.gotPlayer (MessageToPlayer(message.name));
	}

	public void gotPlayerTourney(NetworkMessage m){
		GotPlayerTourneyMessage message = m.ReadMessage<GotPlayerTourneyMessage> ();
		gm.gotPlayerTourney (MessageToPlayer(message.name));
	}

	public void questAttack(NetworkMessage m){
		SelectionMessage message = m.ReadMessage<SelectionMessage> ();
		gm.questAttack (MessageToSelection (message.selection, message.connectionID));
	}

	public void  forfeitQuest(NetworkMessage m){
		gm.forfeitQuest ();
	}

	public void gotPlayerTournamentCards(NetworkMessage m){
		SelectionMessage message = m.ReadMessage<SelectionMessage> ();
		gm.gotTournamentCards (MessageToSelection (message.selection, message.connectionID));
	}

	public void bidPhase(NetworkMessage m){
		SelectionMessage message = m.ReadMessage<SelectionMessage> ();
		gm.bidPhase (MessageToSelection (message.selection, message.connectionID));
	}

	public void endQuest(NetworkMessage m){
		EndQuestMessage message = m.ReadMessage<EndQuestMessage> ();
		if (message.input != null) {
			gm.endQuest (message.input);
		} else {
			gm.endQuest ();
		}
	}

	public void endStage(NetworkMessage m){
		gm.endStage ();
	}

	public void endStageWeaponSetup(NetworkMessage m){
		SelectionMessage message = m.ReadMessage<SelectionMessage> ();
		gm.endStageWeaponSetup (MessageToSelection(message.selection, message.connectionID));
	}

	public void gotMordredTarget(NetworkMessage m){
		GotMordredTargetMessage message = m.ReadMessage<GotMordredTargetMessage> ();
		gm.gotMordredTarget (message.target);
	}

	public void endQuestSetup(NetworkMessage m){
		SelectionMessage message = m.ReadMessage<SelectionMessage> ();
		gm.endQuestSetup (MessageToSelection (message.selection, message.connectionID));
	}

	public void gotCardLimitReached(NetworkMessage m){
		SelectionMessage message = m.ReadMessage<SelectionMessage> ();
		gm.gotCardLimitReached (MessageToSelection (message.selection, message.connectionID));
	}

	public void getSponsor(NetworkMessage m){
		gm.getSponsor ();
	}

	public void mouseOverShowHand(NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		Player tempPlayer = gm.getPlayerWithID (connectionID);;
		GetCurrentPlayerMessage message = new GetCurrentPlayerMessage (tempPlayer.getHand ().Length);
		for (int i = 0; i < tempPlayer.getHand ().Length; i++) {
			message.hand [i] = tempPlayer.getHand () [i].getName ();
		}
		message.name = tempPlayer.getName ();
		message.BP = tempPlayer.getBP ("null");
		message.shields = tempPlayer.getShields ();
		message.rank = tempPlayer.getRank ();
		//SendToClient
		NetworkServer.SendToClient(connectionID, Msg.mouseOverShowHand, message);
	}

	public void mouseOverShowOther(NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		StringMessage message = new StringMessage ();
		message.value = gm.getOtherPlayerInfo (gm.getPlayerWithID (connectionID));
		//SendToClient
		NetworkServer.SendToClient(connectionID, Msg.mouseOverShowOther, message);
	}

	public void checkCardSelection(NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		GetUserInputStateMessage message = new GetUserInputStateMessage ();
		message.newState = gm.getUserInputState ();
		//SendToClient
		NetworkServer.SendToClient(connectionID, Msg.checkCardSelection, message);
	}

	public void checkCardRemoval (NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		GetUserInputStateMessage message = new GetUserInputStateMessage ();
		message.newState = gm.getUserInputState ();
		//SendToClient
		NetworkServer.SendToClient(connectionID, Msg.checkCardRemoval, message);
	}

	public void checkButtonClick(NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		GetUserInputStateMessage message = new GetUserInputStateMessage ();
		message.newState = gm.getUserInputState ();
		//SendToClient
		NetworkServer.SendToClient(connectionID, Msg.checkButtonClick, message);
	}

	//getters
	public void getUserInputState(NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		state newState = gm.getUserInputState ();
		GetUserInputStateMessage message = new GetUserInputStateMessage ();
		message.newState = newState;
		//SendToClient
		NetworkServer.SendToClient (connectionID, Msg.getUserInputState, message);
	}

	public void getCurrentPlayer(NetworkMessage m){
		int connectionID = m.ReadMessage<IntegerMessage> ().value;
		Player tempPlayer = gm.getPlayerWithID (connectionID);
		GetCurrentPlayerMessage message = new GetCurrentPlayerMessage (tempPlayer.getHand ().Length);
		for (int i = 0; i < tempPlayer.getHand ().Length; i++) {
			message.hand [i] = tempPlayer.getHand () [i].getName ();
		}
		message.name = tempPlayer.getName ();
		message.BP = tempPlayer.getBP ("null");
		message.shields = tempPlayer.getShields ();
		message.rank = tempPlayer.getRank ();
		//SendToClient
		NetworkServer.SendToClient (connectionID, Msg.getCurrentPlayer, message);
	}

	public void getOtherPlayerInfo(NetworkMessage m){
		GetOtherPlayerMessage message = m.ReadMessage<GetOtherPlayerMessage> ();
		Player tempPlayer = MessageToPlayer (message.name);
		int connectionID = message.connectionID;
		string stringToReturn = gm.getOtherPlayerInfo (tempPlayer);
		StringMessage newMessage = new StringMessage ();
		newMessage.value = stringToReturn;
		//SendToClient
		NetworkServer.SendToClient(connectionID, Msg.getOtherPlayerInfo, newMessage);
	}

	//helpers
	public Player MessageToPlayer(string name){
		return gm.IdentifyPlayer (name);
	}

	public Card[] MessageToSelection(string[] selection, int id){
		return gm.IdentifySelection (selection, id);
	}
}
