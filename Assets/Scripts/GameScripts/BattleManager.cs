using CardScripts;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private int playerEnergy = 3;
    [SerializeField] private int maxPlayerEnergy = 3;
    [SerializeField] private int cardsDrawnPerTurn = 4;

    [SerializeField] private FighterStats playerStats;
    [SerializeField] private FighterStats enemyStats;

    [SerializeField] private TextMeshProUGUI energyText;

    [SerializeField] private int enemyArmorGain = 12;
    [SerializeField] private int enemyBigAttackDamage = 18;
    private int enemyTurnCounter = 0;

    [SerializeField] private DrawPileManager drawPileManager;
    private HandManager handManager;
    private DiscardManager discardManager;

    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private TextMeshProUGUI battleResultText;
    private bool battleEnded = false;

    private void Awake()
    {
        handManager = FindAnyObjectByType<HandManager>();
        discardManager = FindAnyObjectByType<DiscardManager>();
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (battleResultText != null)
        {
            battleResultText.gameObject.SetActive(false);
        }

        // Do NOT call StartPlayerTurn() here.
        // DeckManager.BattleSetup() already draws the starting hand.
        playerEnergy = maxPlayerEnergy;
        UpdateEnergyUI();

        if (playerStats != null)
        {
            playerStats.ClearArmor();
        }
    }

    private void CheckBattleEnd()
    {
        if (battleEnded)
        {
            return;
        }

        if (enemyStats.IsDead)
        {
            EndBattle("You win!");
        }
        else if (playerStats.IsDead)
        {
            EndBattle("You lose!");
        }
    }

    private void EndBattle(string message)
    {
        battleEnded = true;

        if (battleResultText != null)
        {
            battleResultText.text = message;
            battleResultText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void StartPlayerTurn()
    {
        playerEnergy = maxPlayerEnergy;
        UpdateEnergyUI();
        playerStats.ClearArmor();

        for (int i = 0; i < cardsDrawnPerTurn; i++)
        {
            drawPileManager.DrawCard(handManager);
        }

        Debug.Log("Player turn started. Energy: " + playerEnergy);
    }

    public void EndPlayerTurn()
    {
        DiscardHand();
        enemyStats.ClearArmor();
        EnemyTurn();

        if (!battleEnded)
        {
            StartPlayerTurn();
        }
    }

    private void EnemyTurn()
    {
        enemyTurnCounter++;

        if (enemyTurnCounter % 3 == 0)
        {
            enemyAnimator?.SetTrigger("Attack");
            playerStats.TakeDamage(enemyBigAttackDamage);
            CheckBattleEnd();
            Debug.Log("Enemy used a big attack for " + enemyBigAttackDamage);
        }
        else
        {
            enemyStats.GainArmor(enemyArmorGain);
            Debug.Log("Enemy gained " + enemyArmorGain + " armor.");
        }
    }

    private void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = "Energy: " + playerEnergy + " / " + maxPlayerEnergy;
        }
    }

    private void DiscardHand()
    {
        for (int i = handManager.cardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject cardObject = handManager.cardsInHand[i];

            if (cardObject != null && cardObject.TryGetComponent(out CardDisplay cardDisplay))
            {
                discardManager.AddToDiscard(cardDisplay.cardData);
            }

            Destroy(cardObject);
        }

        handManager.cardsInHand.Clear();
        handManager.UpdateHandVisuals();
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

        if (!card.isBrittleCard)
        {
            discardManager.AddToDiscard(card); //brittle ones go poof
        }

        Destroy(cardObject);

        return true;
    }

    private void ResolveCardEffect(Card card)
    {
        for (int i = 0; i < card.timesActivated; i++)
        {
            int damageDealt = 0;

            if (card.damageMax > 0)
            {
                int damage = Random.Range(card.damageMin, card.damageMax + 1);
                enemyStats.TakeDamage(damage);
                damageDealt += damage;
            }

            if (card.healthGain > 0)
            {
                playerStats.Heal(card.healthGain);
            }

            if (card.armorGain > 0)
            {
                playerStats.GainArmor(card.armorGain);
            }

            if (card.selfDamage > 0)
            {
                playerStats.TakeDamage(card.selfDamage);
            }

            if (card.isaLifestealCard && damageDealt > 0)
            {
                playerStats.Heal(damageDealt);
            }

            if (card.energyGained > 0)
            {
                playerEnergy += card.energyGained;
                UpdateEnergyUI();
            }

            if (card.cardsDrawn > 0)
            {
                for (int j = 0; j < card.cardsDrawn; j++)
                {
                    drawPileManager.DrawCard(handManager);
                }
            }

            CheckBattleEnd();
        }

        Debug.Log("Played card: " + card.cardName);
    }
}