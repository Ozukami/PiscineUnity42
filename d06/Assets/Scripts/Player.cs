using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

	public float detectionFactor;

	public Canvas playerHUD;
	public Canvas endScreen;
	public Canvas hint;
	public Text hintText;

	public Slider stealthSlider;
	public Image sliderFill;
	public Sprite fill1;
	public Sprite fill2;	

    private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	// public GameObject camera;

	public AudioSource mainAS;
	public AudioSource endAS;
	public AudioSource runAS;
	public AudioSource keyPickUp;
	public AudioClip stealthAC;
	public AudioClip panicAC;

	public ParticleSystem smoke;
	public GameObject smokeEffect;
	public GameObject laser;
	public GameObject door;

	public Light alarmLight;
	
	public float mouseSensitivity;

	private bool alarm = false;
	private bool end = false;
	public bool key = false;
	public bool running = false;
 
    private float rotationZ = 0;
    private float rotationY = 0;
    private float maximumX = 30;
    private float minimumX = -30;

	private float time = 0;

    // private GameManager gM;
    // private Rigidbody rb;

    private Vector3 tmp = Vector3.zero;

	private IEnumerator alarmCoroutine;

    void Start () {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
		endScreen.gameObject.SetActive(false);
		alarmLight.gameObject.SetActive(false);
		hint.gameObject.SetActive(false);
		smokeEffect.gameObject.SetActive(false);
		alarmCoroutine = Alarm();
    }

	void Update () {
		time += Time.deltaTime;
		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Vector3.Distance(controller.velocity, Vector3.zero) > 0) {
			if (time >= 0.1f && stealthSlider.value < 1.00f) {
				if (!runAS.isPlaying)
					runAS.Play();
				stealthSlider.value += 0.03f * detectionFactor;
				time = 0;
			}
		} else if (time >= 0.1f && stealthSlider.value > 0) {
			stealthSlider.value -= 0.005f * detectionFactor;
			time = 0;
		}
		if (stealthSlider.value >= 1.00f && !end) {
			end = true;
			playerHUD.gameObject.SetActive(false);
			endScreen.gameObject.SetActive(true);
			StartCoroutine(ReloadScene());
		} else if (stealthSlider.value >= 0.75f) {
			sliderFill.sprite = fill1;
			if (!alarm)
				StartCoroutine(alarmCoroutine);
			alarm = true;
			if (mainAS.clip != panicAC && !end) {
				mainAS.clip = panicAC;
				mainAS.Play();
			}
		}
		else {
			sliderFill.sprite = fill2;
			if (alarm) {
				alarmLight.gameObject.SetActive(false);
				StopCoroutine(alarmCoroutine);
			}
			alarm = false;
			if (mainAS.clip != stealthAC && !end) {
				mainAS.clip = stealthAC;
				mainAS.Play();
			}
		}
	}

    void FixedUpdate () {
		rotationZ += Input.GetAxis("Mouse Y") * mouseSensitivity * 0.5f;
		rotationZ = Mathf.Clamp(rotationZ, minimumX, maximumX);
		rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
		tmp.z = -rotationZ;
		tmp.y = rotationY;
		tmp.x = 0;
		transform.localEulerAngles = tmp;

        if (controller.isGrounded) {
            moveDirection = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            // if (Input.GetButton("Jump"))
            //     moveDirection.y = jumpSpeed;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				moveDirection *= 2;
        }
        moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
    }

	void OnTriggerStay (Collider other) {
		if (other.tag == "CameraDetection")
			stealthSlider.value += 0.01f * detectionFactor;
		else if (other.tag == "LightDetection")
			stealthSlider.value += 0.005f * detectionFactor;
		if (other.tag == "Fan") {
			if (Input.GetKeyDown(KeyCode.E)) {
				smoke.Play();
				smokeEffect.gameObject.SetActive(true);
			}
		} else if (other.tag == "DoorLocker") {
			if (Input.GetKeyDown(KeyCode.E) && key) {
				// door.transform.Translate(2.2f, 0, 0);
				other.gameObject.GetComponent<Lockers>().Activate();
			}
		} else if (other.tag == "Key") {
			if (Input.GetKeyDown(KeyCode.E)) {
				key = true;
				keyPickUp.Play();
				hint.gameObject.SetActive(false);
				Destroy(other.gameObject);
			}
		} else if (other.tag == "LaserLocker") {
			if (Input.GetKeyDown(KeyCode.E)) {
				laser.gameObject.SetActive(false);
				other.gameObject.GetComponent<Lockers>().Activate();
			}
		} else if (other.tag == "TV")
			if (Input.GetKeyDown(KeyCode.E))
				StartCoroutine(ReloadScene());
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Smoke")
			detectionFactor = 0.2f;
		else if (other.tag == "LaserDetection")
			StartCoroutine(ReloadScene());
		else if (other.tag == "TV") {
			hintText.text = "Press 'E' to end the simulation";
			hint.gameObject.SetActive(true);
		} else if (other.tag == "Fan" && !end) {
			hintText.text = "Press 'E' to activate the fan";
			hint.gameObject.SetActive(true);
			if (Input.GetKeyDown(KeyCode.E))
				smoke.Play();
		} else if (other.tag == "DoorLocker" && !end) {
			hintText.text = (key) ? "Press 'E' to activate the door" : "You need to find the door key first";
			hint.gameObject.SetActive(true);
		} else if (other.tag == "Key" && !end) {
			hintText.text = "Press 'E' to get the key";
			hint.gameObject.SetActive(true);
		} else if (other.tag == "LaserLocker" && !end) {
			hintText.text = "Press 'E' to disable lasers";
			hint.gameObject.SetActive(true);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Smoke")
			detectionFactor = 1;
		if (other.tag == "Fan" || other.tag == "DoorLocker" || other.tag == "Key" || other.tag == "LaserLocker")
			hint.gameObject.SetActive(false);
	}

    IEnumerator Alarm() {
		while (true) {
			alarmLight.gameObject.SetActive(true);
        	yield return new WaitForSeconds(0.3f);
			alarmLight.gameObject.SetActive(false);
        	yield return new WaitForSeconds(0.3f);
		}
    }

    IEnumerator ReloadScene() {
		end = true;
		mainAS.Stop();
		endAS.Play();
		hintText.text = "";
		hint.gameObject.SetActive(false);
		playerHUD.gameObject.SetActive(false);
		endScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
