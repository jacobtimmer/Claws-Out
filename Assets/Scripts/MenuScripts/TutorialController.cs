using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        public string title;

        [TextArea(3, 8)]
        public string description;
    }

    [Header("Tutorial Content")]
    [SerializeField] private TutorialPage[] pages;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Buttons")]
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject startFightButton;

    [Header("Scene")]
    [SerializeField] private string firstFightSceneName = "Fight1";

    private int currentPageIndex = 0;

    private void Start()
    {
        Time.timeScale = 1f;
        ShowPage(0);
    }

    public void ShowNextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            ShowPage(currentPageIndex + 1);
        }
    }

    public void ShowPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            ShowPage(currentPageIndex - 1);
        }
    }

    public void StartFirstFight()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstFightSceneName);
    }

    private void ShowPage(int pageIndex)
    {
        if (pages == null || pages.Length == 0)
        {
            Debug.LogWarning("No tutorial pages assigned.");
            return;
        }

        currentPageIndex = Mathf.Clamp(pageIndex, 0, pages.Length - 1);

        TutorialPage currentPage = pages[currentPageIndex];

        titleText.text = currentPage.title;
        descriptionText.text = currentPage.description;

        backButton.SetActive(currentPageIndex > 0);
        nextButton.SetActive(currentPageIndex < pages.Length - 1);

        // Start Fight is available on every tutorial page.
        startFightButton.SetActive(true);
    }
}