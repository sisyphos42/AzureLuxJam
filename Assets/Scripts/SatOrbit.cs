using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatOrbit : MonoBehaviour
{
    public Controls controls;
    public float speed = 1f;
    public float altitude;
    public Vector3 plane;
    public float phase;

    public float phi, theta;

    public int i, j;

    [SerializeField]
    Material mat;
    Material _mat_orig;
    [SerializeField]
    Material mat_upd;

    public bool infected = false;
    public bool updated = false;
    bool coll = false;

    float _immuneTime = 0f;

    public GameObject sp;

    void OnEnable() {
        _mat_orig = GetComponent<Renderer>().material;
    }
    // Start is called before the first frame update
    void Start()
    {
        phi = 0;
        theta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Quaternion q = Quaternion.Euler(sb, 0, 0);
        // transform.position = q * new Vector3(Mathf.Cos(sa + phi), 0, Mathf.Sin(sa + phi)) * altitude;

        //transform.position = altitude * (Quaternion.Euler(0, Mathf.Rad2Deg * sc, 0) * Quaternion.Euler(Mathf.Rad2Deg * sb, 0, 0) * Quaternion.Euler(0, Mathf.Rad2Deg * (sa + phi), 0) * Vector3.right);
        
        transform.position =  altitude * (Quaternion.FromToRotation(Vector3.up, plane) * Quaternion.Euler(0, -Mathf.Rad2Deg * (phase + phi), 0) * Vector3.right);

        //transform.position = Quaternion.AngleAxis(0.0001f * speed * Mathf.Rad2Deg, plane) * transform.position;

        phi += 0.01f * speed * Time.deltaTime;

        coll = true;

        if (_immuneTime > 0) {
            _immuneTime -= Time.deltaTime;
        }
    }

    void FixedUpdate() {
        // if (sp.GetComponent<SatSpawner>().infected < 10) {
        //     if (Random.value < 0.000001f * (1 + Time.time/180f)) {
        //         SetInfected(true);
        //         Debug.Log("infect");
        //     }
        // }
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision);
    }

    void OnTriggerStay(Collider other) {
        if (!controls.gameRunning) return;

        if (!coll) return;
        if (!infected) return;
        if (updated) return;

        //Debug.Log(other);
        //Debug.Log(i.ToString() + " " + j.ToString() + " " + other.GetComponent<SatOrbit>().i.ToString() + " " + other.GetComponent<SatOrbit>().j.ToString());
        //GetComponent<Renderer>().material = mat;
        SatOrbit so = other.GetComponent<SatOrbit>();
        so?.SetInfected(true);
        // if (!so.infected) {
        //     so.infected = true;
        //      other.GetComponent<Renderer>().material = mat;
        //      Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
        //     rb.useGravity = false;
        // }
    }

    public void SetInfected(bool inf) {
        //Debug.Log(string.Format("{0}, {1}, {2}", inf, infected, _immuneTime));
        //if (infected == inf) return;
        //if (inf && (_immuneTime > 0 || updated)) return;
        if (inf && updated) return;
        infected = inf;
        if (inf) {
            GetComponent<Renderer>().material = mat;
            if (GetComponent<Rigidbody>() == null) {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
            }
        } else {
            GetComponent<Renderer>().material = mat_upd;
            updated = true;
            if (GetComponent<Rigidbody>() == null) {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
            }
        }
        _immuneTime = 5f;
    }

    public void Reset() {
        phi = 0;
        infected = false;
        updated = false;
        GetComponent<Renderer>().material = _mat_orig;
    }
}
