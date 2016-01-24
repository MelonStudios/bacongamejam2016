using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelController
{
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadLevel(LevelGroup levelGroup, int levelNumber)
    {
        SceneManager.LoadScene("Level_" + levelGroup + levelNumber);
    }
}

public enum LevelGroup
{
    A
}