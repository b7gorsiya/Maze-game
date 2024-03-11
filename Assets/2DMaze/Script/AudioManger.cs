using UnityEngine;

public class AudioManger : MonoBehaviour
{
    [SerializeField]
    AudioSource musicADS;
    [SerializeField]
    AudioSource sfxADS;


    [Header("Music")]
    [SerializeField]
    AudioClip backgroundMusic;

    [Header("SFX ")]
    [SerializeField]
    AudioClip buttonClick;
    [SerializeField]
    AudioClip gameWin;
    [SerializeField]
    AudioClip gameLoose;
    [SerializeField]
    AudioClip playerMove;
    [SerializeField]
    AudioClip hintDisplay;
    [SerializeField]
    AudioClip coinCollected;
    [SerializeField]
    AudioClip enemyHit;
    [SerializeField]
    AudioClip trap;
    [SerializeField]
    AudioClip timer;
    [SerializeField]
    AudioClip giftBox;
    [SerializeField]
    AudioClip pauseSound;
    [SerializeField]
    AudioClip extraTime;
    [SerializeField]
    AudioClip sucessSound;
    [SerializeField]
    AudioClip swip;

    public void MuteUnMute(bool _mute)
    {
        this.sfxADS.mute = _mute;
        musicADS.mute = _mute;
    }

    public void SwipeSound() => sfxADS.PlayOneShot(swip,0.1f);
    public void SucessSound() => sfxADS.PlayOneShot(sucessSound);
    public void ButtonClick_Audio() => sfxADS.PlayOneShot(buttonClick,0.5f);
    public void GiftBoX_Audio() { sfxADS.PlayOneShot(this.giftBox); }
    public void GameWin_Audio()
    {
        sfxADS.Stop();
        sfxADS.PlayOneShot(gameWin);
    }


    public void GameLoose_Audio() { sfxADS.Stop();  sfxADS.PlayOneShot(gameLoose); }

    public void Tarap_Audio() => sfxADS.PlayOneShot(trap);
    public void CoinCollect_Audio() => sfxADS.PlayOneShot(coinCollected,0.25f);
    public void Pause_Audio() => sfxADS.PlayOneShot(pauseSound);
    public void PlayerMove_Audio() => sfxADS.PlayOneShot(playerMove,0.4f);
    public void Hint_Audio() => sfxADS.PlayOneShot(hintDisplay, 0.05f);
    public void ExtraTime_Audio() => sfxADS.PlayOneShot(extraTime);
    public void Countdown_Audio() => sfxADS.PlayOneShot(timer);
    public void EnemyHit_Audio() => sfxADS.PlayOneShot(enemyHit);



    public void Background_Audio() => musicADS.Play();

}
