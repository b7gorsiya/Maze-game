using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardLabel : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI score;
    public Text scoretext;
    public Image badge;
    [SerializeField]
    List<Sprite> badges;

    public void SetScore(int? _score)
    {
        score.text = _score.ToString();
        badge.sprite = (_score > 5000) ? ((_score > 10000) ? ((_score > 15000) ? badges[0] : badges[1]) : badges[2]) : badges[3];
    }

    public void SetRank(int? _rank) => rank.text = _rank.ToString();
   
}
