using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndGameButton : MonoBehaviour
{

    public GameObject creditsPanel;
    public GameObject loadingPanel;





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


    public void PlayGame()
    {
        StartCoroutine(PlayGameCoroutine());
    }

    private IEnumerator PlayGameCoroutine()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        while (!Input.anyKey)
        {
            // While the user does not press any key, we simply wait until next frame. 
            yield return null;
        }


        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync("Niveau1");
        while (!loadSceneOperation.isDone)
        {
            yield return null;
        }
    }


}
