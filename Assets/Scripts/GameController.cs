using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GridLocation grid;
    private Camera mainCamera;
    public GameObject winCanevas;
    public GameObject pauseCanevas;
    public GameObject rightPanel;
    private bool _menuIsActive;
    private bool _gameWon;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridLocation>();
        mainCamera = FindObjectOfType<Camera>();
        pauseCanevas.SetActive(false);
        winCanevas.SetActive(false);
        _menuIsActive = false;
        _gameWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && grid.buildingEnds && !_gameWon)
        {
            if (!_menuIsActive)
            {
                pauseCanevas.SetActive(true);
                _menuIsActive = true;
            }
            else
            {
                ResumeGame();
            }
        }

        if (!_menuIsActive && !_gameWon) mainCamera.GetComponent<CameraManager>().CameraUpdate();

    }

    public void ResumeGame()
    {
        pauseCanevas.SetActive(false);
        rightPanel.GetComponent<MenuRightPanelManager>().InitPanel();
        _menuIsActive = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextLevel()
    {
        // Define name of the next scene
        string[] sceneDef = SceneManager.GetActiveScene().name.Split('_');
        int nextLevel = int.Parse(sceneDef[1]) + 1;
        sceneDef[1] = nextLevel.ToString();
        string nextScene = string.Concat(sceneDef[0], "_0", sceneDef[1]);

        if (DoesSceneExist(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public static bool DoesSceneExist(string name) // get on github: thanks to yagero
    {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }

    public void ActivateWinMenu()
    {
        winCanevas.SetActive(true);
        _gameWon = true;

        // Unlock the next level on the screen menu
        string[] sceneDef = SceneManager.GetActiveScene().name.Split('_');
        int nextLevel = int.Parse(sceneDef[1]) + 1;
        if (nextLevel < 10) //only 9 level and there is a bug on MainMenu if the array is greater than 9
        {
            if (sceneDef[0].Equals("Adventure"))
            {
                FindObjectOfType<DataManager>().SetAdventureLevel(nextLevel);
            }
            else if (sceneDef[0].Equals("Puzzle"))
            {
                FindObjectOfType<DataManager>().SetPuzzleLevel(nextLevel);
            }
        }
    }
}
