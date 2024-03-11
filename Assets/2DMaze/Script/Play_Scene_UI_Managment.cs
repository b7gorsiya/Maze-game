using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//using GoogleMobileAds.Api;
using UnityEngine.Analytics;
using System;

public class Play_Scene_UI_Managment : MonoBehaviour
{
    #region Data Memebers
    [Header("Panels")]
    [SerializeField]
    private GameObject gameplay_panel;
    [SerializeField]
    private GameObject pause_panel;
    [SerializeField]
    private GameObject win_panel;
    [SerializeField]
    private GameObject gameover_panel;

    [Header("Play Screen UI")]
    [SerializeField]
    private Button back_btn;
    [SerializeField]
    private Button hint_btn;
    [SerializeField]
    public TextMeshProUGUI timer;
    [SerializeField]
    private Button pause_btn;
    [SerializeField]
    List<Sprite> backgrounds;
    [SerializeField]
    Image background_Image;
    [SerializeField]
    Text armour_text;

    public GameObject armourImage;

    [Header("Pause Screen UI")]
    [SerializeField]
    private Button restart_btn;
    [SerializeField]
    private Button resume_btn;
    [SerializeField]
    private Button pause_home_btn;
    [SerializeField]
    private Button store_btn;

    [Header("Win Screen UI")]
    [SerializeField]
    private Button win_home_btn;
    [SerializeField]
    private Button next_btn;
    [SerializeField]
    private Button doubleCoin_btn;
    [SerializeField]
    TextMeshProUGUI win_panel_level;
    [SerializeField]
    TextMeshProUGUI earn_cookie;
    [SerializeField]
    TextMeshProUGUI scoreTXT;

    [Header("Game Over UI")]
    [SerializeField]
    private Button retryForFree;
    [SerializeField]
    private Button retryForCoin;
    [SerializeField]
    private Button over_home_btn;
    [SerializeField]
    Button retry_free;
    [SerializeField]
    Button store_btn_gameover;
    [SerializeField]
    TextMeshProUGUI over_panel_level;

    [SerializeField]
    public GameObject timer_display;

    [SerializeField]
    public Level_Manager lvl_manager;

    public int collectedCoin;

    public int armour;

    #endregion

    [SerializeField]
    ParticleSystem winParticle;
    [SerializeField]
    ParticleSystem looseParticle;

    private void Awake()
    {
        SetBackground();
        Button_Click_Listner();
    }

    private void OnEnable()
    {
        //UnityAds.doubleCoinRewarded += DoubleWinCoin;
        //UnityAds.retryRewarded += RewardCollected;
       // GameController.instanse.admobAds.doubleCoinRewardAd.OnUserEarnedReward += DoubleWinCoin;
       // GameController.instanse.admobAds.retryRewardAd.OnUserEarnedReward += RewardCollected;

       // GameController.instanse.adPlugin.rewardDoubleCoin += DoubleWinCoin;
        //GameController.instanse.adPlugin.rewardRetry += RewardCollected;

        scoreTXT.gameObject.SetActive(false);
        //pausePanelPos = pause_panel.transform.position;
        GameController.instanse.gamestatus = GameController.GameStatus.PlayMode;
        armour = UserData.instance.avtar_data.armour;
        UpdateAvatarData();
    }

   public bool TakeHitOnPlayer()
    {
        if (armour == 0)
            return true;
        armour--;
        UpdateAvatarData();
        return false;
    }
    void UpdateAvatarData()
    {
        armour_text.text = armour.ToString("00");
    }
    private void Button_Click_Listner()
    {
        //game play panel
        back_btn.onClick.AddListener(()=> { Home_Button_Click(); });
        pause_btn.onClick.AddListener(()=> { PauseResume(true); });
        hint_btn.onClick.AddListener(()=>{ OnHintButton_Click(); });

        //pause panel
        resume_btn.onClick.AddListener(() => { PauseResume(false); });
        pause_home_btn.onClick.AddListener(() => {Home_Button_Click(); });
        restart_btn.onClick.AddListener(() => { SceneManager.LoadSceneAsync(1); });
        store_btn.onClick.AddListener(() =>GameController.instanse.ShowStore());

        //game win panel
        win_home_btn.onClick.AddListener(() => { Home_Button_Click(); });
        next_btn.onClick.AddListener(() => { NextButton_Click(); });
        doubleCoin_btn.onClick.AddListener(() => DoubleCoin());

        //game over panel
        over_home_btn.onClick.AddListener(() => { Home_Button_Click(); });
        retryForCoin.onClick.AddListener(() => RetryFor_Coins());
        retryForFree.onClick.AddListener(() => RetryFor_WatchAd());
        store_btn_gameover.onClick.AddListener(() => GameController.instanse.ShowStore());
    }
    void SetBackground()
    {
        background_Image.sprite = backgrounds[UnityEngine.Random.Range(0, backgrounds.Count - 1)];
        lvl_manager.MazeLoading_Completed();
    }

    #region RetryGame

    void RetryFor_WatchAd()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        //Analytics.CustomEvent("RetryVideo");
        //GameController.instanse.firebase.LogEvent("RetryVideo", "percent", 1f);
        LeanTween.cancel(retryForFree.gameObject);

        GameController.instanse.CheckInternet();
        //GameController.instanse.adPlugin.HideBannerAd();
       // GameController.instanse.admobAds.bannerView.Hide();

       // GameController.instanse.unityAds.ShowAd(2);
       // GameController.instanse.admobAds.ShowRetry_RewardVideo();
       // GameController.instanse.adPlugin.ShowRewardAd(4);
        //GameController.instanse.admobAds.retryRewardAd.OnUserEarnedReward += RewardCollected;
    }
    public void RewardCollected()
    {
        //GameController.instanse.admobAds.bannerView.Show();
       // GameController.instanse.adPlugin.ShowBannerAd();

        RetryGame();
    }
    void RetryFor_Coins()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

       // GameController.instanse.firebase.LogEvent("RetryForCoin", "percent", 1f);


        if (GameController.instanse.getcoins < 10)
        {
            GameController.instanse.ShowStore();
            return;
        }
        GameController.instanse.UpdateCoins(- 10);
        RetryGame();
    }
    void RetryGame()
    {

        LeanTween.moveLocalX(gameover_panel.transform.GetChild(0).gameObject, -700, 0.3f).setEaseOutBack().setOnComplete(()=>  Panel_Setup(1));

        GameController.instanse.gamestatus = GameController.GameStatus.PlayMode;
        lvl_manager.mazecreator.maze2D.SetActive(true);
        lvl_manager.extra_objects.SetActive(true);
        lvl_manager.playerAI.routeCollection.SetActive(true);
        lvl_manager.playerAI.route.Clear();
        foreach(var i in lvl_manager.playerAI.routeCollection.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Destroy(i.gameObject);
        }
        lvl_manager.playerAI.allowMove = true;
        lvl_manager.Retry_Level();
    }

    #endregion
   
    public void StoreClose_FromGameController()
    {
        Time.timeScale = 1;
        // GameController.instanse.StorePanel.SetActive(false);
        lvl_manager.mazecreator.maze2D.SetActive(true);
        lvl_manager.extra_objects.SetActive(true);

        //  OnHintButton_Click();
    }

    #region Hint

    bool hintDisplaying = false;
    private void OnHintButton_Click()
    {
        if (hintDisplaying) return;

       // GameController.instanse.firebase.LogEvent("Hint", "percent", 1f);


        //Analytics.CustomEvent("Hint");

        hintDisplaying = true;
        GameController.instanse.audiomanager.ButtonClick_Audio();

        if (GameController.instanse.getcoins<10)
        {
            GameController.instanse.close_store.onClick.AddListener(() => { StoreClose_FromGameController(); });
            lvl_manager.mazecreator.maze2D.SetActive(false);
            lvl_manager.extra_objects.SetActive(false);
            Time.timeScale=0;
            GameController.instanse.ShowStore();
           
            return;
        }
        GameController.instanse.UpdateCoins( - 10);
        lvl_manager.SetHint_StartPoints();
        lvl_manager.hintobj.CheckMove();
        DisplayHint(lvl_manager.hintobj.bestRoute);
    }

    [SerializeField]
    List<GameObject> hints;
    [SerializeField]
    List<Vector3> hints_pos;
    void DisplayHint(List<Vector3> _hint_pos)
    {
        hints_pos = new List<Vector3>();
        int hintsTodisplay = (_hint_pos.Count < 10) ? _hint_pos.Count / 2 : 8;
        for (int i= 0;i< hintsTodisplay;i++)
        {
            hints_pos.Add(_hint_pos[i]);
        }
        StartCoroutine(DisplayHint());
    }
    IEnumerator DisplayHint()
    {
        hints = new List<GameObject>();
        int i = 0;
    GENRATE_HINT:
        hints.Add(Instantiate(lvl_manager.hint_prefeb, hints_pos[i], Quaternion.identity));
        GameController.instanse.audiomanager.Hint_Audio();
        LeanTween.scale(hints[i], Vector3.one * 5f, 0.5f).setEasePunch();
        yield return new WaitForSeconds(0.2f);
        i++;
        if (i < hints_pos.Count) goto GENRATE_HINT;
        yield return new WaitForSeconds(0.5f);
        Delete_Hint();
    }

    private void Delete_Hint()
    {
        foreach(var h in hints)
        {
            Destroy(h);
        }
        hintDisplaying = false;

    }

    #endregion

    private void Home_Button_Click()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        GameController.instanse.gamestatus = GameController.GameStatus.MenuMode;
        GameController.instanse.ShowLoading();
        SceneManager.LoadSceneAsync(0);
    }
    private void PauseResume(bool pause)
    {
        GameController.instanse.audiomanager.Pause_Audio();

        if (pause)
        {
            Panel_Setup((pause) ? 2 : 1);

            pause_panel.transform.GetChild(0).transform.localScale = Vector3.one * 0.50f;
       
            LeanTween.scale(pause_panel.transform.GetChild(0).gameObject, Vector3.one, 0.2f).setEaseInCirc().setOnComplete(() => Time.timeScale = (pause) ? 0 : 1);
            PauseFunction(pause);

        }
        else
        {
            Time.timeScale = (pause) ? 0 : 1;

            LeanTween.scale(pause_panel.transform.GetChild(0).gameObject, Vector3.one*0.50f, 0.25f).setEaseInOutBack().setOnComplete(()=>
            {
                Panel_Setup((pause) ? 2 : 1);

                PauseFunction(pause);
            });
        }
    }
   
    void PauseFunction(bool pause)
    {
        lvl_manager.mazecreator.maze2D.SetActive(!pause);
        lvl_manager.extra_objects.SetActive(!pause);
        lvl_manager.playerAI.routeCollection.SetActive(!pause);
    }
    private void Panel_Setup(int panel)
    {
        gameplay_panel.SetActive(false);
        pause_panel.SetActive(false);
        win_panel.SetActive(false);
        gameover_panel.SetActive(false);

        switch (panel)
        {
            case 1:
                gameplay_panel.SetActive(true);
                break;
            case 2:
                pause_panel.SetActive(true);
                break;
            case 3:
                LeanTween.scale(doubleCoin_btn.gameObject, Vector3.one * 1.03f, 1f).setEasePunch().setLoopPingPong();
                win_panel.SetActive(true);
                break;
            case 4:
                LeanTween.scale(retryForFree.gameObject, Vector3.one * 1.03f, 1f).setEasePunch().setLoopPingPong();
                gameover_panel.SetActive(true);
                break;
        }
    }

    #region DoubleCoin
    private void DoubleCoin()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();
        //GameController.instanse.firebase.LogEvent("DoubleCoin", "percent", 1f);
        LeanTween.cancel(doubleCoin_btn.gameObject);

        //  GameController.instanse.unityAds.ShowAd(3);
       // GameController.instanse.adPlugin.ShowRewardAd(3);
        //GameController.instanse.admobAds.ShowRetry_DoubleCoinReward();
        //GameController.instanse.admobAds.bannerView.Hide();

    }
    public void DoubleWinCoin()
    {
        GiveDoubleCoinReward();
    }
    void GiveDoubleCoinReward()
    {
        doubleCoin_btn.interactable = false;

        LeanTween.cancel(earn_cookie.gameObject);
        LeanTween.value(earn_cookie.gameObject, collectedCoin, collectedCoin*2, collectedCoin*0.1f).setOnUpdate(SetCoinText);
        //GameController.instanse.admobAds.bannerView.Show();
       // GameController.instanse.adPlugin.ShowBannerAd();

        Debug.Log("Rewareded");
        GameController.instanse.UpdateCoins(collectedCoin);
        collectedCoin = 0;
    }
    #endregion
    private void NextButton_Click()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();

        GameController.instanse.ShowLoading();
        SceneManager.LoadSceneAsync(1);
    }

    public void GameWin()
    {
        GameController.instanse.UpdateCoins(5);
        collectedCoin += 5;

        if (PlayerPrefs.GetInt("Rated",0)!=1)
        {
            if (lvl_manager.level_data.level_no % 2 == 0) GameController.instanse.reviewManager.OpenInAppReview();
        }
      

        GameController.instanse.audiomanager.GameWin_Audio();
        if (GameController.instanse.gamemode == GameController.GameMode.class6)
        {
            scoreTXT.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 100);
            scoreTXT.text = "Score : " + PlayerPrefs.GetInt("Score");
        }
        //if (GameController.instanse.adPlugin.IsDoubleCoinLoaded)
        //{
        //    doubleCoin_btn.interactable = true;
        //}
        //else
        //{
        //    doubleCoin_btn.interactable = false;
        //}
        winParticle.gameObject.SetActive(true);
        winParticle.Play();
        LeanTween.cancel(earn_cookie.gameObject);
        LeanTween.value(earn_cookie.gameObject, 0, collectedCoin, collectedCoin*0.1f).setOnUpdate(SetCoinText);
        //earn_cookie.text = collectedCoin.ToString("00");
        win_panel_level.text = "LEVEL "+ GameController.instanse.current_leve_no.ToString();
        //if(lvl_manager.level_data.level_no%FCGSMonetization.gameWin_AdFrequency==0)
        //{
        //   // GameController.instanse.unityAds.ShowAdInterstitial();
        //    GameController.instanse.adPlugin.ShowIntertisialAd();
        //}
        lvl_manager.extra_objects.SetActive(false);
        lvl_manager.mazecreator.maze2D.SetActive(false);
        lvl_manager.playerAI.routeCollection.SetActive(false);

        GameController.instanse.gamestatus = GameController.GameStatus.Win;
        Panel_Setup(3);
        win_panel.transform.GetChild(0).transform.localScale = Vector3.one * 0.8f;
        LeanTween.scale(win_panel.transform.GetChild(0).gameObject, Vector3.one,0.05f).setEaseInOutCubic();
        next_btn.gameObject.SetActive(GameController.instanse.Level_Clear());
    }

    void SetCoinText(float value)
    {
        LeanTween.scale(earn_cookie.gameObject, Vector2.one * 1.1f, 0.1f).setEasePunch();
        //GameController.instanse.audiomanager.CoinCollect_Audio();
        earn_cookie.text = value.ToString("00");
    }

    public void GameOver()
    {
        //if(GameController.instanse.adPlugin.IsRetryRewardLoaded)
        //{
        //    retryForFree.interactable = true;
        //}
        //else
        //{
        //    retryForFree.interactable = false;
        //}
        GameController.instanse.audiomanager.GameLoose_Audio();
        looseParticle.gameObject.SetActive(true);
        looseParticle.Play();
        over_panel_level.text = "LEVEL " + GameController.instanse.current_leve_no.ToString();
        // GameController.instanse.unityAds.ShowAdInterstitial();
        //if (lvl_manager.level_data.level_no % FCGSMonetization.gameOver_AdFrequency == 0)
        //{
        //    GameController.instanse.adPlugin.ShowIntertisialAd();
        //}
        lvl_manager.extra_objects.SetActive(false);
        lvl_manager.mazecreator.maze2D.SetActive(false);
        lvl_manager.playerAI.routeCollection.SetActive(false);
        GameController.instanse.gamestatus = GameController.GameStatus.lose;
        Panel_Setup(4);
        LeanTween.moveLocalX(gameover_panel.transform.GetChild(0).gameObject, 0, 0.3f).setEaseInBack();
    }

    private void OnDisable()
    {
       // GameController.instanse.adPlugin.rewardRetry -= RewardCollected;
       // GameController.instanse.adPlugin.rewardDoubleCoin -= DoubleWinCoin;
        //UnityAds.retryRewarded -= RewardCollected;
       // UnityAds.doubleCoinRewarded -= DoubleWinCoin;
    }

}
