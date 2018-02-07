using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderUI : MonoBehaviour {

	// Use this for initialization
	public void init(){
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		return;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
