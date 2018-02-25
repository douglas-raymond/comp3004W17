using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowHandUI : MonoBehaviour {

	// Use this for initialization
	UI ui;
	
	
	GameObject blackScreen;
	
	float panelWidth;
	float panelHeight;
	float panelPosX;
	float panelPosY;
	GameObject[] headers;
	public void init(UI _ui) {
		ui = _ui;
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Canvas>().transform);
		GameObject canvas = GameObject.Find("Canvas");
		panelWidth = canvas.GetComponent<RectTransform>().rect.width * canvas.GetComponent<RectTransform>().localScale.x;
		panelHeight = canvas.GetComponent<RectTransform>().rect.height * canvas.GetComponent<RectTransform>().localScale.y;
		panelPosX = canvas.GetComponent<RectTransform>().position.x;
		panelPosY = canvas.GetComponent<RectTransform>().position.y;
		
		Debug.Log("dadsda");
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	
	public void PointerEnter()
    {
		//GameObject showHandUITemp = (GameObject)Instantiate(Resources.Load("UIShowHand"), new Vector2(panelPosX + panelWidth/3, panelPosY + panelHeight/4) , Quaternion.identity);
        blackScreen = (GameObject)Instantiate(Resources.Load("UIBlackScreen"), new Vector2(panelPosX, panelPosY), Quaternion.identity);
		blackScreen.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,.85f);
		
		
		string[] headerStrings = ui.mouseOverShowHandIcon();
		headers = new GameObject[headerStrings.Length +2 ];
		Renderer blackScreenRenderer = blackScreen.GetComponent<Renderer>();
		for(int i = 0; i < headerStrings.Length; i++) {
			headers[i] = createHeader(headerStrings[i],  new Vector2(panelPosX - panelPosX/3 - panelPosX/3 - panelPosX/5 , panelPosY + panelHeight/3 - i*(panelHeight/20)), blackScreenRenderer);			
		}
		
		
		headers[headerStrings.Length] = createHeader("Cards in play",  new Vector2(panelPosX , panelPosY + panelHeight/2- (panelHeight/20)), blackScreenRenderer);	
		headers[headerStrings.Length] = createHeader("Current hand",  new Vector2(panelPosX , panelPosY + panelHeight/2- (panelHeight/3)), blackScreenRenderer);			
    }
	
	public void PointerExit()
    {
		Destroy(blackScreen);
		for(int i = 0; i < headers.Length; i++) {
			Destroy(headers[i]);
		}
        ui.mouseLeaveShowHandIcon();
    }
	
	
	private GameObject createHeader(string text, Vector2 pos, Renderer _blackScreenRenderer) {
			GameObject temp = (GameObject)Instantiate(Resources.Load("UIHeader"), pos , Quaternion.identity);
			temp.GetComponent<HeaderUI>().init(new Vector3(1,1,1));
			temp.GetComponent<TextMesh>().text = text;
			Renderer tempRenderer = temp.GetComponent<Renderer>();
			tempRenderer.sortingLayerID = _blackScreenRenderer.sortingLayerID;
			tempRenderer.sortingOrder = _blackScreenRenderer.sortingOrder+1;
			return temp;
	}

}
