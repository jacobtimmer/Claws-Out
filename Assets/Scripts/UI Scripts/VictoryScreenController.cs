using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenController : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "Start Menu";

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}