using UnityEngine;
using UnityEngine.SceneManagement;
using CardScripts;

public class StarterDeckButton : MonoBehaviour
{
    [SerializeField] private StarterDeckData starterDeck;
    [SerializeField] private string firstFightSceneName = "Fight1";

    public void ChooseDeck()
    {
        if (starterDeck == null)
        {
            Debug.LogWarning("No starter deck assigned.");
            return;
        }

        if (GameManager.Instance == null)
        {
            GameObject gameManagerPrefab = Resources.Load<GameObject>("Prefabs/GameManager");
            Instantiate(gameManagerPrefab);
        }

        GameManager.Instance.StartNewRun(starterDeck.startingCards);
        SceneManager.LoadScene(firstFightSceneName);
    }
}