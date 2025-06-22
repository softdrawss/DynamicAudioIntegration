using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }

    public GameObject StartMenu;
    public GameObject PauseMenu;
    public GameObject EndMenu;

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

        StartMenu.SetActive(true);
        PauseMenu.SetActive(false);
        EndMenu.SetActive(false);

        PauseGame();
    }

    private void Update()
    {
        if (NumberDisplay.Instance.currentDigit == 9)
            HandleEndMenu();
        else if (Input.GetKeyDown(KeyCode.Escape) && !StartMenu.activeSelf)
            HandlePauseMenu();
    }

    private void HandlePauseMenu()
    {
        if (PauseMenu.activeSelf)
        {
            PlayGame();
            PauseMenu.SetActive(false);
        }
        else
        {
            PauseGame();
            PauseMenu.SetActive(true);
        }
    }

    private void HandleEndMenu()
    {
        PauseGame();
        EndMenu.SetActive(true);
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gamePaused = false;
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
