using UnityEngine;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }

    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject endMenu;

    public bool gamePaused;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Another NumberDisplay already exists. Destroying this.");
            Destroy(gameObject);
            return;
        }

        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);

        PauseGame();
    }

    private void Update()
    {
        if (NumberDisplay.Instance.currentDigit == 9)
            HandleEndMenu();
        else if (Input.GetKeyDown(KeyCode.Escape) && !startMenu.activeSelf)
            HandlePauseMenu();
    }

    private void HandlePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            PlayGame();
            pauseMenu.SetActive(false);
        }
        else
        {
            PauseGame();
            pauseMenu.SetActive(true);
        }
    }

    private void HandleEndMenu()
    {
        PauseGame();
        endMenu.SetActive(true);
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gamePaused = false;
        if (endMenu.activeSelf) endMenu.SetActive(false);
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gamePaused = true;
    }


    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
