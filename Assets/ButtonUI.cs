using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour{
	UI ui;
	
	
	public void init(UI _ui)
	{
		
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		ui = _ui;
		return;
	}
	
	public void Click(){
		ui.gotButtonClick(GetComponentInChildren<Text>().text);
	}
	
	
}
