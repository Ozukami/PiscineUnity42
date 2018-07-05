using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : Stats {

	public float range;
	private bool isDead = false;

	public GameObject lifeOrbe;

	private NavMeshAgent agent;
	private Animator animator;

	private GameObject target;

	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		
		maxHealthPoints = constitution * 5;
		healthPoints = maxHealthPoints;
		minDamage = strengh / 2;
		maxDamage = minDamage + 4;
	}
	
	void Start () {
	}
	
	void Update () {
		UpdateStats();
		if (agent != null)
			animator.SetBool("run", (agent.remainingDistance > 0.5f));

		/* Track target */
		if (target != null) {
			if (Vector3.Distance(transform.position, target.transform.position) <= range
					&& !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
				StopAllCoroutines();
				StartCoroutine(Attack());
			} else if (agent != null && Vector3.Distance(transform.position, target.transform.position) > range)
				agent.destination = target.transform.position;
		}

		if (agent != null)
			agent.speed = (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) ? 0.5f : 5.5f;
	}

	void UpdateStats () {
		maxHealthPoints = constitution * 5;
		minDamage = strengh / 2;
		maxDamage = minDamage + 4;
	}

	void OnTriggerEnter (Collider other) {
		Debug.Log("Trigger: " + other.gameObject.name);
		if (agent != null && other.gameObject.tag == "Maya") {
			target = other.gameObject;
			agent.destination = target.transform.position;
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
		Maya maya = target.GetComponent<Maya>();
		transform.LookAt(target.transform);
		animator.SetTrigger("attack");
		yield return new WaitForSeconds(0.8f);
		if (Random.Range(1, 101) < (75 + agility - maya.agility)) {
			if (!maya.TakeDamage(Random.Range(minDamage, maxDamage + 1))) {
				target = null;
			}
		} else {
			Debug.Log("Enemy attack missed..");
		}
	}

	IEnumerator Die () {
		StopAllCoroutines();
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
		if (Random.Range(0,100) >= 85) {
			Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
			GameObject orbe = Instantiate(lifeOrbe, pos, Quaternion.identity);
		}
		float time = 0;
		while (time < 3) {
			time += Time.deltaTime;
			transform.Translate(new Vector3(0, -0.05f, 0));
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(gameObject);
	}
}
