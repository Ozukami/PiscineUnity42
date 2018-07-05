using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public delegate void TownEvent();
	public event TownEvent OnTownAttacked;
	public event TownEvent OnTownSafe;

	public GameObject mainTarget;
	public GameObject mainBuilding;
	public string type;
	public string race;
	public string buildingName;
	public int maxHealthPoints;
	public int healthPoints;
	public int armor;
	public float spawnTime;
	public GameObject unit;
	public Transform spawnFlag;
	public bool isDestroyed = false;
	public bool alert = false;

	private float timer = 10;
	private float timeWithoutTakingDamage;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (healthPoints <= 0) {
			if (!isDestroyed) { Destroy(); }
		} else if (timer >= spawnTime) {
			timer = 0;
			if (unit != null) {
				CreateUnit();
			}
		}
		timer += Time.deltaTime;
		if (timeWithoutTakingDamage > 20) {
			if (OnTownSafe != null && alert) {
				Debug.Log("Raising event: On Town Safe");
				OnTownSafe();
				alert = false;
			}
		} else { timeWithoutTakingDamage += Time.deltaTime; }
	}

	void CreateUnit () {
		Vector3 pos = (spawnFlag != null) ? spawnFlag.position : transform.position;
		GameObject newUnit = GameObject.Instantiate(unit, transform.position, new Quaternion());			
		newUnit.SendMessage("GoTo", pos);
		newUnit.SendMessage("SetMainTarget", mainTarget);
		newUnit.SendMessage("SetMainBuilding", this.gameObject);
	}

	void Destroy () {
		isDestroyed = true;
		MusicManager.instance.Play(tag, "Death");
		if (mainBuilding != null) {
			mainBuilding.GetComponent<Building>().spawnTime += 2.5f;
		} else {
			if (race == "Orc") {
				Debug.Log("The Human Team wins.");
			} else {
				Debug.Log("The Orc Team wins.");
			}
			Time.timeScale = 0;
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}
		}
		Destroy(gameObject);
	}

	public void TakeDamage (int amount) {
		if (amount - armor > 0) {
			timeWithoutTakingDamage = 0;
			if (mainBuilding == null) {
				if (OnTownAttacked != null && !alert) {
					Debug.Log("Raising event: On Town Attacked");
					OnTownAttacked();
					alert = true;
				}
			}
			healthPoints -= amount - armor;
			Debug.Log(race + " " + buildingName + " [" + healthPoints + "/" + maxHealthPoints + "]HP has been attacked.");
		}
	}
}
