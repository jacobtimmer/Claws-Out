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

    [SerializeField] private DrawPileManager drawPileManager; //place drawPilemnager here in inspector
    private HandManager handManager;
    private DiscardManager discardManager;

    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private TextMeshProUGUI battleResultText;
    private bool battleEnded = false;



    private void Start()
    {
        Time.timeScale = 1f;

        if (battleResultText != null)
        {
            battleResultText.gameObject.SetActive(false);
        }

        StartPlayerTurn();
    }

    private void Awake()
    {
        handManager = FindAnyObjectByType<HandManager>();
        discardManager = FindAnyObjectByType<DiscardManager>();
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
        DiscardHand();
        enemyStats.ClearArmor(); //enemy armor goes away
        EnemyTurn();
        StartPlayerTurn();
    }

    private void EnemyTurn()
    {
        enemyTurnCounter++;

        if (enemyTurnCounter % 3 == 0) //every 3rd turn, enemy does big attack instead of gaining armor
        {
            enemyAnimator?.SetTrigger("Attack"); //animate attack
            playerStats.TakeDamage(enemyBigAttackDamage);
            CheckBattleEnd(); //check if player died from attack
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
        energyText.text = playerEnergy + " / " + maxPlayerEnergy;
    }

    private void DiscardHand()
    {
        for (int i = handManager.cardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject cardObject = handManager.cardsInHand[i]; //grabs card

            if (cardObject != null && cardObject.TryGetComponent(out CardDisplay cardDisplay))
            {
                discardManager.AddToDiscard(cardDisplay.cardData); //discards and destroys
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
                CheckBattleEnd(); //check if enemy died from attack
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
