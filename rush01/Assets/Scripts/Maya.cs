using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Maya : Stats {
	
	private Dictionary<int, int> skillTree;
	private Dictionary<TypeItem, GameObject> inventory;
//	private GameObject[] inventory;
	private GameObject[] bag;

	private int statsPoints = 0, skillPoints = 1;

	public Canvas pauseUI, gameOverUI;
	public Canvas playerUI, targetUI, upgradeStatsUI, levelUpUI;
	public Canvas inventoryUI, skillTreeUI, skillHintUI, statsUI, skillBarUI;
	public Canvas bagUI;
	public Button upgradeButton;
	
	public Text playerLvl, playerEXP;
	public Slider playerHPBar, playerManaBar, playerEXPBar;
	
	public Text targetName, targetLvl, targetHP;
	public Slider targetHPBar;

	public Text nameUI, strUI, agiUI, conUI, intUI, armorUI;
	public Text upgradePointsUI, damageUI, hpUI, manaUI;
	public Text expUI, nextExpUI, creditsUI;
	public Text skillPointsUI;

	public ParticleSystem levelUpEffect;

	private NavMeshAgent agent;
	private Animator animator;

	private GameObject target;

	public float attackSpeed;
	private float time = 0;

	private Ray ray;
	private RaycastHit hit;

	private bool hold = false;
	private bool interactionUI = false;

	public GameObject[] spells;

	void Awake ()
	{
		spells = new GameObject[10];
		skillTree = new Dictionary<int, int>();
		
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		maxHealthPoints = constitution * 5;
		healthPoints = maxHealthPoints;
		manaPoints = maxManaPoints;
		minDamage = strength / 2;
		maxDamage = minDamage + 4;
//		inventory = new GameObject[6];
		inventory = new Dictionary<TypeItem, GameObject>();
		bag = new GameObject[12];
		bag = new GameObject[12];
	}

	void Cheat () {
		if (Input.GetKey(KeyCode.U)) {
			GainExp(expMax - 1);
		}
		if (Input.GetKeyDown(KeyCode.Y)) {
			strength = 2000;
			agility = 2000;
			constitution = 2000;
			intelligence = 2000;
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			for (int i = 0; i < bag.Length; i++) {
				if (bag[i] != null)
					Debug.Log(bag[i].gameObject.name);
			}
		}
	}
	
	void Update () {
		Cheat();
		
		time += Time.deltaTime;
		if (!isDead) {
			animator.SetBool("run", (agent.remainingDistance > 0.5f));
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			UpdateStats();
			UpdateUI();
			ShowStats();
			MouseTargetEnemy();
			SingleClick();
			HoldClick();
			TargetInRange();
		}

		if (Input.GetKeyDown(KeyCode.I) || UIManager.UI.inventoryUI.gameObject.activeSelf) {
			for (int i = 0; i < bag.Length; i++) {
				if (bag[i] != null)
				{
					bagUI.transform.Find("Slot " + (i + 1)).GetComponent<Inventory>().player = gameObject;
					bagUI.transform.Find("Slot " + (i + 1)).GetComponent<Inventory>().item = bag[i].gameObject;
					bagUI.transform.Find("Slot " + (i + 1)).GetComponent<Image>().sprite = bag[i].gameObject.GetComponent<Item>().sprite;
					bagUI.transform.Find("Slot " + (i + 1)).GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

	void UpdateStats () {
		maxHealthPoints = constitution * 5;
		maxManaPoints = intelligence * 5;
		minDamage = strength / 2;
		maxDamage = minDamage + 4;
	}

	void UpdateUI ()
	{
		if (statsPoints <= 0)
		{
			
		}
		playerLvl.text = "Lv." + level;
		playerEXP.text = exp + "/" + expMax;
		playerHPBar.value = ((float)healthPoints / (float)maxHealthPoints);
		playerManaBar.value = ((float)manaPoints / (float)maxManaPoints);
		playerEXPBar.value = ((float)exp / (float)expMax);
		nameUI.text = "Maya [Lv." + level + "]";
		strUI.text = "" + strength;
		agiUI.text = "" + agility;
		conUI.text = "" + constitution;
		intUI.text = "" + intelligence;
		armorUI.text = "" + (armor + boostArmor);
		upgradePointsUI.text = "" + statsPoints;
		damageUI.text = "" + minDamage + "-" + maxDamage;
		hpUI.text = "" + maxHealthPoints;
		manaUI.text = "" + maxManaPoints;
		expUI.text = "" + exp;
		nextExpUI.text = "" + expMax;
		creditsUI.text = "" + money;
		skillPointsUI.text = "" + skillPoints;
	}

	void ShowStats () {
		if (Input.GetKeyDown(KeyCode.C) && statsUI.gameObject.activeSelf)
			statsUI.gameObject.SetActive(false);
		else if (Input.GetKeyDown(KeyCode.C) && !statsUI.gameObject.activeSelf)
			statsUI.gameObject.SetActive(true);
		if (statsPoints <= 0)
			upgradeStatsUI.gameObject.SetActive(false);
		else
			upgradeStatsUI.gameObject.SetActive(true);
		if ((statsPoints > 0 || skillPoints > 0) && !statsUI.gameObject.activeSelf)
			upgradeButton.gameObject.SetActive(true);
		else if ((statsPoints > 0 || skillPoints > 0) && statsUI.gameObject.activeSelf)
			upgradeButton.gameObject.SetActive(false);
	}

	void MouseTargetEnemy () {
		if (Physics.Raycast(ray, out hit, 50)) {
			if (hit.collider && hit.collider.gameObject.layer == 9) {
				targetUI.gameObject.SetActive(true);
				Enemy mouseTarget = hit.collider.GetComponent<Enemy>();
				targetName.text = mouseTarget.name;
				targetLvl.text = "Lv." + mouseTarget.level;
				targetHP.text = "" + mouseTarget.healthPoints;
				targetHPBar.value = ((float)mouseTarget.healthPoints / (float)mouseTarget.maxHealthPoints);
			} else if (!hold || (hold && target == null)) {
				targetUI.gameObject.SetActive(false);
			} else if (hold && target != null) {
				targetUI.gameObject.SetActive(true);
				Enemy targetStats = target.GetComponent<Enemy>();
				targetName.text = targetStats.name;
				targetLvl.text = "Lv." + targetStats.level;
				targetHP.text = "" + targetStats.healthPoints;
				targetHPBar.value = ((float)targetStats.healthPoints / (float)targetStats.maxHealthPoints);
			}
		}
	}

	void SingleClick () {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			interactionUI = false;
			if (Physics.Raycast(ray, out hit, 50, ~(1 << 10))) {
				Debug.Log(hit.collider.gameObject.name);
				if (hit.collider && hit.collider.gameObject.layer == 9) {
					Debug.Log(hit.collider.gameObject.name);
					target = hit.collider.gameObject;
					if (Vector3.Distance(transform.position, target.transform.position) <= range) {
						if (time > attackSpeed && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
							StartCoroutine("Attack");
							time = 0;
						}
					} else {
						agent.destination = target.transform.position;
					}
				} else {
					target = null;
					agent.destination = hit.point;
				}
			}
		} else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject()) {
			interactionUI = true;
		}
	}

	void HoldClick () {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !interactionUI) {
			hold = true;
			if (target == null) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 50, ~(1 << 10))) {
					agent.destination = hit.point;
				}
			} else {
				if (Vector3.Distance(transform.position, target.transform.position) <= range) {
					if (time > attackSpeed && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
						StartCoroutine("Attack");
						time = 0;
					}
				} else {
					agent.destination = target.transform.position;
				}
			}
		} else { hold = false; }
	}

	void TargetInRange () {
		if (agent.hasPath && target != null) {
			if (Vector3.Distance(transform.position, target.transform.position) <= range) {
				agent.ResetPath();
				if (time > attackSpeed && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
					StartCoroutine("Attack");
					time = 0;
				}
			}
		}
	}

	public void Heal(int heal) {
		healthPoints = Mathf.Min(healthPoints + heal, maxHealthPoints);
	}

	public bool TakeDamage (int amount) {
		int finalDamage = amount * (1 - armor/200);
		if (finalDamage > 0) {
			healthPoints -= (finalDamage);
			if (healthPoints <= 0 && !isDead) {
				UIManager.UI.CloseAllUI();
				gameOverUI.gameObject.SetActive(true);
				StartCoroutine("Die");
				return false;
			}
		}
		return true;
	}

	IEnumerator Attack () {
		Enemy enemy = target.GetComponent<Enemy>();
		transform.LookAt(target.transform);
		animator.SetTrigger("attack");
		yield return new WaitForSeconds(0.5f);
		if (Random.Range(1, 101) < (75 + agility - enemy.agility)) {
			if (!enemy.TakeDamage(Random.Range(minDamage, maxDamage + 1))) {
				target = null;
				GainExp(enemy.exp);
				money += enemy.money;
			}
		} else {
			Debug.Log("Attack missed..");
		}
	}

	IEnumerator Die () {
		StopAllCoroutines();
		GetComponent<CharacterController>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		target = null;
		isDead = true;
		agent.enabled = false;
		agent = null;
		animator.SetTrigger("death");
		yield return StartCoroutine("CorpseToGround");
	}

	IEnumerator CorpseToGround () {
		yield return new WaitForSeconds(3);
		float time = 0;
		playerUI.gameObject.SetActive(false);
		targetUI.gameObject.SetActive(false);
		gameOverUI.gameObject.SetActive(true);
		while (time < 3) {
			time += Time.deltaTime;
			transform.Translate(new Vector3(0, -0.05f, 0));
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(gameObject);
	}

	public void GainExp (double addExp) {
		exp += addExp;
		if (exp >= expMax) {
			StartCoroutine(LevelUpEffect());
			level++;
			exp = 0 + (exp - expMax);
			expMax = (level * 1000 + (level - 1) * (level - 1) * 450);
			statsPoints += 5;
			skillPoints++;
			healthPoints = maxHealthPoints;
			manaPoints = maxManaPoints;
		}
	}

	public void UpgradeStats (string stat) {
		if (statsPoints > 0) {
			if (stat == "STR")
				strength++;
			else if (stat == "AGI")
				agility++;
			else if (stat == "CON") {
				constitution++;
				healthPoints += 5;
			} else if (stat == "INT") {
				intelligence++;
				manaPoints += 5;
			}
			statsPoints--;
		}
	}

	IEnumerator LevelUpEffect () {
		levelUpUI.gameObject.SetActive(true);
		Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
		ParticleSystem effect = Instantiate(levelUpEffect, pos, Quaternion.identity);
		effect.transform.eulerAngles = new Vector3(90, 0, 0);
		//Destroy(effect, effect.main.duration);
		yield return new WaitForSeconds(2);
		levelUpUI.gameObject.SetActive(false);
	}

	void OnTriggerStay (Collider other) {
		if (other.tag == "Orbe") {
			healthPoints = Mathf.Min(healthPoints + ((30 * maxHealthPoints) / 100), maxHealthPoints);
			Destroy(other.gameObject);
		}
	}

	public void ResetSkillPoints () {
		skillPoints = level;
		skillTree.Clear();
		foreach (var spell in spells)
		{
			spell.GetComponent<Spell>().Reset();
		}
	}

	public int GetSkillPoints () {
		return skillPoints;
	}

	public void UpgradeSkill (int skillIndex)
	{
		if (skillPoints > 0) {
			if (skillTree.ContainsKey(skillIndex))
			{
				spells[skillIndex].GetComponent<Spell>().Upgrade();
				skillTree[skillIndex] += 1;
			}
			else
			{
				skillTree.Add(skillIndex, 1);
				if (spells[skillIndex].GetComponent<Spell>().GetType() == Type.Passif)
					spells[skillIndex].GetComponent<Spell>().Cast(gameObject);
			}
			skillPoints--;
		}
	}

	public bool IsSkillAvailable (int skillIndex, int[] requiredSkills) {
		if (skillTree.ContainsKey(skillIndex))
			return true;
		foreach (var requiredSkill in requiredSkills) {
			if (!skillTree.ContainsKey(requiredSkill))
				return false;
		}
		return true;
	}

	public bool IsSkillLearned (int skillIndex) {
		return skillTree.ContainsKey(skillIndex);
	}

	public void AddToBag (GameObject newItem) {
		for (int i = 0; i < bag.Length; i++) {
			if (bag[i] == null) {
				bag[i] = newItem;
				return;
			}
		}
		Debug.Log("bag full");
	}

	public GameObject GetFromBag (int slot) {
		if (bag[slot] != null)
			return bag[slot];
		return null;
	}

	public void Equip (GameObject toEquip) {
		if (inventory[toEquip.GetComponent<Item>().GetTypeItem()] != null) {
			for (int i = 0; i < bag.Length; i++) {
				if (bag[i] == null)
					bag[i] = inventory[toEquip.GetComponent<Item>().GetTypeItem()];
			}
		}
		inventory[toEquip.GetComponent<Item>().GetTypeItem()] = toEquip;
	}

	public GameObject UnEquip (TypeItem type) {
		if (inventory[type] != null) {
			for (int i = 0; i < bag.Length; i++) {
				if (bag[i] == null)
					bag[i] = inventory[type];
			}
		}
		return null;
	}
}
