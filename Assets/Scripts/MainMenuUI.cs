using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

	public Button NewGameBtn, OptionsBtn, ExitGameBtn;
	public Dropdown DropdownAI, DropdownHuman, DropdownTestScenario;
	public GameObject PanelOptions; 
	int ai_DropdownVal,human_DropdownVal,test_DropdownVal;

	public void NewGameBtnOnClick(){
		Debug.Log ("New Game");
		PlayerPrefs.SetInt("aiPlayersNum",ai_DropdownVal);
		PlayerPrefs.SetInt ("humanPlayersNum", human_DropdownVal);
		PlayerPrefs.SetInt ("testScenario", test_DropdownVal);
		SceneManager.LoadScene ("inGame");

	}

	public void OptionsBtnOnClick(){
		PanelOptions.SetActive (true);
		Debug.Log ("Options");
	}


	public void ExitGameBtnOnClick(){
		Debug.Log ("Exit Game");
	}

	public void OptionsExitBtnOnClick ()
	{
		PanelOptions.SetActive (false);
		Debug.Log ("Close Options");
	}

	void Start()
	{
		
		if (ai_DropdownVal == null) {
			DropdownAI = PanelOptions.GetComponent<Dropdown> ();
		}		
		if (human_DropdownVal == null) {
			DropdownHuman = PanelOptions.GetComponent<Dropdown> ();
		}
		if (test_DropdownVal == null) {
			DropdownTestScenario = PanelOptions.GetComponent<Dropdown> ();		
		}
	}

	void Update()
	{
		if(DropdownAI!=null)
		ai_DropdownVal = DropdownAI.value;
		if(DropdownHuman!=null)
		human_DropdownVal = DropdownHuman.value;
		if (DropdownTestScenario != null)
		test_DropdownVal = DropdownTestScenario.value;
	}

}
