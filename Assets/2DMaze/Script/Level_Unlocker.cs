using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Unlocker : MonoBehaviour
{
    public int level_no;
    public bool isUnlocked = false;
    Button btn;
    Text txt;
    //custom method on enable
    public void _OnEnable()
    {
        btn = this.GetComponent<Button>();
        txt = this.GetComponentInChildren<Text>();
        if (isUnlocked)
        {
            btn.interactable = true;
            txt.text = (level_no+1).ToString();
            btn.onClick.AddListener(() => OnClick_btn());
        }
        else
        {
            txt.text = null;
            btn.interactable = false;
        }
    }

    void OnClick_btn()
    {
        GameController.instanse.audiomanager.ButtonClick_Audio();
        GameController.instanse.current_leve_no = level_no+1;
        GameController.instanse.ShowLoading();
        GameController.instanse.gamestatus = GameController.GameStatus.PlayMode;
        SceneManager.LoadSceneAsync(1);
    }

}
