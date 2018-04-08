using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

	public Button NewGameBtn, OptionsBtn, ExitGameBtn, HostGameBtn, JoinGameBtn;
	public Dropdown DropdownAI, DropdownHuman, DropdownTestScenario, DropdownAIStrategy;
	public GameObject PanelOptions;
	public InputField HostIP, HostPort;
	public Text TextAlert;
	int ai_DropdownVal,human_DropdownVal,test_DropdownVal,aistrat_DropdownVal;
	string ip_InputVal,port_InputVal;

	public void NewGameBtnOnClick(){
		Debug.Log ("New Game");
		Debug.Log(human_DropdownVal);
		PlayerPrefs.SetInt("aiPlayerNum",ai_DropdownVal);
		PlayerPrefs.SetInt ("humanPlayerNum", human_DropdownVal);
		PlayerPrefs.SetInt ("testScenario", test_DropdownVal);
		PlayerPrefs.SetInt ("aiStrategy", aistrat_DropdownVal);
		SceneManager.LoadScene ("inGame");

	}

	public void OptionsBtnOnClick(){
		PanelOptions.SetActive (true);
		Debug.Log ("Options");
	}


	public void ExitGameBtnOnClick(){
		Debug.Log ("Exit Game");
		Application.Quit();
	}

	public void OptionsExitBtnOnClick ()
	{
		PanelOptions.SetActive (false);
		Debug.Log ("Close Options");
	}

	public void JoinBtnOnClick()
	{
		PlayerPrefs.SetString("IP", ip_InputVal);
		PlayerPrefs.SetString("Port", port_InputVal);
		Debug.Log (ip_InputVal);
		Debug.Log(port_InputVal);
	}

	public void HostBtnOnClick()
	{
		PlayerPrefs.SetString("Port", port_InputVal);
		Debug.Log(port_InputVal);
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
		if (HostIP == null) {
			HostIP = PanelOptions.GetComponent<InputField> ();		
		}
		if (HostPort == null) {
			HostPort = PanelOptions.GetComponent<InputField> ();		
		}
		if (TextAlert == null) 
		{
			TextAlert = PanelOptions.GetComponent<Text> ();
		}
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
		if (HostIP != null) {
			ip_InputVal = HostIP.text;		
		}
		if (HostPort != null) {
			port_InputVal = HostPort.text;		
		}

		PlayerPrefs.Save ();
	}

}
