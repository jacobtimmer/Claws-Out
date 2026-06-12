using UnityEngine;
using UnityEngine.SceneManagement;
using CardScripts;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private RewardCardButton[] rewardButtons;
    [SerializeField] private FighterStats playerStats;
    //[SerializeField] private string nextFightSceneName = "Fight1";

    private Card[] rewardPool;

    private void Awake()
    {
        rewardPanel.SetActive(false);
        rewardPool = Resources.LoadAll<Card>("Cards");
    }

    public void ShowRewards()
    {
        rewardPanel.SetActive(true);

        for (int i = 0; i < rewardButtons.Length; i++)
        {
            Card randomCard = rewardPool[Random.Range(0, rewardPool.Length)];
            rewardButtons[i].SetCard(randomCard, this);
        }
    }

    public void ChooseReward(Card card)
    {
        GameManager.Instance.AddCardToRunDeck(card);

        if (GameManager.Instance.HasNextFight())
        {
            string nextScene = GameManager.Instance.GetNextFightSceneName();
            GameManager.Instance.AdvanceFight();

            Time.timeScale = 1f;

            GameManager.Instance.SetPlayerHealth(playerStats.GetCurrentHealth());

            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("Run complete!");
            // Later: load victory scene or show final win panel
        }
    }
}