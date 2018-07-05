using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Maya : Stats {

	public float range;
	private int upgradePoints;

	public Canvas playerUI, targetUI, gameOverUI, statsUI, upgradeStatsUI, levelUpUI;
	public Text playerLvl, playerEXP;
	public Slider playerHPBar, playerEXPBar;
	public Text targetName, targetLvl, targetHP;
	public Slider targetHPBar;
	public Button upgradeButton;

	public Text nameUI, strUI, agiUI, conUI, armorUI;
	public Text upgradePointsUI, damageUI, hpUI;
	public Text expUI, nextExpUI, creditsUI;

	public ParticleSystem levelUpEffect;

	private NavMeshAgent agent;
	private Animator animator;

	private GameObject target;

	public float attackSpeed;
	private float time = 0;

	private Ray ray;
	private RaycastHit hit;

	private bool isDead = false;
	private bool hold = false;

	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		maxHealthPoints = constitution * 5;
		healthPoints = maxHealthPoints;
		minDamage = strengh / 2;
		maxDamage = minDamage + 4;

		targetUI.gameObject.SetActive(false);
		gameOverUI.gameObject.SetActive(false);
		statsUI.gameObject.SetActive(false);
		upgradeButton.gameObject.SetActive(false);
		levelUpUI.gameObject.SetActive(false);
	}
	
	void Update () {
		UpdateStats();
		UpdateUI();
		ShowStats();
		time += Time.deltaTime;
		if (!isDead) {
			animator.SetBool("run", (agent.remainingDistance > 0.5f));

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			MouseTargetEnemy();
			SingleClick();
			HoldClick();
			TargetInRange();
		}
//		if (EventSystem.current.IsPointerOverGameObject()) {
//			Debug.Log("Mouse on the UI");
//		}
	}

	void UpdateStats () {
		maxHealthPoints = constitution * 5;
		minDamage = strengh / 2;
		maxDamage = minDamage + 4;
	}

	void UpdateUI () {
		playerLvl.text = "Lv." + level;
		playerEXP.text = exp + "/" + expMax;
		playerHPBar.value = ((float)healthPoints / (float)maxHealthPoints);
		playerEXPBar.value = ((float)exp / (float)expMax);
		nameUI.text = "Maya [Lv." + level + "]";
		strUI.text = "" + strengh;
		agiUI.text = "" + agility;
		conUI.text = "" + constitution;
		armorUI.text = "" + armor;
		upgradePointsUI.text = "" + upgradePoints;
		damageUI.text = "" + minDamage + "-" + maxDamage;
		hpUI.text = "" + healthPoints;
		expUI.text = "" + exp;
		nextExpUI.text = "" + expMax;
		creditsUI.text = "" + money;
	}

	void ShowStats () {
		if (Input.GetKeyDown(KeyCode.C) && statsUI.gameObject.activeSelf)
			statsUI.gameObject.SetActive(false);
		else if (Input.GetKeyDown(KeyCode.C) && !statsUI.gameObject.activeSelf)
			statsUI.gameObject.SetActive(true);
		if (upgradePoints <= 0)
			upgradeStatsUI.gameObject.SetActive(false);
		else
			upgradeStatsUI.gameObject.SetActive(true);
		if (upgradePoints > 0 && !statsUI.gameObject.activeSelf)
			upgradeButton.gameObject.SetActive(true);
		else if (upgradePoints > 0 && statsUI.gameObject.activeSelf)
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
		}
	}

	void HoldClick () {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
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

	public bool TakeDamage (int amount) {
		int finalDamage = amount * (1 - armor/200);
		if (finalDamage > 0) {
			healthPoints -= (finalDamage);
			if (healthPoints <= 0 && !isDead) {
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

	void GainExp (int addExp) {
		exp += addExp;
		if (exp >= expMax) {
			StartCoroutine(LevelUpEffect());
			level++;
			exp = 0 + (exp - expMax);
			expMax = (int)(expMax * 1.5f);
			upgradePoints += 5;
			healthPoints = maxHealthPoints;
		}
	}

	public void UpgradeStats (string stat) {
		if (upgradePoints > 0) {
			if (stat == "STR")
				strengh++;
			else if (stat == "AGI")
				agility++;
			else if (stat == "CON") {
				constitution++;
				healthPoints += 5;
			}
			upgradePoints--;
		}
	}

	IEnumerator LevelUpEffect () {
		levelUpUI.gameObject.SetActive(true);
		Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
		ParticleSystem effect = Instantiate(levelUpEffect, pos, Quaternion.identity);
		effect.transform.eulerAngles = new Vector3(90, 0, 0);
		Destroy(effect, effect.main.duration);
		yield return new WaitForSeconds(2);
		levelUpUI.gameObject.SetActive(false);
	}

	void OnTriggerStay (Collider other) {
		if (other.tag == "Orbe") {
			healthPoints = Mathf.Min(healthPoints + ((30 * maxHealthPoints) / 100), maxHealthPoints);
			Destroy(other.gameObject);
		}
	}
}
