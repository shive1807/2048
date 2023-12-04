using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI rankTxt;
    public TextMeshProUGUI username;
    public TextMeshProUGUI scoreTxt;


    private void Start()
    {
        //rankTxt = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        //username = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        //scoreTxt = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }
    public void setup(GameData userData, int rank)
    {
        this.rankTxt.text = rank.ToString();
        this.scoreTxt.text = userData.HighScore.ToString();
        this.username.text = userData.User.Username;
    }
}
