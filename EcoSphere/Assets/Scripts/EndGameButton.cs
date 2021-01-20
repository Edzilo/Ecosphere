using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndGameButton : MonoBehaviour
{

    public GameObject creditsPanel;


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

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void ShowCredits(bool isActive)
    {
        creditsPanel.SetActive(isActive);
        EventSystem.current.SetSelectedGameObject(null);
        Button button;

        if (isActive)
        {
            button = creditsPanel.GetComponentInChildren<Button>();
        }
        else
        {
            button = GameObject.FindObjectOfType<Button>();
        }
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }


}
