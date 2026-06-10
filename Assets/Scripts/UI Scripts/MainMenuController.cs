using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject creditsPanel;

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "SampleScene";

    private void Start()
    {
        ShowMainMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowCredits()
    {
        mainMenuButtons.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuButtons.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}