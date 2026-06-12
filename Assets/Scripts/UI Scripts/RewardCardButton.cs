using TMPro;
using UnityEngine;
using CardScripts;

public class RewardCardButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;

    private Card card;
    private RewardManager rewardManager;

    public void SetCard(Card newCard, RewardManager manager)
    {
        card = newCard;
        rewardManager = manager;

        cardNameText.text = card.cardName;
        cardDescriptionText.text = card.cardText;
    }

    public void PickCard()
    {
        rewardManager.ChooseReward(card);
    }
}