using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Controller : MonoBehaviour {

	public View gameViewTemplate;
	public View gameView;

	private Model gameModel;

	private NetworkClient client;

	private int connectedPlayers;

	public bool isAtStartup = true;

	void Update () {
		if (isAtStartup) {
			if (Input.GetKeyDown (KeyCode.S)) {
				SetupServer ();
			}

			if (Input.GetKeyDown (KeyCode.C)) {
				SetupClient ();
			}
		}
	}

	public void SetupServer()
	{
		gameModel = new Model ();
		gameModel.makeModel (4);
		Debug.Log("server");
		NetworkServer.Listen (4444);
		NetworkServer.RegisterHandler (MsgType.Connect, CallBack);
		isAtStartup = false;
	}

	public void SetupClient()
	{
		client = new NetworkClient ();
		gameView = gameViewTemplate.GetComponent<View> ();
		client.Connect ("127.0.0.1", 4444);
		client.RegisterHandler (MsgType.Scene, PostNew);
		isAtStartup = false;
	}

	public void CallBack(NetworkMessage m){
		connectedPlayers++;
		if(connectedPlayers <= 4){
			var mess = new IntegerMessage(gameModel.GetState(connectedPlayers-1));
			NetworkServer.SendToClient (connectedPlayers, MsgType.Scene, mess);
		}
	}

	public void PostNew(NetworkMessage m){
		IntegerMessage temp = m.ReadMessage<IntegerMessage> ();
		gameView.SetIntToShow (temp.value);
	}

	public void UpdateClients(){
		for (int i = 0; i < connectedPlayers; i++) {
			var mess = new IntegerMessage(gameModel.GetState(i)+i);
			NetworkServer.SendToClient (i+1, MsgType.Scene, mess);
		}
	}
		
}
