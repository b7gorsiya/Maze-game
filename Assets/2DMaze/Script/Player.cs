using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Play_Scene_UI_Managment ui;
    private void OnEnable()
    {
        ui = FindObjectOfType<Play_Scene_UI_Managment>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "door")
        {
            ui.GameWin();
            Debug.Log("Win");
            StopPlayer();
        }
        else if (collision.tag == "enemy")
        {
            GameController.instanse.audiomanager.EnemyHit_Audio();
            //if armour avail return 
            if (!ui.TakeHitOnPlayer())
            {
                LeanTween.rotateZ(collision.gameObject, 30, 0.5f).setEasePunch();
                LeanTween.rotateZ(ui.armourImage, 30, 0.2f).setEasePunch().setRepeat(3);
                return;
            }
            Destroy(collision.gameObject);
            ui.GameOver();
            StopPlayer();

            Debug.Log("hit ");
            //EnemeyHit(collision.gameObject);
           
        }else if(collision.tag=="coin")
        {
            GameController.instanse.audiomanager.CoinCollect_Audio();
            RemoveCoin(collision.gameObject);
        }
    }

    private void StopPlayer()
    {
      //  Destroy(this.gameObject);
    }

    void EnemeyHit(GameObject _enemy)
    {

        LeanTween.rotateZ(_enemy.gameObject, 30, 0.5f).setEasePunch().setRepeat(1);
        LeanTween.rotateZ(this.gameObject, 30, 0.5f).setEasePunch().setOnComplete(()=> {

            
            Debug.Log("hit ");
        });
        
    }
    private void RemoveCoin(GameObject _coin)
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 60, Screen.height - 95, 0));
        LeanTween.scale(_coin, Vector3.one * 0.30f, 0.5f).setEasePunch();
        LeanTween.move(_coin, pos, 0.5f).setOnComplete(() =>
        {
            ui.collectedCoin++;
            Debug.Log("Coin collected");
            GameController.instanse.Coin_Collected();
            Destroy(_coin);
        });
       
    }
}
