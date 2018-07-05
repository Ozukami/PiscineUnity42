using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Tank : MonoBehaviour {
    public GameObject player;
    public GameObject canon;
    private GameObject target;

    public int lifeMax;
    private int lifeCount;
    public float range;

    public Canvas canvasUI;
    public Slider lifeUI;

    private NavMeshAgent agent;
    public Transform[] points;
    private int currentPoint;
    [HideInInspector] public bool isPatrolling = true;
    private bool inRange = false;

    private ParticleSystem[] fireEffects;
    public AudioSource gunShot, noHit, explosion;
    public GameObject explosionEffect;

    private IEnumerator _trackCoroutine;

    // private ParticleSystem[] fireEffects;

    void Start () {
        fireEffects = GetComponentsInChildren<ParticleSystem>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        lifeCount = lifeMax;
        _trackCoroutine = Track();
        GoToNextPoint();
    }

    void Update () {
        if (lifeCount > 0) {
            if (!agent.pathPending && agent.remainingDistance < 5f && isPatrolling)
                GoToNextPoint();
            lifeUI.value = lifeCount;
            RotateUI();
            if (target != null)
                canon.transform.LookAt(target.transform);
            else
                canon.transform.LookAt(transform.forward);
        }
        else {
            agent.Stop();
        }
    }

    void GoToNextPoint () {
        agent.Resume();
        if (points.Length < 1)
            return;
        agent.destination = points[currentPoint].position;
        currentPoint = (currentPoint + 1) % points.Length;
    }

    public void TakeDamage (int amount, GameObject from) {
        if (amount > 0)
            lifeCount -= amount;
        if (lifeCount <= 0) {
            StopAllCoroutines();
            StartCoroutine(Death());
        }

        if (target == null) {
            target = from;
            StartCoroutine(_trackCoroutine);
        }
    }

    IEnumerator Death () {
        if (!explosion.isPlaying)
            explosion.Play();
        GameObject exp = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(exp, exp.GetComponent<ParticleSystem>().main.duration);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void RotateUI () {
        if (player != null)
            canvasUI.transform.LookAt(player.transform);
    }

    void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Tank" && target == null) {
            target = other.gameObject;
            isPatrolling = false;
            // agent.destination = target.transform.position;
            StartCoroutine(_trackCoroutine);
        }
        else if (other.gameObject.tag == "Tank"
                 && Vector3.Distance(transform.position, other.transform.position) <
                 Vector3.Distance(transform.position, target.transform.position)) {
            target = other.gameObject;
        }
    }

    IEnumerator Track () {
        while (true) {
            if (target != null && Vector3.Distance(transform.position, target.transform.position) < range) {
                agent.Stop();
                Fire();
            }
            else if (target != null) {
                agent.Resume();
                agent.destination = target.transform.position;
            }
            else {
                isPatrolling = true;
                GoToNextPoint();
            }

            yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));
        }
    }

    void Fire () {
        Vector3 targetDir = target.transform.position - transform.position;
        float angle = Vector3.Angle(target.transform.position, transform.position);
        transform.eulerAngles = new Vector3(0, angle, 0);
        int type = Random.Range(0, 2);
        fireEffects[type].Play();
        if (type == 0) {
            gunShot.Play();
        }
        else {
            noHit.Play();
        }

        RaycastHit hit;
        Vector3 randomVector = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        Vector3 fireDirection =
            target.transform.Find("canon").transform.position - canon.transform.position + randomVector;
        if (Physics.Raycast(canon.transform.position, fireDirection, out hit)) {
            if (hit.collider.gameObject.layer == 8) {
                Debug.DrawRay(canon.transform.position, fireDirection, Color.green);
                hit.collider.transform.root.GetComponent<Player>().TakeDamage(5 + (type * 10));
                if (hit.collider.transform.root.GetComponent<Player>().IsDead()) {
                    target = null;
                    isPatrolling = true;
                    StopAllCoroutines();
                    GoToNextPoint();
                }
            }
            else if (hit.collider.gameObject.layer == 9) {
                Debug.DrawRay(canon.transform.position, fireDirection, Color.green);
                hit.collider.transform.root.gameObject.GetComponent<Tank>().TakeDamage(5 + (type * 10), gameObject);
            }
            else {
                Debug.DrawRay(canon.transform.position, fireDirection, Color.red);
            }
        }
    }
}