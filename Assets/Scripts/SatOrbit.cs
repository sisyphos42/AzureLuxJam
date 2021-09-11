using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatOrbit : MonoBehaviour
{
    public float altitude;
    public float sa, sb, sc;
    public Vector3 plane;
    public float phase;

    float phi, theta;

    public int i, j;

    [SerializeField]
    Material mat;

    public bool infected = false;
    bool coll = false;

    // Start is called before the first frame update
    void Start()
    {
        phi = 0;
        theta = 0;
        //Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        // Quaternion q = Quaternion.Euler(sb, 0, 0);
        // transform.position = q * new Vector3(Mathf.Cos(sa + phi), 0, Mathf.Sin(sa + phi)) * altitude;

        //transform.position = altitude * (Quaternion.Euler(0, Mathf.Rad2Deg * sc, 0) * Quaternion.Euler(Mathf.Rad2Deg * sb, 0, 0) * Quaternion.Euler(0, Mathf.Rad2Deg * (sa + phi), 0) * Vector3.right);
        
        transform.position =  altitude * (Quaternion.FromToRotation(Vector3.up, plane) * Quaternion.Euler(0, -Mathf.Rad2Deg * (phase + phi), 0) * Vector3.right);

        phi += 0.0002f;

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
        if (!so.infected) {
            so.infected = true;
            other.GetComponent<Renderer>().material = mat;
            Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }
}
