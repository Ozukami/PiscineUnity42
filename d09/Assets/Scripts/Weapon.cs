using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Transform CannonPoint;
    public LineRenderer Trail;
    public ParticleSystem Effect;
    public AudioSource FireAudio;

    public int atkDmg;
    public float FireRate;
    private float time = 10;

    void Update () {
        time += Time.deltaTime;
    }

    public void Fire () {
        if (time >= FireRate) {
            time = 0;
            FireAudio.Play();
            Trail.positionCount = 2;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100,
                ~(1 << 8))) {
                Debug.Log(hit.collider.name);

                Trail.SetPosition(0, CannonPoint.position);
                Trail.SetPosition(1, hit.point);
                StartCoroutine(ResorbTrail(CannonPoint, hit.transform));
                Instantiate(Effect, hit.point, Quaternion.identity);

                if (hit.collider.gameObject.layer == 9) {
                    if (hit.collider.gameObject.GetComponent<Enemy>() != null) {
                        hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(atkDmg);
                        hit.collider.gameObject.GetComponent<Enemy>().GoAtPos(GameObject.Find("Player").transform.position);
                    }
                }
            }

            StartCoroutine(Recoil());
        }
    }

    IEnumerator ResorbTrail (Transform pos1, Transform pos2) {
        while (Vector3.Distance(Trail.GetPosition(0), Trail.GetPosition(1)) > 0.5f) {
            Trail.SetPosition(0, Vector3.MoveTowards(Trail.GetPosition(0), Trail.GetPosition(1), 20f * Time.deltaTime));
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Recoil () {
        float tmp = transform.localEulerAngles.x;
        float witness = transform.localEulerAngles.x;
        if (tag == "Gun") {
            tmp = transform.localEulerAngles.y;
            witness = transform.localEulerAngles.y;
            while (witness < tmp + 30) {
                transform.Rotate(0, 3, 0);
                witness += 3;
                yield return new WaitForFixedUpdate();
            }

            while (witness > tmp) {
                transform.Rotate(0, -3, 0);
                witness -= 3;
                yield return new WaitForFixedUpdate();
            }
        }
        else if (tag == "Zod") {
            while (witness > tmp - 30) {
                transform.Rotate(-3, 0, 0);
                witness -= 3;
                yield return new WaitForFixedUpdate();
            }

            while (witness < tmp) {
                transform.Rotate(3, 0, 0);
                witness += 3;
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForFixedUpdate();
    }
}