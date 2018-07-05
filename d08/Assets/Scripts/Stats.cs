using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

	/*
	• 3 caractéristiques principales : Strengh(STR), Agility(AGI), Constitution(CON)
	• Une stat Armor qui sera attribuée subjectivement pour le moment et pourra être
		modifiée plus tard par de l’équipement.
	• Une stat HP qui est egale a 5 * CON
	• Une stat minDamage qui est égale à STR / 2, et maxDamage qui est égale à minDamage + 4.
		Comme l’armor ces stats basiques seront modifiables par l’équi- pement.
	• Une stat Level qui correspond à leur niveau.
	• Une stat XP. Pour Maya c’est l’xp qu’elle a accumulé en tuant les ennemis, pour
		les ennemis c’est leur valeur individuelle en xp.
	• Une stat money. Pour Maya c’est les crédits qu’elle a accumulé, pour les ennemis c’est leur valeur moyenne en crédits.
	*/
	
	public int strengh, agility, constitution;
	// [HideInInspector]
	public int healthPoints, maxHealthPoints;
	public int armor;
	// [HideInInspector]
	public int minDamage, maxDamage;
	public int level, exp, expMax;
	public int money;

	void Awake () {
		maxHealthPoints = constitution * 5;
		healthPoints = maxHealthPoints;
		minDamage = strengh / 2;
		maxDamage = minDamage + 4;
	}

	void Update () {
		UpdateStats();
	}

	void UpdateStats () {
		maxHealthPoints = constitution * 5;
		minDamage = strengh / 2;
		maxDamage = minDamage + 4;
	}

	// public bool TakeDamage (int amount) {
	// 	if (amount - armor > 0) {
	// 		healthPoints -= (amount - armor);
	// 		if (healthPoints <= 0)
	// 			return false;
	// 	}
	// 	return true;
	// }
}
