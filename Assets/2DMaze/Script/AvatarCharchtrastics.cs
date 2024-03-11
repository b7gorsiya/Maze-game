using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarData", menuName = "ScriptableObjects/AvatarCharchtrastics", order = 2)]
public class AvatarCharchtrastics : ScriptableObject
{
    public List<int> avtarPrice_list = new List<int>(10);
    public List<float> speed_data;
    public List<int> armours_data;
    public List<int> extra_time;
}
