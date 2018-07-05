using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
	melee, ranged
}

public enum FireMode {
	infinite = 0, singleShot = 1, auto = 2, rifle = 3
}

public class WeaponScript : MonoBehaviour {

    public string weaponName;   // weaponName
	public WeaponType type;		// melee ou distance
	public int ammoCount;		// munitions restantes
	public int ammoCapactity;	// nombre max de munitions
	public FireMode fireMode;	// nombre de munition tiree d'un coup
	public float fireRate;		// frequence de tir
	private float lastFire;		// temps ecoule depuis le dernier tir
	public bool killOnDropHit;	// true si l'arme tue l'ennemi lorsqu'on lui jette dessus
	public float projSpeed;		// vitesse du projectile
	public int targetLayer;	// 10 player, 11 enemy

	public GameObject projectile;	// type de munitions

	public AudioSource aFire;		// audio de l'arme lors du tir
	public AudioSource aClicClic;	// audio de l'arme quand il n'y a plus de munitions

	public Sprite onGround;		// sprite quand l'arme est au sol
	public Sprite onBody;		// sprite quand l'arme est equipee

	public bool isEquiped;		// true si equipee

	private Rigidbody2D rb2d;
	private SpriteRenderer sR;

	private IEnumerator scaleCoRoutine;
	private IEnumerator colorCoRoutine;

	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D>();
		sR = GetComponent<SpriteRenderer>();
		scaleCoRoutine = ScaleCoRoutine();
		colorCoRoutine = ColorCoRoutine();
		StartCoroutine(scaleCoRoutine);
		StartCoroutine(colorCoRoutine);
	}
	
	// Update is called once per frame
	void Update () {
		lastFire += Time.deltaTime;
	}

	IEnumerator ScaleCoRoutine () {
		while (true) {
			while (transform.localScale.x < 1) {
				transform.localScale += new Vector3(0.02f, 0.02f, 0);
				yield return new WaitForSeconds(0.1f);
			}
			while (transform.localScale.x > 0.8f) {
				transform.localScale -= new Vector3(0.02f, 0.02f, 0);
				yield return new WaitForSeconds(0.1f);
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator ColorCoRoutine () {
		bool check = true;
		while (true) {
			if (check) {
				sR.color = new Color(0.7f, 0.7f, 0.7f, 1);
				check = false;
			} else {
				sR.color = new Color(1, 1, 1, 1);
				check = true;
			}
			yield return new WaitForSeconds(0.4f);
		}
	}

	public void Fire () {
		if (ammoCount == 0 && WeaponType.ranged.Equals(type)) {
			Debug.Log("* clic clic *");
            SoundManager.soundManager.PlayDelayedAudioClip(aClicClic.clip, 0.05f);
		} else if (lastFire > fireRate) {
            SoundManager.soundManager.PlayAudioClip(aFire.clip, 0.5f);
			ammoCount = (FireMode.auto.Equals(fireMode)) ? ammoCount - 1 : ammoCount - (int)fireMode;
			lastFire = 0;
			if (WeaponType.ranged.Equals(type)) {
				Debug.Log("* piou piou *");
				StartCoroutine(FireCoRoutine((int)fireMode));
			} else {
				Debug.Log("* slash slash *");
				StartCoroutine(FireCoRoutine(1));
			}
		}
	}

	IEnumerator FireCoRoutine (int n) {
		for (int i = 0; i < n; i++) {
			GameObject newProj = Instantiate(projectile, transform);
			newProj.GetComponent<AmmoScript>().targetLayer = targetLayer;
			newProj.transform.parent = null;
			newProj.transform.rotation = transform.rotation;
			newProj.transform.Rotate(0, 0, 90);
            Vector3 dir = -newProj.transform.right;
			Vector3 additinalForce = transform.parent.transform.GetComponent<Rigidbody2D>().velocity;
			if (WeaponType.ranged.Equals(type))
				additinalForce = Vector2.zero;
			newProj.GetComponent<Rigidbody2D>().AddForce(additinalForce + (dir * projSpeed), ForceMode2D.Impulse);
			yield return new WaitForSeconds(0.02f);
		}
	}

	public void Equip () {
		rb2d.bodyType = RigidbodyType2D.Static;
		isEquiped = true;
		sR.sprite = onBody;
		sR.sortingLayerName = "Equiped";
		gameObject.layer = 12;
		StopCoroutine(scaleCoRoutine);
		StopCoroutine(colorCoRoutine);
	}

	public void unEquip () {
		rb2d.bodyType = RigidbodyType2D.Dynamic;
		isEquiped = false;
		sR.sprite = onGround;
		sR.sortingLayerName = "Weapon";
		gameObject.layer = 0;
		StartCoroutine(scaleCoRoutine);
		StartCoroutine(colorCoRoutine);
	}

	void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 8 && isEquiped == false) {
			rb2d.bodyType = RigidbodyType2D.Static;
		}
		else if (other.GetType() == typeof(CapsuleCollider2D)) {
			if (other.gameObject.layer == 10 && killOnDropHit && isEquiped == false && rb2d.velocity != Vector2.zero)
				other.GetComponent<Enemy>().Die();
		}
	}
}
