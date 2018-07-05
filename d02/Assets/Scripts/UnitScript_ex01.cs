using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript_ex01 : MonoBehaviour {

	public string type;
	public int direction;
	public float speed;
	public AudioClip[] startMovingSounds;
	public int healthPoints;
	public int maxHealthPoints;
	public int attackDamage;
	public int attackSpeed;	
	public int range;
	public int armor;
	public bool isDead = false;
	public bool spawn = false;

	private Animator animator;
	private Vector3 targetPosition;
	public GameObject enemyTarget;
	public GameObject mainTarget;
	public GameObject mainBuilding;
	private bool attacking = false;
	private float lastAtk = 0;
	private bool haveOrder = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		if (!spawn) {
			targetPosition = transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {
		if (healthPoints <= 0) {
			if (!isDead) { Die(); }
		} else if (attacking && enemyTarget != null) {
			Attack();
		} else if (transform.position != targetPosition) {
			if (animator) { animator.SetBool("moving", true); }
			if (enemyTarget != null) { targetPosition = enemyTarget.transform.position; }
			var angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle + 90);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
		} else {
			if (animator) { animator.SetBool("moving", false); }
			if (enemyTarget == null) { haveOrder = false; }
			spawn = false;
		}
	}

	public void MoveToPos (Vector3 targetPos) {
		haveOrder = true;
		enemyTarget = null;
		targetPosition = targetPos;
		if (animator) { animator.SetBool("moving", true); }
	}

	public void TargetEnemy (GameObject enemy) {
		haveOrder = true;
		if (enemyTarget != enemy) {
			if (enemy.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>())) {
				attacking = true;
				this.enemyTarget = enemy;
			} else if (!enemyTarget || enemyTarget.transform.position != enemy.transform.position) {
				attacking = false;
				this.enemyTarget = enemy;
				targetPosition = enemyTarget.transform.position;
				if (animator) { animator.SetBool("moving", true); }
			} else {
				this.enemyTarget = enemy;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (Vector3.Distance(collider.transform.position, transform.position) < 2) {
			if (collider.GetType() == typeof(BoxCollider2D) || collider.gameObject.layer == 9) {
				if (collider.gameObject == enemyTarget) {
					attacking = true;
					targetPosition = transform.position;
				}
			}
		}
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (tag != collider.tag && !haveOrder) {
			if (collider.gameObject.layer == 8 && !collider.GetComponent<UnitScript_ex01>().isDead) {
				if (enemyTarget == null) {	
					TargetEnemy(collider.gameObject);
				} else if (Vector3.Distance(transform.position, enemyTarget.transform.position) > Vector3.Distance(transform.position, collider.transform.position)) {
					TargetEnemy(collider.gameObject);
				} else if (enemyTarget.gameObject.layer == 9 && collider.gameObject.layer == 8) {
					TargetEnemy(collider.gameObject);
				}
			} else if (collider.gameObject.layer == 9 && enemyTarget == null) {
				TargetEnemy(collider.gameObject);
			} else if (collider.gameObject.layer == 9 && Vector3.Distance(transform.position, enemyTarget.transform.position) > Vector3.Distance(transform.position, collider.transform.position)) {
				TargetEnemy(collider.gameObject);
			}
		}
	}

	void OnTriggerExit2D (Collider2D collider) {
		if (Vector3.Distance(collider.transform.position, transform.position) < 2) {
			if (collider.GetType() == typeof(BoxCollider2D) || collider.gameObject.layer == 9) {
				if (collider.gameObject == enemyTarget && attacking) {
					attacking = false;
					targetPosition = enemyTarget.transform.position;
				}
			}
		}
	}

	void Die () {
		isDead = true;
		targetPosition = transform.position;
		if (animator) { animator.SetTrigger("death"); }
		MusicManager.instance.Play(tag, "Death");
		Destroy(gameObject, 4);
	}

	void Attack () {
		if (animator) { animator.SetBool("moving", false); }
		if ((lastAtk -= Time.deltaTime) <= 0) {
			lastAtk = attackSpeed;
			if (animator){ animator.SetTrigger("attack"); }
			if (enemyTarget.layer == 8) {
				enemyTarget.GetComponent<UnitScript_ex01>().TakeDamage(attackDamage);
				if (enemyTarget.GetComponent<UnitScript_ex01>().healthPoints <= 0) {
					attacking = false;
					haveOrder = false;
					enemyTarget = null;
				}
			} else if (enemyTarget.layer == 9) {
				enemyTarget.GetComponent<Building>().TakeDamage(attackDamage);
				if (enemyTarget.GetComponent<Building>().healthPoints <= 0) {
					attacking = false;
					haveOrder = false;
					enemyTarget = null;
				}
			}
		}
	}

	public void TakeDamage (int amount) {
		if (amount - armor > 0) {
			healthPoints -= amount - armor;
			Debug.Log(tag + " " + type + " [" + healthPoints + "/" + maxHealthPoints + "]HP has been attacked.");
		}
	}

	public void GoTo (Vector3 targetPos) {
		enemyTarget = null;	
		targetPosition = targetPos;
		spawn = true;
	}

	public void SetMainTarget (GameObject mainTarget) {
		this.mainTarget = mainTarget;
	}

	public void SetMainBuilding (GameObject mainBuilding) {
		this.mainBuilding = mainBuilding;
	}
}
