using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int adventureLevel;
    private int puzzleLevel;
    public static DataManager instance;

    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("AdventureLevel"))
            {
                adventureLevel = 1;
                SetAdventureLevel(adventureLevel);
            }
            else
            {
                adventureLevel = PlayerPrefs.GetInt("AdventureLevel", 1);
            }

            if (!PlayerPrefs.HasKey("PuzzleLevel"))
            {
                puzzleLevel = 1;
                SetPuzzleLevel(puzzleLevel);
            }
            else
            {
                puzzleLevel = PlayerPrefs.GetInt("PuzzleLevel", 1);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int GetAdventureLevel()
    {
        adventureLevel = PlayerPrefs.GetInt("AdventureLevel", 1);
        return adventureLevel;
    }

    public int GetPuzzleLevel()
    {
        puzzleLevel = PlayerPrefs.GetInt("PuzzleLevel", 1);
        return puzzleLevel;
    }

    public void SetAdventureLevel(int level)
    {
        int max = Mathf.Max(level,PlayerPrefs.GetInt("AdventureLevel"));
        PlayerPrefs.SetInt("AdventureLevel", max);
    }

    public void SetPuzzleLevel(int level)
    {
        int max = Mathf.Max(level, PlayerPrefs.GetInt("PuzzleLevel"));
        PlayerPrefs.SetInt("PuzzleLevel", level);
    }

    public void ResetLevels()
    {
        SetAdventureLevel(1);
        SetPuzzleLevel(1);
    }
}
