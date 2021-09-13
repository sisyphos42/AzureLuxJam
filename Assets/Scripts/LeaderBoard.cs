using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderBoard : MonoBehaviour
{
    public Text[] entries;
    // Start is called before the first frame update
    void Start()
    {
        //RequestLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get the players with the top 10 high scores in the game
public void RequestLeaderboard() {
    PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest {
            StatisticName = "HighScores",
            StartPosition = 0,
            MaxResultsCount = 10
    }, result=> DisplayLeaderboard(result), FailureCallback);
}

public void DisplayLeaderboard(GetLeaderboardResult result) {
    Debug.Log(result.ToJson());
    int i = 0;
    foreach(var stat in result.Leaderboard) {
        if (i < 10) {
            entries[i].text = string.Format("{0}. {1} : {2}", i+1, stat.DisplayName, stat.StatValue);
            i++;
        }
    }
}

private void FailureCallback(PlayFabError error){
    Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
    Debug.LogError(error.GenerateErrorReport());
}

public void Send(int score) {
    var request = new UpdatePlayerStatisticsRequest {
        Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "HighScores",
                Value = score
            }
        }
    };
    PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
}

void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result) {
    Debug.Log("Leaderboard updated successfully");
    UpdateLeaderboard();
}

void OnError(PlayFabError error) {
    Debug.Log(error.GenerateErrorReport());
}

public void UpdateLeaderboard() {
    RequestLeaderboard();
}

public void Close() {

}

}
