using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

	public int level;
	public double exp, expMax;
	public int healthPoints, maxHealthPoints;
	public int lifeRegenPerSecond, manaRegenPerSecond;
	public int manaPoints, maxManaPoints;
	public int strength, agility, constitution, intelligence;
	public int armor;
	public int minDamage, maxDamage;
	public float range;
	public int money;
	public int boostArmor, boostManaRegen, boostHealthRegen, boostHealth, boostDamage;
	protected bool isDead = false;

	void Awake () {
		maxHealthPoints = constitution * 5;
		healthPoints = maxHealthPoints;
		minDamage = strength / 2;
		maxDamage = minDamage + 4;
	}
	
	void Start () {
		StartCoroutine(LifeRegen());
		StartCoroutine(ManaRegen());
	}

	void Update () {
		UpdateStats();
	}

	void UpdateStats () {
		maxHealthPoints = constitution * 5 + boostHealth;
		minDamage = strength / 2 + boostDamage;
		maxDamage = minDamage + 4 + boostDamage;
	}

	protected IEnumerator LifeRegen () {
		while (!isDead) {
			healthPoints = Mathf.Min(healthPoints + lifeRegenPerSecond + boostHealthRegen, maxHealthPoints);
			yield return new WaitForSeconds(5);
		}
	}

	protected IEnumerator ManaRegen () {
		while (!isDead) {
			manaPoints = Mathf.Min(manaPoints + manaRegenPerSecond + boostManaRegen, maxManaPoints);
			yield return new WaitForSeconds(1);
		}
	}
}
