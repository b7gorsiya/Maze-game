
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentLevel
{
    public string game_mode;
    public int level_no;
    public int spwan_coins;
    public int win_points;
    public int row;
    public int column;
    public int enemy_no;
    public int time_toclear;
    public int no_trap;
}
public class Level_Manager : MonoBehaviour
{
    public MazeGenrator mazecreator;
    //hint mechanism
    public HintMechanism hintobj;

    private int max_row = 22;
    private int max_col = 15;

    public CurrentLevel level_data;

    [SerializeField]
    Play_Scene_UI_Managment ui;
    // variables for cell genrationg 
    [SerializeField]
    private GameObject wall; // wall prefeb
    [SerializeField]
    private GameObject floor; // floor prefeb
    [SerializeField]
    private int columns;//width/column
    [SerializeField]
    private int rows;//height/row
    [SerializeField]
    private float wall_size; // this means wall dimenstion
    public cell[,] cells; // to store cellss

    [SerializeField]
    public GameObject hint_prefeb;

    private List<int> startPoints = new List<int>();
    private List<int> finishPoints = new List<int>();
    [SerializeField]
    GameObject coin;

    //Player
    public GameObject player;

    [SerializeField]
    List<GameObject> avtars;

    ///ENEMEY 
    [SerializeField]
    GameObject enemyPrefeb;

    [SerializeField]
    List<GameObject> enemy_Pefebs;

    [SerializeField]
    List<Sprite> finsh_doors;

    [SerializeField]
    Sprite _safe_trapSprite;
    [SerializeField]
    Sprite _trap_sprite;

    //timer variables
    [SerializeField]
    int timer = 12;
    int countDown;
    // trap variables
    public static int trapDealy = 3;
    [SerializeField]
    GameObject door;

    [SerializeField]
    GameObject _trap_prefeb;

    Vector2 startPoint;
    Vector2 finishPoint;


    List<Vector2> coins_pos;
    List<Vector2> trap_pos;
    List<Vector2> enemy_pos;

    [SerializeField]
    public GameObject extra_objects;
    void Get_Current_Level()
    {
        if (GameController.instanse.gamemode != GameController.GameMode.class6)
        {
            int level_no = GameController.instanse.current_leve_no;
            string level_class = GameController.instanse.gamemode.ToString();
            level_data.game_mode = level_class;
            level_data.level_no = level_no;
            level_data.column = (level_no > 4) ? ((level_no + 1) / 5) + 5 : 5;
            level_data.row = (level_no > 4) ? ((level_no + 1) / 3) + 5 : 5;
            columns = (level_data.column > max_col) ? max_col : level_data.column;
            rows = (level_data.row > max_row) ? max_row : level_data.row;

            //coins to be spawn
            level_data.spwan_coins = (level_no > 4) ? (level_data.column - 5) + 3 : 3;
        }
        else
        {
            SetUp_LegendLevel();
        }
        /// enemy and traps and time valuse
        switch (GameController.instanse.gamemode)
        {
            //Monster Level
            case GameController.GameMode.class2:
                level_data.enemy_no = (level_data.level_no < 20) ? 1 : (level_data.level_no < 40) ? 2 : 3;
                break;
            //trap level
            case GameController.GameMode.class3:
                level_data.no_trap = (level_data.level_no < 15) ? 1 : (level_data.level_no < 35) ? 2 : 3;
                break;
            // time level
            case GameController.GameMode.class4:
                level_data.time_toclear = (level_data.level_no < 12) ? 11 : (level_data.level_no < 35) ? 18 : 25;
                break;
            case GameController.GameMode.class6:
                level_data.enemy_no = level_data.no_trap = Random.Range(2, 5);
                level_data.time_toclear = Random.Range(25, 35);
                break;
        }
    }
    void SetUp_LegendLevel()
    {
        int level_no = GameController.instanse.current_leve_no;
        string level_class = GameController.instanse.gamemode.ToString();
        level_data.game_mode = level_class;
        level_data.level_no = level_no;
        level_data.column = max_col;
        level_data.row = Random.Range(15, max_row);
        columns = level_data.column;
        rows = level_data.row;

        //coins to be spawn
        level_data.spwan_coins = Random.Range(8, 15);
    }
    private void Awake()
    {
        Time.timeScale = 1;
        Get_Current_Level();
        //initilize pass by refrense variable
        cells = new cell[rows, columns];
        mazecreator.Init_variables(wall, floor, columns, rows, wall_size, ref cells);
        Camera.main.GetComponent<CameraEffect>().ScaleCamera(columns, rows);
        Set_entryNexit();

    }

    private void OnEnable()
    {
        SpawnCoins(level_data.spwan_coins);
        SetUpHintData();
    }

    public void MazeLoading_Completed()
    {
        LeanTween.cancelAll();
        LeanTween.delayedCall(3f, (() =>
         {
             Instansiate_Player();
             CheckGame_Mode();
             GameController.instanse.HideLoading_Focefully();
         }));
       
    }
    void SetUpHintData()
    {
        //set hint mechanism data
        hintobj.rows = rows;
        hintobj.column = columns;
        hintobj.cells = cells;
        hintobj.SetStartPoint(startPoints[0], startPoints[1]);
        hintobj.finishColumn = finishPoints[1];
        hintobj.finishRow = finishPoints[0];

    }

    public void SetHint_StartPoints()
    {
        hintobj.SetStartPoint(playerAI.currentRow, playerAI.currentColumn);
    }
    public void CheckGame_Mode()
    {
        // hid the timer for normal mode
        ui.timer_display.SetActive(false);

        switch (GameController.instanse.gamemode)
        {
            case GameController.GameMode.class2:
                Instansiate_Enemy(level_data.enemy_no);

                break;
            case GameController.GameMode.class4:
                StartTimer(level_data.time_toclear);
                break;
            case GameController.GameMode.class3:
                InstansiateTrap(level_data.no_trap);
                break;
            case GameController.GameMode.class6:
                Instansiate_Enemy(level_data.enemy_no);
                InstansiateTrap(level_data.no_trap);

                StartTimer(level_data.time_toclear);

                break;

        }

    }
    public void Set_entryNexit()
    {

        //set current start and finsih point 
        startPoints.Add(Random.Range(0, 1));
        startPoints.Add(Random.Range(0, columns));
        finishPoints.Add(Random.Range(rows - 1, rows));
        finishPoints.Add(Random.Range(columns / 2, columns));

        // make radomized start Point
        startPoint = cells[startPoints[0], startPoints[1]].floor.transform.position;

        // finish point
        var _door = Instantiate(door, cells[finishPoints[0], finishPoints[1]].floor.transform);
        LeanTween.rotateZ(_door, 180, 3).setLoopType(LeanTweenType.linear);
        _door.transform.parent = extra_objects.transform;
        finishPoint = cells[finishPoints[0], finishPoints[1]].floor.transform.position;


    }


    void SpawnCoins(int no_of_coins)
    {
        coins_pos = new List<Vector2>();
    Spawn:
        Vector2 pos = CheckPos(false);

        foreach (var i in coins_pos)
        {
            if (i == pos) goto Spawn;
        }
        coins_pos.Add(pos);

        var _coin = Instantiate(coin, cells[(int)pos.x, (int)pos.y].floor.transform.position, Quaternion.identity);
        LeanTween.scale(_coin, Vector3.one*0.30f, 1f).setDelay(Random.Range(0,5)).setLoopPingPong();
        _coin.transform.parent = extra_objects.transform;
        no_of_coins--;
        if (no_of_coins > 0) goto Spawn;

    }
    [SerializeField]
    public AIBasePlayerMovement playerAI;
    void Instansiate_Player()
    {
        player = Instantiate(GetAvatar(), startPoint, Quaternion.identity);
        player.transform.parent = extra_objects.transform;
        playerAI = player.GetComponent<AIBasePlayerMovement>();
        playerAI.speed = 0.5f - ((UserData.instance.avtar_data.speed / 100) / 2);// setting up avatar speed
        playerAI.cells = cells;
        playerAI.currentColumn = startPoints[1];
        playerAI.currentRow = startPoints[0];
        playerAI.InitPlayer();
    }
    GameObject GetAvatar()
    {
        return avtars[UserData.instance.avtar_data.current_Avtar - 1];
    }
    GameObject Get_Random_Enmey()
    {
        return enemy_Pefebs[Random.Range(0, enemy_Pefebs.Count - 1)];
    }
    public void Retry_Level()
    {
        switch (GameController.instanse.gamemode)
        {
            case GameController.GameMode.class2:
                // Instansiate_Enemy(level_data.enemy_no);

                break;
            case GameController.GameMode.class4:
                StartTimer(level_data.time_toclear);
                break;
            case GameController.GameMode.class3:
                //InstansiateTrap(level_data.no_trap);
                break;
        }
    }
    // Monster Levels
    void Instansiate_Enemy(int no_of_enemy)
    {
        enemy_pos = new List<Vector2>();
    REDO_RANDOM:
        //intansiate enemy
        var pos = CheckPos(false);
        foreach (var i in enemy_pos)
        {
            if (i == pos) goto REDO_RANDOM;
        }
        enemy_pos.Add(pos);
        //  var row = Random.Range(rows / 4, rows - 1);
        EnemyAI _enemy = Instantiate(Get_Random_Enmey(), cells[(int)pos.x, (int)pos.y].floor.transform.position, Quaternion.identity).GetComponent<EnemyAI>();
        _enemy.transform.parent = extra_objects.transform;
        _enemy.currentColumn = (int)pos.y;
        _enemy.currentRow = (int)pos.x;

        no_of_enemy--;

        if (no_of_enemy > 0) goto REDO_RANDOM;
        // _enemy.StartEnemy_Movement();
    }
    Vector2 CheckPos(bool is_doorTofinish)
    {
    Random:
        var _clmn = Random.Range((is_doorTofinish) ? columns / 2 : 0, columns - 1);
        var _row = Random.Range((is_doorTofinish) ? rows / 4 : 0, rows - 1);

        if (_row == startPoints[0] && _clmn == startPoints[1])
            goto Random;
        else if (_row == finishPoints[0] && _clmn == startPoints[1])
            goto Random;
        else
            return new Vector2(_row, _clmn);
    }
    //Timer Level
    int _extraTime;
    void StartTimer(int time)
    {
        countDown = time;
        _extraTime = UserData.instance.avtar_data.extra_time;
        Debug.Log("extra time " + UserData.instance.avtar_data.extra_time);
        StartCoroutine(starttimer());
    }

    IEnumerator starttimer()
    {
        ui.timer_display.SetActive(true);
    REDO:
        //countdown start
        countDown--;
        ui.timer.text = countDown.ToString("00");

        yield return new WaitForSeconds(1);
        if (GameController.instanse.gamestatus != GameController.GameStatus.PlayMode) yield return null;
        else
        {
            if (_extraTime <= 0 && countDown == 3) GameController.instanse.audiomanager.Countdown_Audio();
            if (countDown > 0) goto REDO;

            if (_extraTime != 0)
            {
                GameController.instanse.audiomanager.ExtraTime_Audio();
                LeanTween.scale(ui.timer_display, Vector2.one * 1.1f, 0.3f).setEasePunch();
                countDown = _extraTime;
                _extraTime = 0;
                ui.timer.text = countDown.ToString("00");
                goto REDO;
            }
            ui.GameOver();
            Debug.Log("Game Over");
        }
        //game over
    }


    //Trap Level
    void InstansiateTrap(int no_of_trap)
    {
        trap_pos = new List<Vector2>();
    REDO_RANDOM:

        var pos = CheckPos(false);
        foreach (var i in trap_pos)
        {
            if (i == pos) goto REDO_RANDOM;
        }

        trap_pos.Add(pos);

        var _trap = Instantiate(_trap_prefeb, cells[(int)pos.x, (int)pos.y].floor.transform);
        _trap.transform.parent = extra_objects.transform;
        no_of_trap--;
        if (no_of_trap > 0) goto REDO_RANDOM;
    }
}
