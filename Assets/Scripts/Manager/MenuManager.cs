using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused;

    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenu.SetActive(isPaused);

        if (isPaused) 
        {
            Time.timeScale = 0f;
            GameEvents.Instance.Pause.Invoke();
        } 
        else 
        { 
            Time.timeScale = 1f;
            GameEvents.Instance.Confirm.Invoke();
        } 
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void Continue()
    {
        TogglePause();
    }

    public void Restart()
    {
        TogglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        GameEvents.Instance.Confirm.Invoke();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
