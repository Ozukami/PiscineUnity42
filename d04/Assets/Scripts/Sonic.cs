using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Sonic : MonoBehaviour
{
	public GameObject levelManager;

	public Text scoreUI, timeUI, ringsUI;
	public Canvas scoreBoard;
	public Text timeBonus, ringBonus, scoreTotal;

	public AudioSource aMusic;
	private float time;
	private bool end = false;

	[HideInInspector]public int			rings;
	public float		speedFactor;
	[HideInInspector]public float		speed;
	public float		maxSpeed;
	public float		jumpHeight;
	public float		rollingBoost;

	private Animator	animator;
	private Rigidbody2D	rbody;
	private Vector2 velocity;
	private float vMagnitude;
	private float acceleration;
	private float launchTime;
	[HideInInspector]public bool isGrounded;
	private bool isOnGroundNow;
	[HideInInspector]public bool isCharging;
	[HideInInspector]public bool isRolling;
	[HideInInspector]public bool isJumpball;
	[HideInInspector]public bool isAirborne;
	[HideInInspector]public bool isHit;
	[HideInInspector]public bool isDead;
	[HideInInspector]public bool isInvincible;
	[HideInInspector]public bool isShielded;
	[HideInInspector]public float charge;
	public GameObject currentShield;
	public PhysicsMaterial2D standardMat;
	public PhysicsMaterial2D rollMat;
	private PhysicsMaterial2D currentMat;
	public GameObject checkpoint;
	public AudioSource aRoll;
	public AudioSource aJump;
	public AudioSource aCharge;
	public AudioSource aDestroy;
	public AudioSource aLoseRings;
	public AudioSource aSpike;
	public AudioSource aDeath;

	void Awake()	{
		animator = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		currentMat = GetComponent<CircleCollider2D>().sharedMaterial;
		scoreBoard.gameObject.SetActive(false);
	}

	void FixedUpdate() 	{
		accelerate();
		checkRoll();
	}

	void Update() {
		calcAcceleration();
		checkCharge();
		lookUpAndDown();
		checkSpaceButton();
		checkFalling();
		time += Time.deltaTime;
		int min = Mathf.RoundToInt(time) / 60;
		int sec = Mathf.RoundToInt(time) % 60;
		timeUI.text = "time " + min + ":";
		if (sec < 10) { timeUI.text += "0" + sec; }
		else { timeUI.text += "" + sec; }
		ringsUI.text = "" + rings;
	}

	void Start() {
		respawn();
	}

	void checkFalling() {
		if (transform.position.y < -15 && isDead == false)
			dead ();
	}

	void calcAcceleration() {
		acceleration = Input.GetAxis("Horizontal") * speedFactor;
		if (isRolling == true) {
			if (isGrounded == false)
				acceleration = acceleration / 3;
			else
				acceleration = 0;
		}
		if (acceleration != 0 && isHit == false && isCharging == false) {
			if (rbody.velocity.x < -0.05f)
				transform.localScale = new Vector2(-1, 1);
			else if (rbody.velocity.x > 0.05f)
				transform.localScale = new Vector2(1, 1);
		}
		velocity = rbody.velocity;
		vMagnitude = velocity.magnitude;
		animator.SetFloat("speed", Mathf.Abs(vMagnitude));
	}

	void accelerate() {
		if (vMagnitude < maxSpeed && isHit == false && isCharging == false)
			rbody.AddForce(new Vector2(acceleration, 0));
	}

	void lookUpAndDown() {
		if (Input.GetAxis("Vertical") < 0) {
			if (vMagnitude == 0)
				animator.SetBool("down", true);
		}
		else
			animator.SetBool("down", false);
	}

	void checkRoll() {
		if (Input.GetAxis("Vertical") < 0 && vMagnitude > 5 && isGrounded == true) {
			isRolling = true;
			GetComponent<CircleCollider2D>().sharedMaterial  = rollMat;
			rbody.drag = 0;
			animator.SetBool ("rolling", true);
			if (!aRoll.isPlaying)
				aRoll.Play();
		}
		if (vMagnitude < 5 && Time.time > launchTime ) {
			isRolling = false;
			GetComponent<CircleCollider2D>().sharedMaterial = standardMat;
			rbody.drag = 0.5f;
			animator.SetBool ("rolling", false);
		}
	}


	void checkCharge() {
		if ((Input.GetKeyUp("s") || Input.GetKeyUp(KeyCode.DownArrow)) && isCharging == true) {
			animator.SetBool ("rolling", true);
			animator.SetBool ("charge", false);
			aRoll.Play();
			isRolling = true;
			isCharging = false;
			launchTime = Time.time + 1;
			rbody.AddForce(new Vector2(charge * maxSpeed * transform.localScale.x, 0), ForceMode2D.Impulse);
			charge = 0;
		}
	}

	void checkSpaceButton() {
		if (Input.GetKeyDown("space") && isGrounded == true && rbody.velocity.y < 5 && isHit == false) {
			if (vMagnitude == 0 && Input.GetAxis("Vertical") < 0 )
				chargeRoll();
			else
				jump ();
		}
	}

	void jump() {
		isRolling = false;
		isJumpball = true;
		isGrounded = false;
		aJump.Play();
		animator.SetBool("rolling", false);
		animator.SetBool("jumpball", true);
		rbody.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
	}

	void chargeRoll() {
		aCharge.Play ();
		animator.SetBool("charge", true);
		isCharging = true;
		charge += 0.75f;
		if (charge >= 2.25f)
			charge = 2.25f;
	}


	public void rampBoost(float direction, float boost) {
		if (rbody.velocity.x > 2.5f) {
			rbody.AddForce(new Vector2(boost * transform.localScale.x, 1), ForceMode2D.Impulse);
		}
	}

	public void bumper(float boostX, float boostY) {
		if (boostY  > 0) {
			isJumpball = false;
			isAirborne = true;
			animator.SetBool("jumpball", false);
			animator.SetBool("airborne", true);
		}
		if (boostY != 0)
			rbody.velocity = new Vector2(rbody.velocity.x, 0);
		if (boostX != 0)
			rbody.velocity = new Vector2(0, rbody.velocity.y);
		if (boostX < 0)
			transform.localScale = new Vector2(-1, 1);
		else
			transform.localScale = new Vector2(1, 1);
		rbody.AddForce(new Vector2(boostX * 4, boostY * 4), ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "moving") {
			isJumpball = false;
			animator.SetBool("jumpball", false);
			isAirborne = false;
			animator.SetBool("airborne", false);
		} else if (collision.gameObject.tag == "ring") {
			Camera.main.GetComponent<AudioSource>().Play();
			GameObject.Destroy(collision.gameObject);
			rings++;
		} else if (collision.gameObject.tag == "sign" && !end) {
			Debug.Log("sign reached");
			end = true;
			aMusic.Stop();
			collision.GetComponent<AudioSource>().Play();
			StartCoroutine(FinalScore());
		}
	}

	IEnumerator FinalScore () {
		yield return new WaitForSeconds(6);
		Time.timeScale = 0;
		Debug.Log("end");
		scoreBoard.gameObject.SetActive(true);
		timeBonus.text = "" + Mathf.Max(20000 - (100 * Mathf.RoundToInt(time)), 0);
		ringBonus.text = "" + 100 * rings;
		scoreTotal.text = "" + (int.Parse(timeBonus.text) + int.Parse(ringBonus.text));
		levelManager.GetComponent<LevelManager>().SaveData(int.Parse(scoreTotal.text), rings);
	}

	void OnTriggerStay2D(Collider2D collision) {
		if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "moving") {
			isGrounded = true;
			isOnGroundNow = true;
			if (isHit == false)
				animator.SetBool("getHit", false);
		}
		if (collision.gameObject.tag == "moving")
			transform.parent = collision.transform;
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag == "ground") {
			isOnGroundNow = false;
			Invoke("deGround", 0.4f);
		}

		if (collision.gameObject.tag == "moving") {
			isOnGroundNow = false;
			Invoke("deGround", 0.4f);
			transform.parent = null;
		}
	}

	void deGround() {
		if (isOnGroundNow == false)
			isGrounded = false;
	}

	public void destroy() {
		rbody.AddForce(new Vector2(0, 12), ForceMode2D.Impulse);
		aDestroy.Play ();
	}

	public void getHit() {

		// ▌─────────────────────────▐█─────▐
		// ▌────▄──────────────────▄█▓█▌────▐
		// ▌───▐██▄───────────────▄▓░░▓▓────▐
		// ▌───▐█░██▓────────────▓▓░░░▓▌────▐
		// ▌───▐█▌░▓██──────────█▓░░░░▓─────▐
		// ▌────▓█▌░░▓█▄███████▄███▓░▓█─────▐
		// ▌────▓██▌░▓██░░░░░░░░░░▓█░▓▌─────▐
		// ▌─────▓█████░░░░░░░░░░░░▓██──────▐
		// ▌─────▓██▓░░░░░░░░░░░░░░░▓█──────▐
		// ▌─────▐█▓░░░░░░█▓░░▓█░░░░▓█▌─────▐
		// ▌─────▓█▌░▓█▓▓██▓░█▓▓▓▓▓░▓█▌─────▐
		// ▌─────▓▓░▓██████▓░▓███▓▓▌░█▓─────▐
		// ▌────▐▓▓░█▄▐▓▌█▓░░▓█▐▓▌▄▓░██─────▐
		// ▌────▓█▓░▓█▄▄▄█▓░░▓█▄▄▄█▓░██▌────▐
		// ▌────▓█▌░▓█████▓░░░▓███▓▀░▓█▓────▐
		// ▌───▐▓█░░░▀▓██▀░░░░░─▀▓▀░░▓█▓────▐
		// ▌───▓██░░░░░░░░▀▄▄▄▄▀░░░░░░▓▓────▐
		// ▌───▓█▌░░░░░░░░░░▐▌░░░░░░░░▓▓▌───▐
		// ▌───▓█░░░░░░░░░▄▀▀▀▀▄░░░░░░░█▓───▐
		// ▌──▐█▌░░░░░░░░▀░░░░░░▀░░░░░░█▓▌──▐
		// ▌──▓█░░░░░░░░░░░░░░░░░░░░░░░██▓──▐
		// ▌──▓█░░░░░░░░░░░░░░░░░░░░░░░▓█▓──▐
		// ▌──██░░░░░░░░░░░░░░░░░░░░░░░░█▓──▐
		// ▌──█▌░░░░░░░░░░░░░░░░░░░░░░░░▐▓▌─▐
		// ▌─▐▓░░░░░░░░░░░░░░░░░░░░░░░░░░█▓─▐
		// ▌─█▓░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓─▐
		// ▌─█▓░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▌▐
		// ▌▐█▓░░░░░░░░░░░░░░░░░░░░░░░░░░░██▐
		// ▌█▓▌░░░░░░░░░░░░░░░░░░░░░░░░░░░▓█▐
		// ██████████████████████████████████
		// █░▀░░░░▀█▀░░░░░░▀█░░░░░░▀█▀░░░░░▀█
		// █░░▐█▌░░█░░░██░░░█░░██░░░█░░░██░░█
		// █░░▐█▌░░█░░░██░░░█░░██░░░█░░░██░░█
		// █░░▐█▌░░█░░░██░░░█░░░░░░▄█░░▄▄▄▄▄█
		// █░░▐█▌░░█░░░██░░░█░░░░████░░░░░░░█
		// █░░░█░░░█▄░░░░░░▄█░░░░████▄░░░░░▄█
		// ██████████████████████████████████

	}

	void stopHit() {
		isHit = false;
	}

	void dead(){
		if (PlayerPrefs.GetInt("lives") == null)
			PlayerPrefs.SetInt("lives", 1);
		else
			PlayerPrefs.SetInt("lives", PlayerPrefs.GetInt("lives") + 1);
		aDeath.Play ();
		animator.SetBool("dead", true);
		isDead = true;
		rbody.AddForce (new Vector2(0, 15), ForceMode2D.Impulse);
		GetComponent<CircleCollider2D>().enabled = false;
		Camera.main.transform.parent = null;
		Invoke("newLife", 2);
	}

	void newLife() {
		isHit = false;
		isDead = false;
		Application.LoadLevel(Application.loadedLevel);
	}

	void respawn() {
		Camera.main.transform.parent = transform;
		Camera.main.transform.localPosition = new Vector2(0, 1.5f);
		rbody.velocity = Vector2.zero;
		transform.position = checkpoint.transform.position;
	}

	IEnumerator invincible() {
		isInvincible = true;
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		for (int i = 0; i < 15; i++) {
			sr.color = Color.clear;
			yield return new WaitForSeconds(0.1f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
		isInvincible = false;
	}
}
