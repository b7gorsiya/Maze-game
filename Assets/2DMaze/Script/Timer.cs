using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text txt;
    [SerializeField]
    Button giftBox_btn;
    [SerializeField]
    Start_Scene_UI_Mangment initUI;
    int min;
    int sec;

    bool startedOnce_inLifecycle = false;

    private void OnEnable()
    {
        if (!startedOnce_inLifecycle) return;
        StartTimer();
    }

    private void StartTimer()
    {
        int min = PlayerPrefs.GetInt("Min", 0);
        int sec = PlayerPrefs.GetInt("Sec", 0);

        if (min <= 0 && sec <= 0)
        {
            Init_timer(min, sec);
            return;
        }

        var old_time = PlayerPrefs.GetString("Time", System.DateTime.Now.ToString());
        System.DateTime old = System.DateTime.Parse(old_time);
        var diff = System.DateTime.Now - old;
        // incase another next day
        if (diff.Days > 0 || diff.Hours > 1)
        {
            PlayerPrefs.SetInt("Min", 0);
            PlayerPrefs.SetInt("Sec", 0);

            Init_timer(0, 0);
            return;
        }
        var oldtotal = (min * 60) + sec;
        var newtotal = (diff.Minutes * 60) + diff.Seconds;

        var leftPositive = (oldtotal > newtotal) ? oldtotal - newtotal : 0;

        var leftmin = (leftPositive == 0) ? 0 : leftPositive / 60;
        var leftsec = (leftPositive == 0) ? 0 : leftPositive % 60;
        Init_timer(leftmin, leftsec);
    }

    private void OnDisable()
    {
        if (!startedOnce_inLifecycle) return;

        PlayerPrefs.SetInt("Min", min);
        PlayerPrefs.SetInt("Sec", sec);
        PlayerPrefs.SetString("Time", System.DateTime.Now.ToString());
        //GameController.instanse.notificationManager.InitilizeNotificationChannel
        //    (new FCGSNotificationManager.GameNotificationChannel
        //    (GameController.instanse.notificationManager.notificationsIDs[0], "Gift For You", "GiftBox Is ready to collect"));
        //GameController.instanse.notificationManager.CreateNoification
        //    (new FCGSNotificationManager.NotificationCreate
        //    ("Gift For You", "Come back and collect your Reward", min, GameController.instanse.notificationManager.notificationsIDs[0], "gift_small"));
        //// StopAllCoroutines();
    }
    public void Init_timer(int min_left, int sec_left)
    {
        Debug.Log("life Cycle : "+startedOnce_inLifecycle);
        ButtonEffect(false);
        //do nothing on countdown finish
        if (min_left <= 0 && sec_left <= 0)
        {
            this.gameObject.SetActive(false);
            ButtonEffect(true);
            return;
        }
        this.gameObject.SetActive(true);

        txt = this.gameObject.GetComponent<Text>();
        StartCoroutine(StartTimer(min_left, sec_left));
    }

    void ButtonEffect(bool isActive)
    {
        if(isActive) initUI.OpenGift();
        giftBox_btn.interactable = isActive;
        if (isActive) LeanTween.rotate(giftBox_btn.gameObject, Vector3.one * 10f, 1f).setEasePunch().setLoopPingPong();
    }
    IEnumerator StartTimer(int _min_left, int _sec_left)
    {
        startedOnce_inLifecycle = true;

        min = _min_left;
        sec = _sec_left;
        txt.text = "" + min.ToString("00") + ":" + sec.ToString("00");

    Start_agin:
        yield return new WaitForSeconds(1f);
        if (sec > 0)
        {
            sec--;
            txt.text = "" + min.ToString("00") + ":" + sec.ToString("00");
            goto Start_agin;
        }
        else if (min > 0)
        {
            min--;
            sec = 59;
            txt.text = "" + min.ToString("00") + ":" + sec.ToString("00");
            if (min >= 0) goto Start_agin;
        }

        this.gameObject.SetActive(false);
        ButtonEffect(true);
        yield return null;
    }

    private void OnDestroy()
    {
        RegisterTimer();
    }
    void RegisterTimer()
    {
        Debug.Log("regidterd timers ");
        PlayerPrefs.SetInt("Min", min);
        PlayerPrefs.SetInt("Sec", sec);
        PlayerPrefs.SetString("Time", System.DateTime.Now.ToString());
        StopAllCoroutines();
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus) StartTimer();
        else RegisterTimer();

    }
}
