using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    public float inertia = 500f;
    public AudioClip pickupSound;

    public string currentRoom;
    public GameObject menuGUI;

    private GameObject Head;
    private GameObject Body;
    private GameObject Legs;
    private Rigidbody2D rb2d;

    private GameObject weapon;
    private MainMenuTextButton titleText;

	void Start () {
        titleText = menuGUI.transform.Find("Title").GetComponent<MainMenuTextButton>();
        Head = this.transform.Find("Head").gameObject;
        Body = this.transform.Find("Body").gameObject;
        Legs = this.transform.Find("Legs").gameObject;
        rb2d = this.GetComponent<Rigidbody2D>();
        Legs.GetComponent<Animator>().Play("LegAnimation"); // start the animation
	}
	
	void FixedUpdate () {
        HandleMovement();
        FollowMouseOrientation();
        AnimateLegs();
        HandleWeapon();

        if (GameLogic.gameLogic.levelComplete)
        {
            titleText.text = "You win!";
            menuGUI.SetActive(true);
        }
	}

    void HandleMovement() {
        Vector2 forces = new Vector3(Mathf.Cos(Input.GetAxis("Horizontal") - Mathf.PI * 0.5f), Mathf.Sin(Input.GetAxis("Vertical")));
        rb2d.AddForce(forces * inertia);
    }

    void FollowMouseOrientation() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 dir = (this.transform.position - mousePos).normalized;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        this.transform.rotation = rot;
    }

    // animate the legs when player moves (depends on velocity)
    void AnimateLegs() {
        Legs.GetComponent<Animator>().speed = rb2d.velocity.magnitude * 0.35f;
    }

    void HandleWeapon() {
        if (Input.GetMouseButtonDown(1) && weapon != null) {
            ThrowWeapon();
        } else if (Input.GetMouseButtonDown(0) && weapon != null) {
            weapon.GetComponent<WeaponScript>().Fire();
        } else if (Input.GetMouseButton(0) && weapon != null) {
            if (FireMode.auto.Equals(weapon.GetComponent<WeaponScript>().fireMode)
                    || FireMode.rifle.Equals(weapon.GetComponent<WeaponScript>().fireMode)) {
                Debug.Log("Auto/Rifle");
                weapon.GetComponent<WeaponScript>().Fire();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit") {
            titleText.text = "You win!";
            menuGUI.SetActive(true);
        }
        if (other.tag == "Ammo" && other.gameObject.GetComponent<AmmoScript>().targetLayer == 11) {
            Debug.Log("HIT");
            Die();
        }
        if (other.tag == "Room") {
            // Debug.Log(other.name);
            currentRoom = other.name;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (weapon == null && other.tag == "Weapon" && other.gameObject.layer == 0 && Input.GetKeyDown(KeyCode.E)) {
            EquipWeapon(other.gameObject);
            SoundManager.soundManager.PlayAudioClip(pickupSound, 0.5f);
        }
    }

    void EquipWeapon(GameObject weap) {
        weap.GetComponent<WeaponScript>().Equip();
        weapon = weap.gameObject;
        weapon.transform.parent = this.transform;
        weapon.transform.localPosition = new Vector3(-0.247f, -0.209f, 0);
        weapon.transform.localEulerAngles = new Vector3(0, 0, 1.22f);
    }

    void ThrowWeapon() {
        weapon.GetComponent<WeaponScript>().unEquip();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 dir = (mousePos - this.transform.position).normalized;
		weapon.GetComponent<Rigidbody2D>().AddForce(dir * 50, ForceMode2D.Impulse);
		weapon.GetComponent<Rigidbody2D>().AddTorque(20, ForceMode2D.Impulse);
        weapon.transform.parent = null;
        weapon = null;
    }

    void Die() {
        menuGUI.SetActive(true);
        GameObject.Destroy(gameObject);
    }

    public WeaponScript GetWeapon() {
        if (weapon == null)
            return null;
        return weapon.GetComponent<WeaponScript>();
    }
}
