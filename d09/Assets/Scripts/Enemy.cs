using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	public float range;
	public int healthPoints;
	public int atkDmg;
	private bool isDead = false;

	private NavMeshAgent agent;
	private Animator animator;

	private GameObject target;

	public Transform mapCenter;

	private bool isLost = false;

	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}
	
	void Update () {
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

		if (agent != null && target == null && mapCenter != null && !isLost)
			agent.destination = mapCenter.position;
	}

	void OnTriggerEnter (Collider other) {
		if (agent != null && other.gameObject.tag == "Player") {
			target = other.gameObject;
			agent.destination = target.transform.position;
		}
	}

	void OnTriggerExit (Collider other) {
		if (agent != null && other.gameObject.tag == "Player") {
			target = null;
			isLost = true;
			GoAtPos(other.transform.position);
		}
	}

	public void GoAtPos (Vector3 pos) {
		if (agent != null)
			agent.destination = pos;
	}

	public bool TakeDamage (int amount) {
		if (amount > 0) {
			healthPoints -= amount;
			if (healthPoints <= 0 && !isDead) {
				StartCoroutine("Die");
				return false;
			}
		}
		return true;
	}

	IEnumerator Attack () {
		Player player = target.GetComponent<Player>();
		transform.LookAt(target.transform);
		animator.SetTrigger("attack");
		yield return new WaitForSeconds(0.8f);
		if (!player.TakeDamage(atkDmg))
			target = null;
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
		float time = 0;
		while (time < 3) {
			time += Time.deltaTime;
			transform.Translate(new Vector3(0, -0.05f, 0));
			yield return new WaitForSeconds(0.05f);
		}
		GameManager.gM.enemies--;
		Destroy(gameObject);
	}

}
