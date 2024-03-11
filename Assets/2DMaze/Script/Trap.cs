using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Sprite safe;
    public Sprite trap;
    private void OnEnable()
    {
        StartCoroutine(Throw(Level_Manager.trapDealy));
    }
    bool isenable = false;
    IEnumerator Throw(int dealy)
    {
    Start:
        GetComponent<SpriteRenderer>().sprite = safe;
        GetComponent<BoxCollider2D>().enabled = false;
        isenable = false;
        yield return new WaitForSeconds(dealy);
        isenable = true;
        GetComponent<BoxCollider2D>().enabled = true;

        GetComponent<SpriteRenderer>().sprite=trap;
        yield return new WaitForSeconds(dealy);

        if(GameController.instanse.gamestatus==GameController.GameStatus.PlayMode) goto Start;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isenable)
        {
            GameController.instanse.audiomanager.Tarap_Audio();
            //if armour avail return 
            if (!collision.GetComponent<Player>().ui.TakeHitOnPlayer())
            {
                LeanTween.rotateZ(collision.gameObject, 30, 0.5f).setEasePunch();
                LeanTween.rotateZ(collision.GetComponent<Player>().ui.armourImage, 30, 0.2f).setEasePunch().setRepeat(3);
                return;
            }
            collision.GetComponent<Player>().ui.GameOver();
            Debug.Log("colide with" + collision.name);
            StopAllCoroutines();
            GetComponent<SpriteRenderer>().sprite = safe;
            Destroy(this.gameObject);
        }
    }
}
