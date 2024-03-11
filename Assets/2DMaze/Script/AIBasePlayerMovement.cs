using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasePlayerMovement : MonoBehaviour
{

    public cell[,] cells;
    int rows = 0;
    int column = 0;
    public int currentRow = 0;
    public int currentColumn = 0;

    // low the value higer the speed
    public float speed = 0.5f;
    [SerializeField]
    GameObject path_prefeb;

    public List<GameObject> route;
    public GameObject routeCollection;

    int oldR;
    int oldC;

    const float MAX_SWIPE_TIME = 0.5f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    const float MIN_SWIPE_DISTANCE = 0.10f;
    Vector2 startPos;
    float startTime;
    [SerializeField]
    public bool allowMove = true;

    public void InitPlayer()
    {
        routeCollection = new GameObject();
        rows = cells.GetLength(0);
        column = cells.GetLength(1);
        Application.targetFrameRate = 60;
        route = new List<GameObject>();
        SetSwipeHint();
        allowMove = true;
    }


    void Update()
    {
        //retun from here if game not in play mode
        if (GameController.instanse.gamestatus != GameController.GameStatus.PlayMode) return;
        if (allowMove)
        {
#if UNITY_EDITOR
            if (Input.anyKeyDown)
            {
                oldC = currentColumn;
                oldR = currentRow;

                EditorInput();
            }
#endif

            if (Input.touchCount > 0)
            {

                oldC = currentColumn;
                oldR = currentRow;
                TouchControl();
            }
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
                        PlayerMovement(2);
                        //swipedRight = true;
                    }
                    else
                    {
                        PlayerMovement(1);
                        //swipedLeft = true;
                    }
                }
                else
                { // Vertical swipe
                    if (swipe.y > 0)
                    {
                        PlayerMovement(3);
                        //swipedUp = true;
                    }
                    else
                    {
                        PlayerMovement(4);
                        //swipedDown = true;
                    }
                }
            }
        }
        /*
        float hori = 0;
        float vert = 0;
       
        if(Input.GetTouch(0).phase==TouchPhase.Began)
        {
            hori = Input.GetTouch(0).position.x;
            vert = Input.GetTouch(0).position.y;

        }

        if (Input.GetTouch(0).phase==TouchPhase.Ended)
        {
            hori = Input.GetTouch(0).position.x - hori;
            vert = Input.GetTouch(0).position.y - vert;
            if (Mathf.Abs(vert) > Mathf.Abs(hori))
            {
                PlayerMovement((vert < 0) ? 4 : 3);
                //up
                if (vert > 0)
                {
                    Debug.Log("Up");
                }
                // down
                else if (vert < 0)
                {
                    Debug.Log("Down");
                }
            }
            else if (Mathf.Abs(vert) < Mathf.Abs(hori))
            {
                PlayerMovement((hori < 0) ? 1 : 2);

                //right
                if (hori > 0)
                {
                    Debug.Log("right");
                }
                //left
                else if(hori<0)
                {
                    Debug.Log("left");
                }
            }

        }
        */

    }

    void EditorInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PlayerMovement(1);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) PlayerMovement(2);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) PlayerMovement(3);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) PlayerMovement(4);
        else return;

    }

    //1=left 2=right 3=up 4=down
    void PlayerMovement(int direction)
    {
        HideSwipeHint();
        var oldR = currentRow;
        var oldC = currentColumn;

        switch (direction)
        {
            //left
            case 1:
                if (LeftMove(true))
                {

                    StartCoroutine(PredictNextMove(1));
                }
                break;
            //right
            case 2:
                if (RightMove(true))
                {
                    StartCoroutine(PredictNextMove(2));
                }
                break;
            //up
            case 3:
                if (UpMove(true))
                {

                    StartCoroutine(PredictNextMove(3));
                }

                break;
            //down
            case 4:
                if (DownMove(true))
                {

                    StartCoroutine(PredictNextMove(4));
                }
                break;

        }
        /*if (route.Count > 1)
        {
            var newR = currentRow;
            var newC = currentColumn;
            currentRow = oldR;
            currentColumn = oldC;
            AddPath();
            currentRow = newR;
            currentColumn = newC;
        }*/
        // Debug.Log("row :" + currentRow + "column :" + currentColumn);

    }

    GameObject oldpath;
    void AddPath()
    {
        if (GameController.instanse.gamestatus != GameController.GameStatus.PlayMode) return;

        foreach (var p in route)
        {
            if (p.transform.position == cells[currentRow, currentColumn].floor.transform.localPosition)
            {
                if (oldpath != null)
                {
                    route.Remove(oldpath);
                    DestroyImmediate(oldpath);
                    oldpath = null;
                }
                route.Remove(p);
                DestroyImmediate(p);
                return;
            }

        }
        GameObject tmp = Instantiate(path_prefeb, cells[currentRow, currentColumn].floor.transform.localPosition, Quaternion.identity);
        LeanTween.scale(tmp,Vector3.one*1.5f,0.2f).setEaseInBack();
        tmp.transform.parent = routeCollection.transform;
        route.Add(tmp);
        if (currentRow == oldR && currentColumn == oldC)
        {
            oldpath = tmp;
        }
    }
    bool LeftMove(bool move)
    {
        //do not do anything
        if (currentColumn == 0)
            return false;

        if (cells[currentRow, currentColumn - 1].right != null)
            return false;
        allowMove = false;

        if (move)
        {
            AddPath();
            currentColumn -= 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);

            // transform.position = cells[currentRow, currentColumn].floor.transform.localPosition;
        }

        return true;
    }
    bool RightMove(bool move)
    {
        //do not do anything
        if (currentColumn == column - 1)
            return false;

        if (cells[currentRow, currentColumn].right != null)
            return false;
        allowMove = false;

        if (move)
        {
            AddPath();

            currentColumn += 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);

            //transform.position = cells[currentRow, currentColumn].floor.transform.localPosition;
        }
        return true;
    }
    bool UpMove(bool move)
    {
        //do not do anything
        if (currentRow == rows - 1)
            return false;

        if (cells[currentRow, currentColumn].bottom != null)
            return false;

        allowMove = false;

        if (move)
        {
            AddPath();
            currentRow += 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);

            //transform.position = cells[currentRow, currentColumn].floor.transform.localPosition;
        }
        return true;
    }
    bool DownMove(bool move)
    {
        //do not do anything
        if (currentRow == 0)
            return false;

        if (cells[currentRow - 1, currentColumn].bottom != null)
            return false;

        allowMove = false;

        if (move)
        {
            AddPath();

            currentRow -= 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);
            //  transform.position = cells[currentRow, currentColumn].floor.transform.localPosition;
        }
        return true;
    }

    void PlaycornerSound()
    {
        bool bottamWall = true;
        bool leftwall = true;
        if (currentRow!=0)bottamWall =cells[currentRow - 1, currentColumn].bottom != null;
        var rightWall = cells[currentRow, currentColumn].right != null;
        if(currentColumn!=0) leftwall = cells[currentRow, currentColumn-1].right != null;
        var topWall = cells[currentRow, currentColumn].bottom != null;

        bool playSound = false;

        if (bottamWall && rightWall)
            playSound = true;

        else if (leftwall && bottamWall)
            playSound = true;


        else if (rightWall && topWall)
            playSound = true;


        else if (leftwall && topWall)
            playSound = true;

        if (playSound) GameController.instanse.audiomanager.PlayerMove_Audio();

    }

    void SmoothMove(Vector3 pos)
    {
        if (GameController.instanse.gamestatus != GameController.GameStatus.PlayMode)  return;
       
        LeanTween.move(this.gameObject, pos, speed).setOnComplete(() => PlaycornerSound());
        
    }
    IEnumerator PredictNextMove(int move)
    {
        yield return new WaitForSeconds(speed);
        int possibleRoute = 0;
        switch (move)
        {
            //from left move
            case 1:
                possibleRoute += (LeftMove(false)) ? 1 : 0;
                possibleRoute += (UpMove(false)) ? 1 : 0;
                possibleRoute += (DownMove(false)) ? 1 : 0;
                if (possibleRoute == 1)
                {
                    if (LeftMove(true))
                    {
                        StartCoroutine(PredictNextMove(1));
                    }
                    else if (UpMove(true))
                    {
                        StartCoroutine(PredictNextMove(3));
                    }
                    else if (DownMove(true))
                    {
                        StartCoroutine(PredictNextMove(4));
                    }
                }
                break;
            //from right move
            case 2:
                possibleRoute += (RightMove(false)) ? 1 : 0;
                possibleRoute += (UpMove(false)) ? 1 : 0;
                possibleRoute += (DownMove(false)) ? 1 : 0;
                if (possibleRoute == 1)
                {
                    if (RightMove(true))
                    {
                        StartCoroutine(PredictNextMove(2));
                    }
                    else if (UpMove(true))
                    {
                        StartCoroutine(PredictNextMove(3));
                    }
                    else if (DownMove(true))
                    {
                        StartCoroutine(PredictNextMove(4));
                    }
                }
                break;
            //from up move
            case 3:
                possibleRoute += (RightMove(false)) ? 1 : 0;
                possibleRoute += (UpMove(false)) ? 1 : 0;
                possibleRoute += (LeftMove(false)) ? 1 : 0;
                if (possibleRoute == 1)
                {
                    if (LeftMove(true))
                    {
                        StartCoroutine(PredictNextMove(1));
                    }
                    else if (UpMove(true))
                    {
                        StartCoroutine(PredictNextMove(3));
                    }
                    else if (RightMove(true))
                    {
                        StartCoroutine(PredictNextMove(2));
                    }
                }
                break;
            //from down move
            case 4:
                possibleRoute += (RightMove(false)) ? 1 : 0;
                possibleRoute += (DownMove(false)) ? 1 : 0;
                possibleRoute += (LeftMove(false)) ? 1 : 0;
                if (possibleRoute == 1)
                {
                    if (LeftMove(true))
                    {
                        StartCoroutine(PredictNextMove(1));
                    }
                    else if (DownMove(true))
                    {
                        StartCoroutine(PredictNextMove(4));
                    }
                    else if (RightMove(true))
                    {
                        StartCoroutine(PredictNextMove(2));
                    }
                }
                break;
        }
        if (possibleRoute < 1 || possibleRoute > 1)
        {
            SetSwipeHint();
            foreach (var p in route)
            {
                if (p.transform.position == cells[currentRow, currentColumn].floor.transform.localPosition)
                {
                    if (oldpath != null)
                    {
                        route.Remove(oldpath);
                        DestroyImmediate(oldpath);
                        oldpath = null;
                    }
                    route.Remove(p);
                    Destroy(p);
                    Debug.Log("Last destroyed");
                    break;
                }

            }
            allowMove = true;

        }
        yield return null;
    }

   [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;
    [SerializeField] GameObject downArrow;
    [SerializeField] GameObject upArrow;

    void HideSwipeHint()
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        upArrow.SetActive(false);
        downArrow.SetActive(false);
    }
    void SetSwipeHint()
    {
        leftArrow.SetActive(LeftMove(false));
        rightArrow.SetActive(RightMove(false));
        upArrow.SetActive(UpMove(false));
        downArrow.SetActive(DownMove(false));
    }
}
