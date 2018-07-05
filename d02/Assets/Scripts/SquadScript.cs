using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadScript : MonoBehaviour {

	public string playerRace;
	public int annoyed;
	public LayerMask layerMask;

	private List<GameObject> units = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		checkAlive();
		if (annoyed < 1000) { annoyed += 5; }
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hits.Length == 0) {
				Command(new RaycastHit2D());
			} else {
				foreach (var elem in hits) {
					if (elem) {
						if (elem.collider && elem.collider.tag == playerRace) {
							Select(elem);
							break;
						}
					}
				}
				if (!(hits[0].collider && hits[0].collider.tag == playerRace)) {
					Command(hits[0]);
				}	
			}
		} else if (Input.GetMouseButtonDown(1)) {
			ClearSelection();
		}
	}

	void checkAlive () {
		for (int i=0; i<units.Count; i++) {
			if (units[i] && units[i].GetComponent<UnitScript_ex01>().isDead) {
				units.Remove(units[i]);
			}
		}
	}

	void Select (RaycastHit2D hit) {
		if (hit.collider.gameObject.layer == 8) {
			if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
				ClearSelection();
			}
			MusicManager.instance.Play(playerRace, "Selected");
			units.Add(hit.collider.gameObject);
			hit.collider.transform.parent = this.transform;
		} else if (hit.collider.gameObject.layer == 9) {
			// Buildings
		}
	}

	void Command (RaycastHit2D hit) {
		if (units.Count > 0) {
			if (annoyed < 0) {
				MusicManager.instance.Play(playerRace, "Annoyed");
				annoyed += 500;
			} else {
				MusicManager.instance.Play(playerRace, "Acknowledge");
				annoyed -=100;
			}
			var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (hit.collider && hit.collider.gameObject && hit.collider.tag != playerRace
					&& (hit.collider.GetType() == typeof(BoxCollider2D) || hit.collider.gameObject.layer == 9)) {
				Debug.Log("Attaaaack !");
				foreach (var unit in units) {
					unit.GetComponent<UnitScript_ex01>().TargetEnemy(hit.collider.gameObject);
				}
			} else {
				foreach (var unit in units) {
					unit.GetComponent<UnitScript_ex01>().MoveToPos(new Vector3(mousePosition.x, mousePosition.y, 1));
				}
			}
		}
	}

	void ClearSelection () {
		foreach (var unit in units) { if (unit) { unit.transform.parent = null; } }
		units.Clear();
	}
}
