using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Controls : MonoBehaviour
{
    public SatSpawner sp;
    public MyText mt;
    public int money = 0;
    public GameObject gameover;
    public GameObject score;
    public GameObject button;
    int _startmoney;
    bool _running = true;
    // Start is called before the first frame update
    void Start()
    {
        _startmoney = money;
        StartCoroutine(MoneyTick());
    }

    // Update is called once per frame
    void Update()
    {
        mt.SetValue(0, money);
        mt.SetValue(1, sp.N - sp.infected - sp.updated);
        mt.SetValue(2, sp.infected);
        mt.SetValue(3, sp.updated);

        if (!_running) return;

        if (money <= 0) {
            gameover.GetComponent<TextMeshProUGUI>().text = "Game Over";
            gameover.SetActive(true);
            button.SetActive(true);
            //score.SetActive(true);
            Time.timeScale = 0;
            _running = false;
        }

        if (sp.updated == sp.N) {
            gameover.GetComponent<TextMeshProUGUI>().text = "You Won!";
            gameover.SetActive(true);
            button.SetActive(true);
            score.SetActive(true);
            score.GetComponent<TextMeshProUGUI>().text = string.Format("Score: {0}", money);
            Time.timeScale = 0;
            _running = false;
        }
    }

    void FixedUpdate() {
        
    }

    IEnumerator MoneyTick() {
        while(true) {
            money--;
            yield return new WaitForSeconds(1f);
        }
    }

    public void PayRansom() {
        money -= 10;
    }

    public void Restart() {
        money = _startmoney;
        gameover.SetActive(false);
        button.SetActive(false);
        score.SetActive(false);
        sp.Reset();
        Time.timeScale = 1f;
        _running = true;
    }
}
