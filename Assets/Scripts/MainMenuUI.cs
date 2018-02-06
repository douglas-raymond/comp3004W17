using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

	public Button NewGameBtn, OptionsBtn, ExitGameBtn;
	public GameObject PanelOptions; 

	public void NewGameBtnOnClick(){
		Debug.Log ("New Game");
	}

	public void OptionsBtnOnClick(){
		PanelOptions.SetActive (true);
		Debug.Log ("Options");
	}

	public void ExitGameBtnOnClick(){
		Debug.Log ("Exit Game");
	}

	public void OptionsExitBtnOnClick(){
		PanelOptions.SetActive (false);
		Debug.Log ("Close Options");
	}
}
