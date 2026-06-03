using CardScripts;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private int playerEnergy = 3;
    [SerializeField] private int maxPlayerEnergy = 3;
    [SerializeField] private int cardsDrawnPerTurn = 5;

    [SerializeField] private FighterStats playerStats;
    [SerializeField] private FighterStats enemyStats;

    [SerializeField] private TextMeshProUGUI energyText;

    [SerializeField] private DrawPileManager drawPileManager; //place drawPilemnager here in inspector
    private HandManager handManager;
    private DiscardManager discardManager;

    private void Start()
    {
        StartPlayerTurn();
    }

    private void Awake()
    {
        handManager = FindAnyObjectByType<HandManager>();
        discardManager = FindAnyObjectByType<DiscardManager>();
    }

    public void StartPlayerTurn()
    {
        playerEnergy = maxPlayerEnergy; //new energy
        UpdateEnergyUI(); //update energy UI
        playerStats.ClearArmor(); //armor goes away

        for (int i = 0; i < cardsDrawnPerTurn; i++)
        {
            drawPileManager.DrawCard(handManager);
        }

        Debug.Log("Player turn started. Energy: " + playerEnergy);
    }

    public void EndPlayerTurn()
    {
        EnemyTurn();
        StartPlayerTurn();
    }

    private void EnemyTurn() //just doing damage for now
    {
        int enemyDamage = 6;
        playerStats.TakeDamage(enemyDamage);

        Debug.Log("Enemy attacked for " + enemyDamage);
    }

    private void UpdateEnergyUI()
    {
        energyText.text = playerEnergy + " / " + maxPlayerEnergy;
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
        UpdateEnergyUI();

        ResolveCardEffect(card);

        handManager.cardsInHand.Remove(cardObject);
        handManager.UpdateHandVisuals();

        discardManager.AddToDiscard(card);

        Destroy(cardObject);

        return true;
    }

    private void ResolveCardEffect(Card card)
    {
        for (int i = 0; i < card.timesActivated; i++)
        {
            if (card.damageMax > 0)
            {
                int damage = Random.Range(card.damageMin, card.damageMax + 1); //Random.Range(4, 5) always returns 4, so add 1 to max works
                enemyStats.TakeDamage(damage);
            }

            if (card.healthGain > 0)
            {
                playerStats.Heal(card.healthGain);
            }

            if (card.armorGain > 0)
            {
                playerStats.GainArmor(card.armorGain);
            }
        }

        Debug.Log("Played card: " + card.cardName);
    }
}