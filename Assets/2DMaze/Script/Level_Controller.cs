using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Controller : MonoBehaviour
{
    [SerializeField]
    GameObject levels_Container;

    List<GameObject> levels;

    Level_Unlocker lvl_unlocker;


    // get user current data
    public int GetUserData(int class_no)
    {
        levels = new List<GameObject>();
        foreach (var lvl in levels_Container.GetComponentsInChildren<Button>())
        {
            levels.Add(lvl.gameObject);
        }
        int ulockedLevels=0;
        switch (class_no)
        {
            case 1:
                ulockedLevels = UserData.instance.levels.class1;
                break;
            case 2:
                ulockedLevels = UserData.instance.levels.class2;
                break;
            case 3:
                ulockedLevels = UserData.instance.levels.class3;
                break;
            case 4:
                ulockedLevels = UserData.instance.levels.class4;
                break;
        }
        for (int i = 0; i < levels.Count; i++)
        {
            if (i < ulockedLevels)
                setUpUnlock_Levels(levels[i],i);
            else
                SetUpLocked_Level(levels[i]);
        }

        return ulockedLevels;
    }

    private void setUpUnlock_Levels(GameObject level,int _level)
    {
        lvl_unlocker = level.GetComponent<Level_Unlocker>();
        lvl_unlocker.isUnlocked = true;
        lvl_unlocker.level_no = _level;
        lvl_unlocker._OnEnable();
    }

    private void SetUpLocked_Level(GameObject level)
    {
        lvl_unlocker = level.GetComponent<Level_Unlocker>();
        lvl_unlocker.isUnlocked = false;
        lvl_unlocker._OnEnable();
    }
}
