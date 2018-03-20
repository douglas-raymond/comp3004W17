using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	public Text textTemplate;
	private Text text;
	public int intToShow = 0;

	void Start(){
		text = textTemplate.GetComponent<Text> ();
	}

	public void SetIntToShow(int n){
		intToShow = n;
		text.text = n.ToString();
	}
}
