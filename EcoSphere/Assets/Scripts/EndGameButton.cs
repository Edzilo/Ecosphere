using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameButton : MonoBehaviour
{

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync("Niveau1"); // loads current scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }


}
