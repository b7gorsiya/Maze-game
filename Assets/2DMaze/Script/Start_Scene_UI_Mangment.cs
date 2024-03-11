//using CloudOnce;
//using GoogleMobileAds.Api;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;

public class Start_Scene_UI_Mangment : MonoBehaviour
{
    [Header("Panels ")]
    //panels
    [SerializeField]
    private GameObject mainMenu_Panel;
    [SerializeField]
    private GameObject gameMode_Panel;
    [SerializeField]
    GameObject leaderBoardPanel;
    [SerializeField]
    GameObject level_selection;
    [SerializeField]
    GameObject charchter_selection_panel;
    [SerializeField]
    GameObject gift_panel;

    [Header("Main menus buttons")]
    //mainmenu panel ui
    [SerializeField]
    private Button play_btn;
    [SerializeField]
    private Button store_btn;
    [SerializeField]
    private Button rate_btn;
    [SerializeField]
    private Button sound_btn;
    [SerializeField]
    private Button achivment_btn;
    [SerializeField]
    Button charchter_slect_Btn;
    [SerializeField]
    Button gift_btn;
    [Header("Sprites")]
    [SerializeField]
    Sprite mute_img;
    [SerializeField]
    Sprite unmute_img;
    [SerializeField]
    public Button removeAd;
    [SerializeField]
    GameObject handTutorial;


    [Header("Store")]
    [SerializeField]
    private GameObject store_Panel;
    [SerializeField]
    Button store_closeBTN;
    [SerializeField]
    Button pack1BTN;
    [SerializeField]
    Button pack2BTN;
    [SerializeField]
    Button pack3BTN;

    [Header("Charchter selecton menus fields")]
    [SerializeField]
    GameObject char_btns_collection;
    [SerializeField]
    List<Button> char_selectBTNs;
    [SerializeField]
    List<Button> char_buyBTNs;
    [SerializeField]
    List<Image> char_btns;
    [SerializeField]
    Button charchter_select_backBTN;
  

    [Header("Game mode selection buttons")]
    // game mode panel ui
    [SerializeField]
    private Button gameMode_backbtn;
    [SerializeField]
    private Button classic;
    [SerializeField]
    private Button monster;
    [SerializeField]
    private Button timer;
    [SerializeField]
    Button level_selction_backBTN;

    [SerializeField]
    private Button obstacle;
    [SerializeField]
    private Button random;
    [SerializeField]
    Button legend_Mode;

    [SerializeField]
    Level_Controller level_controller;

    [Header("Gift Collection")]
    [SerializeField]
    Timer _timer;
    [SerializeField]
    Button gift_collectBTN;
    [SerializeField]
    Button free_giftBTN;
    [SerializeField]
    Button gift_collection_CloseBTN;

    [SerializeField]
    Button leaderboard_backBTN;
    [SerializeField]
    AvatarCharchtrastics avatarData;

    [SerializeField]
    ParticleSystem giftBoxParticle;

    [SerializeField]
    GameObject playTutorial;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("UI Trained") == 0)
        {
            handTutorial.SetActive(true);
            LeanTween.scale(handTutorial, Vector3.one * 1.1f, .5f).setLoopPingPong();
        }
        Init_Panels(1);
        sound_btn.GetComponent<Image>().sprite = (PlayerPrefs.GetInt("Mute",0)==1) ? mute_img : unmute_img;
        Time.timeScale = 1;
        storepos = store_Panel.transform.position;
    }
    private void OnEnable()
    {
      //  GameController.instanse.adPlugin.rewardGift += GiftCollected;
    }
    private void Start()
    {
        Init_Char_Select_UI();
        ButtonClick_init();
        GiftBox_Timer();
    }
    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AndroidBackButton();
        }
#endif
    }

    private void ButtonClick_init()
    {
        removeAd.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("AdFree", 0) != 1)
        {
            removeAd.gameObject.SetActive(true);
            LeanTween.scale(removeAd.gameObject, Vector3.one * 1.05f, 2f).setEasePunch().setLoopPingPong().setDelay(2f);
        }
        play_btn.onClick.AddListener(() => { Play_Click(); });
       // achivment_btn.onClick.AddListener(() => GameController.instanse.audiomanager.ButtonClick_Audio());
        classic.onClick.AddListener(() => { GameMode_Choose(1); });
        monster.onClick.AddListener(() => { GameMode_Choose(2); });
        obstacle.onClick.AddListener(() => { GameMode_Choose(3); });
        random.onClick.AddListener(() => { GameMode_Choose(5); });
        timer.onClick.AddListener(() => { GameMode_Choose(4); });
        legend_Mode.onClick.AddListener(() => GameMode_Choose(6));
        sound_btn.onClick.AddListener(() => SoundSettings());
        charchter_slect_Btn.onClick.AddListener(() => Chrachter_Selection());

        rate_btn.onClick.AddListener(() => {
            GameController.instanse.audiomanager.ButtonClick_Audio();

#if UNITY_ANDROID
            Application.OpenURL("market://details?id=" + Application.identifier);
#else
            UnityEngine.iOS.Device.RequestStoreReview(); 
#endif
        });

        // achivment_btn.onClick.AddListener(() => OpenLeaderBoard());

        //Store
        store_btn.onClick.AddListener(() => Store());
        store_closeBTN.onClick.AddListener(() => CloseStore());
        pack1BTN.onClick.AddListener(() => GameController.instanse.audiomanager.ButtonClick_Audio());
        pack2BTN.onClick.AddListener(() => GameController.instanse.audiomanager.ButtonClick_Audio());
        pack3BTN.onClick.AddListener(() => GameController.instanse.audiomanager.ButtonClick_Audio());
        removeAd.onClick.AddListener(() => GameController.instanse.audiomanager.ButtonClick_Audio());
        // Gift Collection
        gift_btn.onClick.AddListener(() => OpenGiftCollectionPanel());
        gift_collectBTN.onClick.AddListener(() => CollectGift());
        free_giftBTN.onClick.AddListener(() => CollectFreeGift());
        gift_collection_CloseBTN.onClick.AddListener(() => GiftCollectionClose());
        // back buttons click
        gameMode_backbtn.onClick.AddListener(() => { Back_btn_Click(1); });
        charchter_select_backBTN.onClick.AddListener(() => { Back_btn_Click(1); });
        level_selction_backBTN.onClick.AddListener(() => { Back_btn_Click(2); });
        leaderboard_backBTN.onClick.AddListener(() => { Back_btn_Click(1); });
    }
    private void Init_Panels(int panel_no)
    {
        mainMenu_Panel.SetActive(panel_no == 1);
        gameMode_Panel.SetActive(panel_no == 2);
        level_selection.SetActive(panel_no == 4);
        store_Panel.SetActive(panel_no == 5);
        charchter_selection_panel.SetActive(panel_no == 6);
        gift_panel.SetActive(panel_no == 7);
        leaderBoardPanel.SetActive(panel_no == 8);
    }

   
#region Gift Box

    public void OpenGift()
    {
        if (!mainMenu_Panel.activeSelf) return;
        Init_Panels(7);
        GameController.instanse.audiomanager.GiftBoX_Audio();
        gift_panel.transform.GetChild(0).gameObject.transform.localScale = Vector3.one * 0.5f;
        LeanTween.scale(gift_panel.transform.GetChild(0).gameObject, Vector2.one * 1, 0.5f).setEaseInOutBack();
        free_giftBTN.interactable = true;
        //if (!GameController.instanse.adPlugin.IsGiftLoaded)
        //{
        //    gift_collectBTN.interactable = false;
        //}
        //else
            gift_collectBTN.interactable = true;

        giftBoxParticle.Play();
    }
    public void OpenGiftCollectionPanel()
    {
        if (!mainMenu_Panel.activeSelf) return;

        GameController.instanse.audiomanager.GiftBoX_Audio();
        //Analytics.CustomEvent("GiftBox");
       // GameController.instanse.firebase.LogEvent("GiftBox", "percent", 1f);
        giftBoxParticle.Play();
        Init_Panels(7);
        gift_panel.transform.GetChild(0).gameObject.transform.localScale = Vector3.one * 0.5f;
        LeanTween.scale(gift_panel.transform.GetChild(0).gameObject, Vector2.one * 1, 0.5f).setEaseInOutBack();
        free_giftBTN.interactable = true;
        //if (!GameController.instanse.adPlugin.IsGiftLoaded)
        //{
        //    gift_collectBTN.interactable = false;
        //    /*
        //    Debug.Log("Gift not loaded..");
        //    gift_collectBTN.onClick.RemoveAllListeners();
        //    gift_collectBTN.onClick.AddListener(GiftCollectedButton);*/
        //}
        //else
            gift_collectBTN.interactable = true;
    }
    void GiftBox_Timer()
    {
        int min = PlayerPrefs.GetInt("Min", 0);
        int sec = PlayerPrefs.GetInt("Sec", 0);

        if (min <= 0 && sec <= 0)
        {
            LeanTween.rotate(gift_btn.gameObject, Vector3.one * 10f, 1f).setEasePunch().setLoopPingPong();
            _timer.Init_timer(min, sec);
            return;
        }
        var old_time = PlayerPrefs.GetString("Time", System.DateTime.Now.ToString());
        System.DateTime old = System.DateTime.Parse(old_time);
        var diff = System.DateTime.Now - old;
        var oldtotal = (min * 60) + sec;
        var newtotal = (diff.Minutes * 60) + diff.Seconds;
        var leftPositive = (oldtotal > newtotal) ? oldtotal - newtotal : 0;
        var leftmin = (leftPositive == 0) ? 0 : leftPositive / 60;
        var leftsec = (leftPositive == 0) ? 0 : leftPositive % 60;
        _timer.Init_timer(leftmin, leftsec);
    }

    void OpenLeaderBoard()
    {
        Init_Panels(8);
        backbutton_index = 1;
    }
    public void CollectFreeGift()
    {
        free_giftBTN.interactable = false;
       // _timer.Init_timer(15, 00);
        GameController.instanse.UpdateCoins(50);
        GameController.instanse.ShowMsg("!! Congratulation !!\nYou Have Earn 50 Coins", gift_panel);
        //if (!GameController.instanse.adPlugin.IsGiftLoaded)
        //{
        //    gift_collectBTN.interactable = false;
        //}
        //else
            gift_collectBTN.interactable = true;
    }
    void CollectGift()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();
        GameController.instanse.CheckInternet();
       // GameController.instanse.adPlugin.HideBannerAd();
       // UnityAds.giftRewarded += GiftCollected;
        //GameController.instanse.unityAds.ShowAd(1);
       // GameController.instanse.adPlugin.ShowRewardAd(1);
    }
    public void GiftCollected()
    {
        GiftCollectedButton();
    }
    void GiftCollectedButton()
    {
        gift_panel.SetActive(false);
        //GameController.instanse.adPlugin.ShowBannerAd();

        GameController.instanse.ShowMsg("!! Congratulation !!\nYou Have Earn 50 Coins", mainMenu_Panel);
        Init_Panels(1);
        LeanTween.cancel(gift_btn.gameObject);
        _timer.Init_timer(15, 00);
        GameController.instanse.UpdateCoins(50);
    }
    void GiftCollectionClose()
    {
        LeanTween.scale(gift_panel.transform.GetChild(0).gameObject, Vector2.one * 0.5f, 0.1f).setEaseInOutBack().setOnComplete(()=> {
            Init_Panels(1);
            LeanTween.cancel(gift_btn.gameObject);
            _timer.Init_timer(15, 00);
        });
        
    }

#endregion

#region Button Click
    bool mute = true;
    void SoundSettings()
    {
        PlayerPrefs.SetInt("Mute", (PlayerPrefs.GetInt("Mute",0)==0) ? 1 : 0);
        mute = (PlayerPrefs.GetInt("Mute", 0) == 1) ? true : false;
        GameController.instanse.audiomanager.MuteUnMute(mute);
        GameController.instanse.audiomanager.ButtonClick_Audio();

        sound_btn.GetComponent<Image>().sprite = (mute) ? mute_img : unmute_img;
        mute = !mute;
    }

    public void Play_Click()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();
        if(PlayerPrefs.GetInt("Trained")==0)
        {
            playTutorial.SetActive(true);
            return;
        }

        Init_Panels(2);
        backbutton_index = 1;
        
    //// Leaderboards.HighScore.SubmitScore(PlayerPrefs.GetInt("Score",1), (response) => {
    //     if (!response.HasError)
    //     {
    //         Debug.Log("Score Submited" + response.Result);
    //     }
    //     else
    //     {
    //         Debug.Log("failed to submit score" + response.Error);
    //     }
    // });
     
    }

    Vector3 storepos;
    void Store()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        LeanTween.cancel(store_Panel);
        Init_Panels(5);
        backbutton_index = 1;
        store_Panel.transform.position = storepos;
        LeanTween.moveY(store_Panel, 250, 1f).setEasePunch();
    }
    void CloseStore()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        LeanTween.moveY(store_Panel, -100, 0.5f).setEaseInBack().setOnComplete(()=> { 
        Init_Panels(1);
        });
    }

    public void Back_btn_Click(int panel_no)
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        Init_Panels(panel_no);
    }

    int backbutton_index;
    void AndroidBackButton()
    {
        if (backbutton_index == 0) return;
        GameController.instanse.audiomanager.ButtonClick_Audio();

        Init_Panels(backbutton_index);
        if (backbutton_index == 2) backbutton_index = 1;
    }

#endregion

#region Avatar Selection
    void Chrachter_Selection()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();
        if (handTutorial.activeSelf)
        {
            PlayerPrefs.SetInt("UI Trained", 1);
            LeanTween.cancel(handTutorial);
            handTutorial.SetActive(false);
        }
        Init_Panels(6);
        backbutton_index = 1;
    }
    
    void Init_Char_Select_UI()
    {
        char_btns.Clear();
        char_buyBTNs.Clear();
        char_selectBTNs.Clear();

        var child_img = char_btns_collection.GetComponentsInChildren<Image>(true);
        foreach (var btns in child_img)
        {
            if (btns.sprite == null)
            {
                char_btns.Add(btns);

                char_selectBTNs.Add(btns.gameObject.GetComponentsInChildren<Button>(true)[0]);

                char_buyBTNs.Add(btns.GetComponentsInChildren<Button>(true)[1]);
            }
        }
        SetupButtons_CharchterSelection(UserData.instance.avtar_data.current_Avtar);
    }

    //use this method in swap function to display bottom ui buttons
    public void Set_Buttons_ForCharachter(int index)
    {
        foreach (var group in char_btns)
        {
            group.gameObject.SetActive(false);
        }
        char_btns[index].gameObject.SetActive(true);

    }
    void SetupButtons_CharchterSelection(int selected_Index)
    {
        for (int i = 0; i < char_btns.Count; i++)
        {
            // if the avtart is purchused/Unlocked
            if (UserData.instance.avtar_data.avtars_List["Avatar_" + (i + 1)] == 1)
            {
                char_selectBTNs[i].gameObject.SetActive(true);
                char_selectBTNs[i].image.color = Color.white;
                var _index = i + 1;

                char_selectBTNs[i].onClick.RemoveAllListeners();
                char_selectBTNs[i].onClick.AddListener(() => SelectAvatar(_index));
                char_selectBTNs[i].GetComponentInChildren<TextMeshProUGUI>().text = "Select";

                char_buyBTNs[i].gameObject.SetActive(false);
            }
            else
            {
                char_selectBTNs[i].gameObject.SetActive(false);
                char_buyBTNs[i].gameObject.SetActive(true);
                char_buyBTNs[i].GetComponentInChildren<TextMeshProUGUI>().text = avatarData.avtarPrice_list[i].ToString();
                //sending index accrroding to 1-10
                var _index = i + 1;
                char_buyBTNs[i].onClick.RemoveAllListeners();

                char_buyBTNs[i].onClick.AddListener(() => { BuyAvatar(_index); });
            }
        }
        UserData.instance.SetAvatarChrachterstics(avatarData.speed_data[selected_Index - 1], avatarData.armours_data[selected_Index - 1], avatarData.extra_time[selected_Index - 1]);
        char_selectBTNs[selected_Index - 1].image.color = Color.green;
        char_selectBTNs[selected_Index - 1].GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
    }

    void BuyAvatar(int _index)
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        if (UserData.instance.GetCoins < avatarData.avtarPrice_list[_index - 1])
        {
            GameController.instanse.ShowStore();
            return;
        }
        int updatedCoins = UserData.instance.GetCoins - avatarData.avtarPrice_list[_index - 1];
        PlayerPrefs.SetInt("Coins", updatedCoins);
        PlayerPrefs.SetInt("Selected_Avatar", _index);
        PlayerPrefs.SetInt("Avatar_" + _index, 1);
        UserData.instance.Update_Avatars(_index);
        UserData.instance.UpdateCoins();
        // SetupButtons_CharchterSelection(_index);
        GameController.instanse.SetCoin_text();
        Init_Char_Select_UI();
    }


    void SelectAvatar(int _index)
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        PlayerPrefs.SetInt("Selected_Avatar", _index);
       
        UserData.instance.Update_Avatars(_index);
        SetupButtons_CharchterSelection(_index);
        // Init_Char_Select_UI();
    }
#endregion

    public void GameMode_Choose(int mode)
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        backbutton_index = 2;
        bool is_Random = (mode == 5) ? true : false;
        mode = (mode == 5) ? UnityEngine.Random.Range(1, 4) : mode;
        // fetching level details
        GameController.instanse.unlocked_levels = level_controller.GetUserData(mode);

        switch (mode)
        {
            case 1:
                //Analytics.CustomEvent("ClassicLevel" + level_controller.GetUserData(mode));
        //GameController.instanse.firebase.LogEvent("ClassicLevel" + level_controller.GetUserData(mode), "percent", 1f);

                GameController.instanse.gamemode = GameController.GameMode.class1;
                break;

            case 2:
                //Analytics.CustomEvent("EnemyLevel"+ level_controller.GetUserData(mode));
                //GameController.instanse.firebase.LogEvent("EnemyLevel" + level_controller.GetUserData(mode), "percent", 1f);

                GameController.instanse.gamemode = GameController.GameMode.class2;

                break;
            case 3:
                //Analytics.CustomEvent("ObstacleLevel" + level_controller.GetUserData(mode));
                //GameController.instanse.firebase.LogEvent("ObstacleLevel" + level_controller.GetUserData(mode), "percent", 1f);

                GameController.instanse.gamemode = GameController.GameMode.class3;

                break;
            case 4:
               // Analytics.CustomEvent("TimeLevel" + level_controller.GetUserData(mode));
                //GameController.instanse.firebase.LogEvent("TimeLevel" + level_controller.GetUserData(mode), "percent", 1f);

                GameController.instanse.gamemode = GameController.GameMode.class4;

                break;
            case 6:
                //legend_Mode
               //Analytics.CustomEvent("LegendModeClick" + level_controller.GetUserData(mode));
               // GameController.instanse.firebase.LogEvent("LegendModeClick" + level_controller.GetUserData(mode), "percent", 1f);

                if (!CheckForLegendBade())
                {
                    GameController.instanse.ShowMsg("Earn Legend Badge To Play", null);
                    return;
                }
                GameController.instanse.gamemode = GameController.GameMode.class6;
                LoadPlayScene();
                return;
            default:
                break;


        }

        if (is_Random) CheckRandom();
        else
            Init_Panels(4);
        // display level selection panel
        //loading level scene


    }
     bool CheckForLegendBade()
    {
        if (UserData.instance._progressbar._badge == 1)
            return true;

        return false;
    }
    public void LoadPlayScene()
    {
        GameController.instanse.ShowLoading();
        GameController.instanse.gamestatus = GameController.GameStatus.PlayMode;
        SceneManager.LoadSceneAsync(1);
    }

    void CheckRandom()
    {
        //Analytics.CustomEvent("Randomlevel");
        //GameController.instanse.firebase.LogEvent("Randomlevel", "percent", 1f);

        GameController.instanse.current_leve_no = GameController.instanse.unlocked_levels;
        LoadPlayScene();
    }
    private void OnDisable()
    {
        //UnityAds.giftRewarded -= GiftCollected;
        //GameController.instanse.adPlugin.rewardGift -= GiftCollected!;
    }

    /*
    #region ContexMenu
    GameObject avatrsCollection;
    List<TextMeshProUGUI> armourText;
    List<TextMeshProUGUI> speedTxt;
    List<TextMeshProUGUI> timeTxt;
    [ContextMenu("Refrence data filed")]
    void UpdatePlayerData()
    {
        int i = 0;
        foreach (var img in avatrsCollection.GetComponentsInChildren<Animator>())
        {
            speedTxt.Add(img.GetComponentsInChildren<TextMeshProUGUI>()[0]);
            speedTxt[i].text = "+" + avatarData.speed_data[i].ToString("00") + "%";
            armourText.Add(img.GetComponentsInChildren<TextMeshProUGUI>()[1]);
            armourText[i].text = avatarData.armours_data[i].ToString("00");
            timeTxt.Add(img.GetComponentsInChildren<TextMeshProUGUI>()[2]);
            timeTxt[i].text = avatarData.extra_time[i].ToString("00");
            i++;
        }
    }
    #endregion
    */
}
