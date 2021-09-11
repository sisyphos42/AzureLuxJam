using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject satPrefab;
    [SerializeField]
    float altitude;

    int Ni = 60;
    int Nj = 40;

    [SerializeField]
    Material mat;

    GameObject[] sats;
    
    // Start is called before the first frame update
    void Start()
    {
        sats = new GameObject[Ni * Nj];

        for (int i = 0; i < Ni; i++) {
            for (int j = 0; j < Nj; j++) {
                GameObject sat = Instantiate(satPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //Rigidbody rb = sat.AddComponent<Rigidbody>();
                //rb.useGravity = false;
                sat.transform.parent = transform;
                SatOrbit so = sat.GetComponent<SatOrbit>();
                so.plane = Quaternion.Euler(0, j * 9, 0) * Quaternion.Euler(60, 0, 0) * Vector3.up;
                so.phase = Mathf.Deg2Rad * ((360 * i/Ni) + 180);
                so.altitude = altitude;

                so.i = i;
                so.j = j;

                if (i == 0 && j == 0) {
                    //sat.GetComponent<Collider>().isTrigger = false;
                    Rigidbody rb = sat.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    sat.GetComponent<Renderer>().material = mat;
                    so.infected = true;
                }

                sats[i*Nj + j] = sat;
            }
        }

        StartCoroutine(CountSats());
        //StartCoroutine(CheckDistance());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckDistance() {
        yield return null;
        while (true) {
            for (int i = 0; i < Ni; i++) {
                for (int j = 0; j < Nj; j++) {
                    GameObject sat = sats[i*Nj + j];
                    for (int k = 0; k < Ni * Nj; k++) {
                        GameObject sat2 = sats[k];
                        float angle = Vector3.Angle(sat.transform.position, sat2.transform.position);
                        if (angle < 2) {
                            SatOrbit so = sat.GetComponent<SatOrbit>();
                            SatOrbit so2 = sat2.GetComponent<SatOrbit>();
                            if (so.infected || so2.infected) {
                                so.infected = true;
                                so2.infected = true;
                                sat.GetComponent<Renderer>().material = mat;
                                sat2.GetComponent<Renderer>().material = mat;
                            }
                        }
                    }
                    //yield return null;
                }
                yield return null;
            }
            //yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator CountSats() {
        while (true) {
            int n = 0;
            for (int i = 0; i < Ni * Nj; i++) {
                if (sats[i].GetComponent<SatOrbit>().infected) {
                    n++;
                }
            }
            Debug.Log("infected: " + n);

            yield return new WaitForSeconds(1f);
        }
    }

}
