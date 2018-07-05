using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Slider lifeBarUI;
	public Text lifeUI;
	
	public int healthPoints;
	private bool isDead = false;

	public GameObject[] Weapons;
	private int _currentWeapon = 0;

	private LineRenderer _lineRenderer;

	void Start () {
		_lineRenderer = GetComponentInChildren<LineRenderer>();
	}

	void Update () {
		Fire();
		SwitchWeapon();
		float tmp = healthPoints;
		lifeBarUI.value = (tmp / 100);
		lifeUI.text = "" + healthPoints;
	}

	void Fire () {
		if (Input.GetMouseButtonDown(0)) {
			Weapons[_currentWeapon].GetComponent<Weapon>().Fire();
		}
	}

	void SwitchWeapon () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			_currentWeapon = 0;
			Weapons[0].SetActive(true);
			Weapons[1].SetActive(false);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			_currentWeapon = 1;
			Weapons[0].SetActive(false);
			Weapons[1].SetActive(true);
		}
	}

	public bool TakeDamage (int amount) {
		if (amount > 0) {
			healthPoints -= amount;
			if (healthPoints <= 0 && !isDead) {
				Debug.Log("Player is dead");
				GetComponent<CharacterController>().enabled = false;
				isDead = true;
				GameManager.gM.Loose();
				return false;
			}
		}
		return true;
	}
	
}
