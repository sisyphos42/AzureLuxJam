using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatOrbit : MonoBehaviour
{
    public float speed = 1f;
    public float altitude;
    public Vector3 plane;
    public float phase;

    float phi, theta;

    public int i, j;

    [SerializeField]
    Material mat;
    Material _mat_orig;

    public bool infected = false;
    bool coll = false;

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

        phi += 0.0001f * speed;

        coll = true;
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision);
    }

    void OnTriggerStay(Collider other) {
        if (!coll) return;
        if (!infected) return;

        //Debug.Log(other);
        //Debug.Log(i.ToString() + " " + j.ToString() + " " + other.GetComponent<SatOrbit>().i.ToString() + " " + other.GetComponent<SatOrbit>().j.ToString());
        //GetComponent<Renderer>().material = mat;
        SatOrbit so = other.GetComponent<SatOrbit>();
        so.SetInfected(true);
        // if (!so.infected) {
        //     so.infected = true;
        //      other.GetComponent<Renderer>().material = mat;
        //      Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
        //     rb.useGravity = false;
        // }
    }

    public void SetInfected(bool inf) {
        if (infected == inf) return;
        infected = inf;
        if (inf) {
            GetComponent<Renderer>().material = mat;
            if (GetComponent<Rigidbody>() == null) {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
            }
        } else {
            GetComponent<Renderer>().material = _mat_orig;
        }
    }
}
