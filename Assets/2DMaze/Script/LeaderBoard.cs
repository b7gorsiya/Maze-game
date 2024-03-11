//using LootLocker.Requests;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using CloudOnce;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    string playerIdentifier = "999999";
    [SerializeField]
    int leaderBoardID;
    [SerializeField]
    GameObject collection;

    [SerializeField]
    List<GameObject> leaderboard_labels;

    [Header("Player score")]
    [SerializeField]
    TextMeshProUGUI playerrank;
    [SerializeField]
    TextMeshProUGUI playerscore;
    [SerializeField]
    List<Sprite> badges;
    [SerializeField]
    Image badge;

    int memeberID;

    private void OnEnable()
    {
        GameController.instanse.ShowLoading();
        //FatchLeaderBoard();
    }
    private void Awake()
    {
        // check for ios when realsing on ios
#if UNITY_ANDROID
        playerIdentifier = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_IOS
        playerIdentifier=UnityEngine.iOS.Device.advertisingIdentifier;
#endif
       // GetPlayerResponese(playerIdentifier);

    }

/*
    void GetPlayerResponese(string _playerIdentifier)
    {
        if (LootLockerSDKManager.CheckInitialized())
            return;
      
        LootLockerSDKManager.StartSession(_playerIdentifier, (response) =>
        {
            if (response.success)
            {
                memeberID = response.player_id;
                

                Debug.Log("Login To leaderboard");
            }
            else
            {
                Debug.Log("failed to start sessions" + response.Error);
            }
        });

    }

   public void GetPlayerDeatils()
    {
        //if (!LootLockerSDKManager.CheckInitialized())
            GetPlayerResponese(playerIdentifier);

        LootLockerSDKManager.GetMemberRank(leaderBoardID.ToString(), memeberID, (outpot) => {

            if (outpot.success)
            {
                playerrank.text = outpot.rank.ToString();
                playerscore.text = outpot.score.ToString();
                ChangePlayerBadge(outpot.score);
            }
            else
            {
                Debug.Log("failed to get data" + outpot.Error);
            }
          
        });
    }

    public void SubmitScoreToLeaderbard(int _score)
    {

       

    }


    public void FatchLeaderBoard()
    {
        List<int> highScores=new List<int>(100);

        LootLockerSDKManager.GetScoreList(leaderBoardID,100, (response) => {
            if (response.success)
            {
                Debug.Log("Score Recived");
                LootLockerLeaderboardMember []scores= response.items; 
                for(int i=0;i<scores.Length;i++)
                {
                    leaderboard_labels[i].GetComponent<LeaderBoardLabel>().SetScore(scores[i].score);
                    leaderboard_labels[i].GetComponent<LeaderBoardLabel>().SetRank(scores[i].rank);
                }
               for(int i=scores.Length-1;i<leaderboard_labels.Count;i++)
                {
                    leaderboard_labels[i].GetComponent<LeaderBoardLabel>().SetScore(null);
                    leaderboard_labels[i].GetComponent<LeaderBoardLabel>().SetRank(null);
                }
                GameController.instanse.HideLoading_Focefully();

            }
            else
            {
                Debug.Log("failed to start sessions" + response.Error);
            }
        });

    }
*/
        
    void ChangePlayerBadge(int _score)=>badge.sprite = (_score > 5000) ? ((_score > 10000) ? ((_score > 15000) ? badges[0] : badges[1]) : badges[2]) : badges[3];
   

}
