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
    public GameObject leaderBoard;
    public GameObject score;
    public GameObject button;
    public GameObject startscreen;
    public int ransom;
    public bool gameRunning = false;
    int _startmoney;
    bool _running = false;
    float _moneytickTime;
    bool _online = false;
    // Start is called before the first frame update
    void Start()
    {
        _moneytickTime = Time.time;
        _startmoney = money;
        //StartCoroutine(MoneyTick());
    }

    // Update is called once per frame
    void Update()
    {
        mt.SetValue(0, money);
        mt.SetValue(1, sp.N - sp.infected - sp.updated);
        mt.SetValue(2, sp.infected);
        mt.SetValue(3, sp.updated);

        if (!_running) return;

        if (Time.time - _moneytickTime > 1f) {
            _moneytickTime = Time.time;
            money--;
            money -= sp.infected/20;
        }

        if (money <= 0) {
            gameover.GetComponent<TextMeshProUGUI>().text = "Game Over";
            gameover.SetActive(true);
            button.SetActive(true);
            //score.SetActive(true);
            Time.timeScale = 0;
            _running = false;
        }

        if (sp.updated == sp.N) {
            Win();
        }
    }

    void FixedUpdate() {
        
    }

    IEnumerator MoneyTick() {
        while(true) {
            money--;
            money -= sp.infected/20;
            yield return new WaitForSeconds(1f);
        }
    }

    public void PayRansom() {
        money -= ransom;
    }

    public void Restart() {
        money = _startmoney;
        _moneytickTime = Time.time;
        gameover.SetActive(false);
        button.SetActive(false);
        score.SetActive(false);
        sp.Reset();
        Time.timeScale = 1f;
        _running = true;
        gameRunning = true;
    }

    public void StartGame(int difficulty, bool online) {
        switch (difficulty) {
            case 0:
                sp.Ni = 17;
                sp.Nj = 43;
                ransom = 5;
                break;
            case 1:
                sp.Ni = 23;
                sp.Nj = 59;
                ransom = 2;
                break;
            case 2:
                sp.Ni = 27;
                sp.Nj = 61;
                ransom = 1;
                break;
        }
        _online = online;
        Restart();
    }

    public void Win() {
        if (_online) {
            leaderBoard.SetActive(true);
            leaderBoard.GetComponent<LeaderBoard>().Send(money);
            //leaderBoard.GetComponent<LeaderBoard>().UpdateLeaderboard();
        } else {
            gameover.GetComponent<TextMeshProUGUI>().text = "You Won!";
            gameover.SetActive(true);
            button.SetActive(true);
            score.SetActive(true);
            score.GetComponent<TextMeshProUGUI>().text = string.Format("Score: {0}", money);
        }
        Time.timeScale = 0;
        _running = false;
    }

    public void CloseLeaderboard() {
        leaderBoard.SetActive(false);
        startscreen.SetActive(true);
        leaderBoard.GetComponent<LeaderBoard>().UpdateLeaderboard();
    }
}
