using UnityEngine;

public class Leaderboard : Singleton<Leaderboard>
{
    public int leaderboardSize = 6;
    public GameData[] users = new GameData[6];

    public Score[] scores = new Score[6];

    public int rank = 1;

    private void Start()
    {
        users = new GameData[leaderboardSize];
        scores = new Score[leaderboardSize];

        GetScoresUI();
    }

    public void GetScoresUI()
    {

        Transform Leaderboard = UiManager.Instance.LeaderBoard.transform;

        for (int i = 0; i < users.Length; i++)
        {
            scores[i] = Leaderboard.GetChild(i).GetComponent<Score>();
        }
    }

    public async void FetchData()
    {
        users = await DatabaseRealtimeManager.Instance.FetchCurrentUserRanks();
        SetLeaderboard();
    }

    private void SetLeaderboard()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].setup(users[i], rank + i);
        }
    }

}
