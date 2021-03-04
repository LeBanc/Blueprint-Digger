using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InitPanelLevel : MonoBehaviour
{
     public Button[] levelsArray;

    private DataManager dataManager;
    private int adventureLevel;
    private int puzzleLevel;
    private int maxLevelAllowed = 1;

    // Start is called before the first frame update
    void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
        adventureLevel = Mathf.Min(dataManager.GetAdventureLevel(),9); // only 9 adventure levels at max
        puzzleLevel = Mathf.Min(dataManager.GetPuzzleLevel(),9); // only 9 puzzle levels at max

        if (this.gameObject.name.Contains("Adventure"))
        {
            maxLevelAllowed = adventureLevel;
        }
        else
        {
            maxLevelAllowed = puzzleLevel;
        }

        RefreshAccessibleLevels();
    }

    public void RefreshAccessibleLevels()
    {
        for (int i = 0; i < maxLevelAllowed; i++)
        {
            levelsArray[i].interactable = true;
            levelsArray[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(255f, 250f, 0f);
        }
    }

    public void UpdateMaxLevel(int newLevel)
    {
        maxLevelAllowed = newLevel;
        RefreshAccessibleLevels();
    }

    public void LoadAdventureLevel(string level)
    {
        SceneManager.LoadScene(string.Concat("Adventure_", level));
    }

    public void LoadPuzzleLevel(string level)
    {
        SceneManager.LoadScene(string.Concat("Puzzle_", level));
    }
}
