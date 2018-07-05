using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventsHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public GameObject turret;

	private towerScript tS;
	private Vector3 initialPos;

	void Awake () {
		tS = turret.GetComponent<towerScript>();
	}

	public void OnBeginDrag (PointerEventData eventData) {
		initialPos = transform.position;
	}

	public void OnDrag (PointerEventData eventData) {
		if (gameManager.gm.playerEnergy >= tS.energy) {
			transform.position = Input.mousePosition;
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider && hit.collider.tag == "empty") {
			if (gameManager.gm.playerEnergy >= tS.energy) {
				Instantiate(turret, hit.collider.transform.position, hit.collider.transform.rotation);
				Debug.Log("Ready to fire !");
				gameManager.gm.playerEnergy -= tS.energy;
			}
		}
		transform.position = initialPos;
	}
}
