using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSequenceController : MonoBehaviour
{
    [SerializeField] private Image slideImage;
    [SerializeField] private Sprite[] slides;
    [SerializeField] private string nextSceneName = "Fight1";

    private int currentSlideIndex = 0;

    private void Start()
    {
        ShowCurrentSlide();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceSlide();
        }
    }

    public void AdvanceSlide()
    {
        currentSlideIndex++;

        if (currentSlideIndex >= slides.Length)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        ShowCurrentSlide();
    }

    private void ShowCurrentSlide()
    {
        if (slideImage != null && slides.Length > 0)
        {
            slideImage.sprite = slides[currentSlideIndex];
        }
    }
}