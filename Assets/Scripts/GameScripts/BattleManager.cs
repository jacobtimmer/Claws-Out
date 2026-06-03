using CardScripts;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private int playerEnergy = 3;

    private HandManager handManager;
    private DiscardManager discardManager;

    private void Awake()
    {
        handManager = FindAnyObjectByType<HandManager>();
        discardManager = FindAnyObjectByType<DiscardManager>();
    }

    public bool TryPlayCard(Card card, GameObject cardObject)
    {
        if (card == null)
        {
            return false;
        }
        if (playerEnergy < card.energyCost)
        {
            Debug.Log("Not enough energy.");
            return false;
        }

        playerEnergy -= card.energyCost;

        ResolveCardEffect(card);

        handManager.cardsInHand.Remove(cardObject);
        handManager.UpdateHandVisuals();

        discardManager.AddToDiscard(card);

        Destroy(cardObject);

        return true;
    }

    private void ResolveCardEffect(Card card)
    {
        Debug.Log("Played card: " + card.cardName);

        // Damage, healing, armor, draw effects, etc. go here later.
    }
}