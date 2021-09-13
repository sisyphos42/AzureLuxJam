using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using Zeptomoby.OrbitTools;

public class SatSpawner : MonoBehaviour
{
    [SerializeField]
    Controls controls;
    [SerializeField]
    GameObject satPrefab;
    [SerializeField]
    float altitude;

    public int Ni;
    public int Nj;
    public int N {get => Ni*Nj;}
    public int infected;
    public int updated;
    float _starttime;

    [SerializeField]
    Material mat;

    [SerializeField]
    Material mat2;

    GameObject[] sats;

    int _threshold;
    public TextAsset satData;
    
    public SatData d;
    
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
        d = JsonUtility.FromJson<SatData>(satData.text.Replace("@", ""));

        // Debug.Log(d.context);
        // Debug.Log(d.id);
        // Debug.Log(d.type);
        // Debug.Log(d.totalItems);
        // Debug.Log(d.member[0].name);
        // Debug.Log(d.member[0].line1);
        // Debug.Log(d.member[0].line2);


        // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY));
        //   HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //   StreamReader reader = new StreamReader(response.GetResponseStream());
        //   string jsonResponse = reader.ReadToEnd();
        //   WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        //   return info;

        for (int i = 1; i <= 25 && false; i++) {

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://tle.ivanstanojevic.me/api/tle/?search=starlink&page={0}&page-size=100", i));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd().Replace("@", "");
        SatData sd = JsonUtility.FromJson<SatData>(json);
        //Debug.Log(json);

        foreach (var x in sd.member) {
            Tle tle = new Tle(x.name, x.line1, x.line2);
            Satellite sa = new Satellite(tle);
            try {
                Eci e = sa.PositionEci(System.DateTime.Now);
                Vector3 p = new Vector3((float)e.Position.X, (float)e.Position.Z, (float)e.Position.Y);
                p = p.normalized * altitude;

                GameObject nsat = Instantiate(satPrefab, p, Quaternion.identity);
                nsat.transform.parent = transform;

                SatOrbit so = nsat.GetComponent<SatOrbit>();
                //so.plane = Quaternion.Euler((float)sa.Orbit.Inclination, 0, 0) * Quaternion.Euler(0, (float)sa.Orbit.RAAN * Mathf.Rad2Deg, 0) * Vector3.up;
                so.plane = Quaternion.Euler(0, -(float)sa.Orbit.RAAN * Mathf.Rad2Deg, 0) * Quaternion.Euler(-(float)sa.Orbit.Inclination * Mathf.Rad2Deg + 180, 0, 0) * Vector3.up;
                so.phase = 0;//(float)sa.Orbit.RAAN;

                Debug.Log(string.Format("{0}: x: {1}, y: {2}, z: {3}, Inc: {4}, RAAN: {5}", x.name, p.x, p.y, p.z, sa.Orbit.Inclination*Mathf.Rad2Deg, sa.Orbit.RAAN*Mathf.Rad2Deg));


            } catch {}
        }

        }

        //return;

        // for (int i = 1; i <= 25; i++) {
        //     StreamReader sr = new StreamReader("Assets/Data/starlink/t" + i.ToString() + ".json");
        //     SatData sd = JsonUtility.FromJson<SatData>(sr.ReadToEnd().Replace("@",""));
        //     foreach (var x in sd.member) {
        //         Tle tle = new Tle(x.name, x.line1, x.line2);
        //         Satellite sa = new Satellite(tle);
        //         //Eci e = sa.PositionEci(100);
        //         Eci e;
        //         try {
        //             e = sa.PositionEci(System.DateTime.Now);
        //             //Debug.Log(e.Position);
        //             Vector3 p = new Vector3((float)e.Position.X, (float)e.Position.Z, (float)e.Position.Y);
        //             p = p/6371 * altitude;

        //             if (!float.IsNaN(p.x) && !float.IsNaN(p.y) && !float.IsNaN(p.z)) {

        //                 GameObject nsat = Instantiate(satPrefab, p, Quaternion.identity);
        //                 nsat.transform.parent = transform;
        //             }
        //         }
        //         catch {}

                
        //     }
        // }

        //Debug.Log(sr.ReadToEnd());

        //return;

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

        _starttime = Time.time;

        Reset();
        return;


        sats = new GameObject[Ni * Nj];

        for (int i = 0; i < Ni; i++) {
            for (int j = 0; j < Nj; j++) {
                GameObject sat = Instantiate(satPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //Rigidbody rb = sat.AddComponent<Rigidbody>();
                //rb.useGravity = false;
                sat.transform.parent = transform;
                SatOrbit so = sat.GetComponent<SatOrbit>();
                so.plane = Quaternion.Euler(0, i * 360/Ni, 0) * Quaternion.Euler(60, 0, 0) * Vector3.up;
                so.phase = Mathf.Deg2Rad * ((360 * j/Nj) + 180);
                so.altitude = altitude;// * (1 + (j*0.1f/Nj));
                so.controls = controls;

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

                if (i == 0) {
                    //sat.transform.localScale *= 2;
                    //sat.GetComponent<Renderer>().material = mat2;
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

    void FixedUpdate() {
        if (!controls.gameRunning) return;

        _threshold = 3 + Mathf.FloorToInt((Time.time - _starttime)/60f);
        if (infected > _threshold) return;
        if (Random.value < 0.000002f * N * (1 + (Time.time - _starttime)/120f)) {
            int i;
            do {
                i = Random.Range(0, sats.Length);
            } while (sats[i].GetComponent<SatOrbit>().infected);
            sats[i].GetComponent<SatOrbit>().SetInfected(true);

            //SetInfected(true);
            //Debug.Log("infect");
        }
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
            int u = 0;
            foreach (var s in sats) {
                if (s) {
                    if (s.GetComponent<SatOrbit>().infected)
                        n++;
                    if (s.GetComponent<SatOrbit>().updated)
                        u++;
                }
            }
            infected = n;
            updated = u;
            //Debug.Log(string.Format("infected: {0}, updated: {1}, threshold: {2}", n, u, _threshold));

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Reset() {
        // foreach (var s in sats) {
        //     SatOrbit so = s.GetComponent<SatOrbit>();
        //     so.phi = 0;
        //     //so.updated = false;
        //     so.Reset();
        // }
        // infected = 0;
        // updated = 0;
        // _starttime = Time.time;
        if (sats != null) {
            foreach (var s in sats) {
                Destroy(s);
            }
        }

_starttime = Time.time;
infected = 0;
updated = 0;

        sats = new GameObject[Ni * Nj];

        for (int i = 0; i < Ni; i++) {
            for (int j = 0; j < Nj; j++) {
                GameObject sat = Instantiate(satPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //Rigidbody rb = sat.AddComponent<Rigidbody>();
                //rb.useGravity = false;
                sat.transform.parent = transform;
                SatOrbit so = sat.GetComponent<SatOrbit>();
                so.plane = Quaternion.Euler(0, i * 360/Ni, 0) * Quaternion.Euler(60, 0, 0) * Vector3.up;
                so.phase = Mathf.Deg2Rad * ((360 * j/Nj) + 180);
                so.altitude = altitude;// * (1 + (j*0.1f/Nj));
                so.controls = controls;

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

                if (i == 0) {
                    //sat.transform.localScale *= 2;
                    //sat.GetComponent<Renderer>().material = mat2;
                }

                sats[i*Nj + j] = sat;
            }
        }

        StartCoroutine(CountSats());

    }

}
