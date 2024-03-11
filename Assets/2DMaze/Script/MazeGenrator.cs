using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell
{
    public GameObject floor;
    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject bottom;
    public bool isVisited = false;
}

public class MazeGenrator : MonoBehaviour
{

    // variables for cell genrationg 
   [SerializeField]
    private GameObject wall; // wall prefeb
    [SerializeField]
    private GameObject floor; // floor prefeb
    private int columns;//width/column
    private int rows;//height/row
    [SerializeField]
    private float wall_size; // this means wall dimenstion
    public cell[,] cells; // to store cellss


    // varibles for creating a maze
    private int cur_column = 0;
    private int cur_row = 0;
    private bool allgood = false;

    public GameObject player;
    [SerializeField]
    GameObject enemyPrefeb;
    [SerializeField]
    GameObject dootToFinish;

    Vector2 startPoint;
    Vector2 finishPoint;

    [SerializeField]
    public GameObject maze2D;

    public void  Init_variables(GameObject _wall,GameObject _floor,int _columns,int _rows,float _wall_size,ref cell[,] _cells)
    {
    wall=_wall; // wall prefeb
    floor=_floor; // floor prefeb
    columns=_columns;//width/column
    rows=_rows;//height/row
    wall_size=_wall_size; // this means wall dimenstion
    cells = _cells; // to store cellss

    MazeCreation();

        
    }
    private void MazeCreation()
    {
        Make2DCell();
        allgood = false;
        Making_maze();


       // FindObjectOfType<Level_Manager>().MazeReady();
    }


    #region Genrate_Maze

    public void Making_maze()
    {
        cells[cur_row,cur_column].isVisited = true;
        while (!allgood)
        {
            find_deadend();
            processed_unVisited();
        }
    }

    #region DEAD END FINIDING
    private void find_deadend()
    {
        while (RouteStillAvailable(cur_row, cur_column))
        {
            int direction = Random.Range(1, 5);//THE BEST RANDOM

            if (direction == 1 && CellIsAvailable(cur_row - 1, cur_column))
            {
                // North
                DestroyWallIfItExists(cells[cur_row, cur_column].up);
                DestroyWallIfItExists(cells[cur_row - 1, cur_column].bottom);
                cur_row--;
            }
            else if (direction == 2 && CellIsAvailable(cur_row + 1, cur_column))
            {
                // South
                DestroyWallIfItExists(cells[cur_row, cur_column].bottom);
                DestroyWallIfItExists(cells[cur_row + 1, cur_column].up);
                cur_row++;
            }
            else if (direction == 3 && CellIsAvailable(cur_row, cur_column + 1))
            {
                // east
                DestroyWallIfItExists(cells[cur_row, cur_column].right);
                DestroyWallIfItExists(cells[cur_row, cur_column + 1].left);
                cur_column++;
            }
            else if (direction == 4 && CellIsAvailable(cur_row, cur_column - 1))
            {
                // west
                DestroyWallIfItExists(cells[cur_row, cur_column].left);
                DestroyWallIfItExists(cells[cur_row, cur_column - 1].right);
                cur_column--;
            }

            cells[cur_row, cur_column].isVisited = true;
        }
    }

    private void DestroyWallIfItExists(GameObject wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall);
        }
    }

    private bool RouteStillAvailable(int row, int column)
    {
        int availableRoutes = 0;

        if (row > 0 && !cells[row - 1, column].isVisited)
        {
            availableRoutes++;
        }

        if (row < rows - 1 && !cells[row + 1, column].isVisited)
        {
            availableRoutes++;
        }

        if (column > 0 && !cells[row, column - 1].isVisited)
        {
            availableRoutes++;
        }

        if (column < columns - 1 && !cells[row, column + 1].isVisited)
        {
            availableRoutes++;
        }

        return availableRoutes > 0;
    }

    private bool CellIsAvailable(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns && !cells[row, column].isVisited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    private void processed_unVisited()
    {
        allgood = true;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (!cells[r, c].isVisited && CellHasAnAdjacentVisitedCell(r, c))
                {
                    allgood = false; // still unvisited cell.
                    cur_row = r;
                    cur_column = c;
                    DestroyAdjacentWall(cur_row, cur_column);
                    cells[cur_row, cur_column].isVisited = true;
                    return; // Exit the function
                }
            }
        }
    }

    private void DestroyAdjacentWall(int row, int column)
    {
        bool wallDestroyed = false;

        while (!wallDestroyed)
        {
            int direction = Random.Range(1, 5);

            if (direction == 1 && row > 0 && cells[row - 1, column].isVisited)
            {
                DestroyWallIfItExists(cells[row, column].up);
                DestroyWallIfItExists(cells[row - 1, column].bottom);
                wallDestroyed = true;
            }
            else if (direction == 2 && row < (rows - 2) && cells[row + 1, column].isVisited)
            {
                DestroyWallIfItExists(cells[row, column].bottom);
                DestroyWallIfItExists(cells[row + 1, column].up);
                wallDestroyed = true;
            }
            else if (direction == 3 && column > 0 && cells[row, column - 1].isVisited)
            {
                DestroyWallIfItExists(cells[row, column].left);
                DestroyWallIfItExists(cells[row, column - 1].right);
                wallDestroyed = true;
            }
            else if (direction == 4 && column < (columns - 2) && cells[row, column + 1].isVisited)
            {
                DestroyWallIfItExists(cells[row, column].right);
                DestroyWallIfItExists(cells[row, column + 1].left);
                wallDestroyed = true;
            }
        }

    }

    private bool CellHasAnAdjacentVisitedCell(int row, int column)
    {
        int visitedCells = 0;

        // Look 1 row up (north) if we're on row 1 or greater
        if (row > 0 && cells[row - 1, column].isVisited)
        {
            visitedCells++;
        }

        // Look one row down (south) if we're the second-to-last row (or less)
        if (row < (rows - 2) && cells[row + 1, column].isVisited)
        {
            visitedCells++;
        }

        // Look one row left (west) if we're column 1 or greater
        if (column > 0 && cells[row, column - 1].isVisited)
        {
            visitedCells++;
        }

        // Look one row right (east) if we're the second-to-last column (or less)
        if (column < (columns - 2) && cells[row, column + 1].isVisited)
        {
            visitedCells++;
        }

        // return true if there are any adjacent visited cells to this one
        return visitedCells > 0;
    }
    #endregion

    #region Cell_Genrating
    /*
    public void Make3DCell()
    {
        //defining maze length and width
        cells = new cell[mazeY, mazeX];
        GameObject gs = new GameObject("Maze");
        for (int r = 0; r < mazeY; r++)
        {
            for (int c = 0; c < mazeX; c++)
            {
                cells[r, c] = new cell();
                GameObject cell = new GameObject("cell[" + r + "," + c + "]");

                //For now, use the same wall object for the floor!
                cells[r, c].floor = GameObject.Instantiate(floor, new Vector3(c * wall_size, -(wall_size / 2f), r * wall_size), Quaternion.identity) as GameObject;
                cells[r, c].floor.name = "Floor " + r + "," + c;
                cells[r, c].floor.transform.parent = cell.transform;

                if (c == 0)
                {
                    cells[r, c].left = GameObject.Instantiate(wall, new Vector3(((c * wall_size) - (wall_size / 2)), 0, r * wall_size), Quaternion.identity) as GameObject;
                    cells[r, c].left.name = "Left Wall " + r + "," + c;
                    cells[r, c].left.transform.parent = cell.transform;

                }

                cells[r, c].right = GameObject.Instantiate(wall, new Vector3((wall_size / 2) + (c * wall_size), 0, r * wall_size), Quaternion.identity) as GameObject;
                cells[r, c].right.name = "Right Wall " + r + "," + c;
                cells[r, c].right.transform.parent = cell.transform;

                if (r == 0)
                {
                    cells[r, c].top = GameObject.Instantiate(wall, new Vector3(c * wall_size, 0, ((r * wall_size) - (wall_size / 2))), Quaternion.identity) as GameObject;
                    cells[r, c].top.name = "Up Wall " + r + "," + c;
                    cells[r, c].top.transform.Rotate(Vector3.up * 90f);
                    cells[r, c].top.transform.parent = cell.transform;

                }

                cells[r, c].bottom = GameObject.Instantiate(wall, new Vector3(c * wall_size, 0, ((wall_size / 2) + (r * wall_size))), Quaternion.identity) as GameObject;
                cells[r, c].bottom.name = "Down Wall " + r + "," + c;
                cells[r, c].bottom.transform.Rotate(Vector3.up * 90f);
                cells[r, c].bottom.transform.parent = cell.transform;


                cell.transform.parent = gs.transform;

            }
        }
    }
    */
    public void Make2DCell()
    {
        //defining maze length and width
        //cells = new cell[rows, columns];
       
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                cells[r, c] = new cell();
                GameObject cell = new GameObject("cell[" + r + "," + c + "]");

                //For now, use the same wall object for the floor!
                cells[r, c].floor = GameObject.Instantiate(floor, new Vector3(c * wall_size, r * wall_size, 0), Quaternion.identity) as GameObject;
                cells[r, c].floor.name = "Floor " + r + "," + c;
                cells[r, c].floor.transform.parent = cell.transform;

                if (c == 0)
                {
                    cells[r, c].left = GameObject.Instantiate(wall, new Vector3(((c * wall_size) - (wall_size / 2)), r * wall_size,0 ), Quaternion.identity) as GameObject;
                    cells[r, c].left.name = "Left Wall " + r + "," + c;
                    cells[r, c].left.transform.parent = cell.transform;

                }

                cells[r, c].right = GameObject.Instantiate(wall, new Vector3((wall_size / 2) + (c * wall_size), r * wall_size, 0), Quaternion.identity) as GameObject;
                cells[r, c].right.name = "Right Wall " + r + "," + c;
                cells[r, c].right.transform.parent = cell.transform;

                if (r == 0)
                {
                    cells[r, c].up = GameObject.Instantiate(wall, new Vector3(c * wall_size, ((r * wall_size) - (wall_size / 2)), 0), Quaternion.identity) as GameObject;
                    cells[r, c].up.name = "Up Wall " + r + "," + c;
                    cells[r, c].up.transform.Rotate(Vector3.forward * 90f);
                    cells[r, c].up.transform.parent = cell.transform;

                }

                cells[r, c].bottom = GameObject.Instantiate(wall, new Vector3(c * wall_size, ((wall_size / 2) + (r * wall_size)), 0), Quaternion.identity) as GameObject;
                cells[r, c].bottom.name = "Down Wall " + r + "," + c;
                cells[r, c].bottom.transform.Rotate(Vector3.forward * 90f);
                cells[r, c].bottom.transform.parent = cell.transform;


                cell.transform.parent = maze2D.transform;

            }
        }

    }
    #endregion



}
