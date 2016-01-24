using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelController
{
    public static void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadLevel(int levelNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level " + levelNumber);
    }
}