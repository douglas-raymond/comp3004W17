using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderUI : MonoBehaviour {

	// Use this for initialization
	public void init(Vector3 _color){
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		GetComponent<TextMesh>().color = new Color(_color.x, _color.y, _color.z);
		return;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
