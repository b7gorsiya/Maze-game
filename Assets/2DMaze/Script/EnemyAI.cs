using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Vector3 startpos;
    public float speed = 1.2f;
    RaycastHit2D hit;
    float raycast_length = 0.3f;
    int hitcount = 0;

    public Vector2 initPosition;
    public  cell[,] cells;
    int rows = 0;
    int column = 0;
   public int currentRow = 0;
   public int currentColumn = 0;
   

    int oldR;
    int oldC;


    private void OnEnable()
    {
        Invoke("StartEnemy_Movement", 2f);
    }
    public void StartEnemy_Movement()
    {
        cells = FindObjectOfType<MazeGenrator>().cells;
        //Debug.Log("Calling enmy move 1st time");
        PlayerMovement(Random.Range(1, 5));
       // Debug.Log("first time called");
    }

    private void EnemeyMovement()
    {
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), raycast_length))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.red);

            if (hitcount >= 3)
            {
                transform.Rotate(new Vector3(0,0, transform.rotation.z + 180));
                hitcount = 0;
            }
            else
            {
                transform.Rotate(new Vector3(0,0, 90));
                hitcount++;
            }

        }

        transform.position += transform.up * Time.deltaTime * speed;
    }

    void PlayerMovement(int direction)
    {
        if (GameController.instanse.gamestatus != GameController.GameStatus.PlayMode) { Debug.Log("not aplay mode"); return; }

        var oldR = currentRow;
        var oldC = currentColumn;
        switch (direction)
        {
            //left
            case 1:
                if (LeftMove(true))
                {
                   // Debug.Log("Move Mad");
                    StartCoroutine(PredictNextMove(1));
                }
                else {
                   // Debug.Log("random ..not possible move");
                   PlayerMovement(Random.Range(1, 5)); 
                }
                break;
            //right
            case 2:
                if (RightMove(true))
                {
                   // Debug.Log("Move Mad");

                    StartCoroutine(PredictNextMove(2));
                }
                else {
                   // Debug.Log("random ..not possible move");
                   PlayerMovement(Random.Range(1, 5));
                }

                break;
            //up
            case 3:
                if (UpMove(true))
                {
                    //Debug.Log("Move Mad");

                    StartCoroutine(PredictNextMove(3));
                }
                else {
                    //Debug.Log("random ..not possible move");
                    PlayerMovement(Random.Range(1, 5));
                }

                break;
            //down
            case 4:
                if (DownMove(true))
                {
                   // Debug.Log("Move Mad");

                    StartCoroutine(PredictNextMove(4));
                }
                else {
                   // Debug.Log("random ..not possible move");
                   PlayerMovement(Random.Range(1, 5));
                }

                break;

        }
       // Debug.Log("Movement swith case fnish");
    }

    GameObject oldpath;
    
    bool LeftMove(bool move)
    {
        //do not do anything
        if (currentColumn == 0)
            return false;

        if (cells[currentRow, currentColumn - 1].right != null)
            return false;

        if (move)
        {
            currentColumn -= 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);
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

        if (move)
        {

            currentColumn += 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);
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


        if (move)
        {
            currentRow += 1;
            SmoothMove(cells[currentRow, currentColumn].floor.transform.localPosition);
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


        if (move)
        {

            currentRow -= 1;
           SmoothMove( cells[currentRow, currentColumn].floor.transform.localPosition);
        }
        return true;
    }
    void SmoothMove(Vector3 pos)
    {
        if (GameController.instanse.gamestatus != GameController.GameStatus.PlayMode) return;

        LeanTween.move(this.gameObject, pos, speed);
    }
    IEnumerator PredictNextMove(int move)
    {
        //stop if game is not in play mode 
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
           // Debug.Log("calling Auto movement");
            PlayerMovement(Random.Range(1, 5));
        }
        yield return null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("Floor"))
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
      
    }

   
}