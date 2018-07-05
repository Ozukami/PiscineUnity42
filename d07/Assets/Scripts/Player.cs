using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float speed;
    public float jumpSpeed;
    public float gravity;
	public float mouseSensitivity;

    public int lifeMax;
    private int lifeCount;
    public int ammoMax;
    private int ammoCount;
    public float range;
    public int boostMax;
    private int boostCount;

    public Text lifeUI, ammoUI;
    public Image crossHair;
    public Canvas mainUI, deathUI;

	public GameObject camera;

    private Vector3 canonRotation = Vector3.zero;
	private Rigidbody rb;
    private ParticleSystem[] fireEffects;
    public GameObject explosionEffect;

	void Awake () {
        rb = GetComponent<Rigidbody>();
        fireEffects = GetComponentsInChildren<ParticleSystem>();
        Cursor.lockState = CursorLockMode.Locked;
        deathUI.gameObject.SetActive(false);
        lifeCount = lifeMax;
        ammoCount = ammoMax;
        boostCount = boostMax;
	}

    void Update () {
        lifeUI.text = "" + Mathf.Clamp(lifeCount, 0, lifeMax);
        ammoUI.text = "x" + ammoCount;
        if (Input.GetMouseButtonDown(0))
            Fire();
        else if (Input.GetMouseButtonDown(1))
            SecondaryFire();
    }

    void FixedUpdate () {
        RotateCanon();
        MoveTank();
        RotateTank();
    }

    void RotateCanon () {
		canonRotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
		camera.transform.localEulerAngles = canonRotation;
    }

    void MoveTank () {
        Vector3 movement = transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && boostCount > 0) {
            movement *= 3;
            boostCount -= 2;
            if (boostCount <= 0)
                boostCount -= 20;
        } else if (boostCount < boostMax)
            boostCount++;
        rb.MovePosition(rb.position + movement);
    }

    void RotateTank () {
        float turn = Input.GetAxis("Horizontal") * speed * 10 * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler (0, turn, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void Fire () {
        fireEffects[0].Play();
        MusicManager.instance.gunShot.Play();
        RaycastHit hit;
        Vector3 forward = camera.transform.TransformDirection(Vector3.forward) * range;
        if (Physics.Raycast(camera.transform.position, forward, out hit)) {
            if (hit.collider.gameObject.layer == 9) {
                Debug.DrawRay(camera.transform.position, forward, Color.green);
                hit.collider.transform.root.gameObject.GetComponent<Tank>().TakeDamage(5, gameObject);
                StopAllCoroutines();
                StartCoroutine(HitFeedback());
            } else {
                Debug.DrawRay(camera.transform.position, forward, Color.red);
            }
        }
    }

    void SecondaryFire () {
        if (ammoCount > 0) {
            ammoCount--;
            fireEffects[1].Play();
            MusicManager.instance.noHit.Play();
            RaycastHit hit;
            Vector3 forward = camera.transform.TransformDirection(Vector3.forward) * range;
            if (Physics.Raycast(camera.transform.position, forward, out hit)) {
                if (hit.collider.gameObject.layer == 9) {
                    Debug.DrawRay(camera.transform.position, forward, Color.green);
                    MusicManager.instance.explosion.Play();
                    GameObject exp = Instantiate(explosionEffect, hit.collider.transform.position, Quaternion.identity);
                    Destroy(exp, exp.GetComponent<ParticleSystem>().main.duration);
                    hit.collider.transform.root.gameObject.GetComponent<Tank>().TakeDamage(15, gameObject);
                    StopAllCoroutines();
                    StartCoroutine(HitFeedback());
                } else {
                    Debug.DrawRay(camera.transform.position, forward, Color.red);
                }
            }
        }
    }

    IEnumerator HitFeedback () {
        crossHair.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        crossHair.color = Color.white;
    }

	public void TakeDamage (int amount) {
		if (amount > 0)
			lifeCount -= amount;
		if (IsDead()) {
            StopAllCoroutines();
            StartCoroutine(Death());
		}
	}

    IEnumerator Death () {
        GetComponent<SphereCollider>().enabled = false;
        mainUI.gameObject.SetActive(false);
        deathUI.gameObject.SetActive(true);
        GameObject exp = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(exp, exp.GetComponent<ParticleSystem>().main.duration);
        yield return new WaitForSeconds(3);
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Application.Quit();
    }

    public bool IsDead () {
        if (lifeCount <= 0)
            return true;
        return false;
    }

}
