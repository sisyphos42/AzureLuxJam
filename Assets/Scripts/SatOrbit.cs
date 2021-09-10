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

        phi += 0.0001f;
    }
}
