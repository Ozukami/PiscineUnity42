using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour {

	public GameObject mainTarget;
	public GameObject mainBuilding;
	public bool isAlerted = false;

	private UnitScript_ex01 unitScript;
	private bool sub = false;

	// Use this for initialization
	void Awake () {
		unitScript = gameObject.GetComponent<UnitScript_ex01>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!sub) {
			unitScript.mainBuilding.GetComponent<Building>().OnTownAttacked += TownAttackedListener;
			unitScript.mainBuilding.GetComponent<Building>().OnTownSafe += TownSafeListener;
			sub = true;
		}
		if (!unitScript.spawn && unitScript.enemyTarget == null) {
			unitScript.TargetEnemy(unitScript.mainTarget);
		}
		if (isAlerted) {
			if (Vector3.Distance(transform.position, unitScript.mainBuilding.transform.position) > 4) {
				unitScript.MoveToPos(unitScript.mainBuilding.transform.position);
			} else if (Vector3.Distance(transform.position, unitScript.mainBuilding.transform.position) > 2) {
				isAlerted = false;
			} else if (Vector3.Distance(transform.position, unitScript.mainBuilding.transform.position) < 2) {
				isAlerted = false;
				unitScript.MoveToPos(unitScript.mainTarget.transform.position);
			}
		}
	}

	void TownAttackedListener () {
		Debug.Log("Received event: On Town Attacked");
		isAlerted = true;
		if (unitScript) { unitScript.enemyTarget = null; }
	}

	void TownSafeListener () {
		Debug.Log("Received event: On Town Safe");
		isAlerted = false;
		if (unitScript) { unitScript.MoveToPos(unitScript.mainTarget.transform.position); }
	}

	void OnTriggerEnter2D (Collider2D collider) {
		AquireTarget(collider);
	}

	void OnTriggerStay2D (Collider2D collider) {
		AquireTarget(collider);
	}

	void OnTriggerExit2D (Collider2D collider) {
		AquireTarget(collider);
	}

	void AquireTarget (Collider2D collider) {
		if (tag != collider.tag) {
			if (collider.gameObject.layer == 8 && !collider.GetComponent<UnitScript_ex01>().isDead) {
				if (unitScript.enemyTarget == null) {
					unitScript.TargetEnemy(collider.gameObject);
				} else if (Vector3.Distance(transform.position, unitScript.enemyTarget.transform.position) > Vector3.Distance(transform.position, collider.transform.position)) {
					unitScript.TargetEnemy(collider.gameObject);
				} else if (unitScript.enemyTarget.gameObject.layer == 9 && collider.gameObject.layer == 8) {
					unitScript.TargetEnemy(collider.gameObject);
				}
			} else if (collider.gameObject.layer == 9 && unitScript.enemyTarget == null) {
				unitScript.TargetEnemy(collider.gameObject);
			} else if (collider.gameObject.layer == 9 && Vector3.Distance(transform.position, unitScript.enemyTarget.transform.position) > Vector3.Distance(transform.position, collider.transform.position)) {
				unitScript.TargetEnemy(collider.gameObject);
			}
		} 
	}
}
