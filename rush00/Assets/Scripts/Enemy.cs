using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    public float inertia = 500f;
    public float fieldOfView = 120f;
    public float sightDistance = 8f;
    public float omniDetectionRadius = 1.25f;
    public float trackingTime = 20f;
    public bool isTracking = false;
    public bool isBlocked = false;
    public bool showDebug = false;
    public bool isDead = false;

    public string currentRoom;

    public AudioClip[] aDeath;

    private GameObject Head;
    private GameObject Body;
    private GameObject Legs;
    private Rigidbody2D rb2d;

    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Transform DoorTransform;
    [SerializeField] private float startTrackingTimepoint;
    [SerializeField] private Vector3 startPosition;

	private IEnumerator trackPlayer;

    private int pathIndex = -1;
    private List<Transform> path;

    void Start()
    {
        Head = this.transform.Find("Head").gameObject;
        Body = this.transform.Find("Body").gameObject;
        Legs = this.transform.Find("Legs").gameObject;
        rb2d = this.GetComponent<Rigidbody2D>();
        this.GetComponent<CircleCollider2D>().radius = sightDistance;
        Legs.GetComponent<Animator>().Play("LegAnimation"); // start the animation
        startPosition = this.transform.position;
    }

    void FixedUpdate()
    {
        HandleMovement();
        AnimateLegs();
        HandleWeapon();
        ShowDebug();

        if (Time.time - startTrackingTimepoint > trackingTime) {
            PlayerTransform = null;
            isTracking = false;
        }
    }

    void HandleMovement()
    {
        if (path != null && path.Count > 0 && pathIndex < path.Count && !isDead) {
            if (Vector3.Distance(this.transform.position, path[pathIndex].position) <= 1f) {
                pathIndex++;
            }
            if (pathIndex < path.Count) {
                Vector3 forces = -(this.transform.position - path[pathIndex].position).normalized;
                this.transform.rotation = Quaternion.LookRotation(Vector3.forward, -forces);
                rb2d.AddForce(forces * inertia);
            } else {
                // tourner
                // path = null;
                // pathIndex = -1;
                Debug.Log("spin");
                rb2d.AddTorque(0.5f, ForceMode2D.Impulse);
            }
        } else if (PlayerTransform != null && !isBlocked && !isDead) {
            Vector3 forces = -(this.transform.position - PlayerTransform.position).normalized;
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, -forces);
            rb2d.AddForce(forces * inertia);
        } else if (PlayerTransform != null && isBlocked && !isDead) {
            // Vector3 forces = -(this.transform.position - DoorTransform.position).normalized;
            // this.transform.rotation = Quaternion.LookRotation(Vector3.forward, -forces);
            // rb2d.AddForce(forces * inertia);
        }
    }

    Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    Vector3 GetMouseWorldPosition() {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }

    void FacePosition(Vector3 position)
    {
        if (!isDead)
        {
            Vector3 dir = (this.transform.position - position).normalized;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
            this.transform.rotation = rot;
        }
    }

    void TrackPlayerMode(Transform transform) {
        if (!isTracking) {
            PlayerTransform = transform;
            Debug.Log(PlayerTransform.position);
            trackPlayer = TrackPlayer();
            StartCoroutine(trackPlayer);
            FacePosition(PlayerTransform.position);
        }
        isTracking = true;
    }

    void AnimateLegs()
    {
        Legs.GetComponent<Animator>().speed = rb2d.velocity.magnitude * 0.35f;
    }

    void HandleWeapon()
    {
        // if (Input.GetMouseButtonDown(0) && weapon != null)
        // {
        //     weapon.GetComponent<WeaponScript>().Fire();
        // }
        // else if (Input.GetMouseButton(0) && weapon != null)
        // {
        //     if (FireMode.auto.Equals(weapon.GetComponent<WeaponScript>().fireMode)
        //             || FireMode.rifle.Equals(weapon.GetComponent<WeaponScript>().fireMode))
        //     {
        //         Debug.Log("Auto/Rifle");
        //         weapon.GetComponent<WeaponScript>().Fire();
        //     }
        // }
    }

    void OnTriggerEnter2D(Collider2D other) {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if ((this.transform.position - other.transform.position).magnitude < omniDetectionRadius) {
                startTrackingTimepoint = Time.time;
                TrackPlayerMode(other.transform);
                return;
            }
            Vector3 directionToPlayer = -(this.transform.position - other.transform.position).normalized;
            Vector3 direction = Vector2FromAngle(this.transform.rotation.eulerAngles.z - 90f);
            float angle = Vector3.Angle(direction, directionToPlayer);

            if (angle < fieldOfView * 0.5f)
            {
                RaycastHit2D[] hits2D = Physics2D.RaycastAll(this.transform.position, directionToPlayer, sightDistance);
                foreach (RaycastHit2D hit2D in hits2D)
                {
                    if (showDebug)
                        Debug.DrawRay(this.transform.position, directionToPlayer * sightDistance, (hit2D.collider.tag == "Player" ? Color.blue : Color.red));
                    if (hit2D.collider.tag == "Player")
                    {
                        startTrackingTimepoint = Time.time;
                        TrackPlayerMode(other.transform);
                        if (weapon != null && !isDead)
                            weapon.GetComponent<WeaponScript>().Fire();
                        return;
                    }
                    if (hit2D.collider.tag == "Wall")
                        return;
                }
            }
        } else if (other.tag == "Room" && other.OverlapPoint(transform.position)) {
            // Debug.Log(other.name);
            currentRoom = other.name;
        }
    }

    bool isPlayerVisible() {
        if (PlayerTransform == null)
            return false;
        Vector3 directionToPlayer = -(this.transform.position - PlayerTransform.position).normalized;
        Vector3 direction = Vector2FromAngle(this.transform.rotation.eulerAngles.z - 90f);
        RaycastHit2D[] hits2D = Physics2D.RaycastAll(this.transform.position, directionToPlayer, sightDistance * 4);
        foreach (RaycastHit2D hit2D in hits2D)
        {
            if (showDebug)
                Debug.DrawRay(this.transform.position, directionToPlayer * sightDistance * 4, (hit2D.collider.tag == "Player" ? Color.blue : Color.red));
            if (hit2D.collider.tag == "Player")
            {
                startTrackingTimepoint = Time.time;
                if (weapon != null && !isDead)
                    weapon.GetComponent<WeaponScript>().Fire();
                return true;
            }
            if (hit2D.collider.tag == "Wall")
                return false;
        }
        return false;
    }

	private IEnumerator TrackPlayer () {
        Debug.Log("coroutine");
        isTracking = true;
        if (PlayerTransform != null)
        {
            while (isTracking)
            {
                if (PlayerTransform == null)
                    break;
                isBlocked = !isPlayerVisible();
                if (isBlocked)
                {
                    if (int.Parse(currentRoom) > 0)
                    {

                        // Debug.Log(currentRoom);
                        // Debug.Log(PathFinding.instance.CheckPoints[int.Parse(currentRoom)].transform);
                        path = PathFinding.instance.GetPathFromTo(PathFinding.instance.CheckPoints[int.Parse(currentRoom)].GetComponent<CheckPoint>(), PathFinding.instance.CheckPoints[int.Parse(PlayerTransform.gameObject.GetComponent<Player>().currentRoom)].GetComponent<CheckPoint>());
                        pathIndex = 0;
                        // foreach (var transform in path) {
                        //     if (transform)
                        //         Debug.Log(transform);
                        // }
                        PathFinding.instance.ResetPathFinding();
                    }
                    // PathFinding.instance.GetPathFromTo();
                    // float minDist = Mathf.Infinity;
                    // float minAngle = Mathf.Infinity;
                    // Vector3 directionToPlayer = -(this.transform.position - PlayerTransform.position).normalized;
                    // foreach (var door in Doors.instance.doors) {
                    //     if (door) {
                    //         Vector3 directionToDoor = -(this.transform.position - door.transform.position).normalized;
                    //         Debug.DrawRay(this.transform.position, directionToDoor * sightDistance * 4, Color.cyan);
                    //         // Vector3 direction = Vector2FromAngle(this.transform.rotation.eulerAngles.z - 90f);
                    //         float angle = Vector3.Angle(directionToDoor, directionToPlayer);
                    //         // if (Vector3.Distance(transform.position, door.transform.position) < minDist) {
                    //         //     minDist = Vector3.Distance(transform.position, door.transform.position);
                    //         //     FacePosition(door.transform.position);
                    //         // }
                    //         if (angle < minAngle) {
                    //             minAngle = angle;
                    //             FacePosition(door.transform.position);
                    //             DoorTransform = door.transform;
                    //         }
                    //     }
                    // }
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void ShowDebug()
    {
        if (showDebug)
        {
            Debug.DrawRay(this.transform.position, Vector2FromAngle(this.transform.rotation.eulerAngles.z - 90f + fieldOfView * 0.5f) * sightDistance, Color.green);
            Debug.DrawRay(this.transform.position, Vector2FromAngle(this.transform.rotation.eulerAngles.z - 90f - fieldOfView * 0.5f) * sightDistance, Color.green);
        }
    }

    public void EquipWeapon(GameObject weap) {
        weap.GetComponent<WeaponScript>().Equip();
        weapon = weap.gameObject;
        weapon.transform.parent = this.transform;
        weapon.transform.localPosition = new Vector3(-0.247f, -0.209f, 0);
        weapon.transform.localEulerAngles = new Vector3(0, 0, 1.22f);
    }

    public void Die() {
        Debug.Log("* yaaaarg *");
        if (!isDead)
        {
            rb2d.AddTorque(0.25f, ForceMode2D.Impulse);
            SoundManager.soundManager.PlayAudioClip(aDeath[Random.Range(0, aDeath.Length)], 0.5f);
            GameLogic.gameLogic.enemyCount -= 1;
            GameObject.Destroy(gameObject, 1f);
        }
        isDead = true;
    }
}
