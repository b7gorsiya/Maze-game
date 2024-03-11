using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using GoogleMobileAds.Api;
using System;

public class GameController : MonoBehaviour
{
    //instanse
    public static GameController instanse;
    public enum GameMode
    {
        class1,
        class2,
        class3,
        class5,
        class4,
        class6
    }

    public enum GameStatus
    {
        MenuMode,
        levelSelectio,
        PlayMode,
        Pause,
        Win,
        lose

    }

    public GameMode gamemode;
    public GameStatus gamestatus;
    public int current_leve_no;
    public int unlocked_levels;
    public TextMeshProUGUI coin_text;
    int current_coin;

    [Header("Msg Box")]
    [SerializeField]
    GameObject msg_panel;
    [SerializeField]
    Text msg_text;
    [SerializeField]
    Button msg_boxCloseBTN;

    [Header("StorePanel")]
    [SerializeField]
    public GameObject StorePanel;
    [SerializeField]
    public Button close_store;
    [SerializeField]
    Button pack1_buy;
    [SerializeField]
    Button pack2_buy;
    [SerializeField]
    Button rewardvideo;

    [Header("Loading")]
    [SerializeField]
    GameObject loading_panel;
    [SerializeField]
    Text tips;
    [SerializeField]
    List<string> tips_text;

    [SerializeField]
    public RManger reviewManager;

    Vector3 msgpanelPos;
    public int getcoins { get { return current_coin; } }

    //public FCGSMonetization adPlugin;
    //public AdMobMonetization admobAds;
    //public UnityAds unityAds;

    public AudioManger audiomanager;
    //public FCGSNotificationManager notificationManager;
    //public FCGSFireBasePlugin firebase;
    private void Awake()
    {
        //firebase.InitFirebase_Custom();
        audiomanager.MuteUnMute((PlayerPrefs.GetInt("Mute", 0) == 1));
        audiomanager.Background_Audio();
        msgpanelPos = msg_panel.transform.GetChild(0).gameObject.transform.position;
        ButtonClick_Inti();
        if (instanse == null)
        {
            ShowLoading();
            instanse = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
            //do nothing
        }
    }

    void ButtonClick_Inti()
    {
        pack1_buy.onClick.AddListener(()=> audiomanager.ButtonClick_Audio());
        pack2_buy.onClick.AddListener(() => audiomanager.ButtonClick_Audio());
    }
    public void ShowMsg(string msg, GameObject? to_display)
    {
        audiomanager.SucessSound();
        msg_text.text = msg;
        msg_panel.SetActive(true);
        msg_panel.transform.GetChild(0).gameObject.transform.position = msgpanelPos;
        if(gamestatus==GameStatus.MenuMode) LeanTween.moveY(msg_panel.transform.GetChild(0).gameObject, 500, 0.5f).setEasePunch();
        msg_boxCloseBTN.onClick.RemoveAllListeners();
        msg_boxCloseBTN.onClick.AddListener(() => MessageBox_CloseBtn(to_display));
    }

    void MessageBox_CloseBtn(GameObject? toActive)
    {

        audiomanager.ButtonClick_Audio();

        if (gamestatus == GameStatus.MenuMode)
        {
            LeanTween.moveY(msg_panel.transform.GetChild(0).gameObject, -300, 0.5f).setEaseOutBack().setOnComplete(() =>
            {
                msg_panel.SetActive(false);
                if (toActive != null) toActive.SetActive(true);
            });
        }else
        {
            msg_panel.SetActive(false);
            if (toActive != null) toActive.SetActive(true);  
        }
    }
    private void OnEnable()
    {
        //adPlugin.rewardStore += RewardCollected;
        msg_panel.SetActive(false);
        StorePanel.SetActive(false);
        SetCoin_text();
        SetBUttons();
    }

    void SetBUttons()
    {
        close_store.onClick.AddListener(() => Close_Store());
        rewardvideo.onClick.AddListener(() => RewardButtonClick());
    }

    void RewardButtonClick()
    {
        audiomanager.ButtonClick_Audio();

        CheckInternet();
        StorePanel.SetActive(false);

        // UnityAds.storeRewarded += RewardCollected;

        //unityAds.ShowAd(4);
       // adPlugin.ShowRewardAd(2);
       // admobAds.bannerView.Hide();
        //admobAds.ShowStore_RewardVideo();
        //admobAds.storeRewardAd.OnUserEarnedReward += RewardCollected!;
    }
    public void CheckInternet()
    {
        if(Application.internetReachability==NetworkReachability.NotReachable)
        {
            ShowMsg("Opps !!\n Check internet connection", null);
        }
    }
    public void RewardCollected()
    {
        ShowMsg("! Congratulation !\nYou Have Earn 20 Coins", StorePanel);
        //admobAds.bannerView.Show();

        UpdateCoins(20);

    }
     void Close_Store()
    {
        audiomanager.ButtonClick_Audio();

        if (gamestatus == GameStatus.MenuMode)
        {
            LeanTween.scale(StorePanel.transform.GetChild(0).gameObject, Vector3.one * 0.1f, 0.1f).setEaseOutCirc().setOnComplete(() => StorePanel.SetActive(false));
        }
        else
            StorePanel.SetActive(false);
    }
    public bool Level_Clear()
    {
        if (unlocked_levels == 49)
        {
            return false;
        }
        else
            current_leve_no++;

        if (current_leve_no > unlocked_levels) UserData.instance.Update_UserData(gamemode.ToString(), current_leve_no);
        return true;

    }

    public void SetCoin_text()
    {
        current_coin = UserData.instance.GetCoins;
        coin_text.text = current_coin.ToString("00");
    }
    public void Coin_Collected()
    {
        current_coin++;
        PlayerPrefs.SetInt("Coins", current_coin);
        coin_text.text = current_coin.ToString("00");
        UserData.instance.UpdateCoins();
    }
    public void UpdateCoins(int coins)
    {
        current_coin += coins;
        PlayerPrefs.SetInt("Coins", current_coin);
        coin_text.text = current_coin.ToString("00");
        UserData.instance.UpdateCoins();
    }
    
    public void ShowStore()
    {
      //  if (!adPlugin.IsStoreRewardLoaded) rewardvideo.interactable = false;

       // if (!admobAds.storeRewardAd.IsLoaded()) rewardvideo.interactable = false;
        StorePanel.SetActive(true);

        if (gamestatus == GameStatus.MenuMode)
        {
            StorePanel.transform.GetChild(0).gameObject.transform.localScale = Vector3.one * 0.5f;
            LeanTween.scale(StorePanel.transform.GetChild(0).gameObject, Vector3.one, 1f).setEaseOutBounce();
        }else
            StorePanel.transform.GetChild(0).gameObject.transform.localScale = Vector3.one;

    }

    public void ExitStore()
    {
        LeanTween.scale(StorePanel, Vector3.one * 0.5f, 1f).setEaseInOutSine().setOnComplete(()=>StorePanel.SetActive(false));
    }

    public void ShowLoading()
    {
        loading_panel.SetActive(true);
        tips.text = tips_text[UnityEngine.Random.Range(0, tips_text.Count - 1)];
       // StartCoroutine(HideLoading());

    }
    public void HideLoading_Focefully()
    {
        loading_panel.SetActive(false);
        StopAllCoroutines();
    }
    IEnumerator HideLoading()
    {
        yield return new WaitForSeconds(5f);
        loading_panel.SetActive(false);
    }
    private void OnDisable()
    {
        // UnityAds.storeRewarded -= RewardCollected;
        //admobAds.storeRewardAd.OnUserEarnedReward -= RewardCollected;
      //  adPlugin.rewardStore -= RewardCollected;

        //notificationManager.InitilizeNotificationChannel
        //   (new FCGSNotificationManager.GameNotificationChannel
        //   (notificationManager.notificationsIDs[1], "it's been while", "Restart your journey of maze legend"));
        //notificationManager.CreateNoification
        //    (new FCGSNotificationManager.NotificationCreate
        //    ("it's been while", "Restart your journey of maze legend", 24*60, notificationManager.notificationsIDs[1]));
    }
}


