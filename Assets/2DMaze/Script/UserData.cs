using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Levels
{
    public int class1;
    public int class2;
    public int class3;
    public int class4;
    public int class5;
    public int class6;
}

[System.Serializable]
public class AvtarData
{
    [SerializeField]
    public Dictionary<string, int> avtars_List;
    public int current_Avtar;
    public float speed;
    public int armour;
    public int extra_time;

}

public class ProgressData
{
    public int _badge;
    public int _badge_Progress;

}
public class UserData : MonoBehaviour
{
    public static UserData instance;

    public ProgressData _progressbar;
    int coins;
    public Levels levels;

    public AvtarData avtar_data;
    //public Dictionary<string, int> avtar_DATA=new Dictionary<string, int>();
    
    public int GetCoins { private set { } get { return coins; } }


    private void Awake()
    {
        instance = this;
        levels = new Levels();
        _progressbar = new ProgressData();
        GetLevelData();
        GetCoinData();
        GetAvatar_Deatils(PlayerPrefs.GetInt("Selected_Avatar", 1));
        GetProgress();

    }

    void GetProgress()
    {
        UpdateProgressData();
        _progressbar._badge = PlayerPrefs.GetInt("Badge", 4);
      _progressbar._badge_Progress = PlayerPrefs.GetInt("Badge_" + _progressbar._badge, 0);

    }

    int _cat1;
    int _cat2;
    int _cat3;
    int _cat4;
    public void UpdateProgressData()
    {
        if(levels.class1<=10|| levels.class2 <= 10 || levels.class3 <= 10 || levels.class4 <= 10)
        {
            //update blue badge
             _cat1 = (levels.class1 <= 10) ? levels.class1 / 2 : 5;
             _cat2 = (levels.class2 <= 10) ? levels.class2 / 2 : 5;
             _cat3 = (levels.class3 <= 10) ? levels.class3 / 2 : 5;
             _cat4 = (levels.class4 <= 10) ? levels.class4 / 2 : 5;
            PlayerPrefs.SetInt("Badge_4" , ((_cat1+_cat2+_cat3+_cat4))*5);
            PlayerPrefs.SetInt("Badge", 4);
            PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Badge_4") * 5000) / 100);

        }
        else if (levels.class1 <= 20 || levels.class2 <= 20 || levels.class3 <= 20 || levels.class4 <= 20)
        {
            //update brown badge
             _cat1 = (levels.class1 <= 20) ? (levels.class1 / 2)-5 : 5;
             _cat2 = (levels.class2 <= 20) ? (levels.class2 / 2)-5 : 5;
             _cat3 = (levels.class3 <= 20) ? (levels.class3 / 2)-5 : 5;
             _cat4 = (levels.class4 <= 20) ? (levels.class4 / 2)-5 : 5;
            PlayerPrefs.SetInt("Badge_3", ((_cat1 + _cat2 + _cat3 + _cat4)*5));
            PlayerPrefs.SetInt("Badge", 3);

            PlayerPrefs.SetInt("Score", ((PlayerPrefs.GetInt("Badge_3") * 10000) / 100)+5000);

        }
        else if (levels.class1 <= 30 || levels.class2 <= 30 || levels.class3 <= 30 || levels.class4 <= 30)
        {
            //update silver badge
            _cat1 = (levels.class1 <= 30) ? (levels.class1 / 2) - 10 : 5;
            _cat2 = (levels.class2 <= 30) ? (levels.class2 / 2) - 10 : 5;
            _cat3 = (levels.class3 <= 30) ? (levels.class3 / 2) - 10 : 5;
            _cat4 = (levels.class4 <= 30) ? (levels.class4 / 2) - 10 : 5;
            PlayerPrefs.SetInt("Badge_2", ((_cat1 + _cat2 + _cat3 + _cat4)*5));
            PlayerPrefs.SetInt("Badge", 2);

            PlayerPrefs.SetInt("Score",( (PlayerPrefs.GetInt("Badge_2") * 15000) / 100)+10000);

        }
        else if (levels.class1 <= 40 || levels.class2 <= 40 || levels.class3 <= 40 || levels.class4 <= 40)
        {
            //update gold badge
            _cat1 = (levels.class1 <= 40) ? (levels.class1 / 2) - 15 : 5;
            _cat2 = (levels.class2 <= 40) ? (levels.class2 / 2) - 15 : 5;
            _cat3 = (levels.class3 <= 40) ? (levels.class3 / 2) - 15 : 5;
            _cat4 = (levels.class4 <= 40) ? (levels.class4 / 2) - 15 : 5;
            PlayerPrefs.SetInt("Badge_1", ((_cat1 + _cat2 + _cat3 + _cat4)*5));
            PlayerPrefs.SetInt("Badge", 1);
            PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Badge_1") * 20000) / 15000 );

        }
        else
        {
            //aready gold badge holder
            //become legend
        }

    }
    void GetCoinData()
    {
        coins = PlayerPrefs.GetInt("Coins", 00);
    }
    public void UpdateCoins() => GetCoinData();
    void GetLevelData()
    {
        levels.class1 = GetLevel("class1");
        levels.class2 = GetLevel("class2");
        levels.class3 = GetLevel("class3");
        levels.class4 = GetLevel("class4");
        levels.class5 = GetLevel("class5");
        levels.class6 = GetLevel("class6");
    }

    void GetAvatar_Deatils(int _default)
    {
        avtar_data = new AvtarData();
        avtar_data.avtars_List = new Dictionary<string, int>();

        //seting defualt avtar
        PlayerPrefs.SetInt("Avatar_"+_default, 1);
        avtar_data.current_Avtar = PlayerPrefs.GetInt("Selected_Avatar", _default);

        for (int i = 1; i <= 10; i++)
        {
           avtar_data.avtars_List.Add("Avatar_" + i, PlayerPrefs.GetInt("Avatar_" + i, 0));
        }
    }

    public void SetAvatarChrachterstics(float _speed,int _armour,int _extra_time)
    {
        avtar_data.armour = _armour;
        avtar_data.extra_time = _extra_time;
        avtar_data.speed = _speed;
    }
    public void Update_Avatars(int selected)
    {
        GetAvatar_Deatils(selected);
    }
    private int GetLevel(string class_name) {
         return PlayerPrefs.GetInt(class_name,1);
    }

    public void UpdateCoins(int _coins)
    {
        if (_coins > coins) PlayerPrefs.SetInt("Coins", _coins);
        GetCoinData();
    }
    public void Update_UserData(string _class_name, int _level)
    {

        SetLevel(_class_name, _level);
        GetLevelData();
    }

    private void SetLevel(string _class,int _levels)
    {
        PlayerPrefs.SetInt(_class, _levels);
    }
}
