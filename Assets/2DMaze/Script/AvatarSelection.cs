using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField]
    List<Sprite> avtar_sprite;
    [SerializeField]
    Image img;
    [SerializeField]
    Animator anim;

    [SerializeField]
    TextMeshProUGUI speed_text;
    [SerializeField]
    TextMeshProUGUI armour_text;
    [SerializeField]
    TextMeshProUGUI extratime_text;

    [SerializeField]
    AvatarCharchtrastics avatar_data;
    private void OnEnable()
    {
        img.sprite = avtar_sprite[UserData.instance.avtar_data.current_Avtar - 1];
        anim.SetInteger("avtar", UserData.instance.avtar_data.current_Avtar);
        speed_text.text = "+" + avatar_data.speed_data[UserData.instance.avtar_data.current_Avtar - 1];
        armour_text.text = "" + avatar_data.armours_data[UserData.instance.avtar_data.current_Avtar - 1].ToString("00");
        extratime_text.text = "+" + avatar_data.extra_time[UserData.instance.avtar_data.current_Avtar - 1].ToString("00");

    }
}
