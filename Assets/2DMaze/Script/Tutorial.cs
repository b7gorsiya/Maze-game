using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject pathWay;

    [SerializeField]
    List<Vector3> path;

    const float MAX_SWIPE_TIME = 0.5f;
    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    const float MIN_SWIPE_DISTANCE = 0.10f;
    Vector2 startPos;
    float startTime;

    [SerializeField]
    public bool allowMove = true;


    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject door;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    TextMeshProUGUI ins;
    [SerializeField]
    ParticleSystem sucess;

    [SerializeField]
    GameObject mainPanel;
    private void Awake()
    {
        foreach (var i in pathWay.GetComponentsInChildren<Transform>())
        {
            path.Add(i.position);
        }
        path.RemoveAt(0);
        player.transform.position = path[0];
        LeanTween.rotateZ(door, 180, 5f).setLoopType(LeanTweenType.linear);
        hand.transform.position = new Vector3(-0.5f, -2, 0);

        ins.text = "Move Right";
        LeanTween.moveLocalX(hand, 0.5f, 1f).setLoopPingPong();
    }
    private void OnEnable()
    {
        mainPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchControl();
        }
    }

    void TouchControl()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
                    return;

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                    return;

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                { // Horizontal swipe
                    if (swipe.x > 0)
                    {
                        if (player.transform.position == path[0])
                        {
                            CheckMove();
                            Debug.Log("right");
                        }
                    }

                }
                else
                { // Vertical swipe
                    if (swipe.y > 0)
                    {
                        if (player.transform.position == path[8] || player.transform.position == path[5])
                        {
                            CheckMove();
                            Debug.Log("Up");
                        }
                    }

                }
            }
        }
    }

    void CheckMove()
    {
        if (player.transform.position == path[0])
        {
            Move_one();
        }
        else if (player.transform.position == path[5])
        {
            Move_Two();
        }
        else if (player.transform.position == path[8])
        {
            Move_Three();
        }
    }

    void Move_one()
    {
        GameController.instanse.audiomanager.PlayerMove_Audio();
        LeanTween.move(player, path[1], 0.2f).setOnComplete(() =>
          {
              GameController.instanse.audiomanager.PlayerMove_Audio();
              LeanTween.move(player, path[5], 0.2f).setOnComplete(() =>
              {
                  allowMove = true;
                  hand.transform.position = new Vector3(0f, -2, 0);
                  ins.text = "Move UP";
                  LeanTween.cancel(hand);
                  LeanTween.moveLocalY(hand, -1, 0.5f).setLoopPingPong();


              });
          });
    }

    void Move_Two()
    {
        GameController.instanse.audiomanager.PlayerMove_Audio();
        LeanTween.move(player, path[9], 0.2f).setOnComplete(() =>
        {
            GameController.instanse.audiomanager.PlayerMove_Audio();
            LeanTween.move(player, path[8], 0.2f).setOnComplete(() =>
            {
                allowMove = true;
                hand.transform.position = new Vector3(0f, -2, 0);
                LeanTween.cancel(hand);
                LeanTween.moveLocalY(hand, -1, .5f).setLoopPingPong();

            });
        });
    }

    void Move_Three()
    {
        hand.gameObject.SetActive(false);
        GameController.instanse.audiomanager.PlayerMove_Audio();
        LeanTween.move(player, path[12], 0.2f).setOnComplete(() =>
        {
            GameController.instanse.audiomanager.PlayerMove_Audio();
            LeanTween.move(player, path[13], 0.2f).setOnComplete(() =>
            {
                GameController.instanse.audiomanager.PlayerMove_Audio();
                LeanTween.move(player, path[14], 0.2f).setOnComplete(() =>
                {
                    GameController.instanse.audiomanager.PlayerMove_Audio();
                    LeanTween.move(player, path[15], 0.2f).setOnComplete(() => TutorialCompleted());
                });
            });
        });
    }

    void TutorialCompleted()
    {
        ins.text = "Congratulation";
        sucess.Play();
        GameController.instanse.audiomanager.GameWin_Audio();
        LeanTween.delayedCall(3f, () =>
        {
            PlayerPrefs.SetInt("Trained", 1);
            Destroy(this.gameObject);
            mainPanel.SetActive(true);
        });
    }
}
