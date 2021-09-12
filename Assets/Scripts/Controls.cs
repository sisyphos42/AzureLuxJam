using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controls : MonoBehaviour
{
    public SatSpawner sp;
    public MyText mt;
    int money = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        money++;

        mt.SetValue(0, money);
        mt.SetValue(1, sp.N - sp.infected);
        mt.SetValue(2, sp.infected);
    }
}
