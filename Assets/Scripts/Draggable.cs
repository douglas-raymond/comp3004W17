using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Vector2 dragOffset = new Vector2(0f, 0f);
	Transform parentReturn = null;

	public void OnBeginDrag(PointerEventData eventData){
		Debug.Log ("Begin Dragging object");
		dragOffset = eventData.position - (Vector2)this.transform.position;
		parentReturn = this.transform.parent;
		this.transform.SetParent (this.transform.parent.parent);
	}

	public void OnDrag(PointerEventData eventData){
		Debug.Log ("Dragging object");
		this.transform.position = eventData.position-dragOffset;
	}

	public void OnEndDrag(PointerEventData eventData){
		Debug.Log ("End Dragging object");
		this.transform.SetParent (parentReturn);
	}
}
