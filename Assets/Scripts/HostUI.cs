using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostUI : MonoBehaviour {

	public InputField port;
	public Button NewGameBtn, OptionsBtn, TempNewGameBtn, TempOptionsBtn;
	public Dropdown DropdownAI, DropdownHuman, DropdownTestScenario, DropdownAIStrategy;
	public GameObject PanelOptions, standard; 
	int ai_DropdownVal,human_DropdownVal,test_DropdownVal,aistrat_DropdownVal;

	public void NewGameBtnOnClick(){
		Debug.Log ("New Game");
		Debug.Log(human_DropdownVal);
		PlayerPrefs.SetInt("aiPlayerNum",ai_DropdownVal);
		PlayerPrefs.SetInt ("humanPlayerNum", human_DropdownVal);
		PlayerPrefs.SetInt ("testScenario", test_DropdownVal);
		PlayerPrefs.SetInt ("aiStrategy", aistrat_DropdownVal);
		PlayerPrefs.SetInt ("hostPort", int.Parse(port.text));
		PlayerPrefs.SetString ("hostIP", "127.0.0.1");
		SceneManager.LoadScene ("inGameHost");
	}


	public void OptionsBtnOnClick(){
		PanelOptions.SetActive (true);
		Debug.Log ("Options");
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
		if (aistrat_DropdownVal == null) {
			DropdownAIStrategy = PanelOptions.GetComponent<Dropdown> ();		
		}
		NewGameBtn = TempNewGameBtn.GetComponent<Button> ();
		OptionsBtn = TempOptionsBtn.GetComponent<Button> ();
		NewGameBtn.onClick.AddListener (NewGameBtnOnClick);
		OptionsBtn.onClick.AddListener (OptionsBtnOnClick);
	}

	void Update()
	{
		if (DropdownAI != null) {
			ai_DropdownVal = DropdownAI.value;
			PlayerPrefs.SetInt("aiPlayerNum",ai_DropdownVal);
		}
		if (DropdownHuman != null) {
			human_DropdownVal = DropdownHuman.value;
			PlayerPrefs.SetInt ("humanPlayerNum", human_DropdownVal);
		}
		if (DropdownTestScenario != null) {
			test_DropdownVal = DropdownTestScenario.value;
			PlayerPrefs.SetInt ("testScenario", test_DropdownVal);
		}
		if (DropdownAIStrategy != null) {
			aistrat_DropdownVal = DropdownAIStrategy.value;
			PlayerPrefs.SetInt ("aiStrategy",  aistrat_DropdownVal);
		}
		PlayerPrefs.Save ();
	}
}
