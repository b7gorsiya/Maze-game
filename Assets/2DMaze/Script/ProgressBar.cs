using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using CloudOnce;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    Slider _slider;

    [SerializeField]
    Image badge_sprite;

    [SerializeField]
    List<Sprite> _badges;

    [SerializeField]
    List<Color> slider_color;

    [SerializeField]
    LeaderBoard ld;
    private void OnEnable()
    {
        _slider.value = UserData.instance._progressbar._badge_Progress;
        badge_sprite.sprite = _badges[UserData.instance._progressbar._badge - 1];
        _slider.fillRect.gameObject.GetComponentInChildren<Image>().color = slider_color[UserData.instance._progressbar._badge - 1];
        GameController.instanse.HideLoading_Focefully();
    }

}
