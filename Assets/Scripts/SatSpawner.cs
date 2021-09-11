using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeptomoby.OrbitTools;

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

    public TextAsset satData;
    
    
    [System.Serializable]
    public struct SatData {
        public string context;
        public string id;
        public string type;
        public int totalItems;

        [System.Serializable]
        public struct DataI {
            public string id;
            public string type;
            public string satelliteId;
            public string name;
            public string date;
            public string line1;
            public string line2;

    }
        public DataI[] member;
    }

    // Start is called before the first frame update
    void Start()
    {
        SatData d = JsonUtility.FromJson<SatData>(satData.text.Replace("@", ""));

        Debug.Log(d.context);
        Debug.Log(d.id);
        Debug.Log(d.type);
        Debug.Log(d.totalItems);
        Debug.Log(d.member[0].name);
        Debug.Log(d.member[0].line1);
        Debug.Log(d.member[0].line2);


        string str1 = "SGP4 Test";
        string str2 = "1 88888U          80275.98708465  .00073094  13844-3  66816-4 0     8";
        string str3 = "2 88888  72.8435 115.9689 0086731  52.6988 110.5714 16.05824518   105";

        //"2021-09-10T20:05:16+00:00"
        str1 = "STARLINK";
        str2 = "1 44742U 19074AF  21253.83699967  .00004109  00000-0  27869-3 0  9991";
        str3 = "2 44742  52.9990   7.6517 0001171  60.8538 299.2568 15.08486968101203";

        Tle tle1 = new Tle(str1, str2, str3);

        Satellite s = new Satellite(tle1);
        Eci eci = s.PositionEci(0);
        
        Debug.Log(string.Format("{0,16:f8} {1,16:f8} {2,16:f8}\n",
                          eci.Position.X,
                          eci.Position.Y,
                          eci.Position.Z));

        Debug.Log(eci.Position.Magnitude());


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
                so.altitude = altitude;// * (1 + (j*0.1f/Nj));

                so.i = i;
                so.j = j;

                if (i == 0 && j == 0) {
                    //sat.GetComponent<Collider>().isTrigger = false;
                    //Rigidbody rb = sat.AddComponent<Rigidbody>();
                    //rb.useGravity = false;
                    //sat.GetComponent<Renderer>().material = mat;
                    //so.infected = true;
                    so.SetInfected(true);
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
