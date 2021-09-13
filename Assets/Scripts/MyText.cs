using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyText : MonoBehaviour
{
    int _money = 0;
    int _healthy = 0;
    int _infected = 0;
    int _updated = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateUI());
    }

    // Update is called once per frame
    void Update()
    {
        //money++;

        

    }

    public void SetValue(int n, int val) {
        switch (n) {
            case 0:
                _money = val;
                break;
            case 1:
                _healthy = val;
                break;
            case 2:
                _infected = val;
                break;
            case 3:
                _updated = val;
                break;
        }
    }

    IEnumerator UpdateUI() {
        while (true) {
            transform.Find("Text").GetComponent<Text>().text = string.Format("{0}", _money);
            transform.Find("healthy").GetComponent<Text>().text = string.Format("{0}", _healthy);
            transform.Find("infected").GetComponent<Text>().text = string.Format("{0}", _infected);
            transform.Find("updated").GetComponent<Text>().text = string.Format("{0}", _updated);
            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }
    }
}
