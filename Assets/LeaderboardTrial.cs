using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.Android;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class LeaderboardTrial : MonoBehaviour
{
    public GameObject submitMenu;
    public GameObject leaderboardMenu;
    public TMPro.TMP_Text leaderboardText;
    public TMPro.TMP_InputField submitField;
    bool leaderBoardReady = false;
    string leaderboardTextStored = "";

    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Social.ReportScore(999, "CgkIi7Kc8NwUEAIQAA", (bool success) => 
            {
                if (success) Debug.Log("Success");
                else Debug.LogError("Not success");
            });
        }

        if (leaderBoardReady)
        {
            leaderBoardReady = false;
            leaderboardText.text = leaderboardTextStored;
        }
    }

    public void OpenSubmitMenu()
    {
        submitMenu.SetActive(true);
    }
    public void CloseSubmitMenu()
    {
        submitMenu.SetActive(false);
    }
    public void SubmitScore()
    {
        string scoreInput = submitField.text;

        if (int.TryParse(scoreInput, out int parsed))
        {
            Social.ReportScore(parsed, "CgkIi7Kc8NwUEAIQAA", (bool result) => 
            {
                if (result)
                {
                    
                }
                else
                {
                    Debug.LogError("Report score failed");
                }
            });
            submitField.text = "";
        }
        else
        {
            Debug.LogError("Input must be an integer");
        }
    }
    public void OpenLeaderboard()
    {
        leaderboardText.text = "Loading...";
        Social.LoadScores("CgkIi7Kc8NwUEAIQAA", LeaderboardCallback);
    }
    internal void LeaderboardCallback(IScore[] scores)
    {
        leaderboardTextStored = "";
        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
        sBuilder.Append("Leaderboard\n\n");

        for (int i = 0; i < scores.Length; ++i)
        {
            IScore score = scores[i];
            sBuilder.Append("[" + score.rank + "] " + score.userID + ": " + score.formattedValue + "\n");
        }

        leaderboardTextStored = sBuilder.ToString();
        leaderBoardReady = true;
    }
    public void CloseLeaderboard()
    {
        leaderboardMenu.SetActive(false);
    }

    internal void ProcessAuth(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("SUCCESS");
        }
        else
        {
            Debug.LogError("Failed. Result: " + status.ToString());
        }
    }
}
