using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject satPrefab;
    [SerializeField]
    float altitude;

    int N = 60;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < 40; j++) {
                GameObject sat = Instantiate(satPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                sat.transform.parent = transform;
                SatOrbit so = sat.GetComponent<SatOrbit>();
                so.plane = Quaternion.Euler(0, j * 9, 0) * Quaternion.Euler(60, 0, 0) * Vector3.up;
                so.phase = Mathf.Deg2Rad * 360 * i/N;
                so.altitude = altitude;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
