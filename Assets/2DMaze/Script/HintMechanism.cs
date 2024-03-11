using System.Collections.Generic;
using UnityEngine;

public class HintMechanism : MonoBehaviour
{
    public cell[,] cells;
    public int rows { private get; set; }
    public int column { private get; set; }

    public int startRow { private get; set; }
    public int startColumn { private get; set; }

    public int finishRow { private get; set; }
    public int finishColumn { private get; set; }

    int currentRow;
    int currentColumn;

    [SerializeField]
   List<GameObject> route=new List<GameObject>();

    public List<Vector3> bestRoute;

    public List<int> directionTaken = new List<int>();
    public List<int> clmn_indexs = new List<int>();
    public List<int> row_index = new List<int>();

    public void SetStartPoint(int _row,int _clmn)
    {
        currentRow = startRow= _row;
        currentColumn= startColumn = _clmn;
    }
    public void CheckMove()
    {
        bestRoute = new List<Vector3>();
        route = new List<GameObject>();
        foreach (var c in cells)
        {
            c.isVisited = false;
        }
        directionTaken.Clear();
        clmn_indexs.Clear();
        row_index.Clear();

        if (currentRow == finishRow && currentColumn == finishColumn) return;

        CheckForNextMove();
        bool redo = false;

        if (route.Count > 0)
        {
        STARTAGAIN:

            redo = (route.Contains(cells[finishRow, finishColumn].floor)) ? false : true;
            if (redo)
            {
                ChooseDiffrentPath(row_index[row_index.Count - 1], clmn_indexs[clmn_indexs.Count - 1], directionTaken[directionTaken.Count - 1]);

                goto STARTAGAIN;
            }

            int indexOfstart = route.IndexOf(cells[startRow, startColumn].floor);

            for(int i=0;i<route.Count;i++)
            {
                if (i <= indexOfstart) continue;
                else if (route[i] == cells[finishRow, finishColumn].floor) break;
                else bestRoute.Add(route[i].transform.position);
            }
          
        }
    }

    void CheckForNextMove()
    {
        //left possible move
        if (currentColumn != 0 && cells[currentRow, currentColumn - 1].right == null && !cells[currentRow, currentColumn - 1].isVisited) { PredictNextMove(1); }
        //right possible move
        else if (cells[currentRow, currentColumn].right == null && !cells[currentRow, currentColumn + 1].isVisited) { PredictNextMove(2); }

        //up possible move
        else if (cells[currentRow, currentColumn].bottom == null && !cells[currentRow + 1, currentColumn].isVisited) { PredictNextMove(3); }
        // down possible move
        else if (currentRow != 0 && cells[currentRow - 1, currentColumn].bottom == null && !cells[currentRow - 1, currentColumn].isVisited) { PredictNextMove(4); }
    }
    void RemoveVisitedCells()
    {
        for (int i = (route.Count - 1); i >= 0; i--)
        {

            if (cells[currentRow, currentColumn].floor.name == route[i].name)
            {
                break;
            }
            route.RemoveAt(i);

        }
        clmn_indexs.RemoveAt(clmn_indexs.Count - 1);
        row_index.RemoveAt(row_index.Count - 1);
        directionTaken.RemoveAt(directionTaken.Count - 1);
    }
    void ChooseDiffrentPath(int r, int c, int t_dir)
    {
       // List<int> moves = new List<int>() { 1, 2, 3, 4 };
        //moves.Remove(t_dir);
        currentColumn = c;
        currentRow = r;
        cells[currentRow, currentColumn].isVisited = false;
        RemoveVisitedCells();
        CheckForNextMove();
    }
    bool LeftMove(bool move)
    {
        //do not do anything
        if (currentColumn == 0)
            return false;

        if (cells[currentRow, currentColumn - 1].right != null || cells[currentRow, currentColumn - 1].isVisited)
            return false;

        if (move && cells[currentRow, currentColumn - 1].isVisited) return false;

        if (move)
        {
            currentColumn -= 1;
            cells[currentRow, currentColumn].isVisited = true;
            AddPath();
        }

        return true;
    }
    bool RightMove(bool move)
    {
        //do not do anything
        if (currentColumn == column - 1)
            return false;

        if (cells[currentRow, currentColumn].right != null || cells[currentRow, currentColumn + 1].isVisited)
            return false;

        if (move && cells[currentRow, currentColumn + 1].isVisited) return false;
        if (move)
        {
            currentColumn += 1;
            cells[currentRow, currentColumn].isVisited = true;
            AddPath();
        }
        return true;
    }
    bool UpMove(bool move)
    {
        //do not do anything
        if (currentRow == rows - 1)
            return false;

        if (cells[currentRow, currentColumn].bottom != null || cells[currentRow + 1, currentColumn].isVisited)
            return false;

        if (move && cells[currentRow + 1, currentColumn].isVisited) return false;

        if (move)
        {
            currentRow += 1;
            cells[currentRow, currentColumn].isVisited = true;
            AddPath();
        }
        return true;
    }
    bool DownMove(bool move)
    {
        //do not do anything
        if (currentRow == 0)
            return false;

        if (cells[currentRow - 1, currentColumn].bottom != null || cells[currentRow - 1, currentColumn].isVisited)
            return false;

        if (move && cells[currentRow - 1, currentColumn].isVisited) return false;

        if (move)
        {
            currentRow -= 1;
            cells[currentRow, currentColumn].isVisited = true;
            AddPath();
        }
        return true;
    }

    GameObject oldpath;
    void AddPath()
    {
        route.Add(cells[currentRow, currentColumn].floor);
    }

    void RegisterExtraMoves(int m)
    {
        directionTaken.Add(m);
        clmn_indexs.Add(currentColumn);
        row_index.Add(currentRow);
    }
    void PredictNextMove(int move)
    {
        int possibleRoute = 0;
        possibleRoute += (LeftMove(false)) ? 1 : 0;
        possibleRoute += (RightMove(false)) ? 1 : 0;

        possibleRoute += (UpMove(false)) ? 1 : 0;
        possibleRoute += (DownMove(false)) ? 1 : 0;

        switch (move)
        {
            //from left move
            case 1:

                if (possibleRoute > 1) RegisterExtraMoves(1);

                if (LeftMove(true))
                {
                    PredictNextMove(1);
                }
                else if (UpMove(true))
                {
                    PredictNextMove(3);
                }
                else if (DownMove(true))
                {
                    PredictNextMove(4);
                }

                break;
            //from right move
            case 2:

                if (possibleRoute > 1) RegisterExtraMoves(2);

                if (RightMove(true))
                {
                    PredictNextMove(2);
                }
                else if (UpMove(true))
                {
                    PredictNextMove(3);
                }
                else if (DownMove(true))
                {
                    PredictNextMove(4);
                }
                break;
            //from up move
            case 3:
                if (possibleRoute > 1) RegisterExtraMoves(3);

                if (LeftMove(true))
                {
                    PredictNextMove(1);
                }
                else if (UpMove(true))
                {
                    PredictNextMove(3);
                }
                else if (RightMove(true))
                {
                    PredictNextMove(2);
                }
                break;
            //from down move
            case 4:
                if (possibleRoute > 1) RegisterExtraMoves(4);
                if (LeftMove(true))
                {
                    PredictNextMove(1);
                }
                else if (DownMove(true))
                {
                    PredictNextMove(4);
                }
                else if (RightMove(true))
                {
                    PredictNextMove(2);
                }
                break;
        }
    }
}
