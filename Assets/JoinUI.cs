using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinUI : MonoBehaviour {
	public Button startGame;
	public InputField port, ip;

	void Start(){
		startGame = startGame.GetComponent<Button> ();
		startGame.onClick.AddListener (StartGame);
	}

	public void StartGame(){
		PlayerPrefs.SetString ("hostIP", ip.text);
		PlayerPrefs.SetInt ("hostPort", int.Parse(port.text));
		SceneManager.LoadScene ("inGameJoin");
	}

}
