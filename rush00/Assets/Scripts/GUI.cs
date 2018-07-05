using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public Text weaponTextWhite;
	public Text weaponTextPink;
	public Text ammoTextWhite;
	public Text ammoTextPink;
	public Text fireTypeTextWhite;
	public Text fireTypeTextPink;

	private Player player;
	private WeaponScript weapon;

	// Use this for initialization
	void Start () {
		
	}
	
	void Awake(){
		player = GetComponent<Player>();	
	}
	
	// Update is called once per frame
	void Update () {
		if ((weapon = player.GetWeapon()) != null) {
			weaponTextWhite.text = weapon.weaponName;
			weaponTextPink.text = weapon.weaponName;
			if (weapon.ammoCount == -1) {
				ammoTextWhite.text = "-";
				ammoTextPink.text = "-";
			} else {
				ammoTextWhite.text = "" + weapon.ammoCount;
				ammoTextPink.text = "" + weapon.ammoCount;
			}
			fireTypeTextWhite.text = weapon.fireMode.ToString();
			fireTypeTextPink.text = weapon.fireMode.ToString();
		} else {
			weaponTextWhite.text = "No Weapon";
			weaponTextPink.text = "No Weapon";
			ammoTextWhite.text = "-";
			ammoTextPink.text = "-";
			fireTypeTextWhite.text = "";
			fireTypeTextPink.text = "";
		}
	}
}
